(function() {
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
        state: 'histosalario',
        config: {
          url: '/HistoricoSalario',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@histosalario": {
              templateUrl: 'Client/ng-app/consultas/histosalario.html',
              controller: 'ConsultasCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getHistoSalario
          }
        }
      },
      {
        state: 'segsocial',
        config: {
          url: '/SeguridadSocial',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@segsocial": {
              templateUrl: 'Client/ng-app/consultas/segsocial.html',
              controller: 'ConsultasCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getSegsocial
          }
        }
      }
    ];
  }


  getSegsocial.$inject = ['$http', '$q', 'AuthSvc'];
  function getSegsocial($http, $q, AuthSvc) {
    var deferred = $q.defer();
     AuthSvc.getEmpleado().then(function(empleado){
      var DBName = AuthSvc.data.DBName;
       $http.get(ROOTURL + 'API/Empleados/'+DBName.trim()+'/'+empleado.Cod_Empleado+'/Fondos')
        .success(function(data){
          deferred.resolve(data);
        });
     });
     return deferred.promise;
  }

  getHistoSalario.$inject = ['$http', '$q', 'AuthSvc'];
  function getHistoSalario($http, $q, AuthSvc) {
    var deferred = $q.defer();
     AuthSvc.getEmpleado().then(function(empleado){
      var DBName = AuthSvc.data.DBName;
       $http.get(ROOTURL + 'API/histosalario/'+DBName.trim()+'/'+empleado.Cod_Empleado)
        .success(function(data){
          deferred.resolve(data);
        });
     });
     return deferred.promise;
 }

})();
