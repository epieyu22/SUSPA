(function () {
  'use strict';
  angular
    .module('app.consultas')
    .controller('ConsultasCtrl', ConsultasCtrl);

  ConsultasCtrl.$inject = ['NgTableParams', 'data'];
  /* @ngInject */
  function ConsultasCtrl(NgTableParams, data) {
    var vm = this;

    vm.initHistosalarioGrid = initHistosalarioGrid;
    // vm.initGrid = initGrid;

    activate();
    ////////////////
    function activate() {
      vm.data = data;
    }

    function initHistosalarioGrid() {
      var initialParams = {};
      var initialSettings = {
        counts: [5, 10, 20],
        dataset: vm.data.historicos
      };
      vm.configTable = new NgTableParams(initialParams, initialSettings);
    }
  }
})();

//TODO: Alertas
