(function () {
  'use strict';
  angular.module('app.dashboardBi').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'dashboardBi_RRHH',
        config: {
          url: '/Dashboard-RRHH',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@dashboardBi_RRHH": {
              templateUrl: 'Client/ng-app/dashboardBi/dashboardBi.html',
              controller: 'dashboardBiCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'dashboardBi_payroll',
        config: {
          url: '/Dashboard-payroll',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@dashboardBi_payroll": {
              templateUrl: 'Client/ng-app/dashboardBi/dashboardBi_Payroll.html',
              controller: 'dashboardBiCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'dashboardBi_financiero',
        config: {
          url: '/Dashboard-financiero',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@dashboardBi_financiero": {
              templateUrl: 'Client/ng-app/dashboardBi/dashboardBi_Finanzas.html',
              controller: 'dashboardBiCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
    ];
  }
})();
