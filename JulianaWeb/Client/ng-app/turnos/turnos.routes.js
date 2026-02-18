(function () {
  'use strict';
  angular.module('app.turnos').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [

     {
      state: 'turnos',
      config: {
        url: '/Turnos',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@turnos": {
            templateUrl: 'Client/ng-app/turnos/turnos.html',
            controller: 'TurnosCtrl',
            controllerAs: 'vm'
          }
        },
        resolve: {
          data: Get_Data_Turnos,
        }
      }
    },
      {
        state: 'turnos.punch',
        config: {
          url: '/punch',
          views: {
            "": {
              templateUrl: 'Client/ng-app/layout/layout.html',
              controller: 'LayoutCtrl',
              controllerAs: 'vm'
            },
            "MainContent@turnos": {
              templateUrl: 'Client/ng-app/turnos/pages/punch-manager/punch-manager.page.html',
              controller: 'punchManagerController',
              controllerAs: 'vm'
            }
          }
        }
      }]
  }


  Get_Empresas_Empleados.$inject = ['$q', 'AuthSvc'];
  function Get_Empresas_Empleados($q, AuthSvc) {
    var deferred = $q.defer();
    AuthSvc
      .Get_Empleados_Empresa()
      .then(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
  }

  Get_Data_Turnos.$inject = ['$http', '$q', 'AuthSvc'];
  function Get_Data_Turnos($http, $q, AuthSvc) {
    var deferred = $q.defer();
    var Usuario = AuthSvc.data.UserName;
    var url = ROOTURL + 'API/Turnos/' + Usuario + '/Data';
    $http.get(url)
      .success(function (data) {
        deferred.resolve(data);
      });
    return deferred.promise;
  }



})();
