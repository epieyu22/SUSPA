(function () {
  'use strict';
  angular
    .module('app.certificados')
    .controller('CompropagoCtrl', CompropagoCtrl);

  CompropagoCtrl.$inject = ['$rootScope', 'AuthSvc', 'CertificadosSvc', 'NotifySvc', 'config'];
  /* @ngInject */

  function CompropagoCtrl($rootScope, AuthSvc, CertificadosSvc, NotifySvc, config) {
    var vm = this;

    var minDate = {};
    var actualDate = new Date();

    var MESES = [{
        name: 'Enero',
        value: '01'
      },
      {
        name: 'Febrero',
        value: '02'
      },
      {
        name: 'Marzo',
        value: '03'
      },
      {
        name: 'Abril',
        value: '04'
      },
      {
        name: 'Mayo',
        value: '05'
      },
      {
        name: 'Junio',
        value: '06'
      },
      {
        name: 'Julio',
        value: '07'
      },
      {
        name: 'Agosto',
        value: '08'
      },
      {
        name: 'Septiembre',
        value: '09'
      },
      {
        name: 'Octubre',
        value: '10'
      },
      {
        name: 'Noviembre',
        value: '11'
      },
      {
        name: 'Diciembre',
        value: '12'
      },
    ]

    vm.mesesToShow = [];
    vm.viewmodel = {};
    vm.viewmodel.empleados = [];
    vm.years = [];

    vm.checkMeses = checkMeses;
    vm.generateCompropago = generateCompropago;

    activate();
    ////////////////
    function activate() {
      vm.config = config;
      minDate.ano = vm.config.data.Ano_Compropago;
      minDate.mes = parseInt(vm.config.data.Mes_Compropago);
      minDate.quincena = 1;
      actualDate = new Date(
        config.Fec_Nomnia.substr(0, 4),
        config.Fec_Nomnia.substr(4, 2) - 1,
        config.Fec_Nomnia.substr(6, 2)
      );

      AuthSvc.getEmpleado()
        .then(function (empleado) {
          vm.viewmodel.empleados.push(empleado);
          var FecInngreso = empleado.Fec_Ingreso.substr(0, 6)
          if (empleado.Fec_Ingreso.substr(0, 6) >= minDate.ano + vm.config.data.Mes_Compropago) {
            minDate.ano = empleado.Fec_Ingreso.substr(0, 4);
            minDate.mes = parseInt(empleado.Fec_Ingreso.substr(4, 2));
            if (empleado.Fec_Ingreso.substr(6) > 15) {
              minDate.quincena = 2;
            }
          }
          var currentYear = actualDate.getFullYear();
          checkMeses(currentYear);
          vm.viewmodel.ano = currentYear;
          while (currentYear >= minDate.ano) {
            vm.years.push(currentYear);
            currentYear--;
          }
        });


      // vm.viewmodel.quincena = actualDate.getDate() > 15 ? "2" : "1";
      vm.viewmodel.quincena = "2"
    }

    function checkMeses(ano) {
      ano = parseInt(ano);
      vm.mesesToShow = [];
      if (ano <= minDate.ano) {
        if (ano == actualDate.getFullYear()) {
          for (var i = actualDate.getMonth(); i >= minDate.mes - 1; i--) {
            vm.mesesToShow.push(MESES[i]);
          }
        } else {
          for (var i = 11; i >= minDate.mes - 1; i--) {
            vm.mesesToShow.push(MESES[i]);
          }
        }
      } else {
        if (ano == actualDate.getFullYear()) {
          for (var i = actualDate.getMonth(); i >= 0; i--) {
            vm.mesesToShow.push(MESES[i]);
          }
        } else {
          for (var i = 11; i >= 0; i--) {
            vm.mesesToShow.push(MESES[i]);
          }
        }
      }
      vm.viewmodel.mes = vm.mesesToShow[0].value;
    }

    function generateCompropago() {
      vm.loading = true;
      $rootScope.generating = true;
      CertificadosSvc.generateCompropago(vm.viewmodel)
        .then(function () {
          vm.loading = false;
          $rootScope.generating = false;
        }, function () {
          $rootScope.generating = false;
          NotifySvc.error();
        });
    }


  }
})();
