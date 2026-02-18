(function () {
  'use strict';

  angular
    .module('app.facElectronica')
    .factory('facElectronicaSvc', facElectronicaSvc);

  facElectronicaSvc.$inject = ['$http', '$q'];
  function facElectronicaSvc($http, $q) {
    var service = {
      IngresarSolicitud: IngresarSolicitud,
      LoadEmployees: LoadEmployees
    };

    return service;

    ////////////////
    function IngresarSolicitud(viewmodel) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/facElectronica/' + viewmodel.Empresa.trim() + '/DataSend';
      $http
        .put(url, viewmodel)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function (error) {
          deferred.reject(error);
        });
      return deferred.promise;
    }
    function LoadEmployees(viewmodel) {
      var deferred = $q.defer();      
      var url = ROOTURL + 'API/facElectronica/' + viewmodel.Empresa.trim() + '/Employees';
      $http
        .put(url, viewmodel)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function () {
          deferred.reject();
        });
      return deferred.promise;
    }
  }
})();
