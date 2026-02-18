 (function (window) {
  'use strict';

  angular
    .module('app.turnos')
    .factory('TurnosSvc', TurnosSvc);

  TurnosSvc.$inject = ['$http', '$q', '$state', '$resource', 'AuthSvc','NotifySvc'];
  function TurnosSvc($http, $q, $state ,$resource, AuthSvc, NotifySvc) {

    var service = {
      AddDataTurnos: AddDataTurnos,
      GetDataPorDepartamento: GetDataPorDepartamento,
      getTimeControls: getTimeControls,
      registerPunch: registerPunch
    };

    window.getTimeControls = getTimeControls;

    return service;

    function AddDataTurnos(data) {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DbName;
      var url = ROOTURL + 'API/Turnos/' + Empresa.trim() + '/Add_Turno';
      $http.put(url, data)
        .success(function (data) {
          NotifySvc.success();
          deferred.resolve(data);
          $state.go($state.current, {}, { reload: true });
        })
        .error(function (error) {
          NotifySvc.error(error.Message);
          deferred.reject();
        });
      return deferred.promise;
    }

    function GetDataPorDepartamento(Cod_Depto) {      
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Turnos/' + Cod_Depto + '/DataPorDepto';
      debugger
      $http.get(url, Cod_Depto)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function (error) {
          NotifySvc.error(error.Message);
          deferred.reject();
        });
      return deferred.promise;
    }

    function getTimeControls(filters) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Turnos/time-controls';

      filters = { ...filters, business: AuthSvc.data.DBName }

      $http.put(url, filters)
        .success(function (punches) {
          deferred.resolve(punches);
        })
        .error(function (error) {
          deferred.reject(error);
        });
      return deferred.promise;
    }

    function registerPunch(payload) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Turnos/register-punch';

      payload = { ...payload, business: AuthSvc.data.DBName }

      $http.put(url, payload)
        .success(function (punches) {
          deferred.resolve(punches);
        })
        .error(function (error) {
          deferred.reject(error);
        });
      return deferred.promise;
    }

  }
})(window);
