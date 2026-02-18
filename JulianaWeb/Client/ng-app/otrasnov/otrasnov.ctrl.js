(function () {
  'use strict';

  angular
    .module('app.otrasnov')
    .controller('OtrasnovCtrl', OtrasnovCtrl);

  OtrasnovCtrl.$inject = ['$state', '$rootScope', 'ngDialog', 'NotifySvc', 'OtrasNovSvc', 'Empresas'];

  function OtrasnovCtrl($state, $rootScope, ngDialog, NotifySvc, OtrasNovSvc, Empresas) {
    var vm = this;
    vm.data = {};
    vm.viewmodel = {};

    vm.loading = true;
    vm.novedades = [];
    vm.errors = [];

    vm.Buscar_Empleado = Buscar_Empleado;
    vm.Get_Conceptos_OtrasNov = Get_Conceptos_OtrasNov;
    vm.Upload_OtrasNov = Upload_OtrasNov;
    vm.Change_Coutas = Change_Coutas;
    vm.newNovedades = newNovedades;


    vm.onHastaChange = onHastaChange;

    vm.viewmodel.Desde = new Date();

    activate();

    ////////////////

    function activate() {
      vm.Empresas = Empresas;
      if (vm.Empresas.length > 0) {
        vm.viewmodel.Empresa = vm.Empresas[0].BaseDatos;
      }
      Get_Conceptos_OtrasNov();

      Get_Registros_Pendientes();
    }

    function Buscar_Empleado(event) {
      event.preventDefault();
      ngDialog.openConfirm({
        template: 'Client/ng-app/empleados/empleados.dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        controller: 'EmpleadosCtrl',
        data: vm.data.empleados
      })
        .then(function (data) {
          vm.viewmodel.Cod_Empleado = data.Cod_Empleado;
          vm.viewmodel.Empleado = data.Empleado;
          console.log(vm.viewmodel.Empleado)
        });
    }


    function Get_Conceptos_OtrasNov() {
      var Empresa = vm.viewmodel.Empresa.trim();

      OtrasNovSvc
        .Get_Conceptos_OtrasNov(Empresa)
        .then(function (data) {
          vm.data = data;
          if (vm.data.liqNomina == 30) {
            vm.viewmodel.Aplica = 2;
          }
          vm.viewmodel.Desde = "0";
        });
    }



    function Get_Registros_Pendientes() {
      OtrasNovSvc
        .Get_Registros_Pendientes()
        .then(function (data) {
          vm.loading = false;
          vm.novedades = data;
          vm.grilla = {
            altrows: true,
            width: '100%',
            source: vm.novedades,
            columns: [
              {
                text: 'Cod',
                dataField: 'Cod_Empleado',
                width: 50,
                align: 'center',
                pinned: true,
                cellsalign: 'right'
              },
              {
                text: 'Documento',
                width: 100,
                dataField: 'Documento',
                align: 'center',
                pinned: true,
                cellsalign: 'right'
              },
              {
                text: 'Empleado',
                dataField: 'Empleado',
                width: 250,
                align: 'center',
                pinned: true
              },
              { text: 'Sucursal', dataField: 'Nom_Sucursal', align: 'center', filtertype: 'checkedlist', width: 200 },
              { text: 'C. Costo', dataField: 'Nom_Ccosto', align: 'center', filtertype: 'checkedlist', width: 200, },
              { text: 'Concepto', dataField: 'Concepto', align: 'center', filtertype: 'checkedlist', width: 250 },
              {
                text: 'Valor',
                dataField: 'Val_OtrasNov',
                align: 'left',
                cellsformat: 'c',
                width: 100,
                cellsalign: 'right',
                aggregates: [{
                  '<b>Total</b>': function (aggregatedValue, currentValue, column, record) {
                    if (record['Devengo'] === "Si") {
                      return aggregatedValue + parseFloat(record['Val_OtrasNov']);
                    } else if (record['Devengo'] === "No") {
                      return aggregatedValue - parseFloat(record['Val_OtrasNov']);
                    }
                    return aggregatedValue;
                  }
                }]
              },
              {
                text: 'Tipo',
                dataField: 'Tipo',
                align: 'center',
                filtertype: 'checkedlist',
                columntype: 'dropdownlist',
                width: 100,
                cellsalign: 'center',
                aggregates: [{
                  '<b>Devengos</b>': function (aggregatedValue, currentValue, column, record) {
                    if (record['Devengo'] === "Si") {
                      return aggregatedValue + parseFloat(record['Val_OtrasNov']);
                    }
                    return aggregatedValue;
                  }
                }]
              },
              {
                text: 'Devengo',
                dataField: 'Devengo',
                columntype: 'dropdownlist',
                align: 'center',
                width: 100,
                filtertype: 'checkedlist',
                cellsalign: 'center',
                aggregates: [{
                  '<b>Deducidos</b>': function (aggregatedValue, currentValue, column, record) {
                    if (record['Devengo'] === "No") {
                      return aggregatedValue - parseFloat(record['Val_OtrasNov']);
                    }
                    return aggregatedValue;
                  }
                }]
              }
            ],
            showaggregates: true,
            showstatusbar: true,
            groupable: true,
            sortable: true,
            showfilterrow: true,
            filterable: true,
            theme: 'metro',
            height: '100%',
            // editable: true,
            // enabletooltips: true,
            // editmode: 'dblclick',
            // selectionmode: 'checkbox',
          };
        }, function () {
          vm.loading = false;
        })
    }


    function Upload_OtrasNov() {
      var Empresa = vm.viewmodel.Empresa.trim();
      $rootScope.uploading = true;
      OtrasNovSvc.Upload_OtrasNov(Empresa, vm.excel)
        .then(function () {
          $rootScope.uploading = false;
        }, function (msg) {
          $rootScope.uploading = false;
          NotifySvc.error(msg);
        });
    }


    function Change_Coutas() {
      if (!vm.viewmodel.Desde) vm.viewmodel.Desde = "1";
      if (vm.viewmodel.Prioridad === 'P') {
        vm.viewmodel.Coutas = 2;
        vm.viewmodel.Hasta = String(vm.viewmodel.Coutas + parseInt(vm.viewmodel.Desde));
      } else {
        vm.viewmodel.Coutas = 1;
        vm.viewmodel.Hasta = vm.viewmodel.Desde;
      }
    }

    function newNovedades() {
      setTimeout(function () {
        NotifySvc.success();
        $state.reload()
      }, 1000)
    }


    vm.onDesdeChange = onDesdeChange;

    function onDesdeChange() {
      if (vm.viewmodel.Coutas) {
        if (vm.viewmodel.Coutas + parseInt(vm.viewmodel.Desde) >= vm.data.Fecs_Nomina.length) {
          vm.viewmodel.Coutas = vm.data.Fecs_Nomina.length - vm.viewmodel.Desde - 1;
        }
        vm.viewmodel.Hasta = String(vm.viewmodel.Coutas + parseInt(vm.viewmodel.Desde));
      } else {
        vm.viewmodel.Coutas = null;
        vm.viewmodel.Prioridad = ''
        vm.viewmodel.Hasta = null;
      }
    }

    function onHastaChange() {
      vm.viewmodel.Coutas = vm.viewmodel.Hasta - vm.viewmodel.Desde;
      if (vm.viewmodel.Coutas === 0) vm.viewmodel.Coutas = 1;
      if (vm.viewmodel.Coutas > 1) {
        vm.viewmodel.Prioridad = 'P'
      } else {
        vm.viewmodel.Prioridad = 'O'
      }
    }

    vm.onCoutasChange = onCoutasChange;

    function onCoutasChange() {
      vm.viewmodel.Hasta = String(vm.viewmodel.Coutas + parseInt(vm.viewmodel.Desde));
    }



    $('#OtrasNovExcel').on('submit', function (e) {
      e.preventDefault();
      var post_url = $(this).attr("action"); //get form action url
      var request_method = $(this).attr("method"); //get form GET/POST method
      var form_data = new FormData(this);
      $rootScope.uploading = true;
      $rootScope.$digest() || $rootScope.$apply();
      $.ajax({ //ajax form submit
        url: post_url,
        type: request_method,
        data: form_data,
        dataType: "json",
        contentType: false,
        cache: false,
        processData: false
      }).done(function (data) {
        $rootScope.uploading = false;
        var Empresa = vm.viewmodel.Empresa.trim();
        // var url = $state.href('Otrasnov_List', { data: data, Empresa: Empresa });
        // window.open(url,'_blank');
        $state.go('Otrasnov_List', { data: data, Empresa: Empresa });
      }).error(function (err) {
        $rootScope.uploading = false;
        console.log("error", err)
      });
    });

    var input = $('#OtrasNov');
    input.change(function (e) {

      var files = e.target.files;
      if (!e.target.files.length) return;

      var f = files[0];
      $rootScope.uploading = true;
      $rootScope.$digest() || $rootScope.$apply();


      var reader = new FileReader();
      reader.onload = function (e) {
        var data = e.target.result;
        data = new Uint8Array(data);
        var workbook = XLSX.read(data, { type: 'array' });
        var first_sheet_name = workbook.SheetNames[0];
        var worksheet = workbook.Sheets[first_sheet_name];

        validarDatos(XLSX.utils.sheet_to_json(worksheet));

        $rootScope.uploading = false;
        $rootScope.$digest() || $rootScope.$apply();
      }
      reader.readAsArrayBuffer(f);
      input.val(null);
    });

    function validarDatos(data) {
      vm.errors = [];
      vm.models = [];
      for (let index = 0; index < data.length; index++) {
        const row = data[index];
        if (!row.Cod_Empleado || !row.Cod_Concepto) {
          NotifySvc.error("El archivo no se encuentra en el formato correcto");
          return;
        }
        var empleado = buscarEmpleado(row.Cod_Empleado);
        if (!empleado) {
          var msg = "No existe el Empleado con Código: " + row.Cod_Empleado;
          vm.errors.push({ line: index + 1, msg: msg });
          console.log("Error => " + msg);
          continue;
        }
        if (empleado.Estado.trim() == "R") {
          var msg = "El Empleado " + empleado.Empleado.trim() + " se encuentra  retirado";
          vm.errors.push({ line: index + 1, msg: msg });
          console.log("Error => " + msg);
        }
        var concepto = buscarConcepto(row.Cod_Concepto);
        if (!concepto) {
          var msg = "No existe el concepto con Código " + row.Cod_Concepto;
          vm.errors.push({ line: index + 1, msg: msg });
          console.log("Error => " + msg);
        }
        // var model = {
        //   Cod_Empleado: empleado.Cod_Concepto,
        //   Cod_Empleado: concepto.Cod_Concepto,
        //   Empleado: empleado.Empleado.trim(),
        //   Concepto: concepto.Nom_Concepto,
        //   Fecha: row.Fec_Nomina,
        //   Val_OtrasNov: row.Valor,
        //   Prioridad: row.Prioridad,
        //   Coutas: row.Coutas,
        //   Tipo: row.Coutas > 1 ? "Permanente" : "Ocacional",
        //   Devengo: Concepto.Devengo == "S" ? "Si" : "No",
        //   Documento: empleado.cedula.trim(),
        // }
        // vm.models.push(model);
      }
      if (vm.errors.length) {
        NotifySvc.error("Se encontraron errores en el archivo");
        return;
      }

      var Empresa = vm.viewmodel.Empresa.trim();
      $state.go('Otrasnov_List', { data: vm.models, Empresa: Empresa });

    }

    function buscarEmpleado(Cod_Empleado) {
      var result = vm.data.empleados.filter(function (empleado) {
        return empleado.Cod_Empleado == Cod_Empleado
      });
      if (result.length) return result[0];
      return null;
    }

    function buscarConcepto(Cod_Concepto) {
      var result = vm.data.conceptos.filter(function (concepto) {
        return concepto.Cod_Concepto == Cod_Concepto
      });
      if (result.length) return result[0];
      return null;
    }



  }
})();
