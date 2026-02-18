(function() {
  'use strict';

  angular
    .module('app.certificados')
    .controller('CertlaboralCtrl', CertlaboralCtrl)
    .filter('trustAsHTML', [
      '$sce',
      function($sce) {
        return function(text) {
          return $sce.trustAsHtml(text);
        };
      }
    ]);

  CertlaboralCtrl.$inject = [
    '$rootScope',
    '$http',
    'NotifySvc',
    'AuthSvc',
    'Certifica2Svc',
    'CertificadosSvc',
    '$scope',
    '$location'
  ];

  function CertlaboralCtrl($rootScope, $http, NotifySvc, AuthSvc, Certifica2Svc, CertificadosSvc, $scope, $location) {
    var vm = this;
    vm.loading = true;
    vm.cargarDatos = cargarDatos;
    vm.data = {};
    vm.config = {};

    vm.model = {
      Cod_Plantilla: null
    };

    activate();

    ////////////////

    function activate() {
      vm.cargarDatos();
      vm.UseDirigidoCertificado = SETTINGS.UseDirigidoCertificado === 'true';
      if (!vm.UseDirigidoCertificado) {
        vm.model.dirigido = 'A Quien Interese';
      }
    }

    function cargarDatos() {
      GetPlantillas();
      GetConfigEmpleado();
    }

    //Obtener plantillas 
    function GetPlantillas() {
      

      $scope.ruta = $location.absUrl();
      var Ruta = $scope.ruta;
      var tipo = 'certificados';
      var Empresa = AuthSvc.data.DBName.trim();
   
      if (Ruta.match("otroscert")) {
        tipo = 'otroscert'
      }
      if (Ruta.match("politicas")) {
        tipo = 'politicas'
      }

      $http.get(ROOTURL + 'API/Certificados/Certlaboral/' + Empresa.trim() + '/' + tipo + '/Plantillas').success(function (data) {
        vm.loading = false;
        vm.data = data;
        vm.model.Cod_Plantilla = vm.data.plantillas;
        cambiarPlantilla();
      });
    }

    function GetConfigEmpleado() {
      var Empresa = AuthSvc.data.DBName.trim();
      var Cedula = AuthSvc.data.UserName;

      AuthSvc.getEmpleado().then(function(empleado) {
        var url = ROOTURL + 'API/Certificados/Certlaboral/' + Empresa.trim() + '/' + empleado.Cod_Empleado + '/' + Cedula + '/Config';
        $http.get(url).success(function(data) {
          vm.config = data;
        });
      });
    }

    vm.cambiarPlantilla = cambiarPlantilla;

    function cambiarPlantilla() {
      vm.splantilla = vm.data.plantillas.find(function(f) {
        return f.Autonum == vm.model.Cod_Plantilla;
      });
    }

    vm.generateCertlaboral = function() {
      $rootScope.generating = true;
      vm.model.Aprobacion = vm.splantilla.Aprobacion;
      Certifica2Svc.generateCertlab(vm.model).then(
        function() {
          $rootScope.generating = false;
        },
        function(error) {
          $rootScope.generating = false;
          NotifySvc.error();
        }
      );
    };
    //---------------POLITICAS----------------//
    vm.generatePoliticas = function () {
      $rootScope.generating = true;
      vm.model.Aprobacion = vm.splantilla.Aprobacion;
      Certifica2Svc.Generate_Politicas(vm.model).then(
        function () {
          $rootScope.generating = false;
        },
        function (error) {
          $rootScope.generating = false;
          NotifySvc.error();
        }
      );
    };

    vm.Search_Certlab = function() {
      CertificadosSvc.Search_Certlab(vm.searchFile).then(function(data) {
        if (data.length) {
          window.open(ROOTURL + 'Static/CertificadosBk/' + data + '.pdf', '_blank');
        } else {
          NotifySvc.error('No se ha encontrado un documento con ese Cod. de Verificación');
        }
      });
    };
  }
})();
