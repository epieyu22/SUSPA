(function () {

  'use strict';
  
  angular
    .module('app.turnos')
    .controller('punchManagerController', punchCalendarController);

  punchCalendarController.$inject = ["TurnosSvc", 'data', 'AuthSvc', 'loaderSvc', '$timeout']

  function punchCalendarController(turnService, sharedData, authService, loaderSvc, $timeout) {

    let vm = this;

    vm.filters = {};
    vm.sharedData = sharedData;
    vm.events = []
    vm.flags = {
      processing: false,
      hasPunchIn: false,
      hasPunchOut: false
    }
    vm.punchOfDay = {};

    vm.getTimeControls = getTimeControls;
    vm.registerPunch = registerPunch;

    init();
    
    function init() {
      getTimeControls();
    }

    function getTimeControls() {
      loaderSvc.show();

      let status = turnService.getTimeControls({
        employeeId: authService.data.employeeId
      })

      $timeout(() => {

        status
          .then((punches) => vm.events = punches)
          .then(processPunchOfDay)
          .then(loaderSvc.hide);

      }, 1000);
    }

    function registerPunch(type) {
      vm.flags.processing = true;


      let status = turnService.registerPunch({
        punchType: type,
        employeeId: authService.data.employeeId
      });

      status.then(() => {

        vm.flags.processing = false;
        vm.flags[type === 'punch-in' ? 'hasPunchIn' : 'hasPunchOut'] = true;

        getTimeControls();

      });

    }

    function processPunchOfDay() {
      let today = moment();
      
      vm.punchOfDay = vm.events.find(t => moment(t.Fecha_Turno_Desde).format("MM-DD-YYYY") === today.format("MM-DD-YYYY"))

      if (!vm.punchOfDay)
        return;

      vm.flags.hasPunchIn = !!vm.punchOfDay.Hora_Ent_Real;
      vm.flags.hasPunchOut = !!vm.punchOfDay.Hora_Sal_Real;
    }

  }

})();
