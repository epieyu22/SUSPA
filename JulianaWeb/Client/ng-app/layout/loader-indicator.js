(function () {

  let app = angular.module('app.core');
  
  app.component("loaderIndicator", {
    template: `<div class="loading" ng-if="$ctrl.loading">
                <h4 class="m-y-1">Cargando</h4>
                <div class="spinner blue">
                  <div class="bounce1"></div>
                  <div class="bounce2"></div>
                  <div class="bounce3"></div>
                </div>
              </div>`,
    controller: controller
  });

  app.factory("loaderSvc", ['$rootScope', ($rootScope) => {
    return {
      show: show,
      hide: hide
    };

    function show() {
      $rootScope.$broadcast('loader.show');
    }

    function hide() {
      $rootScope.$broadcast('loader.hide');
    }

  }])

  controller.$inject = ["$rootScope"]

  function controller($rootScope) {
    let vm = this;

    vm.loading = false;

    $rootScope.$on("loader.show", () => vm.loading = true);
    $rootScope.$on("loader.hide", () => vm.loading = false);
  }

})();
