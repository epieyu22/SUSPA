(function() {
'use strict';

  angular
    .module('app.solicitudes')
    .controller('CalendarioVacacionesCtrl', CalendarioVacacionesCtrl);

  CalendarioVacacionesCtrl.$inject = ['NgTableParams', '$rootScope', '$state', 'SolicitudesSvc', 'AuthSvc',  'data'];
  function CalendarioVacacionesCtrl(NgTableParams, $rootScope, $state, SolicitudesSvc, AuthSvc, data) {
    var vm = this;
    vm.Exportar_Excel = Exportar_Excel;
    vm.viewmodel = {}

    vm.datetimeInput = {
      culture: 'es-CO',
      height: '38px',
      width: '100%',
    };

    activate();

    ////////////////


    function activate(){
      console.log(data);
    }



    function generateCalendar() { 
      vm.data = data;
      vm.solVacacionesGrid = new NgTableParams({}, {dataset: vm.data.vacaciones});
      vm.solCesantiasGrid  = new NgTableParams({}, {dataset: vm.data.cesantias});

      var Fec_Actual = new Date();
      vm.viewmodel.Desde = new Date(Fec_Actual.getFullYear(), Fec_Actual.getMonth(), 1);
      vm.viewmodel.Hasta = new Date(Fec_Actual.getFullYear(), Fec_Actual.getMonth()+1, 0);

      vm.events = [];
      for(var i = 0; i < data.vacaciones.length; i++){
        var e = data.vacaciones[i];
        var bgColor = "#ffc107";
        var textColor = '#fff';
        switch(e.Estado){
          case 'A': bgColor = "#3F51B5" ; break;
          case 'D': bgColor = "#E91E63" ; break;
          case 'R': bgColor = "#F44336" ; break;
          case 'P': 
            bgColor = "#ffc107" ; 
            textColor = "#333"; 
            break;
        }

        vm.events.push({
          title: e.Cod_Solicitud + " - " + e.empleado.Empleado,
          start: e.Fec_Salida,
          end: e.Fec_Llegada,
          eventTextColor: textColor,
          eventBorderColor: bgColor,
          backgroundColor: bgColor,
          allDay: true,
          Cod_Solicitud:  e.Cod_Solicitud
        })
      }

      for(var i = data.historico.length - 1; i > 0; i--){
        var e = data.historico[i];
        var title = e.Tipo_Vacaciones == "D" ? "$ " +  e.Empleado : e.Empleado;
        vm.events.push({
          title: title,
          start: e.Desde,
          end: e.Hasta,
          eventTextColor: '#fff',
          eventBorderColor: "#009688",
          backgroundColor: "#009688",
          allDay: true
        })
      }
      
      $('#calendar').fullCalendar({
        events: vm.events,
        eventLimit: true,
        eventClick: evenClick 
      });



    }
  

    function Exportar_Excel(){
      $rootScope.generating = true;
      SolicitudesSvc
        .Exportar_Excel()
        .then(function(){
            $rootScope.generating = false;
          },
          function(error){
            $rootScope.generating = false;
            NotifySvc.error();
          });
    }


    function evenClick(data){
      if(data.Cod_Solicitud){
        $state.go('solicitud', {Cod_Solicitud: data.Cod_Solicitud});
      }
    }



  }
})();
