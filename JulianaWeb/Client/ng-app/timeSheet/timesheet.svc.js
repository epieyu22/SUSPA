(function () {
  'use strict';

  angular
    .module('app.timesheet')
    .factory('TimeSheetSvc', TimeSheetSvc);

  TimeSheetSvc.$inject = ['$http', '$q','$resource','AuthSvc'];
  function TimeSheetSvc($http, $q, $resource,  AuthSvc) {
    
    var service = {
      AddRegistroHoras: AddRegistroHoras
    };

    return service;

    //Funciones API

    function AddRegistroHoras(data) {
      debugger
      var deferred = $q.defer();
      var Cedula = AuthSvc.data.UserName;
      var url = ROOTURL + 'API/TimeSheet/' + Cedula + '/AgregarRegHoras';
      $http.put(url, data)
        .success(function () {
          deferred.resolve();
        })
        .error(function (error) {
          NotifySvc.error(error.Message);
          deferred.reject();
        });
      return deferred.promise;
    }
  }
})();
