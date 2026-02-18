(function () {
  'use strict';

  angular
    .module("app.turnos")
    .component("punchSchedule", {
      templateUrl: `Client/ng-app/turnos/components/punch-schedule/punch-schedule.component.html`,
      controller: controller,
      bindings: {
        events: "<"
      }
    })

  controller.$inject = ["AuthSvc", '$scope', '$compile']

  function controller(authSvc, $scope, $compile) {
    let vm = this;
    let calendar = {}
    let hasDynamicRender = false;

    vm.$onInit = () => {

      calendar = $('#calendar').fullCalendar({
        header: {
          center: 'title'
        },
        validRange: {
          start: '2021-04-01',
          end: '2021-12-31'
        },
        events: eventFetcher,
        eventRender: function (event, element) {
          debugger
          var el = element.html();
          let template = '<punch-event event="event"></punch-event>'
          let $childScope = $scope.$new(true)

          $childScope.event = event;
          let newelement = $($compile(template)($childScope));
          return newelement;
        },
        eventAfterAllRender: () => {

          $("punch-event").parents(".fc-row").css({
            height: "auto"
          });

          $(".fc-scroller.fc-day-grid-container").css({ height: 'auto'})

        }
      });


      $scope.$watch(() => vm.events, () => {
        
        calendar.fullCalendar('refetchEvents');

      })

    }

    function eventFetcher(start, end, timezone, callback) {
      
      callback(
        mapEvents(vm.events) || []
      );
    }

    function mapEvents(events) {
      debugger
      return events.map(e => ({
        title: e.Fecha_Turno,
        start: e.Fecha_Turno,
        employeeId: e.Cod_Empleado,
        punchData: {
          checkinHour: e.Hora_Ent,
          checkoutHour: e.Hora_Sal,
          punchCheckinHour: e.Hora_Ent_Real,
          punchCheckoutHour: e.Hora_Sal_Real,
          hasPunch: !!e.Hora_Ent_Real,
          hasTurn: !!e.Hora_Ent,
          delayedHours: e.Tiempo_Retardo,
          workedHours: e.Tiempo_Laborado
        }
      }));
    }

  }

})();
