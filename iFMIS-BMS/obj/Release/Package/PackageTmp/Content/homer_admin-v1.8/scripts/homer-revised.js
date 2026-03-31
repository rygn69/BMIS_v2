//// Initialize animate panel function
//$('.animate-panel').animatePanel();

// Function for collapse hpanel
$('.showhide').click(function (event) {
    event.preventDefault();
    var hpanel = $(this).closest('div.hpanel');
    var icon = $(this).find('i:first');
    var body = hpanel.find('div.panel-body');
    var footer = hpanel.find('div.panel-footer');
    body.slideToggle(300);
    footer.slideToggle(200);

    // Toggle icon from up to down
    icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
    hpanel.toggleClass('').toggleClass('panel-collapse');
    setTimeout(function () {
        hpanel.resize();
        hpanel.find('[id^=map-]').resize();
    }, 50);
});

// Function for close hpanel
$('.closebox').click(function (event) {
    event.preventDefault();
    var hpanel = $(this).closest('div.hpanel');
    hpanel.remove();
});

// Fullscreen for fullscreen hpanel
$('.fullscreen').click(function () {
    var hpanel = $(this).closest('div.hpanel');
    var icon = $(this).find('i:first');
    $('body').toggleClass('fullscreen-panel-mode');
    icon.toggleClass('fa-expand').toggleClass('fa-compress');
    hpanel.toggleClass('fullscreen');
    setTimeout(function () {
        $(window).trigger('resize');
    }, 100);
});

// Initialize tooltips
//$('.tooltip-demo').tooltip({
//    selector: "[data-toggle=tooltip]"
//});


