(function () {
  'use strict';
  angular.module('app.planilla_nomina').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [
      {
        state: 'planilla_nomina',
        config: {
          url: '/PlanillaNomina',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@planilla_nomina": {
              templateUrl: 'Client/ng-app/planilla_nomina/planilla_nomina.html',
              controller: 'PlanillaNominaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            Empresas: Get_Empresas_Usuarios
          }
        }
      },
      {
        state: 'planilla_nomina2',
        config: {
          url: '/PlanillaNomina2',
          views: {
            "": {
              templateUrl: 'Client/ng-app/planilla_nomina/planilla_nomina_2.html',
              controller: 'PlanillaNominaCtrl',
              controllerAs: 'vm'
            }
          },
          resolve: {
            Empresas: Get_Empresas_Usuarios
          }
        }
      },
    ];
  }

  Get_Empresas_Usuarios.$inject = ['$q', 'AuthSvc'];
  function Get_Empresas_Usuarios($q, AuthSvc) {
    var deferred = $q.defer();
    AuthSvc
      .Get_Empresas_Usuarios()
      .then(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
  }

})();
