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
        state: 'vacaciones',
        config: {
          url: '/Vacaciones',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@vacaciones": {
              templateUrl: 'Client/ng-app/vacaciones/vacaciones.html',
              controller: 'VacacionesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            data: getVacaciones
          }
        }
      },
      {
        state: 'report',
        config: {
          url: '/Report/Vacaciones',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@report": {
              templateUrl: 'Client/ng-app/vacaciones/report.html',
              controller: 'ReportVacacionesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            Empresas: Get_Empresas_Usuarios
          }
        }
      },
      {
        state: 'calendario_vacaciones',
        config: {
          url: '/Calendario/Vacaciones',
          views: {
            "":{
              templateUrl: 'Client/ng-app/vacaciones/calendario.html',
              controller: 'Calendario2Ctrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            Empresas: Get_Empresas_Usuarios
          }
        }
      },
      {
        state: 'superior',
        config: {
          url: '/Soliciudes/Admin/Ingresar',
          views: {
            "":{
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@superior": {
              templateUrl: 'Client/ng-app/vacaciones/ingresarsolicitud.html',
              controller: 'VacacionesCtrl',
              controllerAs: 'vm'
            }
          },
          resolve:{
            Empresas: Get_Empresas_Usuarios
          }
        }
      }
    ];
  }

getVacaciones.$inject = ['$http', '$q', 'AuthSvc'];
function getVacaciones($http, $q, AuthSvc) {
  var deferred = $q.defer();
  AuthSvc.getEmpleado().then(function(empleado){
      var DBName = AuthSvc.data.DBName;
    var url = ROOTURL + 'API/Vacaciones/' + DBName.trim() + '/' + empleado.Cod_Empleado + '/';
      $http.get(url)
        .success(function(data){
          deferred.resolve(data);
        });
    });
    return deferred.promise;
  }


// Get the Empresasby Usuario
Get_Empresas_Usuarios.$inject = ['$q', 'AuthSvc'];
function Get_Empresas_Usuarios($q, AuthSvc){
  var deferred = $q.defer();
    AuthSvc
    .Get_Empresas_Usuarios()
    .then(function(data){
      deferred.resolve(data);
    });
    return deferred.promise;
}

})();




