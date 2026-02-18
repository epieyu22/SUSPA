(function() {
  'use strict';

  angular.module('app.certificados').factory('Certifica2Svc', Certifica2Svc);

  Certifica2Svc.$inject = ['$http', '$q', 'AuthSvc', 'ngDialog'];

  function Certifica2Svc($http, $q, AuthSvc, ngDialog) {
    var service = {
      Get_Empleados_Retefuente_Ano: Get_Empleados_Retefuente_Ano,
      Get_Empresas_Usuarios: Get_Empresas_Usuarios,
      Get_Config_Retefuente: Get_Config_Retefuente,
      Generate_Retefuente: Generate_Retefuente,
      Solicitud_Certificdo: Solicitud_Certificdo,
      Generate_SRI_Retefuente: Generate_SRI_Retefuente,
      generateCertlab: generateCertlab,
      generateCertlabMasivo: generateCertlabMasivo,
      Generate_Politicas: Generate_Politicas
    };

    return service;

    ////////////////
    function Get_Config_Retefuente(Empresa) {
      var url = ROOTURL + 'API/Admin/ParametrosWeb/' + Empresa;
      var deferred = $q.defer();
      $http.get(url).success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function Get_Empleados_Retefuente_Ano(Empresa, Ano) {
      var url = ROOTURL + 'API/EMPRESAS/' + Empresa.trim() + '/Empleados/Retefuente/' + Ano;
      var deferred = $q.defer();
      $http.get(url).success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function Get_Empresas_Usuarios() {
      var deferred = $q.defer();
      var usuario = AuthSvc.data.UserName;
      var url = ROOTURL + 'API/Empresas/Usuarios/' + usuario;
      $http.get(url).success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function Generate_Retefuente2(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/retefuente/';
      $http
        .post(url, data, {
          responseType: 'arraybuffer'
        })
        .success(function(data) {
          var file = new Blob([data], {
            type: 'application/pdf'
          });
          saveAs(file, 'Retefuente.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function Generate_Retefuente(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/RetefuenteZip/';
      var filaname = data.DBName.trim();
      $http
        .post(url, data, {
          responseType: 'arraybuffer'
        })
        .success(function(data) {
          var file = new Blob([data], {
            type: 'application/octet-stream'
          });
          saveAs(file, filaname + '.zip');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function Generate_SRI_Retefuente(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/SRIRetefuente/';
      $http
        .post(url, data, {
          responseType: 'arraybuffer'
        })
        .success(function(data) {
          var file = new Blob([data], {
            type: 'application/pdf'
          });
          saveAs(file, 'SRI_Formulario_107.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function Solicitud_Certificdo(viewmodel) {
      var Cedula = AuthSvc.data.UserName;
      var Empresa = AuthSvc.data.DBName;
      var url = ROOTURL + 'API/Admin/Solicitud_Certificado_Laboral/' + Empresa.trim() + '/' + Cedula;
      var deferred = $q.defer();
      $http
        .post(url, {
          mensaje: viewmodel
        })
        .success(function(data) {
          deferred.resolve(data);
        });
      return deferred.promise;
    }

    function Generate_Politicas(model) {
      debugger
      var deferred = $q.defer();
      var url = ROOTURL + 'GenerarCertlaboral/Politicas';
      model.DBName = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function (empleado) {
        model.empleado = empleado;
        $http
          .post(url, model, {
            responseType: 'arraybuffer'
          })
          .success(function (data) {           
            var file = new Blob([data], {
              type: 'application/pdf'
            });
            saveAs(file, 'politicas.pdf');
            deferred.resolve();
          })
          .error(function (data) {
            deferred.reject(data);
          });
      });
      return deferred.promise;

    }

    function generateCertlab(model) {
      var deferred = $q.defer();
      var url = ROOTURL + 'GenerarCertlaboral/Prueba2';
      model.DBName = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function(empleado) {
        model.empleado = empleado;
        $http
          .post(url, model, {
            responseType: 'arraybuffer'
          })
          .success(function(data) {
            if (model.Aprobacion) {
              ngDialog
                .openConfirm({
                  template:
                    '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Solicitud Enviada Con Exito</h4></div>    \
                <div class="col-md-12 m-t-2"> \
                <p> La solicitud ha sido enviada con exito será avisado cuando se de respuesta.</p>\
                </div></div> \
            <div class="text-right">\
              <button class="btn btn-primary" ng-click="confirm()">Aceptar</button>\
            <div>\
        </div>',
                  plain: true,
                  className: 'ngdialog-theme-plain',
                  appendClassName: 'ngdialog-systems-theme'
                })
                .then(function() {
                  $state.go(
                    $state.current,
                    {},
                    {
                      reload: true
                    }
                  );
                });
              deferred.resolve();
              return;
            }
            var file = new Blob([data], {
              type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'
            });
            saveAs(file, 'Certificado_Laboral.pdf');
            deferred.resolve();
          })
          .error(function(data) {
            deferred.reject(data);
          });
      });
      return deferred.promise;
    }

    function generateCertlabMasivo(model) {
      debugger
      var deferred = $q.defer();
      var url = ROOTURL + 'GenerarCertlaboral/Prueba2';
      // model.DBName = AuthSvc.data.DBName;
      $http
        .post(url, model, {
          responseType: 'arraybuffer'
        })
        .success(function(data) {
          if (model.Aprobacion) {
            ngDialog
              .openConfirm({
                template:
                  '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>Solicitud Enviada Con Exito</h4></div>    \
                <div class="col-md-12 m-t-2"> \
                <p> La solicitud ha sido enviada con exito será avisado cuando se de respuesta.</p>\
                </div></div> \
            <div class="text-right">\
              <button class="btn btn-primary" ng-click="confirm()">Aceptar</button>\
            <div>\
        </div>',
                plain: true,
                className: 'ngdialog-theme-plain',
                appendClassName: 'ngdialog-systems-theme'
              })
              .then(function() {
                $state.go(
                  $state.current,
                  {},
                  {
                    reload: true
                  }
                );
              });
            deferred.resolve();
            return;
          }
          var file = new Blob([data], {
            type: 'application/pdf'
          });
          saveAs(file, 'Certificado_Laboral.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }
  }
})();
