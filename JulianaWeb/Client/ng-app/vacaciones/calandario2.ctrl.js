(function() {
  'use strict';

  angular
    .module('app.vacaciones')
    .controller('Calendario2Ctrl', Calendario2Ctrl);

  Calendario2Ctrl.$inject = ['Empresas'];
  function Calendario2Ctrl(Empresas) {
    var vm = this;
    vm.data = {};
    

    activate();

    ////////////////

    function activate() { 
      vm.data.Empresas = Empresas;
    }
  }
})();
