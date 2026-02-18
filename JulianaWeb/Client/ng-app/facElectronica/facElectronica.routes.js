(function () {
  'use strict';
  angular.module('app.facElectronica').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [{
      state: 'facElectronica',
      config: {
        url: '/Nomina-Electrónica',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@facElectronica": {
            templateUrl: 'Client/ng-app/facElectronica/facElectronica.html',
            controller: 'facElectronicaCtrl',
            controllerAs: 'vm'
          }
        },
        resolve: {
          data: Get_Data_Facturacion,
          Empresas: Get_Empresas_Usuarios
        }
      }
    }]
  }


  Get_Data_Facturacion.$inject = ['$http', '$q', 'AuthSvc'];
  function Get_Data_Facturacion($http, $q, AuthSvc) {
    var deferred = $q.defer();
    var DBName = AuthSvc.data.DBName;    
    var url = ROOTURL + 'API/facElectronica/' + DBName.trim() + '/LoadData';
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
