(function() {
  'use strict';
  angular.module('app.solicitudes').controller('AprobadoresCtrl', AprobadoresCtrl);

  AprobadoresCtrl.$inject = ['$http', 'NgTableParams', 'SolicitudesSvc', 'AuthSvc', 'Empresas'];

  /* @ngInject */
  function AprobadoresCtrl($http, NgTableParams, SolicitudesSvc, AuthSvc, Empresas) {
    var vm = this;
    vm.loading = true;
    vm.data = {};
    vm.viewmodel = {};
    vm.data.empresas = Empresas;

    activate();

    function activate() {
      if (Empresas.length) {
        vm.viewmodel.Empresa = vm.data.empresas[0].BaseDatos.trim();
        CargarSolicitudes();
      }
    }

    vm.CargarSolicitudes = CargarSolicitudes;
    function CargarSolicitudes() {
      vm.loading = true;

      AuthSvc.getEmpleado().then(function(empleado) {
        var Empresa = vm.viewmodel.Empresa.trim();
        var url = ROOTURL + 'API/Solicitudes/Pendientes/Aprobador/' + Empresa.trim() + '/' + empleado.Cedula.trim() + '/';
        $http
          .get(url)
          .success(function (data) {
            vm.data.vacaciones = data.vacaciones;
            vm.data.licencias = data.licencias;
            vm.solVacacionesGrid = new NgTableParams({}, {dataset: data.vacaciones});
            vm.solCesantiasGrid = new NgTableParams({}, {dataset: data.cesantias});
            vm.solIncapacidades = new NgTableParams({}, {dataset: data.licencias});
            vm.solCertificados = new NgTableParams({}, {dataset: data.certificados});
            vm.loading = false;
          })
          .error(function(status, error) {
            vm.loading = false;
            vm.data.vacaciones = [];
            vm.solVacacionesGrid = new NgTableParams({}, { dataset: [] });
            vm.solCesantiasGrid = new NgTableParams({}, { dataset: [] });
            vm.solIncapacidades = new NgTableParams({}, { dataset: [] });
            vm.solCertificados = new NgTableParams({}, { dataset: [] });
          });
      });
    }
  }
})();
