var App = function () {

   
    var handleInit = function () {
    
        var plugins = [
       'core',
       'touch-handler',

       'accordion',
       'button-set',
       'date-format',
       'calendar',
       'datepicker',
       'carousel',
       'countdown',
       'dropdown',
       'input-control',
       'live-tile',
       //'drag-tile',
       'progressbar',
       'rating',
       'slider',
       'tab-control',
       'table',
       'times',
       'dialog',
       'notify',
       'listview',
       'treeview',
       'fluentmenu',
       'hint'


        ];
        var Hostpath = $(location).attr('hostname');
        var Port = $(location).attr('port');
        var ISName = '';

      

        if (Hostpath == 'localhost') { // localhost

            
            ISName = 'ISThemeSelector'

            if (Port.length == 0) { // without port

                $("<link/>").attr('href', "/" + ISName + "/Content/css/global.min.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/" + ISName + "/Content/kendo/2013.1.319/kendo.common.min.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/" + ISName + "/Content/kendo/2013.1.319/kendo.dataviz.metro.min.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/" + ISName + "/Content/kendo/2013.1.319/kendo.dataviz.min.css").attr('rel', 'stylesheet').appendTo($('head'));

                $("<link/>").attr('href', "/" + ISName + "/Content/metroui/css/metro-bootstrap.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/" + ISName + "/Content/metroui/css/metro-bootstrap-responsive.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/" + ISName + "/Content/metroui/css/docs.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/" + ISName + "/Content/metroui/js/prettify/prettify.css").attr('rel', 'stylesheet').appendTo($('head'));


                $("<script/>").attr('src', "/" + ISName + "/Scripts/jquery.cookie.min.js ").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Scripts/important.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Scripts/app.js").appendTo($('head'));

               
                $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/jquery/jquery.widget.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/jquery/jquery.mousewheel.js").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/jquery/jquery.easing.1.3.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/holder/holder.js").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/prettify/prettify.js").appendTo($('head'));

                $("<script/>").attr('src', "/" + ISName + "/Scripts/kendo/2013.1.319/kendo.all.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Scripts/kendo/2013.1.319/kendo.aspnetmvc.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/" + ISName + "/Scripts/kendo.modernizr.custom.js").appendTo($('head'));


                $.each(plugins, function (i, plugins) {

                    $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/metro/metro-" + plugins + '.js').appendTo($('head'));
                });
               

            }
            else { //WITH PORT
               

                $("<link/>").attr('href', "/Content/css/global.min.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/Content/kendo/2013.1.319/kendo.common.min.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/Content/kendo/2013.1.319/kendo.dataviz.metro.min.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/Content/kendo/2013.1.319/kendo.dataviz.min.css").attr('rel', 'stylesheet').appendTo($('head'));

                $("<link/>").attr('href', "/Content/metroui/css/metro-bootstrap.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/Content/metroui/css/metro-bootstrap-responsive.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/Content/metroui/css/docs.css").attr('rel', 'stylesheet').appendTo($('head'));
                $("<link/>").attr('href', "/Content/metroui/js/prettify/prettify.css").attr('rel', 'stylesheet').appendTo($('head'));


                $("<script/>").attr('src', "/Scripts/jquery.cookie.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/Scripts/important.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/Scripts/app.js").appendTo($('head'));

                
                $("<script/>").attr('src', "/Content/metroui/js/jquery/jquery.widget.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/Content/metroui/js/jquery/jquery.mousewheel.js").appendTo($('head'));
                $("<script/>").attr('src', "/Content/metroui/js/jquery/jquery.easing.1.3.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/Content/metroui/js/holder/holder.js").appendTo($('head'));
                $("<script/>").attr('src', "/Content/metroui/js/prettify/prettify.js").appendTo($('head'));

                $("<script/>").attr('src', "/Scripts/kendo/2013.1.319/kendo.all.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/Scripts/kendo/2013.1.319/kendo.aspnetmvc.min.js").appendTo($('head'));
                $("<script/>").attr('src', "/Scripts/kendo.modernizr.custom.js").appendTo($('head'));

                //for (var i in plugins) {
                   
                //    $("<script/>").attr('src', "../Content/metroui/js/metro/metro-" + plugins[i] + '.js').appendTo($('head'));   
                //}

                $.each(plugins, function (i, plugins) {

                    $("<script/>").attr('src', "/Content/metroui/js/metro/metro-" + plugins + '.js').appendTo($('head'));
                });

            }

        }

        else {
            //if (Hostpath == '192.168.2.104' || '121.97.216.184:82' 
            var UrlPath = "http://" + Hostpath


            $("<link/>").attr('href', UrlPath + "/themestyle/Content/css/global.min.css").attr('rel', 'stylesheet').appendTo($('head'));
            $("<link/>").attr('href', UrlPath + "/themestyle/Content/kendo/2013.1.319/kendo.common.min.css").attr('rel', 'stylesheet').appendTo($('head'));
            $("<link/>").attr('href', UrlPath + "/themestyle/Content/kendo/2013.1.319/kendo.dataviz.metro.min.css").attr('rel', 'stylesheet').appendTo($('head'));
            $("<link/>").attr('href', UrlPath + "/themestyle/Content/kendo/2013.1.319/kendo.dataviz.min.css").attr('rel', 'stylesheet').appendTo($('head'));

            $("<link/>").attr('href', UrlPath + "/themestyle/Content/metroui/css/metro-bootstrap.css").attr('rel', 'stylesheet').appendTo($('head'));
            $("<link/>").attr('href', UrlPath + "/themestyle/Content/metroui/css/metro-bootstrap-responsive.css").attr('rel', 'stylesheet').appendTo($('head'));
            $("<link/>").attr('href', UrlPath + "/themestyle/Content/metroui/css/docs.css").attr('rel', 'stylesheet').appendTo($('head'));
            $("<link/>").attr('href', UrlPath + "/themestyle/Content/metroui/js/prettify/prettify.css").attr('rel', 'stylesheet').appendTo($('head'));


            $("<script/>").attr('src', UrlPath + "/themestyle/Scripts/jquery.cookie.min.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Scripts/important.min.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Scripts/app.js").appendTo($('head'));

            
            $("<script/>").attr('src', UrlPath + "/themestyle/Content/metroui/js/jquery/jquery.widget.min.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Content/metroui/js/jquery/jquery.mousewheel.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Content/metroui/js/jquery/jquery.easing.1.3.min.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Content/metroui/js/holder/holder.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Content/metroui/js/prettify/prettify.js").appendTo($('head'));

            

            $("<script/>").attr('src', UrlPath + "/themestyle/Scripts/kendo/2013.1.319/kendo.all.min.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Scripts/kendo/2013.1.319/kendo.aspnetmvc.min.js").appendTo($('head'));
            $("<script/>").attr('src', UrlPath + "/themestyle/Scripts/kendo.modernizr.custom.js").appendTo($('head'));

            $.each(plugins, function (i, plugins) {

                $("<script/>").attr('src', UrlPath + "/themestyle/Content/metroui/js/metro/metro-" + plugins + '.js').appendTo($('head'));
            });


        }

    }


    return {
        init: function () {
           
            handleInit();
        }
    };
}();






