

$('.theme-colors > ul > li').click(function () {
    var styleColor = $(this).attr("data-style");

    setColorStyle(styleColor);
});


function setColorStyle(styleColor) {

    var Hostpath_ = $(location).attr('hostname');
    var Port_ = $(location).attr('port');
    var ISName_ = 'ISThemeSelector'

    
    if (themeLocChecker() == "local") // localhost
    {
        
        if (Port_.length == 0) // WITHOUT PORT
        {
            $('body').css({ 'background': "url(" + "/" + ISName_ + "/Content/images/bg36.gif"+ ")" });

            $('#style_color').attr("href","/" + ISName_ +  "/Content/themes/" + styleColor + "/" + styleColor + ".css");
            $('#style_kendo').attr("href", "/" + ISName_ +  "/Content/themes/" + styleColor + "/kendo-" + styleColor + ".css");
        }
        else { // WITHPORT
            $('body').css({ 'background': 'url("../Content/images/bg36.gif")' });

            $('#style_color').attr("href", "../Content/themes/" + styleColor + "/" + styleColor + ".css");
            $('#style_kendo').attr("href", "../Content/themes/" + styleColor + "/kendo-" + styleColor + ".css");
        }
       

    }
    else if (themeLocChecker() == "server") // 192
    {
       
        $('body').css({ 'background': 'url("http://192.168.2.104/webAppsTheme/themes/images/bg36.gif")' });

        $('#style_color').attr("href", "http://192.168.2.104/webAppsTheme/themes/" + styleColor + "/" + styleColor + ".css");
        $('#style_kendo').attr("href", "http://192.168.2.104/webAppsTheme/themes/" + styleColor + "/kendo-" + styleColor + ".css");
    }
    else { // PUBLIC
        $('body').css({ 'background': 'url("http://121.97.216.184:82/webAppsTheme/themes/images/bg36.gif")' });

        $('#style_color').attr("href", "http://121.97.216.184:82/webAppsTheme/themes/" + styleColor + "/" + styleColor + ".css");
        $('#style_kendo').attr("href", "http://121.97.216.184:82/webAppsTheme/themes/" + styleColor + "/kendo-" + styleColor + ".css");
    }


    $.cookie('style_color', styleColor);
    $.cookie('style_kendo', styleColor);
}

function GetHeight(a) {
    var newHeight = $('.' + a).outerHeight(true);
    return (
        newHeight
        );
}

function themeLocChecker() {
    var currentUrl = window.location.host;
    var fromServer;

    if (currentUrl == '192.168.2.104') {

        fromServer = "server"
    }
    if (currentUrl == '121.97.216.184:82') {
        fromServer = "public"

    }
    else {
        if (currentUrl.indexOf('local') == 0) {

            fromServer = "local"
        }

    }
    return fromServer;
}
