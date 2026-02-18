using AngularTemplates.Bundling;
using AngularTemplates.Compile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace JulianaWeb
{
    public static class BundleConfig
    {
        public static void RegisterScriptBundles(BundleCollection bundles)
        {
            const string VENDOR_PATH = "~/Client/vendor/";
            const string ANGULAR_APP_ROOT = "~/Client/ng-app/";
            const string VIRTUAL_BUNDLE_VENDOR_PATH = VENDOR_PATH + "main";
            const string VIRTUAL_BUNDLE_VENDORCSS_PATH = VENDOR_PATH + "css";
            const string VIRTUAL_BUNDLE_PATH = ANGULAR_APP_ROOT + "main";
            const string VIRTUAL_BUNDLE_STYLE_PATH = ANGULAR_APP_ROOT + "css";

            var scriptBundle = new ScriptBundle(VIRTUAL_BUNDLE_PATH)
                .Include(ANGULAR_APP_ROOT + "app.module.js")
                .IncludeDirectory(ANGULAR_APP_ROOT,  "*.module.js", searchSubdirectories: true)
                .IncludeDirectory(ANGULAR_APP_ROOT, "*.js", searchSubdirectories: true);

            var VendorScriptBundle = new ScriptBundle(VIRTUAL_BUNDLE_VENDOR_PATH)
                .Include(VENDOR_PATH + "angular.min.js")
                .Include(VENDOR_PATH + "jquery-2.2.4.min.js")                               
                .IncludeDirectory(VENDOR_PATH, "*.js", searchSubdirectories: true);

            var styleBundle = new StyleBundle(VIRTUAL_BUNDLE_STYLE_PATH)
                .IncludeDirectory(ANGULAR_APP_ROOT, "*.css", searchSubdirectories: true);

            var vendorStyleBundle = new StyleBundle("~/VendorStyles")
                .IncludeDirectory(VENDOR_PATH + "css", "*.css", searchSubdirectories: true);

            var options = new TemplateCompilerOptions
            {
                ModuleName = "JWeb",
                Prefix = "",
            };

            var angularTemplates = new TemplateBundle("~/templates", options)
                .IncludeDirectory(ANGULAR_APP_ROOT, "*.html", searchSubdirectories: true);

            bundles.Add(angularTemplates);

            bundles.Add(scriptBundle);
            bundles.Add(VendorScriptBundle);
            bundles.Add(styleBundle);
            bundles.Add(vendorStyleBundle);

            BundleTable.EnableOptimizations = true;
        }
    }
}
