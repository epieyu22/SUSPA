(function () {
  'use strict';

  angular
    .module('app.certificados')
    .controller('SRICtrl', SRICtrl);

  SRICtrl.$inject = ['$rootScope', 'Certifica2Svc', 'AuthSvc'];
  function SRICtrl($rootScope, Certifica2Svc, AuthSvc) {
    var vm = this;
    vm.model = {};
    vm.years = [];

    activate();

    ////////////////

    function activate() {
      AuthSvc.getEmpleado().then(function (Empleado) {
        if (Empleado) {
          var anoIngreso = parseInt(Empleado.Fec_Ingreso.substr(0, 4));
          var anoActual = new Date().getFullYear() - 1;
          while (anoIngreso <= anoActual) {
            vm.years.push(anoIngreso);
            anoIngreso++;
          };
          vm.model.ano = vm.years[vm.years.length - 1];
          vm.Empleado = Empleado;
        }
      })
    }

    vm.Generate_SRI_Retefuente = Generate_SRI_Retefuente;
    function Generate_SRI_Retefuente() {
      var data = {}
      data.Empresa = AuthSvc.data.DBName;
      data.Empleados = [vm.Empleado];
      $rootScope.generating = true;
      Certifica2Svc
        .Generate_SRI_Retefuente(data)
        .then(function () {
          $rootScope.generating = false;
        }, function () {
          $rootScope.generating = false;
        });
    }
  }
})();
