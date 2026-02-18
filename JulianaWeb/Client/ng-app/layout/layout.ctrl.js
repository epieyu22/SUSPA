(function() {
  'use strict';
  angular.module('app.core').controller('LayoutCtrl', LayoutCtrl);

  LayoutCtrl.$inject = ['$state', 'AuthSvc', 'MenuSvc', 'AdminSvc'];

  function LayoutCtrl($state, AuthSvc, MenuSvc, AdminSvc) {
    var vm = this;
    vm.menu = MenuSvc;
    // vm.username = AuthSvc.data.Empleado;
    vm.username = '';
    vm.version = '';
    vm.currentYear = new Date()
    vm.mostrarcondiciones = false;

    vm.logout = logout;
    vm.hasPermission = AuthSvc.hasPermission;
    vm.sidenavOpen = false;
    vm.accountMenuOpen = false;
    vm.toggleSidenav = toggleSidenav;
    vm.toggleAccountMenu = toggleAccountMenu;
    vm.closeAccountMenu = closeAccountMenu;

    vm.enableSSOLogin = SETTINGS.EnableSSOLogin === 'true';

    vm.aceptarCondiciones = function() {};

    activate();
    ////////////////
    function activate() {
      var hasChangedPassword = JSON.parse(localStorage.getItem('PasswordChanged'));
      if (!AuthSvc.data.loggedIn) {
        if (SETTINGS.UseADLogin === 'true') $state.go('adlogin');
        else $state.go('login');
      } else {
        AuthSvc.getEmpleado().then(function(empleado) {
          vm.username = empleado.Empleado.trim();
        }).finally(function () {
          if (!vm.username) {
            AuthSvc.getTercero().then(function (tercero) {
              vm.username = tercero.Tercero.trim();
            });
          }
        });
        if (!hasChangedPassword && SETTINGS.UseADLogin !== 'true' && !vm.enableSSOLogin) {
          $state.go('passwordChange');
        }
      }

      AdminSvc.getVersion().then(data => vm.version = data?.Valor);
    }

    function logout() {
      const hadSsoLogin = AuthSvc.HadSsoInfo();

      AuthSvc.logout();

      if (SETTINGS.EnableSSOLogin && hadSsoLogin) {
        document.getElementById("sso-logout-form").submit();
        return;
      }

      if (SETTINGS.UseADLogin === 'true') $state.go('adlogin');
      else $state.go('login');
    }

    function toggleSidenav() {
      vm.sidenavOpen = !vm.sidenavOpen;
      console.log(vm.sidenavOpen);
    }

    function toggleAccountMenu(argument) {
      vm.accountMenuOpen = !vm.accountMenuOpen;
    }

    function closeAccountMenu(argument) {
      if (vm.accountMenuOpen) {
        vm.accountMenuOpen = false;
      }
    }
  }
})();
