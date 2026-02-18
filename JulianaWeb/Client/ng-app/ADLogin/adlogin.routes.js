(function() {
  'use strict';
  angular.module('app.adlogin').run(appRun);

  appRun.$inject = ['routerHelper'];
     /* @ngInject */
  function appRun(routerHelper) {
      routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'adlogin',
        config: {
          url: '/ADLogin',
          views: {
            "":{
              templateUrl: 'Client/ng-app/adlogin/RegularLogin.html',
              controller: 'ADLoginCtrl',
              controllerAs: 'vm'
            }
          }
        }        
      },
      {
        state: 'adlogin2',
        config: {
          url: '/ADLogin2',
          views: {
            "":{
              templateUrl: 'Client/ng-app/adlogin/adlogin.html',
              controller: 'ADLoginCtrl',
              controllerAs: 'vm'
            }
          }
        }        
      }
    ];
  }

})();
