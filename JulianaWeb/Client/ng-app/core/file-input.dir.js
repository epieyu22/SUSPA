(function() {
  'use strict';
  angular
    .module('app.core')
    .directive('chooseFileButton', function() {
      return {
        restrict: 'A',
        link: function (scope, elem, attrs) {
            elem.bind('click', function() {
                  angular.element(document.querySelector('#' + attrs.chooseFileButton))[0].click();
                  });
                }
          };
        })
    .directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function(scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function(){
                scope.$apply(function(){
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);
})();


