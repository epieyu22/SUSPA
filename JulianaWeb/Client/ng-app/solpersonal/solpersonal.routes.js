(function () {
  'use strict';
  angular.module('app.solpersonal').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [{
      state: 'solpersonal',
      config: {
        url: '/Solicitud-Personal',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@solpersonal": {
            templateUrl: 'Client/ng-app/solpersonal/solpersonal.html',
            controller: 'SolPersonalCtrl',
            controllerAs: 'vm'
          }
        },
        resolve: {
          data: Get_Data_SolPersonal,
          Empresas: Get_Empresas_Usuarios
        }
      }
    }]
  }


  Get_Data_SolPersonal.$inject = ['$http', '$q', 'AuthSvc'];
  function Get_Data_SolPersonal($http, $q, AuthSvc) {
    var deferred = $q.defer();
    var DBName = AuthSvc.data.DBName;
    var url = ROOTURL + 'API/SolPersonal/' + DBName.trim() + '/Data';
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
