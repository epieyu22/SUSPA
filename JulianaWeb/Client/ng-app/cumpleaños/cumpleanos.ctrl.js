(function () {
  'use strict';

  angular
    .module('app.cumpleanos')
    .controller('CumpleanosCtrl', CumpleanosCtrl);

  CumpleanosCtrl.$inject = ['ngDialog', '$state', 'data', 'NotifySvc', 'CumpleanosSvc','$scope'];
  function CumpleanosCtrl(ngDialog, $state, data, NotifySvc, CumpleanosSvc , $scope) {
    // Variables locales
    var vm = this;

    activate();
    
    function activate() {
      vm.data = data;
    }
  // @Function
    // Description  : Triggered while displaying expiry date
    $scope.formatDate = function(date){
          var dateOut = new Date(date);
          return dateOut;
    };
  }
})();
