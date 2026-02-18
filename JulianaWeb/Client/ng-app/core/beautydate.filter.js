(function() {
    'use strict';
    angular
        .module('app.core')
        .filter('beautydate', beautydate);
    function beautydate() {
        return beautydateFilter;
        ////////////////
        function beautydateFilter(date) {
            return date.replace(/(\d\d\d\d)(\d\d)(\d\d)/g, '$1/$2/$3');
        }
    }
})();
