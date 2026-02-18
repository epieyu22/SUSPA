(function () {
  'use strict';
  angular.module('app.timesheet').run(appRun);

  appRun.$inject = ['routerHelper'];
  /* @ngInject */
  function appRun(routerHelper) {
    routerHelper.configureStates(getStates());
  }

  function getStates() {
    return [{
      state: 'timesheet',
      config: {
        url: '/TimeSheet-Report',
        views: {
          "": {
            templateUrl: 'Client/ng-app/layout/layout.html',
            controller: 'LayoutCtrl',
            controllerAs: 'vm'
          },
          "MainContent@timesheet": {
            templateUrl: 'Client/ng-app/timeSheet/timesheet.html',
            controller: 'TimeSheetCtrl',
            controllerAs: 'vm'
          }
        },
        resolve: {
          data: Get_Data_TimeSheet,
          Empresas: Get_Empresas_Usuarios
        }
      }
    }]
  }


  Get_Data_TimeSheet.$inject = ['$http', '$q', 'AuthSvc'];
  function Get_Data_TimeSheet($http, $q, AuthSvc) {
    var deferred = $q.defer();
    var DBName = AuthSvc.data.DBName;
    var url = ROOTURL + 'API/TimeSheet/' + DBName.trim() + '/Data';
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
