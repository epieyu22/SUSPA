angular.module('app.core')
.directive('permission', ['AuthSvc', function(AuthSvc) {
   return {
     restrict: 'A',
     transclude:true,
     scope: {
        permission: '='
     },

     link: function (scope, elem, attrs) {
        if (AuthSvc.hasPermission(scope.permission)) {
          elem.show();
        } else {
          elem.hide();
        }
     }
   }
}]);
