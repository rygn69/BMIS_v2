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
var ISName = ''
$.each(plugins, function (i, plugin) {

    //alert(Port.length)

    if (Hostpath == 'localhost') { // localhost

        ISName = 'ISThemeSelector'
        if (Port.length == 0) { // without port
            $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/metro/metro-" + plugin + '.js').appendTo($('head'));
        }
        else { //WITH PORT
            $("<script/>").attr('src', "/Content/metroui/js/metro/metro-" + plugin + '.js').appendTo($('head'));
        }
       
    }
  
    else {
        var ISName = 'themeSelector'

        $("<script/>").attr('src', "/" + ISName + "/Content/metroui/js/metro/metro-" + plugin + '.js').appendTo($('head'));
        
    }
    
    
    
});
