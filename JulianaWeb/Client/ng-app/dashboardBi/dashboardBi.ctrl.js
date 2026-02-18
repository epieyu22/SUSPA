(function () {
  'use strict';

  angular
    .module('app.dashboardBi')
    .controller('DashboardBiCtrl', DashboardBiCtrl);

  DashboardBiCtrl.$inject = ['ngDialog', 'data', 'NotifySvc', 'DashboardBiSvc', 'Empresas'];
  function DashboardBiCtrl(ngDialog, data, NotifySvc, DashboardBiSvc, Empresas) {
    var vm = this;


    activate();

    ////////////////

    //function activate() {
    //  vm.data = data;
    //  vm.Empresas = Empresas;
    //  console.log(vm.data.ccostos)
    //}

    //vm.GuardarRequerimiento = GuardarRequerimiento;
    //function GuardarRequerimiento() {
    //  SolPersonalSvc
    //    .IngresarSolicitud(vm.viewmodel)
    //    .then(function () {
    //      NotifySvc.success();
    //      vm.viewmodel = {};
    //    })
    //}
  }
})();
