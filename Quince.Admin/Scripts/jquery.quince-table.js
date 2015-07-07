(function ($) {

    $.fn.quinceTable = function (options) {

        // Establish our default settings
        var settings = $.extend({
            
        }, options);
        var init = function (table)
        {
            table.addClass("quince-table");
        }
        return this.each(function () {
            init($(this));
        });

    };

}(jQuery));