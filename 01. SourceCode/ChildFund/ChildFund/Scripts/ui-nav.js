$(function () {
    "use strict";
    /**
     * Xử lý active menu trên menu dọc
     */
    var loc = window.location.href;
    $(".page-sidebar ul a").each(function () {
        if (loc.indexOf($(this).attr("href")) != -1) {
            var liParent = $(this).parent();
            liParent.addClass("active");
            liParent.parent('ul').toggleClass("open");
            liParent.parent('ul').parent('li').toggleClass("active");
            liParent.parent('ul').parent('li').find(".arrow").html("keyboard_arrow_down");
        }
    });

    var el = $('.page-sidebar ul');
    el.on('click', 'a', function (e) {
        var _this = $(this);
        _this.parent().siblings(".active").toggleClass('active');
        _this.parent().toggleClass('active');
        _this.next().is('ul') && _this.find(".arrow").toggleClass("open");

        if (_this.next().is('ul') && _this.find(".arrow").html() == "keyboard_arrow_right") {
            _this.find(".arrow").html("keyboard_arrow_down") && e.preventDefault();
        } else if (_this.next().is('ul') && _this.find(".arrow").html() == "keyboard_arrow_down") {
            _this.find(".arrow").html("keyboard_arrow_right") && e.preventDefault();
        }
    });

    /**
     * Xử lý Actions trên blog Section
     */
    var elSectionActions = $('.content-wrapper');
    elSectionActions.on('click', '.box header .actions i.box_toggle', function (e) {
        var _this = $(this);
        var txt = _this.html();
        if (txt == "expand_more") {
            _this.html("expand_less") && e.preventDefault();
        } else if (txt == "expand_less") {
            _this.html("expand_more") && e.preventDefault();
        }
        _this.parent().parent().parent().toggleClass("collapsed");
    });

    elSectionActions.on('click', '.box header .actions i.box_close', function (e) {
        var _this = $(this);
        _this.parent().parent().parent().addClass("hide").hide() && e.preventDefault();
    });

    var elSidebarToggle = $('.sidebar-toggle-wrap');
    elSidebarToggle.on('click', 'a.sidebar_toggle', function (e) {
        var _pageSidebar = $('.page-sidebar');
        var _mainContent = $('#main-content');
        var _pageTopbar = $('.page-topbar');
        if (_pageSidebar.hasClass('expandit')) {
            _pageSidebar.removeClass('expandit').addClass("collapseit") && e.preventDefault();
            _mainContent.addClass('sidebar_shift');
            _pageTopbar.addClass('sidebar_shift');
        } else if (_pageSidebar.hasClass('collapseit')) {
            _pageSidebar.removeClass('collapseit').addClass("expandit") && e.preventDefault();
            _mainContent.removeClass('sidebar_shift');
            _pageTopbar.removeClass('sidebar_shift');
        }
    });
});