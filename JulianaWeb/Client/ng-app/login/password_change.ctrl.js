(function() {
  'use strict';

  angular.module('app.login').controller('PasswordChangeCtrl', PasswordChangeCtrl);

  PasswordChangeCtrl.$inject = ['$http', '$state', 'NotifySvc', 'AuthSvc'];
  function PasswordChangeCtrl($http, $state, NotifySvc, AuthSvc) {
    var vm = this;
    vm.viewmodel = {};
    vm.saving = false;

    activate();

    ////////////////

    function activate() {}

    vm.Change_Password = function() {
      vm.saving = true;
      var url = ROOTURL + 'API/Account/' + AuthSvc.data.UserName + '/ChangeFirstPassword';
      $http
        .post(url, vm.viewmodel)
        .success(function(data) {
          if (data.error) {
            NotifySvc.error(data.error);
            vm.saving = false;
          } else {
            localStorage.setItem('PasswordChanged', true);
            NotifySvc.success();
            $state.go('dashboard');
            vm.saving = false;
          }
        })
        .error(function() {
          NotifySvc.error();
        });
    };
  }
})();
