(function() {
  'use strict';

  angular
    .module('app.core')
    .controller('EmpleadosCtrl', EmpleadosCtrl);

  EmpleadosCtrl.$inject = ['$scope', 'ngDialog'];
  function EmpleadosCtrl($scope, ngDialog) {
    $scope.data = $scope.ngDialogData;

    activate();

    ////////////////

    function activate() { }
  }
})();
