(function () {
  'use strict';

  angular
    .module('app.otrasnov')
    .controller('ListOtrasNovCtrl', ListOtrasNovCtrl);

  ListOtrasNovCtrl.$inject = ['ngDialog', '$state', '$stateParams', 'OtrasNovSvc'];

  function ListOtrasNovCtrl(ngDialog, $state, $stateParams, OtrasNovSvc) {
    var vm = this;
    vm.data = {};

    activate();




    function activate() {
      vm.data.novedades = $stateParams.data;
      vm.data.Empresa = $stateParams.Empresa;


      vm.grilla = {
        altrows: true,
        width: '100%',
        source: vm.data.novedades,
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
                if (record['Devengo'] === "S") {
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
            aggregatesi: [{
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
    }

    vm.Upload_Novedades = Upload_Novedades;
    function Upload_Novedades() {
      OtrasNovSvc
        .Upload_Novedades(vm.data.Empresa, vm.data.novedades)
        .then(function (data) {
          console.log(data, "hola mundo");
        })
    }


    vm.confirm_upload = confirm_upload;
    function confirm_upload() {
      ngDialog.openConfirm({
        template: '\
        <div class="ngdialog-body">\
            <div class="row"><div class="col-md-12"><h4>¿Subir Novedades?</h4></div>    \
                <div class="col-md-12 my-5 text-center"> \
                ¿Está seguro de de subir estas novedades al sistema?\
                Revise cuidadosamente los valores antes de aceptar.</div></div> \
            <div>\
              <button class="btn btn-outline-secondary" ng-click="closeThisDialog()">Cancelar</button>\
              <button class="btn btn-primary float-right" ng-click="confirm()">Aceptar</button>\
            <div>\
        </div>',
        plain: true,
        className: 'ngdialog-theme-plain',
        appendClassName: 'ngdialog-systems-theme',
        closeByEscape: false,
        closeByDocument: false
      })
        .then(function () {
          Upload_Novedades();
        }, function () {
          console.log("No borrar")
        });
    }




  }
})();



// aggregates: [{
//               '<b>Devengos</b>': function (aggregatedValue, currentValue, column, record) {
//                 if (record['Devengo'] === "S") {
//                   return aggregatedValue + currentValue;
//                 }
//                 return aggregatedValue;
//             i  },
//               '<b>Deducidos</b>': function (aggregatedValue, currentValue, column, record) {
//                 if (record['Devengo'] === "No") {
//                   return aggregatedValue - currentValue;
//                 }
//                 return aggregatedValue;
//               },
//               '<b>Total</b>': function (aggregatedValue, currentValue, column, record) {
//                 return aggregatedValue + currentValue;
//               }
//             }]

