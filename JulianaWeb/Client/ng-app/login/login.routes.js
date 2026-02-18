(function() {
  'use strict';
  angular.module('app.login').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    if (SETTINGS.UseADLogin === 'true') {
      routerHelper.configureStates(getStates(), '/ADLogin');
    } else {
      routerHelper.configureStates(getStates(), '/app-login');
    }
  }

  function getStates() {
    return [
      {
        state: 'login',
        config: {
          url: '/app-login',
          views: {
            '': {
              templateUrl: 'Client/ng-app/login/login.html',
              controller: 'LoginCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'login.requestPassword',
        config: {
          url: '/RequestPassword',
          views: {
            '': {
              templateUrl: 'Client/ng-app/login/login.html',
              controller: 'LoginCtrl',
              controllerAs: 'vm'
            },
            'login@login': {
              templateUrl: 'Client/ng-app/login/request-password.html',
              controller: 'LoginCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'login.noMailValidation',
        config: {
          url: '/NoMailValidation',
          views: {
            '': {
              templateUrl: 'Client/ng-app/login/login.html',
              controller: 'LoginCtrl',
              controllerAs: 'vm'
            },
            'login@login': {
              templateUrl: 'Client/ng-app/login/no-mail-validation.html',
              controller: 'LoginCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'acceso',
        config: {
          url: '/Acceso/:Empresa',
          views: {
            '': {
              templateUrl: 'Client/ng-app/login/acceso.html',
              controller: 'LoginCtrl',
              controllerAs: 'vm'
            }
          }
        }
      },
      {
        state: 'passwordChange',
        config: {
          url: '/CambioClave',
          views: {
            '': {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            'MainContent@passwordChange': {
              templateUrl: 'Client/ng-app/login/password_Change.html',
              controller: 'PasswordChangeCtrl',
              controllerAs: 'vm'
            }
          }
        }
      }
    ];
  }
})();
