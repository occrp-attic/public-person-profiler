/**
Core script to handle the entire theme and core functions
**/
var Quince = function () {
    //* END:CORE HANDLERS *//
    var breadCrumbHolder = "#breadcrumbHolder";
    var breadCrumbTemplate = Handlebars.compile($("#breadcrumbTemplate").html());
    var windowTemplate = Handlebars.compile($("#windowTemplate").html());
    var errorTemplate = Handlebars.compile($("#errorTemplate").html());
    var warningTemplate = Handlebars.compile($("#warningTemplate").html());
    var modalTemplate = Handlebars.compile($("#modalTemplate").html());
    var resBreakpointMd = Metronic.getResponsiveBreakpoint('md');
    var currentTab = 0;

    var _handleLinks = function () {
        $(document).on("click", ".quince-link", function (e) {
            e.preventDefault();
            Quince.linkManager.openLink(this);
        });
        $(document).on("click", ".quince-breadcrumb", function (e) {
            e.preventDefault();
            Quince.tabManager.activateTab({ windowId: $(this).data("window-id"), breadcrumbId: $(this).attr("id") });
        });
    }
    var _handleCloseTabs = function () {
        $(document).on("click", ".quince-tab-button .close", function (e) {
            e.preventDefault();
            Quince.tabManager.closeTab(this);
        })
    }
    var _handleFixedBodyHeight = function () {
        $(".quince-window.active").each(function () {
            $(".quince-body", this).height($(this).height() - $(".quince-title", this).height());
        });
    }
    var _calculateFixedContentViewportHeight = function () {
        var sidebarHeight = Metronic.getViewPort().height - $('.page-header').outerHeight() - 40 - $(".page-head").outerHeight();
        if ($('body').hasClass("page-footer-fixed")) {
            sidebarHeight = sidebarHeight - $('.page-footer').outerHeight();
        }
        return sidebarHeight;
    };
    var handleFixedContent = function () {
        var content = $('.quince-content');
        if (Metronic.getViewPort().width >= resBreakpointMd) {
            content.height(_calculateFixedContentViewportHeight());

        }
    }

    var _handleFixedContent = function () {
        Metronic.addResizeHandler(handleFixedContent);
        Metronic.addResizeHandler(_handleFixedBodyHeight);
    }

    return {

        //main function to initiate the theme
        init: function () {
            _handleLinks();
            _handleCloseTabs();
            handleFixedContent();
            _handleFixedContent();
        },
        appManager: {
            closeApp: function (uniqueId) {
                var app = $("[data-quinceid = '" + uniqueId + "'");
                Quince.appManager.closeModal(app);
            },
            closeModal: function (modal) {
                modal.modal("hide");
            }
        },
        linkManager: {
            openLink: function (link) {
                options = { href: $(link).attr("href"), target: $(link).data("target"), text: $(link).data("title") != undefined ? $(link).data("title") : $(link).text() }

                if (options.target === "tab") {
                    Quince.tabManager.openTab(options)
                }
                else if (options.target === "modal") {

                    Quince.modalManager.openModal(options);
                }
            }
        },
        tabManager: {
            openTab: function (options) {
                options.template = windowTemplate;
                currentTab++;
                options.windowId = "quinceWindow" + currentTab;
                options.id = currentTab;
                options.breadcrumbId = "bc" + options.windowId;
                Quince.tabManager.addTab(options);
                Quince.tabManager.activateTab(options);
            },
            addBreadcrumb: function (options) {
                $(breadCrumbHolder).append(breadCrumbTemplate(options));
                handleFixedContent();
            },
            addTab: function (options) {
                Quince.tabManager.addBreadcrumb(options);
                options._handleResponse = function (result) {
                    $(".quince-close", result).click(function () {
                        alert("to do: close tab");
                    });
                    $('.quince-content').append(result);
                    Quince.tabManager.activateTab(options);
                }
                Quince.requestManager.processOptions(options);
            },
            closeTab: function (button) {
                var windowId = $(button).data("window-id");
                $(windowId).remove();
                $(button).parent("div.quince-tab-button").remove();
                handleFixedContent();

            },
            activateTab: function (options) {
                $(".quince-window").hide();
                $(".quince-window").removeClass("active");
                $(".quince-breadcrumb").removeClass("green");
                $("#" + options.breadcrumbId).addClass("green");
                $("#" + options.windowId).show();
                $("#" + options.windowId).addClass("active");
                _handleFixedBodyHeight();
            }
        },
        modalManager:
            {
                current: {},
                openModal: function (options) {
                    options.template = modalTemplate;
                    options._handleResponse = function (result) {
                        $(".quince-close", result).attr("data-dismiss", "modal");
                        $('body').prepend(result);
                        result.modal();
                        $(result).on('hidden.bs.modal', function () {
                            $('body').remove(result);
                        })
                    }
                    Quince.requestManager.processOptions(options);
                },
                handleResponse: function (reponse) {

                }
            },
        requestManager: {
            processOptions: function (options) {
                var result = "";
                var jqxhr = $.get(options.href, function () {
                }).done(function (data) {
                    var obj = $(data);
                    var template = $(options.template(options));
                    var actionsTemplate = $("script.actions-template", obj);
                    if (actionsTemplate.length > 0) {
                        var actions = Handlebars.compile(actionsTemplate.html());
                        $(".quince-actions", template).html(actions());
                    }
                    $(".quince-body", template).html(obj);
                    var uniqueId = $(".unique-id", obj);
                    if (uniqueId.length > 0) {
                        template.attr("data-quinceid", uniqueId.val());
                    }
                    if (options._handleResponse) {
                        options._handleResponse(template);
                    }
                })
                 .fail(function () {
                     if (options._handleResponse) {
                         var template = $(options.template(options));
                         $(".quince-body", template).html(errorTemplate({ text: "Something went wrong, and the page could not be loaded" }));

                         options._handleResponse(template);
                     }
                 })
                 .always(function () {
                 });
            }
        },
        messageManager:
            {
                removeMessages: function (uniqueId) {
                    $("#quinceResult" + uniqueId + " .quince-note").remove();
                    $(window).trigger("resize");
                },
                processResponse: function (response, uniqueId) {
                    for (var i in response.Errors) {
                        $("#quinceResult" + uniqueId).prepend(errorTemplate({ text: response.Errors[i] }));
                    }
                    for (var i in response.Warnings) {
                        $("#quinceResult" + uniqueId).prepend(warningTemplate({ text: response.Warnings[i] }));
                    }
                    $(window).trigger("resize");
                }
            }
    };
}();