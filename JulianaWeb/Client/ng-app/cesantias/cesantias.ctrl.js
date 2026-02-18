(function() {
  'use strict';
  angular
    .module('app.solicitudes')
    .controller('CesantiasCtrl', CesantiasCtrl);

  CesantiasCtrl.$inject = ['NgTableParams', 'ngDialog', 'data'];
    /* @ngInject */
  function CesantiasCtrl(NgTableParams, ngDialog, data) {
    var vm = this;
    vm.openSolicitudDialog = openSolicitudDialog;
    activate();
    ////////////////
    function activate() {
      vm.data = data;
      vm.historicoGrid = new NgTableParams({}, {dataset: vm.data.historicoCesantias});
      vm.anticiposGrid = new NgTableParams({}, {dataset: vm.data.historicoAnticipos});
      vm.solPendientesGrid = new NgTableParams({}, {dataset: vm.data.solicitudesPendientes});
      vm.solAprobadasGrid = new NgTableParams({}, {dataset: vm.data.solicitudesAprobadas});
      // initGrid();
    }

    function openSolicitudDialog() {
      ngDialog.open({
        template: 'Client/ng-app/cesantias/new-solicitud.dialog.html',
        className: 'ngdialog-theme-plain',
        appendClassName : 'ngdialog-systems-theme',
        controller: 'VacacionesDialogCtrl',
        data: vm.data
      });
    }

  }
})();
