(function () {
  'use strict';
  angular
    .module('app.core')
    .factory('MenuSvc', MenuSvc);

  MenuSvc.$inject = [];
  /* @ngInject */
  function MenuSvc() {
    var consultasOptions = [];
    var comprobantesOptions = [];
    var dashboardOptions = [];
    var service = {
      consultasOptions: consultasOptions,
      comprobantesOptions: comprobantesOptions,
      dashboardOptions: dashboardOptions
    };
    activate();
   
    return service;

    function activate() {

      // Menu de comprobantes y certificados.
      comprobantesOptions.push({
        icon: "local_atm",
        title: "Comprobante de pago",
        url: "compropago",
        disabled: true,
        big: true,
        perimission: ['certificados.compropago']
      });
      comprobantesOptions.push({
        icon: "receipt",
        title: "Certificado laboral",
        url: "certlaboral2",
        disabled: true,
        big: false,
        perimission: ['certificados.certlab']
      });
      comprobantesOptions.push({
        icon: "receipt",
        title: "Plan Estratégico",
        url: "certlaboral3",
        disabled: true,
        big: false,
        perimission: ['certificados.otroscert']
      });
      comprobantesOptions.push({
        icon: "gavel",
        title: "Políticas Corporativas",
        url: "politicas",
        disabled: true,
        big: false,
        perimission: ['descargar.politicas']
      });
      comprobantesOptions.push({
        icon: "description",
        title: "Ingresos y Retenciones",
        url: "retefuente",
        disabled: false,
        big: false,
        perimission: ['certificados.retefuente']
      });
      // Menu de consultas y solicitudes.
      dashboardOptions.push({
        icon: "web",
        title: "Dashboard Payroll",
        url: "dashboardBi_payroll",
        disabled: true,
        big: false,
        perimission: ['dashboard.payroll']
      });
      dashboardOptions.push({
        icon: "web",
        title: "Dashboard RrHh",
        url: "dashboardBi_RRHH",
        disabled: true,
        big: false,
        perimission: ['dashboard.RrHh']
      });
      dashboardOptions.push({
        icon: "web",
        title: "Dashboard Financiero",
        url: "dashboardBi_financiero",
        disabled: true,
        big: false,
        perimission: ['dashboard.financiero']
      });
      // Menu de consultas y solicitudes.
      consultasOptions.push({
        icon: "beach_access",
        title: "Vacaciones",
        url: "vacaciones",
        disabled: true,
        big: true,
        perimission: ['consultas.vacaciones']
      });
      consultasOptions.push({
        icon: "description",
        title: "Cesantias",
        url: "cesantias",
        disabled: true,
        big: false,
        perimission: ['consultas.cesantias']
      });
      consultasOptions.push({
        icon: "accessible",
        /*title: "Incapacidades / Licencia de luto", MMS*/
        title: "Incapacidades",
        url: "incapacidades",
        disabled: true,
        big: false,
        perimission: ['consultas.incapacidades']
      });
      consultasOptions.push({
        icon: "timer_off",
        title: "Licencias",
        url: "licencias",
        disabled: true,
        big: false,
        perimission: ['consultas.licencias']
      });
      consultasOptions.push({
        icon: "history",
        title: "Historia Salarial",
        url: "histosalario",
        disabled: true,
        big: false,
        perimission: ['consultas.histosalario']
      });
      consultasOptions.push({
        icon: "local_hospital",
        title: "Seguridad Social",
        url: "segsocial",
        disabled: true,
        big: true,
        perimission: ['consultas.seguridadSocial']
      });
      consultasOptions.push({
        icon: "folder_shared",
        title: "Hoja de vida",
        url: "hojavida",
        disabled: true,
        big: false,
        perimission: ['consultas.hojavida']
      });
      consultasOptions.push({
        icon: "folder_shared",
        title: "Aprobar Hv",
        url: "hojavida.aprobar",
        disabled: true,
        big: false,
        perimission: ['aprobar.hojavida']
      });

      consultasOptions.push({
        icon: "access_alarm",
        title: "Time sheet Report",
        url: "timesheet",
        disabled: true,
        big: false,
        perimission: ['timesheet.horas']
      });

      consultasOptions.push({
        icon: "cake",
        title: "Cumpleaños de este mes",
        url: "cumpleanos",
        disabled: true,
        big: false,
        perimission: ['consultas.cumpleaños']
      });      
      consultasOptions.push({
        icon: "access_alarm",
        title: "Turnos",
        url: "turnos",
        disabled: true,
        big: false,
        perimission: ['consultas.turnos']
      });

      consultasOptions.push({
        icon: "access_alarm",
        title: "Registro de tiempos",
        url: "turnos.punch",
        disabled: true,
        big: false,
        perimission: ['consultas.turnos', 'consultas.vacaciones']
      });
      consultasOptions.push({
        icon: "access_alarm",
        title: "Turnos",
        url: "turnos",
        disabled: true,
        big: false,
        perimission: ['consultas.turnos']
      });

    }
  }
})();
