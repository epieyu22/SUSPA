(function() {
  'use strict';
  angular.module('app.login').controller('LoginCtrl', LoginCtrl);
  LoginCtrl.$inject = ['$state', '$stateParams', '$window', 'NotifySvc', 'AuthSvc'];
  /* @ngInject */
  function LoginCtrl($state, $stateParams, $window, NotifySvc, AuthSvc) {
    var vm = this;
    vm.viewmodel = {};

    vm.inputSettings = {
      height: '38px',
      width: '100%',
      theme: 'metro'
    };

    vm.TIPO_DOCUMENTOS = [
      {id: 'C', name: 'Cedula'},
      {id: 'E', name: 'Extranjería'},
      {id: 'N', name: 'Nit'},
      {id: 'P', name: 'Pasaporte'},
      {id: 'R', name: 'Reg. Civil'},
      {id: 'T', name: 'T. Identidad'},
      {id: 'U', name: 'Num. Unico Identif.'}
    ];

    vm.login = login;
    vm.requestPassword = requestPassword;
    vm.createAspirante = createAspirante;
    vm.redirectSSO = redirectSSO;
    vm.loading = false;
    vm.data = {};
    vm.enableSSOLogin = SETTINGS.EnableSSOLogin === 'true';
    activate();
    ////////////////
    function activate() {
      if (AuthSvc.data.loggedIn) {
        $state.go('dashboard');
      }
    }

    function login() {
      vm.loading = true;
      AuthSvc.login(vm.viewmodel).then(
        function(data) {
          $state.go('dashboard');
          vm.loading = false;
        },
        function(data) {
          vm.data = data;
          vm.loading = false;
        },
        function(msg) {
          NotifySvc.error();
          // vm.data.error = "Ha ocurrido un error, por favor verifique la información"
          vm.loading = false;
        }
      );
    }

    function requestPassword() {
      if (vm.loading) return;
      vm.loading = true;
      AuthSvc.requestPassword(vm.viewmodel.cedula).then(
        function(data) {
          vm.data = data;
          vm.loading = false;
        },
        function(msg) {
          NotifySvc.error();
          // vm.data.error = "Ha ocurrido un error, por favor verifique la información"
          vm.loading = false;
        }
      );
    }

    function createAspirante() {
      vm.loading = true;
      vm.viewmodel.DBName = $stateParams.Empresa;
      AuthSvc.createAspirante(vm.viewmodel).then(
        function(data) {
          if (data.error) {
            vm.data = data;
          } else {
            vm.success = true;
          }
          vm.loading = false;
        },
        function(msg) {
          NotifySvc.error(msg);
          // vm.data.error = "Ha ocurrido un error, por favor verifique la información"
          vm.loading = false;
        }
      );
    }

    function redirectSSO() {
      $window.location = ROOTURL + 'SSO/Login';
    }
  }
})();
