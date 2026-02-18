(function () {
  'use strict';
  angular.module('app.cumpleanos').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [{
      state: 'cumpleanos',
      config: {
        url: '/Cumpleaños',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@cumpleanos": {
            templateUrl: 'Client/ng-app/cumpleaños/cumpleanos.html',
            controller: 'CumpleanosCtrl',
            controllerAs: 'vm'
          }
        },
        resolve: {
          data: Get_Data_Cumpleanos,
          empresas: Get_Empresas_Usuarios
        }
      }
    }]
  }


  Get_Data_Cumpleanos.$inject = ['$http', '$q', 'AuthSvc'];
  function Get_Data_Cumpleanos($http, $q, AuthSvc) {
    
    var deferred = $q.defer();
    var DBName = AuthSvc.data.DBName.trim();
    var url = ROOTURL + 'API/Cumpleanos/' + DBName.trim() + '/Data';
    $http.get(url)
      .success(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
  }

  Get_Empresas_Usuarios.$inject = ['$q', 'Certifica2Svc'];
  function Get_Empresas_Usuarios($q, Certifica2Svc) {
    
    var deferred = $q.defer();
    Certifica2Svc.Get_Empresas_Usuarios()
      .then(function (data) {
        deferred.resolve(data);
      })
    return deferred.promise;
  }

})();
