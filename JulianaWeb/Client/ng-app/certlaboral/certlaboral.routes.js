(function () {
  'use strict';
  angular.module('app.consultas').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'certlaboral2',
        config: {
          url: '/Certlaboral2',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            }
            ,
            "MainContent@certlaboral2": {
              templateUrl: 'Client/ng-app/certlaboral/certlaboral.html',
              controller: 'CertlaboralCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'certlaboral3',
        config: {
          url: '/otroscert',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            }
            ,
            "MainContent@certlaboral3": {
              templateUrl: 'Client/ng-app/certlaboral/otroscert.html',
              controller: 'CertlaboralCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'politicas',
        config: {
          url: '/politicas',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            }
            ,
            "MainContent@politicas": {
              templateUrl: 'Client/ng-app/certlaboral/politicas.html',
              controller: 'CertlaboralCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
    ];
  }
})();
