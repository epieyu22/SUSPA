(function() {
'use strict';

  angular
    .module('app.core')
    .factory('DialogSvc', DialogSvc);

  DialogSvc.$inject = ['ngDialog'];
  function DialogSvc(ngDialog) {
    var service = {
      openConfirm: openConfirm
    };
    
    return service;

    ////////////////
    function openConfirm(confirm, reject) { 
      ngDialog.openConfirm({
        template: '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Some message</h4></div></div>     \
            \
            <button ng-click="closeThisDialog()">Cancel</button>\
            <button ng-click="confirm()">Confirm</button>\
        </div>',
        plain: true,
        className: 'ngdialog-theme-plain',
        appendClassName : 'ngdialog-systems-theme',
        controller: ['$scope', 'ngDialog', function($scope, ngDialog) {
          $scope.confirm = confirm;
          $scope.reject = reject;
        }]
      });
    }


  }
})();
