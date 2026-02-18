(function (ng) {

  ng.module('app.core').factory('IdleTimeout', ['$timeout', '$document', '$state', 'ngDialog', 'AuthSvc', function ($timeout, $document, $state, ngDialog, AuthSvc) {

    return function (delay, onIdle, playOnce) {

      var idleTimeout = function (delay, onIdle) {
        var $this = this;
        $this.idleTime = delay;
        $this.goneIdle = function () {
          onIdle();
          $timeout.cancel($this.timeout);
          AuthSvc.logout();
          ngDialog.openConfirm({
            template: '\
            <div class="ngdialog-body">\
                <div class="row"><div class="col-md-12"><h4>La sesión ha terminado</h4></div>    \
                    <div class="col-md-12 m-y-2">La sesión ha terminado por inactividad, ingrese de nuevo si desea seguir usando el sistema</div> \
                <div class="col-md-12  text-right">\
                  <button class="btn btn-primary" ng-click="confirm()">Aceptar</button>\
                <div>\
            </div>',
            plain: true,
            closeByEscape: false,
            showClose: false,
            className: 'ngdialog-theme-plain',
            appendClassName: 'ngdialog-systems-theme'
          })
            .then(function () {
              if (SETTINGS.UseADLogin === 'true') $state.go('adlogin')
              else $state.go('login')
            });
        };
        return {
          cancel: function () {
            return $timeout.cancel($this.timeout);
          },
          start: function (event) {
            if ($state.current.name == 'login' || $state.current.name == 'adlogin') return;
            $this.timeout = $timeout(function () {
              $this.goneIdle();
            }, $this.idleTime);
          }
        };
      };

      var events = ['keydown', 'keyup', 'click', 'mousemove', 'DOMMouseScroll', 'mousewheel', 'mousedown', 'touchstart', 'touchmove', 'scroll', 'focus'];
      var $body = angular.element($document);
      var reset = function () {
        idleTimer.cancel();
        idleTimer.start();
      };
      var idleTimer = idleTimeout(delay, onIdle);



      return {
        active: true,
        cancel: function () {
          idleTimer.cancel();
          ng.forEach(events, function (event) {
            $body.off(event, reset);
          });
        },
        start: function () {
          idleTimer.start();
          ng.forEach(events, function (event) {
            $body.on(event, reset);
          });
        }
      };
    };

  }]).directive('timeoutTest', ['IdleTimeout', '$state', function (IdleTimeout, $state) {
    return {
      restrict: 'AC',
      controller: function ($scope) {
        $scope.msg = '';
        $scope.timer = null;
        $scope.state = $state.current.name;
        $scope.active = false;
        $scope.start = function (timer) {
          $scope.timer = new IdleTimeout(Number(SETTINGS.SessionTimeout), $scope.cancel);
          $scope.timer.start();
          $scope.active = true;
        };
        $scope.cancel = function () {
          // console.log('app has gone idle');
          // $scope.timer.cancel();
          $scope.active = false;
        };
      },
      link: function ($scope, $el, $attrs) {
        if (SETTINGS.UseSessionTimeout === 'false') return;
        $scope.start();
      }
    };
  }]);
}(angular));

