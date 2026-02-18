(function () {
  'use strict';
  angular.module('app.bioseguridad').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [{
      state: 'bioseguridad',
      config: {
        url: '/Bioseguridad-Registro-Horas',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@bioseguridad": {
            templateUrl: 'Client/ng-app/bioseguridad/bioseguridad.html',
            controller: 'BioseguirdadCtrl',
            controllerAs: 'vm'
          }
        },
        resolve: {
          data: Get_Data_Bioseguridad,
          Empresas: Get_Empresas_Usuarios
        }
      }

    },
    {
      state: 'bioseguridadReport',
      config: {
        url: '/Bioseguridad-Reporte-Horas',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@bioseguridadReport": {
            templateUrl: 'Client/ng-app/bioseguridad/bioseguridadReport/bioseguridadReport.html',
            controller: 'BioseguirdadCtrl',
            controllerAs: 'vm'
          }
        },
        resolve: {
          data: Get_Data_Bioseguridad,
          Empresas: Get_Empresas_Usuarios
        }
      }
    },
    ]
  }


  Get_Data_Bioseguridad.$inject = ['$http', '$q', 'AuthSvc'];
  function Get_Data_Bioseguridad($http, $q, AuthSvc) {    
    var deferred = $q.defer();
    var Empresa = AuthSvc.data.DBName;
    var Cedula = AuthSvc.data.UserName;

    var url = ROOTURL + 'API/Bioseguridad/' + Empresa.trim() + '/' + Cedula + '/Data';
    $http.get(url)
      .success(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
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
