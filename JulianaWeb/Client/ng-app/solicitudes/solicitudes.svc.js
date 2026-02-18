(function() {
  'use strict';
  angular.module('app.solicitudes').factory('SolicitudesSvc', SolicitudesSvc);

  SolicitudesSvc.$inject = ['$http', '$q', '$state', 'NotifySvc', 'AuthSvc'];
  /* @ngInject */

  function SolicitudesSvc($http, $q, $state, NotifySvc, AuthSvc) {
    var service = {
      HOLIDAYS: [],
      checkDate: checkDate,
      getFestivos: getFestivos,
      getSolicitudes: getSolicitudes,
      getSolicitudesAprobador: getSolicitudesAprobador,
      getCesantiasData: getCesantiasData,
      sendSolicitudVacaciones: sendSolicitudVacaciones,
      Aprobar_Solicitud: Aprobar_Solicitud,
      Rechazar_Solicitud: Rechazar_Solicitud_Empresa,
      Verificar_Solicitud_Rechazable: Verificar_Solicitud_Rechazable,
      Load_Motivos_Rechazo: Load_Motivos_Rechazo,
      Delete_Solicitud: Delete_Solicitud,
      Get_Solicitud_Data: Get_Solicitud_Data,
      Get_All_Solicitudes: Get_All_Solicitudes,
      Exportar_Excel: Exportar_Excel,
      Decargar_PDF_Solicitud: Decargar_PDF_Solicitud,
      PD_solicitud: PD_solicitud,
      sendSolicitudCesantias: sendSolicitudCesantias,
      SolicitudEmpresaData: SolicitudEmpresaData,
      Aprobar_Solicitud_Empresa: Aprobar_Solicitud_Empresa
    };
    activate();

    return service;

    function getSolicitudes() {
      var deferred = $q.defer();
      AuthSvc.theme = 'Vacaciones';
    
      AuthSvc.getEmpleado().then(function (empleado) {
        if (empleado) {
          var Empresa = AuthSvc.data.DBName;
          var url = ROOTURL + 'API/Solicitudes/Empleados/' + Empresa.trim() + '/' + empleado.Cod_Empleado + '/';
          $http
            .get(url)
            .success(function(data) {
              deferred.resolve(data);
            })
            .error(function(status, error) {
              console.log(status, error);
              deferred.reject(status, error);
            });
        }
      });
      return deferred.promise;
    }

    function getSolicitudesAprobador() {
      var deferred = $q.defer();
      AuthSvc.theme = 'Vacaciones';
      AuthSvc.getEmpleado().then(function (empleado) {
        if (empleado) {
        var Empresa = AuthSvc.data.DBName;        
        var url = ROOTURL + 'API/Solicitudes/Pendientes/Aprobador/' + Empresa.trim() + '/' + empleado.Cedula.trim() + '/';
        $http
          .get(url)
          .success(function(data) {
            deferred.resolve(data);
          })
          .error(function(status, error) {
            console.log(status, error);
            deferred.resolve([]);
          });
        }
      });
      return deferred.promise;
    }

    function getCesantiasData() {
      var deferred = $q.defer();
      var DBName = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function (empleado) {
        if (empleado) {

          var url = ROOTURL + 'API/Cesantias/' + DBName.trim() + '/' + empleado.Cod_Empleado + '/';
          console.log(url);
          $http
            .get(url)
            .success(function(data) {
              deferred.resolve(data);
            })
            .error(function(err, status) {
              deferred.reject();
            });
        }
      });
      return deferred.promise;
    }

    function checkDate(date) {
      if ($.inArray(date.getTime(), SolicitudesSvc.HOLIDAYS) > -1) {
        return true;
      } else {
        var day = date.getDay();
        if (AuthSvc.data.Empleado.Sabado == '2') {
          return day == 0;
        } else {
          return day == 0 || day == 6;
        }
        return false;
      }
    }

    function sendSolicitudVacaciones(viewmodel) {
      var deferred = $q.defer();
      var Cod_Empleado = AuthSvc.empleado.Cod_Empleado;
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Solicitudes/' + Empresa.trim() + '/' + Cod_Empleado + '/';
      viewmodel.Tipo_Solicitud = 'V';
      viewmodel.Cod_Empleado = Cod_Empleado;
      $http
        .post(url, viewmodel)
        .success(function(data) {
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );
          NotifySvc.success();
          deferred.resolve(data);
        })
        .error(function(error, status) {
          NotifySvc.error(error.Message);
          deferred.reject();
        });
      return deferred.promise;
    }

    function sendSolicitudCesantias(viewmodel) {
      var deferred = $q.defer();
      var Cod_Empleado = AuthSvc.empleado.Cod_Empleado;
      var Empresa = AuthSvc.data.DBName;

      var url = ROOTURL + 'API/Solicitudes/Cesantias/' + Empresa.trim() + '/' + Cod_Empleado + '/';
      viewmodel.Tipo_Solicitud = 'C';
      viewmodel.Cod_Empleado = Cod_Empleado;
      $http
        .post(url, viewmodel)
        .success(function(data) {
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );
          NotifySvc.success();
          deferred.resolve(data);
        })
        .error(function(error, status) {
          deferred.reject();
        });
      return deferred.promise;
    }

    function Get_Solicitud_Data(Cod_Solicitud) {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Solicitudes/Solicitud/' + Empresa.trim() + '/' + Cod_Solicitud;
      $http
        .get(url)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function(status, error) {
          // deferred.reject(status, error);
          deferred.resolve([]);
        });
      return deferred.promise;
    }

    function SolicitudEmpresaData(Empresa, Cod_Solicitud) {
      var deferred = $q.defer();
      // var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Solicitudes/Solicitud/' + Empresa.trim() + '/' + Cod_Solicitud;
      $http
        .get(url)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function(status, error) {
          // deferred.reject(status, error);
          deferred.resolve([]);
        });
      return deferred.promise;
    }

    function Exportar_Excel() {
      var deferred = $q.defer();
      var url = ROOTURL + 'Excel/Solicitudes/';
      $http
        .post(url, null, { responseType: 'arraybuffer' })
        .success(function(data) {
          var file = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          saveAs(file, 'Solicitudes.xlsx');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function activate() {
      // getFestivos().then(function(data) {
      //   for (var i = data.length - 1; i >= 0; i--) {
      //     var date = new Date(data[i]);
      //     date.setHours(date.getHours() + date.getTimezoneOffset() / 60);
      //     service.HOLIDAYS.push(date.getTime());
      //   }
      // });
    }

    function getFestivos() {
      var deferred = $q.defer();
      var year = new Date().getFullYear();
      var url = ROOTURL + 'API/Festivos/' + year;
      $http.get(url).success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function Aprobar_Solicitud(Cod_Solicitud) {
      var deferred = $q.defer();
      var Cod_Empleado = AuthSvc.empleado.Cod_Empleado;
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Solicitudes/' + Empresa.trim() + '/' + Cod_Empleado + '/' + Cod_Solicitud + '/Aprobar';
      $http
        .post(url)
        .success(function(data) {
          deferred.resolve(data);
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );
          NotifySvc.success();
        })
        .error(function(error, status) {
          NotifySvc.error(error.Message);
          deferred.reject();
        });
      return deferred.promise;
    }

    function Aprobar_Solicitud_Empresa(Empresa, Cod_Solicitud) {
      var deferred = $q.defer();
      var Cod_Empleado = AuthSvc.empleado.Cod_Empleado;
      /*var Empresa = AuthSvc.data.DBName;*/
      var url = ROOTURL + 'API/Solicitudes/' + Empresa.trim() + '/' + Cod_Empleado + '/' + Cod_Solicitud + '/Aprobar';
      $http
        .post(url)
        .success(function(data) {
          deferred.resolve(data);
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );
          NotifySvc.success(data.message);
        })
        .error(function(error, status) {
          NotifySvc.error(error.Message);
          deferred.reject();
        });
      return deferred.promise;
    }

    function Rechazar_Solicitud_Empresa(solicitud, empresa) {
      var deferred = $q.defer();
      // Usa la empresa del usuario sino se provee la del parametro
      var Empresa = empresa || AuthSvc.data.DBName;
      var Cod_Empleado = AuthSvc.empleado.Cod_Empleado;
      var Cedula = AuthSvc.empleado.Cedula;
      var url = ROOTURL + 'API/Solicitudes/' + Empresa.trim() + '/' + Cod_Empleado + '/' + solicitud.Cod_Solicitud + '/Rechazar' + "?Cedula=" + Cedula;
      $http
        .post(url, solicitud)
        .success(function(data) {
          deferred.resolve(data);
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );

          NotifySvc.success(data.message);
        })
        .error(function(status, error) {
          deferred.reject(status, error);
        });
      return deferred.promise;
    }

    function Verificar_Solicitud_Rechazable(Cod_Solicitud, empresa) {
      var deferred = $q.defer();
      // Usa la empresa del usuario sino se provee la del parametro
      var Empresa = empresa || AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Solicitudes/' + Empresa.trim() + '/' + Cod_Solicitud + '/esRechazable';
      $http
        .get(url)
        .success(function (data) {
          deferred.resolve(data);
          $state.go(
            $state.current,
            {},
            {
              reload: true
            }
          );

          switch (data.status) {
            case "BAD":
              NotifySvc.error(data.message);
              deferred.reject(status, data);
              break;
          }
          
        })
        .error(function (status, error) {
          deferred.reject(status, error);
        });
      return deferred.promise;
    }

    function Load_Motivos_Rechazo() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      $http.get(ROOTURL + 'API/MotivosRechazo/' + Empresa).success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function Delete_Solicitud(Cod_Solicitud, mostrarNotificacion = true) {
      var deferred = $q.defer();
      var DBName = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function (empleado) {
        if (empleado) {
          var url = ROOTURL + 'API/Solicitudes/' + DBName.trim() + '/' + empleado.Cod_Empleado + '/Borrar/' + Cod_Solicitud;
          $http.get(url).success(function (data) {
            if (mostrarNotificacion)
              NotifySvc.success();
            deferred.resolve(data);
          });
        }
      });
      return deferred.promise;
    }

    function PD_solicitud(Cod_Solicitud) {
      var deferred = $q.defer();
      var DBName = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function (empleado) {
        if (empleado) {
          var url = ROOTURL + 'API/Solicitudes/PedidoDesaprobacion/' + DBName.trim() + '/' + Cod_Solicitud;
          $http.post(url).success(function(data) {
            NotifySvc.success();
            deferred.resolve(data);
          });
        }
      });
      return deferred.promise;
    }

    function Get_All_Solicitudes() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function (empleado) {
      if (empleado) {
        var url = ROOTURL + 'API/Solicitudes/' + Empresa.trim() + '/';
        $http.get(url).success(function(data) {
          deferred.resolve(data);
        });
      }
      });
      return deferred.promise;
    }

    function Decargar_PDF_Solicitud(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/Solicitud_Vacaciones/';
      $http
        .post(url, data, { responseType: 'arraybuffer' })
        .success(function(res) {
          var file = new Blob([res], { type: 'application/pdf' });
          saveAs(file, 'solicitud.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }
  }
})();
