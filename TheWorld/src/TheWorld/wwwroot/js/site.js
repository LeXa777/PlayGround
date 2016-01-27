// site.js
(function () {
    var $sidebarAndWrapper = $("#sidebar,#wrapper");

    var icon = $("#sidebarToggle i.fa");

    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            icon.removeClass("fa fa-angle-left");
            icon.addClass("fa fa-angle-right");
        } else {
            icon.removeClass("fa fa-angle-right");
            icon.addClass("fa fa-angle-left");
        }
    });
})();