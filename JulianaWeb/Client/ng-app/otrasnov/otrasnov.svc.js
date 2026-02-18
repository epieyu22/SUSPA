(function () {
  'use strict';

  angular
    .module('app.otrasnov')
    .factory('OtrasNovSvc', OtrasNovSvc);

  OtrasNovSvc.$inject = ['$http', '$q', 'AuthSvc'];
  function OtrasNovSvc($http, $q, AuthSvc) {
    var service = {
      Get_Conceptos_OtrasNov: Get_Conceptos_OtrasNov,
      Upload_OtrasNov: Upload_OtrasNov,
      Upload_Novedades: Upload_Novedades,
      Get_Registros_Pendientes: Get_Registros_Pendientes
    };

    return service;

    ////////////////
    function Get_Conceptos_OtrasNov(Empresa) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/OtrasNov/' + Empresa.trim() + '/Data';
      $http
        .get(url)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function () {
          // NotifySvc.error();
        });
      return deferred.promise;
    }


    function Upload_OtrasNov(Empresa, excel) {
      var deferred = $q.defer();
      var fd = new FormData();
      fd.append('OtrasNov', excel);
      var url = ROOTURL + 'Excel/Upload_OtrasNov/';
      $http.post(url, fd, { headers: { 'Content-Type': undefined, transformRequest: angular.identity, } })
        .success(function (response) {
          deferred.resolve();
        })
        .error(function (error) {
          deferred.reject();
        });
      return deferred.promise;
    }

    function Upload_Novedades(Empresa, data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/OtrasNov/' + Empresa.trim() + '/upload';
      $http.post(url, data)
        .success(function (response) {
          deferred.resolve();
        })
        .error(function (error) {
          deferred.reject();
        });
      return deferred.promise;
    }


    function Get_Registros_Pendientes(){
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName.trim();
      var url = ROOTURL + 'API/OtrasNov/' + Empresa;
      $http.get(url)
        .success(function (response) {
          deferred.resolve(response);
        })
        .error(function (error) {
          deferred.reject(error);
        });
      return deferred.promise;
    }

  }
})();
