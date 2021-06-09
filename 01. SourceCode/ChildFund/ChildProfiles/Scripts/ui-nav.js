$(function () {
    "use strict";
    /**
     * Xử lý active menu trên menu dọc
     */
    var lstMenu = [
        { pUrl: '/ProfileNew/ProfileWard', cUrl: '/ProfileNew/FormProfileNew' }
        , { pUrl: '/ProfileNew/ProfileWard', cUrl: '/profilesupdate/formupdateprofiles' }

        , { pUrl: '/ProfileNew/ProfileConfim', cUrl: '/ProfileNew/DetailProfileConfim' }
        , { pUrl: '/ProfileNew/ProfileProvince', cUrl: '/ProfileNew/DetailProfile' }
        , { pUrl: '/ProfilesUpdate/ProfileProvince', cUrl: '/ProfilesUpdate/CompareProfile' }
        , { pUrl: '/ProfilesUpdate/ProfileWard', cUrl: '/ProfilesUpdate/CompareProfile' }

        , { pUrl: '/ImageLibrary/Index', cUrl: '/ImageLibrary/ViewListUpload' }
        , { pUrl: '/AreaUser/Index', cUrl: '/AreaUser/CreateAreaUser' }
        , { pUrl: '/AreaUser/Index', cUrl: '/AreaUser/UpdateAreaUser' }
    ]; 
    var loc = window.location.pathname;
    var locArray = loc.split('/');
    if (locArray.length > 3) {
        loc = "/" + locArray[1] + "/" + locArray[2];
    }
    var href = '';
    var liParent = null;
    var isFocusParent = false;
    $(".page-sidebar ul a").each(function () {
        href = $(this).attr("href");
        if (loc.toLowerCase() === href.toLowerCase()) {
            isFocusParent = true;
            liParent = $(this).parent();
            liParent.addClass("active");
            liParent.parent('ul').toggleClass("open");
            liParent.parent('ul').parent('li').toggleClass("active");
            liParent.parent('ul').parent('li').find(".arrow").html("keyboard_arrow_down");
        }
    });
    if (isFocusParent === false) {
        $(".page-sidebar ul a").each(function () {
            href = $(this).attr("href");
            for (var i = 0; i < lstMenu.length; i++) {
                if (loc.toLowerCase() === lstMenu[i].cUrl.toLowerCase() && lstMenu[i].pUrl.toLowerCase() === href.toLowerCase()) {
                    liParent = $(this).parent();
                    liParent.addClass("active");
                    liParent.parent('ul').toggleClass("open");
                    liParent.parent('ul').parent('li').toggleClass("active");
                    liParent.parent('ul').parent('li').find(".arrow").html("keyboard_arrow_down");
                }
            }
        });
    }
    function CheckUrl(href) {

    }
    var el = $('.page-sidebar ul');
    el.on('click', 'a', function (e) {
        var _this = $(this);
        _this.parent().siblings(".active").toggleClass('active');
        _this.parent().toggleClass('active');
        _this.next().is('ul') && _this.find(".arrow").toggleClass("open");

        if (_this.next().is('ul') && _this.find(".arrow").html() === "keyboard_arrow_right") {
            _this.find(".arrow").html("keyboard_arrow_down") && e.preventDefault();
        } else if (_this.next().is('ul') && _this.find(".arrow").html() === "keyboard_arrow_down") {
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
        if (txt === "expand_more") {
            _this.html("expand_less") && e.preventDefault();
        } else if (txt === "expand_less") {
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
        if (_pageSidebar.hasClass('expandit')) {
            _pageSidebar.removeClass('expandit').addClass("collapseit") && e.preventDefault();
            _mainContent.addClass('sidebar_shift');
        } else if (_pageSidebar.hasClass('collapseit')) {
            _pageSidebar.removeClass('collapseit').addClass("expandit") && e.preventDefault();
            _mainContent.removeClass('sidebar_shift');
        }
    });
});