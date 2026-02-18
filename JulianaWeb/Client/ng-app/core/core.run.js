(function() {
  'use strict';
  angular
    .module('app.core')
    .run(run);

  run.$inject = ['$state','$rootScope', 'AuthSvc', 'ngDialog', 'NotifySvc'];

  function run($state, $rootScope, AuthSvc, ngDialog, NotifySvc) {
    // $rootScope.$on('startDataLazyLoad', function(){
    //   console.log("Start data lazy load, asdsad")
    // });


    $rootScope.hasPermission = AuthSvc.hasPermission;
    $rootScope
      .$on('$stateChangeStart',
        function(event, toState, toParams, fromState, fromParams){
          $rootScope.loadingView = true;
          ngDialog.closeAll()
      });

    $rootScope
      .$on('$stateChangeSuccess',
        function(event, toState, toParams, fromState, fromParams){
          $rootScope.loadingView = false;
      });

      $rootScope
        .$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
          event.preventDefault();
          $rootScope.loadingView = false;
          console.log(error);
          NotifySvc.error();
        });
  }
})();
