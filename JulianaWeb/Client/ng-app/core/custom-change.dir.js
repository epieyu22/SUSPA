(function() {
    'use strict';
    angular
      .module('app.core')
      .directive('customOnChange', customOnChange);

    function customOnChange(){
      return {
        restrict: 'A',
        link: function (scope, element, attrs) {
          var onChangeFunc = scope.$eval(attrs.customOnChange);
          element.bind('change', onChangeFunc);
        }
      };
    }

})();
