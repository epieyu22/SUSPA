(function() {
  'use strict';

  angular
    .module('app.certificados')
    .controller('AdminCertlaboralCtrl', AdminCertlaboralCtrl)
    .filter('trustAsHTML', [
      '$sce',
      function ($sce) {
        return function (text) {
          return $sce.trustAsHtml(text);
        };
      }
    ]);

  AdminCertlaboralCtrl.$inject = [
    '$http',
    '$rootScope',
    'NgTableParams',
    'NotifySvc',
    'CertificadosSvc',
    'Certifica2Svc',
    'data',
    '$scope',
    '$location'
  ];

  function AdminCertlaboralCtrl($http, $rootScope, NgTableParams, NotifySvc, CertificadosSvc, Certifica2Svc, data, $scope, $location) {
    var vm = this;
    vm.loading = true;
    vm.tableSelectAll = false;
    vm.dataLoaded = false; 
    vm.loadCertlabConfig = loadCertlabConfig;
    vm.loadEmpleados = loadEmpleados;
    vm.selectEmpleado = selectEmpleado;        
    vm.selectedChange = selectedChange;      
    vm.loadCertlabConfig = loadCertlabConfig;
    vm.saveCertlabConfig = saveCertlabConfig;
    vm.generateCertlab = generateCertlab;
    vm.selectedItems = [];
    vm.data = data;
    vm.log = log;
    vm.sebo = sebo;
    vm.model = {
      Cod_Plantilla: null
    };

    vm.viewmodel = {};
    vm.getCompropagoConfigByEmpresa = getCompropagoConfigByEmpresa;
    vm.generateCompropago = generateCompropago;
    vm.Load_Config_Compropago = Load_Config_Compropago;
    vm.Load_Empleados_Compropago = Load_Empleados_Compropago;
    vm.Generate_Admin_Compropago = Generate_Admin_Compropago;
    vm.Load_Config_Retefuente = Load_Config_Retefuente;
    vm.Load_Empleados_Retefuente = Load_Empleados_Retefuente;

    vm.plantillaTolbarOptions = [
      ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear'],
      ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'indent', 'outdent']
    ];

    vm.froalaOptions = {
      toolbarButtons: ['bold', 'italic', 'underline', '|', 'align', 'formatOL', 'formatUL']
    };

    vm.sucursales = [];

    var minDate = {};
    var actualDate = new Date();

    var MESES = [
      {
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
      }
    ];

    vm.MONTHS = MESES;

    vm.mesesToShow = [];
    vm.viewmodel = {};
    vm.viewmodel.empleados = [];
    vm.years = [];

    vm.checkMeses = checkMeses;
    vm.SetLatsFecNomina = SetLatsFecNomina;
    vm.uploadFirmaConfig = uploadFirmaConfig;
    vm.generateRetefuente = generateRetefuente;
    vm.SendMailCompropago = SendMailCompropago;
    vm.Deselect_Items = Deselect_Items;

    vm.froalaOptions = {
      toolbarSticky: false
    };

    function Deselect_Items(source) {
      for (var i = vm.viewmodel.empleados.length - 1; i >= 0; i--) {
        vm.viewmodel.empleados[i].selected = false;
      }
      // vm.viewmodel.empleados = [];
    }

    function log(source) {
      if (vm.tableSelectAll) {
        vm.selectedItems = source;
        for (var i = vm.selectedItems.length - 1; i >= 0; i--) {
          vm.selectedItems[i].selected = true;
        }
      } else {
        for (var i = vm.selectedItems.length - 1; i >= 0; i--) {
          vm.selectedItems[i].selected = false;
        }
        vm.selectedItems = [];
      }
    }

    function selectedChange(source) {
      // Cambiar Source en caso de que sean vario
      if (vm.selectedItems.length == source.length) {
        vm.tableSelectAll = true;
        $('.select-all').prop('indeterminate', false);
      } else if (vm.selectedItems.length == 0) {
        vm.tableSelectAll = false;
        $('.select-all').prop('indeterminate', false);
      } else {
        $('.select-all').prop('indeterminate', true);
      }
    }

    function sebo() {
      NotifySvc.success();
    }

    vm.UseDirigidoCertificado = true;

    activate();

    

    function activate() {
      vm.data.empresas = data;
      vm.UseDirigidoCertificado = SETTINGS.UseDirigidoCertificado === 'true';
      if (!vm.UseDirigidoCertificado) {
        vm.viewmodel.dirigido = 'A Quien Interese';
      }
    }
    //Obtener plantillas Admin
    function GetPlantillas(Empresa) {
      
      var Otroscert = 'certificados';
      $scope.ruta = $location.absUrl();
      var Ruta = $scope.ruta;

      if (Ruta.match("otroscert")) {
        Otroscert = 'otroscert'
      }
      $http.get(ROOTURL + 'API/Certificados/Certlaboral/' + Empresa.trim() + '/' + Otroscert + '/Plantillas').success(function(data) {
        vm.loading = false;
        vm.plantillas = data.plantillas;
        vm.model.Cod_Plantilla = vm.plantillas;
      });
    }

    vm.generateCertlaboral = function() {
      $rootScope.generating = true;
      vm.viewmodel.DBName = vm.viewmodel.Empresa.trim();
      Certifica2Svc.generateCertlabMasivo(vm.viewmodel).then(
        function() {
          $rootScope.generating = false;
        },
        function(error) {
          $rootScope.generating = false;
          NotifySvc.error();
        }
      );
    };

    function loadEmpleados() {
      vm.plantillas = [];
      vm.selectedItems = [];
      vm.loading = true;
      var Empresa = vm.viewmodel.Empresa.trim();
      GetPlantillas(Empresa);
      var url = ROOTURL + 'API/Empresas/' + Empresa.trim() + '/Empleados/Retirados/';
      getCompropagoConfigByEmpresa();
      CertificadosSvc.getDefaultCertLabConfig(Empresa).then(function(data) {
        var empresas = vm.data.empresas;
        vm.data = data;
        vm.data.empresas = empresas;
        vm.empleadosGird = new NgTableParams(
          {},
          {
            dataset: vm.data.empleados
          }
        );
        vm.selectedIndex = null;
        vm.viewmodel.empleado = null;
        vm.viewmodel.dirigido = null;
        vm.loading = false;
      });
    }

    function loadEmpleadosRetefuente() {
      var Empresa = vm.viewmodel.Empresa.trim();
      var url = ROOTURL + 'API/Empresas/' + Empresa.trim() + '/Empleados';
      getCompropagoConfigByEmpresa();
      CertificadosSvc.getDefaultCertLabConfig(Empresa).then(function(data) {
        vm.empleadosGird = new NgTableParams(
          {},
          {
            dataset: vm.data.empleados
          }
        );
      });
    }

    function selectEmpleado(e) {
      vm.selectedIndex = e.Cod_Empleado;
      vm.viewmodel.empleado = e;
    }

    function generateCertlab() {
      $rootScope.generating = true;
      vm.viewmodel.DBName = vm.viewmodel.Empresa.trim();
      CertificadosSvc.generateCustomCertlab(vm.viewmodel).then(
        function() {
          $rootScope.generating = false;
        },
        function(error) {
          $rootScope.generating = false;
          NotifySvc.error();
        }
      );
    }

    function generateRetefuente() {
      $rootScope.generating = true;
      vm.viewmodel.DBName = vm.viewmodel.Empresa.trim();
      CertificadosSvc.generateCustomRetefuente(vm.viewmodel).then(
        function() {
          $rootScope.generating = false;
        },
        function(error) {
          $rootScope.generating = false;
          NotifySvc.error();
        }
      );
    }

    function loadCertlabConfig() {
      var empresa = vm.viewmodel.Empresa.trim();
      var Filter = vm.viewmodel.Filter;
      var Cod_Filter = vm.viewmodel.Cod_Filter;
      var url = ROOTURL + 'API/Config/Certlab/' + empresa.trim() + '/' + Filter + '/' + Cod_Filter;
      $http
        .get(url)
        .success(function(data) {
          vm.data.config = data;
          vm.data.config.Filter = vm.viewmodel.Filter;
          vm.data.config.Cod_Filter = vm.viewmodel.Cod_Filter;
          vm.dataLoaded = true;
        })
        .error(function() {
          NotifySvc.error();
        });
    }

    function uploadFirmaConfig() {
      var fd = new FormData();
      fd.append('file', vm.firmaFile);
      fd.append('Firma_Digital', vm.data.config.Firma_Digital);
      fd.append('Cod_Empleado_Autoriza', vm.data.config.Cod_Empleado_Autoriza);
      var DBName = vm.viewmodel.Empresa.trim();

      var url =
        ROOTURL +
        'API/Config/Certlab/' +
        '/FirmaDigital/' +
        DBName.trim() +
        '/' +
        vm.data.config.Filter +
        '/' +
        vm.data.config.Cod_Filter;
      return $http
        .post(url, fd, {
          headers: {
            'Content-Type': undefined,
            transformRequest: angular.identity
          }
        })
        .success(function(response) {
          NotifySvc.success();
        })
        .error(function(error) {
          NotifySvc.error();
        });
    }

    function getCompropagoConfigByEmpresa(callback) {
      var Empresa = vm.viewmodel.Empresa.trim();
      CertificadosSvc.getCompropagoConfigByEmpresa(Empresa).then(function(data) {
        vm.config = data;
        generateConfig();
        if (callback) callback();
      });
    }

    function generateConfig() {
      minDate.ano = vm.config.data.Ano_Compropago;
      minDate.mes = parseInt(vm.config.data.Mes_Compropago);
      minDate.quincena = 1;
      actualDate = new Date(
        vm.config.Fec_Nomnia.substr(0, 4),
        vm.config.Fec_Nomnia.substr(4, 2) - 1,
        vm.config.Fec_Nomnia.substr(6, 2)
      );
      var currentYear = actualDate.getFullYear();
      checkMeses(currentYear);
      vm.viewmodel.ano = currentYear;
      while (currentYear >= minDate.ano) {
        vm.years.push(currentYear);
        currentYear--;
      }
      vm.viewmodel.anoContable = vm.years[0];
      vm.viewmodel.quincena = vm.config.liqNomina == 15 ? '1' : '2';
    }

    function SetLatsFecNomina() {
      var Empresa = vm.viewmodel.Empresa.trim();
      CertificadosSvc.SetLatsFecNomina(Empresa, vm.config.Ult_Pago).then(
        function() {
          NotifySvc.success();
        },
        function() {
          NotifySvc.error();
        }
      );
    }

    function saveCertlabConfig() {
      console.log(vm.data.config);
      var empresa = vm.viewmodel.Empresa.trim();
      var Filter = vm.viewmodel.Filter;
      var Cod_Filter = vm.viewmodel.Cod_Filter;
      var url = ROOTURL + 'API/Config/Certlab/' + Empresa.trim() + '/' + Filter + '/' + Cod_Filter;
      $http
        .post(url, vm.data.config)
        .success(function(data) {
          NotifySvc.success();
        })
        .error(function() {
          NotifySvc.error();
        });

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
      CertificadosSvc.generateCompropago(vm.viewmodel).then(
        function() {
          vm.loading = false;
          $rootScope.generating = false;
        },
        function() {
          $rootScope.generating = false;
          NotifySvc.error();
        }
      );
    }

    function Generate_Admin_Compropago() {
      vm.loading = true;
      var Empresa = vm.viewmodel.Empresa.trim();
      $rootScope.generating = true;
      CertificadosSvc.Generate_Admin_Compropago(Empresa, vm.viewmodel).then(
        function() {
          vm.loading = false;
          $rootScope.generating = false;
        },
        function() {
          $rootScope.generating = false;
          NotifySvc.error();
        }
      );
    }

    function SendMailCompropago() {
      var Empresa = vm.viewmodel.Empresa.trim();
      
      var Fec_Nomina = ""
      if (vm.viewmodel.quincena == "1") {
        Fec_Nomina = vm.viewmodel.ano + vm.viewmodel.mes + '15';

      } else {
       Fec_Nomina = vm.viewmodel.ano + vm.viewmodel.mes + '30';
      }
      vm.loading = true;
      $rootScope.sendmails = true;
      CertificadosSvc.SendMailCompropago(Empresa, Fec_Nomina, vm.viewmodel.empleados).then(
        function() {
          vm.loading = false;
          $rootScope.sendmails = false;
          NotifySvc.success('Proceso completado con exito.');
        },
        function() {
          $rootScope.sendmails = false;
          NotifySvc.error();
        }
      );
    }

    function Load_Config_Compropago() {
      getCompropagoConfigByEmpresa(Load_Empleados_Compropago);
    }

    function Load_Empleados_Compropago() {
      vm.loading = true;
      var Empresa = vm.viewmodel.Empresa.trim();
      vm.viewmodel.empleados = [];
      CertificadosSvc.Load_Empleados_Compropago(
        Empresa,
        vm.viewmodel.ano,
        vm.viewmodel.mes,
        vm.viewmodel.quincena
      ).then(function(data) {
        vm.empleadosGird = new NgTableParams(
          {},
          {
            dataset: data
          }
        );
        var unique = {};
        vm.sucursales = data
          .map(function(e) {
            return e.Cod_Sucursal;
          })
          .filter(function(value, index, self) {
            return self.indexOf(value) === index;
          })
          .map(function(e) {
            return {id: e, title: e};
          });
        vm.loading = false;
      });
    }

    function Load_Config_Retefuente() {
      getCompropagoConfigByEmpresa(Load_Empleados_Retefuente);
    }

    function Load_Empleados_Retefuente() {
      var Empresa = vm.viewmodel.Empresa.trim();
      vm.viewmodel.empleados = [];
      CertificadosSvc.Load_Empleados_Retefuente(Empresa, vm.viewmodel.anoContable).then(function(data) {
        vm.empleadosGird = new NgTableParams(
          {},
          {
            dataset: data.empleados
          }
        );
      });
    }
  }
})();
