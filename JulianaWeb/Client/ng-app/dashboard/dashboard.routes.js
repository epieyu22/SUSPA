(function() {
  'use strict';
  angular.module('app.dashboard').run(appRun);

  appRun.$inject = ['routerHelper'];
     /* @ngInject */
  function appRun(routerHelper) {
      routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'dashboard',
        config: {
          url: '/Dashboard',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@dashboard": {
              templateUrl: 'Client/ng-app/dashboard/dashboard.html',
              controller: 'DashboardCtrl',
              controllerAs: 'vm'
            }
          }
        }
      }
    ];
  }
})();
