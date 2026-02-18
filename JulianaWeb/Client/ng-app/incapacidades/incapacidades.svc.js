(function () {
  'use strict';

  angular
    .module('app.incapacidades')
    .factory('IncapacidadesSvc', IncapacidadesSvc);

  IncapacidadesSvc.$inject = ['$http', '$q', 'NotifySvc', 'AuthSvc'];

  function IncapacidadesSvc($http, $q, NotifySvc, AuthSvc) {
    var service = {
      Get_Conceptos_Incapacidades: Get_Conceptos_Incapacidades,
      Get_Historico_Empleado: Get_Historico_Empleado,
      New_Incapacidad: New_Incapacidad,
      changeImage: changeImage,
      changeImageLicenciaDeLuto: changeImageLicenciaDeLuto,
      Get_Conceptos_Licencias: Get_Conceptos_Licencias,
      Cargar_Historico_Licencias: Cargar_Historico_Licencias,
      Nueva_Licencia: Nueva_Licencia,
      DeleteIncapacidad: DeleteIncapacidad
    };

    return service;

    ////////////////
    function Get_Conceptos_Incapacidades() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Incapacidades/PreloadData/Incapacidades/' + Empresa;
      $http
        .get(url)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function () {
          NotifySvc.error();
        });
      return deferred.promise;
    }

    function Get_Conceptos_Licencias() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Incapacidades/PreloadData/Licencias/' + Empresa;
      $http
        .get(url)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function () {
          NotifySvc.error();
        });
      return deferred.promise;
    }

    function Get_Historico_Empleado() {
      var deferred = $q.defer();
      AuthSvc.getEmpleado().then(function (empleado) {
        var Empresa = AuthSvc.data.DBName;
        var url = ROOTURL + 'API/Incapacidades/' + Empresa.trim() + '/' + empleado.Cod_Empleado;
        $http.get(url)
          .success(function (data) {
            deferred.resolve(data);
          });
      });
      return deferred.promise;
    }


    function New_Incapacidad(data) {
      var deferred = $q.defer();
      AuthSvc.getEmpleado().then(function (empleado) {
        var Empresa = AuthSvc.data.DBName;
        var url = ROOTURL + 'API/Incapacidades/' + Empresa.trim() + '/' + empleado.Cod_Empleado;
        $http
          .post(url, data)
          .success(function (data) {
            deferred.resolve(data);
          })
          .error(function (error) {
            NotifySvc.error(error.Message);
            deferred.reject();
          });
      });
      return deferred.promise;
    }

    function Nueva_Licencia(data) {
      var deferred = $q.defer();
      AuthSvc.getEmpleado().then(function (empleado) {
        var Empresa = AuthSvc.data.DBName;
        var url = ROOTURL + 'API/Incapacidades/' + Empresa.trim() + '/' + empleado.Cod_Empleado;
        $http
          .post(url, data)
          .success(function (data) {
            NotifySvc.success();
            deferred.resolve(data);
          })
          .error(function (error) {
            NotifySvc.error(error.Message);
            deferred.reject();
          });
      });
      return deferred.promise;
    }


    function Cargar_Historico_Licencias() {
      var deferred = $q.defer();
      AuthSvc.getEmpleado().then(function (empleado) {
        var Empresa = AuthSvc.data.DBName;
        var url = ROOTURL + 'API/Incapacidades/Licencias/' + Empresa.trim() + '/' + empleado.Cod_Empleado;
        $http.get(url)
          .success(function (data) {
            deferred.resolve(data);
          });
      });
      return deferred.promise;
    }

    function changeImage(Cod_Incapacidad, file) {
      var deferred = $q.defer();
      var fd = new FormData();
      fd.append('file', file);
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Incapacidades/Adjunto/' + Empresa.trim() + '/' + Cod_Incapacidad;
      $http.post(url, fd, {
        headers: {
          'Content-Type': undefined,
          transformRequest: angular.identity,
        }
      })
        .success(function (response) {
          deferred.resolve();
        })
        .error(function (error) {
          deferred.reject();
        });
      return deferred.promise;
    }

    function changeImageLicenciaDeLuto(Cod_Solicitud, file) {
      var deferred = $q.defer();
      fd.append('file', file);
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Incapacidades/Adjunto/LicenciaDeLuto/' + Empresa.trim() + '/' + Cod_Solicitud;
      $http.post(url, fd, {
        headers: {
          'Content-Type': undefined,
          transformRequest: angular.identity,
        }
      })
        .success(function (response) {
          deferred.resolve();
        })
        .error(function (error) {
          deferred.reject();
        });
      return deferred.promise;
    }

    function DeleteIncapacidad(Cod_Incapacidad, mostrarNotificacion = true) {
      var deferred = $q.defer();
      // Usa la empresa del usuario sino se provee la del parametro
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Incapacidades/Delete/' + Empresa.trim() + '/' + Cod_Incapacidad;
      $http
        .post(url, {})
        .success(function (data) {
          switch (data.status) {
            case "BAD":
              if (mostrarNotificacion)
                NotifySvc.error(data.message);
              deferred.reject(status, data);
              break;
            case "OK":
              if (mostrarNotificacion)
                NotifySvc.success(data.message);
              deferred.resolve(data);
              break;
          }
        })
        .error(function (status, error) {
          deferred.reject(status, error);
        });
      return deferred.promise;
    }
  }
})();
