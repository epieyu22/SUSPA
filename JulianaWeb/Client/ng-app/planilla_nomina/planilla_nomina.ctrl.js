(function () {
  'use strict';

  angular
    .module('app.planilla_nomina')
    .controller('PlanillaNominaCtrl', PlanillaNominaCtrl);

  PlanillaNominaCtrl.$inject = ['$http', '$state', '$q', 'Empresas'];
  function PlanillaNominaCtrl($http, $state, $q, Empresas) {
    var vm = this;
    vm.data = {};
    vm.viewmodel = {};
    vm.dataLoaded = false;
    vm.viewmodel.Fec_Ing_Novedad = "all";


    vm.generatePlanilla = generatePlanilla;
    function generatePlanilla() {
      vm.dataLoaded = true;

      var fecha1 = formatDate(vm.viewmodel.Fec_Nomina);
      var fecha2 = vm.viewmodel.Fec_Ing_Novedad === "all" ? "all" : formatDate(vm.viewmodel.Fec_Ing_Novedad);
      var url = ROOTURL + 'API/PlanillaNomina/' + vm.viewmodel.Empresa.trim() + '/' + fecha1 + '/' + fecha2;

      vm.grid_principal = {
        altrows: true,
        source: {
          datatype: "json",
          datafields: [
            { name: 'Num_Documento' },
            { name: 'Empleado' },
            { name: 'Sucursal' },
            { name: 'Ccosto' },
            { name: 'cargo' },
            { name: 'Dias_Trabajados' },
            { name: 'Dias_Vacaciones' },
            { name: 'Salario' },
            { name: 'AuxTransporte' },
            { name: 'Vacaciones' },
            { name: 'Comisiones' },
            { name: 'Otros_Devengos' },
            { name: 'Total_Devengos' },
            { name: 'Total_Deducidos' },
            { name: 'ApoSalud' },
            { name: 'ApoPension' },
            { name: 'ApoSolPension' },
            { name: 'Retefuente' },
            { name: 'Otros_Deducidos' },
            { name: 'Total_General' },
            { name: 'Cesantias' },
            { name: 'Intereses_Cesantias' },
          ],
          url: url
        },
        columns: [
          { text: 'Cedula', dataField: 'Num_Documento', cellsalign: 'right', align: 'center', width: 80, pinned: true },
          {
            text: 'Empleado',
            dataField: 'Empleado',
            width: 250,
            align: 'center',
            pinned: true,
            groupable: false,
            aggregates: ['count'],
            cellsrenderer: function (row, column1, value, defaultRender, column2, rowData) {
              if (value.toString().indexOf("Count:") >= 0) {
                defaultRender = defaultRender.replace('style="', 'style="font-weight:bold;')
                return defaultRender.replace("Count:", "Empleados: ");
              }
            },
            aggregatesrenderer: function (aggregates, column, element) {
              var renderstring = '<div style="position: relative; margin-top: 4px; margin-right:5px; text-align: right; overflow: hidden;"> Totales Generales / Empleados : (' + aggregates.count + ')</div>';
              return renderstring;
            }
          },
          { text: 'Sucursal', dataField: 'Sucursal', width: 200, align: 'center', filtertype: 'checkedlist' },
          { text: 'Ccosto', dataField: 'Ccosto', width: 200, align: 'center', filtertype: 'checkedlist' },
          { text: 'Cargo', dataField: 'cargo', width: 250, align: 'center', filtertype: 'checkedlist' },
          { text: 'Días', dataField: 'Dias_Trabajados', cellsalign: 'right', align: 'center', width: 30, filterable: false, groupable: false },
          { text: 'Vac.', dataField: 'Dias_Vacaciones', cellsalign: 'right', align: 'center', width: 30, filterable: false, groupable: false },
          { text: 'Sueldo', columngroup: 'Devengos', dataField: 'Salario', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'A. Transp', columngroup: 'Devengos', dataField: 'AuxTransporte', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Vac.', columngroup: 'Devengos', dataField: 'Vacaciones', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Cesantias', columngroup: 'Devengos', dataField: 'Cesantias', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Int. Ces.', columngroup: 'Devengos', dataField: 'Intereses_Cesantias', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Comisiones', columngroup: 'Devengos', dataField: 'Comisiones', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Otros Dev.', columngroup: 'Devengos', dataField: 'Otros_Devengos', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          {
            text: 'Total',
            columngroup: 'Devengos',
            dataField: 'Total_Devengos',
            cellsformat: 'c',
            cellsalign: 'right',
            align: 'center',
            width: 120,
            filterable: false,
            groupable: false,
            aggregates: ['sum'],
            cellsrenderer: function (row, column1, value, defaultRender, column2, rowData) {
              if (value.toString().indexOf("Sum:") >= 0) {
                defaultRender = defaultRender.replace('style="', 'style="font-weight:bold;')
                return defaultRender.replace("Sum:", "");
              }
            },
            aggregatesrenderer: function (aggregates, column, element) {
              var renderstring = '<div style="position: relative; margin-top: 4px; margin-right:5px; text-align: right; overflow: hidden;font-weight:bold">' + aggregates.sum + '</div>';
              return renderstring;
            }
          },
          { text: 'Salud', columngroup: 'Deducidos', dataField: 'ApoSalud', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Pension', columngroup: 'Deducidos', dataField: 'ApoPension', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'S. Pension', columngroup: 'Deducidos', dataField: 'ApoSolPension', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Retefuente', columngroup: 'Deducidos', dataField: 'Retefuente', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          { text: 'Otros Des.', columngroup: 'Deducidos', dataField: 'Otros_Deducidos', cellsformat: 'c', cellsalign: 'right', align: 'center', width: 80, filterable: false, groupable: false },
          {
            text: 'Total',
            columngroup: 'Deducidos',
            dataField: 'Total_Deducidos',
            cellsformat: 'c',
            cellsalign: 'right',
            align: 'center',
            width: 120,
            filterable: false,
            groupable: false,
            aggregates: ['sum'],
            cellsrenderer: function (row, column1, value, defaultRender, column2, rowData) {
              if (value.toString().indexOf("Sum:") >= 0) {
                defaultRender = defaultRender.replace('style="', 'style="font-weight:bold;')
                return defaultRender.replace("Sum:", "");
              }
            },
            aggregatesrenderer: function (aggregates, column, element) {
              var renderstring = '<div style="position: relative; margin-top: 4px; margin-right:5px; text-align: right; overflow: hidden; font-weight:bold">' + aggregates.sum + '</div>';
              return renderstring;
            }
          },
          {
            text: 'Total a Pagar',
            align: 'center',
            width: 120,
            dataField: 'Total_General',
            cellsformat: 'c',
            cellsalign: 'right',
            filterable: false,
            groupable: false,
            aggregates: ['sum'],
            cellsrenderer: function (row, column1, value, defaultRender, column2, rowData) {
              if (value.toString().indexOf("Sum:") >= 0) {
                return defaultRender.replace("Sum:", "");
              }
            },
            aggregatesrenderer: function (aggregates, column, element) {
              var renderstring = '<div style="position: relative; margin-top: 4px; margin-right:5px; text-align: right; overflow: hidden; font-weight:bold">' + aggregates.sum + '</div>';
              return renderstring;
            }
          }
        ],
        columngroups: [
          { text: 'Descuentos', align: 'center', name: 'Deducidos' },
          { text: 'Devengos', align: 'center', name: 'Devengos', classname: 'green' }
        ],
        groupable: true,
        sortable: true,
        showfilterrow: true,
        showgroupaggregates: true,
        filterable: true,
        theme: 'metro',
        height: '100%',
        width: '100%',
        showaggregates: true,
        showstatusbar: true,
      }
    }







    vm.datetimeInput = {
      culture: 'es-CO',
      height: '38px',
      width: '100%',
    };














    activate();

    ////////////////

    function activate() {
      vm.data.Empresas = Empresas;
      if (vm.data.Empresas.length > 0) {
        vm.viewmodel.Empresa = vm.data.Empresas[0].BaseDatos;
        Get_Datos_Plantilla_Nomina()
          .then(function (data) {
            Object.assign(vm.data, data);
          })
      }
    }


    function Get_Panilla_Nomina_Empresa() {

    }

    vm.Get_Fec_Ing_Novedad = Get_Fec_Ing_Novedad;
    function Get_Fec_Ing_Novedad() {
      var Empresa = vm.viewmodel.Empresa.trim();
      var fec = vm.viewmodel.Fec_Nomina;
      var Fec_Nomina = fec.substring(0, 4) + fec.substring(5, 7) + fec.substring(8, 10);
      var url = ROOTURL + 'API/PlanillaNomina/' + Empresa.trim() + '/FecsIngNovedad/' + Fec_Nomina;
      $http
        .get(url)
        .success(function (data) {
          vm.data.Fecs_Ing_Novedad = data;
        })
        .error(function () {
           NotifySvc.error();
        });
    }


    function getPlanillaNominaFecNomina() {

    }



    function Get_Datos_Plantilla_Nomina() {
      var Empresa = vm.viewmodel.Empresa.trim();
      var deferred = $q.defer();
      var url = ROOTURL + 'API/PlanillaNomina/Datos/' + Empresa;
      $http
        .get(url)
        .success(function (data) {
          deferred.resolve(data);
        })
        .error(function () {
          NotifySvc.error();
        });
      return deferred.promise;
    }
  }
})();



function formatDate(date) {
  var d = new Date(date),
    month = '' + (d.getMonth() + 1),
    day = '' + d.getDate(),
    year = d.getFullYear();

  if (month.length < 2) month = '0' + month;
  if (day.length < 2) day = '0' + day;

  return [year, month, day].join('');
}
