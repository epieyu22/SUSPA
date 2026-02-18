(function () {

  'use strict';

  angular
    .module('app.turnos')
    .component('punchTrigger', {
      template: `<button ng-disabled="$ctrl.disabled" ng-click="$ctrl.onClick()"
                         type="button"
                         class="btn"
                         ng-class="{ 'btn-primary' : $ctrl.isPunchIn, 'btn-danger': !$ctrl.isPunchIn }" >
                  <ng-transclude></ng-transclude>
                  <i ng-class="{ 'la-hourglass-start' : $ctrl.isPunchIn, 'la-hourglass-end': !$ctrl.isPunchIn }"
                     class="las"></i>
                 </button>`,
      bindings: {
        onClick: '&',
        type: '<',
        disabled: '='
      },
      transclude: true,
      controller: controller
    });

  controller.$inject = [];

  function controller() {
    let vm = this;

    vm.$onInit = () => {
      vm.isPunchIn = vm.type === "punch-in";
    };
  }

})();
