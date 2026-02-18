(function() {
  'use strict';
  angular.module('app.certificados').factory('CertificadosSvc', CertificadosSvc);

  CertificadosSvc.$inject = ['$http', '$q', 'AuthSvc'];
  /* @ngInject */
  function CertificadosSvc($http, $q, AuthSvc) {
    var service = {
      generateCompropago: generateCompropago,
      generateCertlab: generateCertlab,
      generateCustomCertlab: generateCustomCertlab,
      generateRetefuente: generateRetefuente,
      generateCustomRetefuente: generateCustomRetefuente,
      getCompropagoConfig: getCompropagoConfig,
      getCompropagoConfigByEmpresa: getCompropagoConfigByEmpresa,
      getCertLabConfig: getCertLabConfig,
      getDefaultCertLabConfig: getDefaultCertLabConfig,
      getRetefuenteConfig: getRetefuenteConfig,
      getEmpresas: getEmpresas,
      SetLatsFecNomina: SetLatsFecNomina,
      SendMailCompropago: SendMailCompropago,
      Load_Empleados_Compropago: Load_Empleados_Compropago,
      Load_Empleados_Retefuente: Load_Empleados_Retefuente,
      Generate_Admin_Compropago: Generate_Admin_Compropago,
      Search_Certlab: Search_Certlab,
      Generate_SRI_Retefuente: Generate_SRI_Retefuente
    };

    return service;

    function generateCompropago(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'PDF/CompropagoNew';
      var Empresa = AuthSvc.data.DBName;
      data.DBName = Empresa;
      $http
        .post(url, data, {responseType: 'arraybuffer'})
        .success(function(data) {
          var file = new Blob([data], {type: 'application/pdf'});
          saveAs(file, 'Comprobante_De_Pago.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function Generate_Admin_Compropago(Empresa, data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'PDF/CompropagoZip';
      data.DBName = Empresa;
      var filaname = data.DBName.trim();
      $http
        .post(url, data, {responseType: 'arraybuffer'})
        .success(function(data) {
          var file = new Blob([data], {type: 'application/octet-stream'});
          saveAs(file, filaname + '.zip');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function generateCertlab(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/certlaboral/';
      data.DBName = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function(empleado) {
        data.empleado = empleado;
        $http
          .post(url, data, {responseType: 'arraybuffer'})
          .success(function(data) {
            var file = new Blob([data], {type: 'application/pdf'});
            saveAs(file, 'Certificado_Laboral.pdf');
            deferred.resolve();
          })
          .error(function(data) {
            deferred.reject(data);
          });
      });
      return deferred.promise;
    }

    function generateCustomCertlab(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/certlaboral/';
      $http
        .post(url, data, {responseType: 'arraybuffer'})
        .success(function(data) {
          var file = new Blob([data], {type: 'application/pdf'});
          saveAs(file, 'Certificado_Laboral.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function generateRetefuente(data, Empresa) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/retefuente/';
      data.dbname = AuthSvc.data.DBName;
      $http
        .post(url, data, {responseType: 'arraybuffer'})
        .success(function(data) {
          var file = new Blob([data], {type: 'application/pdf'});
          saveAs(file, 'Retefuente.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function generateCustomRetefuente(data) {
      var deferred = $q.defer();
      var url = ROOTURL + 'pdf/retefuente/';
      $http
        .post(url, data, {responseType: 'arraybuffer'})
        .success(function(data) {
          var file = new Blob([data], {type: 'application/pdf'});
          saveAs(file, 'Retefuente.pdf');
          deferred.resolve();
        })
        .error(function(data) {
          deferred.reject(data);
        });
      return deferred.promise;
    }

    function getCompropagoConfig() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      $http.get(ROOTURL + 'API/Config/' + Empresa.trim() + '/Compropago').success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function SetLatsFecNomina(Empresa, Fec_Nomina) {
      var deferred = $q.defer();
      AuthSvc.getEmpleado().then(function(empleado) {
        var url = ROOTURL + 'API/Config/' + Empresa.trim() + '/' + empleado.Cod_Empleado + '/Compropago/AprobarUltPago';
        $http.post(url, Fec_Nomina).success(function(data) {
          deferred.resolve(data);
        });
      });
      return deferred.promise;
    }

    function getCompropagoConfigByEmpresa(Empresa) {
      var deferred = $q.defer();
      $http.get(ROOTURL + 'API/Config/' + Empresa.trim() + '/Compropago').success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function getCertLabConfig() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      AuthSvc.getEmpleado().then(function(empleado) {
        $http.get(ROOTURL + 'API/Config/Certlab/' + Empresa.trim() + '/' + empleado.Cod_Empleado).success(function(data) {
          deferred.resolve(data);
        });
      });
      return deferred.promise;
    }

    function getDefaultCertLabConfig(Empresa) {
      var deferred = $q.defer();

      $http.get(ROOTURL + 'API/Config/Certlab/Default/' + Empresa).success(function(data) {
        deferred.resolve(data);
      });

      return deferred.promise;
    }

    function getRetefuenteConfig() {
      var deferred = $q.defer();
      var Empresa = AuthSvc.data.DBName;
      $http.get(ROOTURL + 'API/Config/' + Empresa.trim() + '/Retefuente').success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function getEmpresas() {
      var deferred = $q.defer();
      var Cedula = AuthSvc.data.UserName;
      $http.get(ROOTURL + 'API/Empresas/Usuarios/' + Cedula).success(function(data) {
        deferred.resolve(data);
      });
      return deferred.promise;
    }

    function SendMailCompropago(Empresa, Fec_Nomina, empleados) {
      var deferred = $q.defer();
      $http
        .post(ROOTURL + 'API/SendMailCompropago/' + Empresa.trim() + '/' + Fec_Nomina, empleados)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function() {
          deferred.reject();
        });
      return deferred.promise;
    }

    function Load_Empleados_Compropago(Empresa, ano, mes, quincena) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Empresas/' + Empresa.trim() + '/Empleados/compropago/' + ano + '/' + mes + '/' + quincena;
      $http
        .get(url)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function() {
          deferred.reject();
        });
      return deferred.promise;
    }

    function Load_Empleados_Retefuente(Empresa, ano) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Empresas/' + Empresa.trim() + '/Empleados/Retefuente/' + ano;
      $http
        .get(url)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function() {
          deferred.reject();
        });
      return deferred.promise;
    }

    function Search_Certlab(search) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Admin/Search/Certlab/' + search;
      $http
        .get(url)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function() {
          deferred.reject();
        });
      return deferred.promise;
    }

    function Generate_SRI_Retefuente(ano) {
      var deferred = $q.defer();
      var url = ROOTURL + 'API/Admin/Search/Certlab/' + search;
      $http
        .get(url)
        .success(function(data) {
          deferred.resolve(data);
        })
        .error(function() {
          deferred.reject();
        });
      return deferred.promise;
    }
  }
})();
