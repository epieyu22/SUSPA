(function () {
  'use strict';

  angular
    .module('app.solpersonal')
    .controller('SolPersonalCtrl', SolPersonalCtrl);

  SolPersonalCtrl.$inject = ['ngDialog', 'data', 'NotifySvc', 'SolPersonalSvc', 'Empresas'];
  function SolPersonalCtrl(ngDialog, data, NotifySvc, SolPersonalSvc, Empresas) {
    var vm = this;


    activate();

    ////////////////

    function activate() {
      vm.data = data;
      vm.Empresas = Empresas;
      console.log(vm.data.ccostos)
    }

    vm.GuardarRequerimiento = GuardarRequerimiento;
    function GuardarRequerimiento() {
      SolPersonalSvc
        .IngresarSolicitud(vm.viewmodel)
        .then(function () {
          NotifySvc.success();
          vm.viewmodel = {};
        })
    }
  }
})();
