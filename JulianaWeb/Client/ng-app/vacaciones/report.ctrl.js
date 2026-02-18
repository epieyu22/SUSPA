(function() {
  'use strict';
  angular.module('app.vacaciones').controller('ReportVacacionesCtrl', ReportVacacionesCtrl);

  ReportVacacionesCtrl.$inject = ['$rootScope', '$state', 'NotifySvc', 'NgTableParams', 'VacacionesSVC', 'Empresas'];
  function ReportVacacionesCtrl($rootScope, $state, NotifySvc, NgTableParams, VacacionesSVC, Empresas) {
    var vm = this;
    vm.data = {};

    vm.loading = false;

    vm.report = {};

    vm.datetimeInput = {
      culture: 'es-CO',
      height: '28px',
      width: '100%'
    };

    vm.Get_Reporte_Vacaciones = Get_Reporte_Vacaciones;
    vm.Solicitudes_2_Excel = Solicitudes_2_Excel;
    vm.Reporte_2_Excel = Reporte_2_Excel;
    vm.generateCalendar = generateCalendar;
    vm.Mail_Solicitudes_Pendientes = Mail_Solicitudes_Pendientes;

    activate();

    function activate() {
      vm.data.Empresas = Empresas;
      if (Empresas.length) vm.report.Empresa = Empresas[0].BaseDatos.trim();
      vm.report.Filtro = 'None';
    }

    function Get_Reporte_Vacaciones() {
      vm.loading = true;
      VacacionesSVC.Get_Reporte_Vacaciones(vm.report).then(function(data) {
        vm.data.his = data.his;
        vm.data.solicitudes = data.solicitudes;
        vm.solVacacionesGrid = new NgTableParams({}, {dataset: data.solicitudes});
        vm.ResumenGrid = new NgTableParams({}, {dataset: data.his});
        vm.loading = false;
        generateCalendar();
      });
    }

    function generateCalendar() {
      var Fec_Actual = new Date();
      // vm.viewmodel.Desde = new Date(Fec_Actual.getFullYear(), Fec_Actual.getMonth(), 1);
      // vm.viewmodel.Hasta = new Date(Fec_Actual.getFullYear(), Fec_Actual.getMonth()+1, 0);

      vm.events = [];
      for (var i = 0; i < vm.data.solicitudes.length; i++) {
        var e = vm.data.solicitudes[i];
        var bgColor = '#ffc107';
        var textColor = '#fff';
        switch (e.Estado) {
          case 'Aprobada':
            bgColor = '#3F51B5';
            break;
          case 'Pagada':
            bgColor = '#3F51B5';
            break;
          case 'Eliminada':
            bgColor = '#E91E63';
            break;
          case 'Rechazada':
            bgColor = '#F44336';
            break;
          case 'Pendiente':
            bgColor = '#ffc107';
            textColor = '#333';
            break;
        }

        vm.events.push({
          title: e.Cod_Solicitud + ' - ' + e.empleado.Empleado,
          start: e.Fec_Salida,
          end: e.Fec_Llegada,
          eventTextColor: textColor,
          eventBorderColor: bgColor,
          backgroundColor: bgColor,
          allDay: true,
          Cod_Solicitud: e.Cod_Solicitud
        });
      }

      for (var i = 0; i < vm.data.his.length; i++) {
        var element = vm.data.his[i];
      }

      $('#calendar').fullCalendar({
        events: vm.events,
        eventLimit: true,
        eventClick: evenClick
      });
    }

    function evenClick(data) {
      if (data.Cod_Solicitud) {
        $state.go('solicitud', {Cod_Solicitud: data.Cod_Solicitud});
      }
    }

    function Solicitudes_2_Excel() {
      $rootScope.generating = true;
      VacacionesSVC.Solicitudes_2_Excel(vm.report).then(function() {
        $rootScope.generating = false;
      });
    }

    function Reporte_2_Excel() {
      $rootScope.generating = true;
      VacacionesSVC.Reporte_2_Excel(vm.report).then(function() {
        $rootScope.generating = false;
      });
    }

    function Mail_Solicitudes_Pendientes() {
      $rootScope.sending = true;
      VacacionesSVC.Mail_Solicitudes_Pendientes(vm.report).then(
        function() {
          $rootScope.sending = false;
          NotifySvc.success('Información enviada con éxito');
        },
        function() {
          $rootScope.sending = false;
          NotifySvc.error();
        }
      );
    }

    vm.getEstado = function() {
      return [
              { id: 'Aprobada', title: 'Aprobada' },
              { id: 'Pagada', title: 'Pagada' },
              { id: 'Pendiente Aprobación', title: 'Pendiente Aprobación' },
              { id: 'Pendiente Desaprobación', title: 'Pendiente Desaprobación' },
              { id: 'Rechazada', title: 'Rechazada' },
              { id: 'Eliminada', title: 'Eliminada' },
            ];
    };

    vm.getTipoSolicitud = function () {
      return [
              { id: 'Vacaciones', title: 'Vacaciones' },
              { id: 'Incapacidad', title: 'Incapacidad' },
              { id: 'Licencia', title: 'Licencia' }
            ];
    }
  }
})();
