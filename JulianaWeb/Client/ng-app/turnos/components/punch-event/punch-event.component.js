(function () {
  'use strict';

  angular
    .module("app.turnos")
    .component("punchEvent", {
      templateUrl: "Client/ng-app/turnos/components/punch-event/punch-event.component.html",
      controller: controller,
      bindings: {
        event: "<"
      }
    })

  controller.$inject = ['$scope']

  function controller($scope) {
    let vm = this;

    vm.$onInit = () => {
      $scope.punchData = (vm.event || {}).punchData || {}
    }

    vm.openEditModal = openEditModal;

    function openEditModal() {
      console.log("Abriendo modal de edicion")
    }
  }

})();
