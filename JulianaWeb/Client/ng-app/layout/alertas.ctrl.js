(function() {
  'use strict';
  angular.module('app.core').controller('AlertasCtrl', AlertasCtrl);

  AlertasCtrl.$inject = ['$state', '$http', 'AuthSvc'];

  function AlertasCtrl($state, $http, AuthSvc) {
    var vm = this;
    vm.alertaBlockLeave = false;
    vm.alertaVacacionesVencidads = false;

    activate();
    ////////////////
    function activate() {
      cargarDatos();
    }

    function cargarDatos() {
      if (SETTINGS.BLOCKLEAVE == 'true') {
        AuthSvc.getEmpleado().then(function(empleado) {
          var Cod_Empleado = empleado.Cod_Empleado;
          var Empresa = AuthSvc.data.DBName;
          var url = ROOTURL + 'API/Empleados/' + Empresa.trim() + '/Alertas/' + Cod_Empleado;
          $http.get(url).success(function(data) {
            vm.alertaBlockLeave = data.alertaBlockLeave;
            SETTINGS.BlockleaveNoCumplido = data.alertaBlockLeave;
            // vm.alertaVacacionesVencidads = !data.alertaVacacionesVencidads;
          });
        });
      }
    }
  }
})();
