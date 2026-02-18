(function (win, $) {
    "use strict";

    Function.prototype.toBind = function (thisArg, ...oldArgs) {
        
        if (typeof this !== "function") {
            return;
        }

        var boundTarget = this;

        if (thisArg) {
            thisArg.isBinded = true;
        }

        return function bound(...args) {
            return boundTarget.apply(thisArg, oldArgs.concat(args));
        };
    }


    /**
    * @description: Utils for common tasks
    * @author: Jeancarlo Fontalvo
    */
    var utils = {
        inject: inject
    };

    init();

    function init() {
        
        (["Array", "String"]).forEach((item) => {
            var clase = win[item];

            inject(clase, equalToMany);
        });

        // Bind decorator
        
    }

    /**
     * Allows to compare many values for an object
     * @param {any} object
     */
    function equalToMany(object, ...args) {
        
        var self;

        if (this) {
            self = this;
            args.unshift(object);
        }
        else {
            self = object;
        }
        
        let last = args.pop();

        let YuO = true;

        if (typeof last === "boolean") {
            YuO = last;
        }
        else {
            args.push(last);
        }

        return args.reduce(function (estado, item) {
            return estado || item == self;
        }, false)
    }

    /**
     * Basic Injector for objects and functions
     * @param {Function|Object} object
     */
    function inject(object, ...args) {
        
        if (!equalToMany((typeof object), "object", "function"))
            throw "UTILS (Inject): Cannot inject stuff to primitive types";

        args.reduce((obj, arg) => {
            
            if (!arg.name)
                throw "UTILS (Inject): Cannot inject stuff without and identifier";

            obj.prototype[arg.name] = arg;

            return obj;
        }, object)
    }

    win.utils = utils;

})(window, jQuery);
