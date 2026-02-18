(function() {
  'use strict';
  angular
    .module('app.dashboard')
    .controller('DashboardCtrl', DashboardCtrl);

  DashboardCtrl.$inject = ['AuthSvc', 'MenuSvc'];
    /* @ngInject */
  function DashboardCtrl(AuthSvc, MenuSvc) {
    var vm = this;
    vm.menu = MenuSvc;
    vm.hasPermission = AuthSvc.hasPermission;
    activate();
    ////////////////
    function activate() {
    }
  }
})();
