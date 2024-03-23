/// <reference path="jquery-2.1.1.js" />
/// <reference path="jquery.signalR-2.1.1.js" />
/// <reference path="jquery-ui-1.11.1.js" />
/// <reference path="~/Scripts/fontFlex.js" />
/// <reference path="~/Scripts/jquery-1.10.2.js" />
/// <reference path="~/Scripts/easypiechart/jquery.easy-pie-chart.js" />
/// <reference path="~/Scripts/jquery.signalR-2.1.2.js" />
/*
################## Adem Balaban #####################
*/


if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

jQuery.fn.flash = function (color, duration) {
    var current = this.css('backgroundColor');
    var notifySound = $('audio');
    this.animate({ backgroundColor: 'rgb(' + color + ')' }, duration)
        .animate({ backgroundColor: current }, duration);
    notifySound[0].play();

}
/*
 * Font boyutunu içeriğe göre değiştirme çabaları
 * amaç : absürt uzun bir isim felan geldiğinde satırı kaydırmasın
 */
//jQuery.fn.FontSizeFlow = function(options) {
//    var settings = $.extend({
//            maximum: 9999,
//            minimum: 1,
//            maxFont: 9999,
//            minFont: 1,
//            fontRatio: 35
//        }, options),
//        changes = function(el) {
//            var $el = $(el),
//                elw = $el.width(),
//                width = elw > settings.maximum ? settings.maximum : elw < settings.minimum ? settings.minimum : elw,
//                fontBase = width / settings.fontRatio,
//                fontSize = fontBase > settings.maxFont ? settings.maxFont : fontBase < settings.minFont ? settings.minFont : fontBase;
//            $el.css('font-size', fontSize + 'px');
//        };
//    return this.each(function() {
//        var that = this;
//        $(this).resize(function() {
//            changes(that);
//        });
//    });~/
//}

$(function () {

    var ticker = $.connection.LineTicker, //Hub sınıf ismi

        $linesection             = $('#line'),
        $lineBody                = $($linesection),
        sectionTemplate          = '' +
            '<div data-lineid    ="{LineId}" class="{Layout}">' +
            '<audio data-soundid = "{LineId}"><source src="../Content/Sounds/Toney7.wav" /></audio>' +
                '<section class  ="panel panel-default" ><div class="bg-light dk wrapper">' +
                    '<div class  ="text-center m-b-n m-t-sm"><div class="panel-body">' +
            '<div class          ="clearfix text-center m-t"><div class="inline">' +
            '<div class          ="easypiechart easyPieChart" data-percent="{Percent}" data-line-width="5" data-bar-color="#4cc0c1" data-track-color="#f5f5f5" data-scale-color="false" data-size="130" data-line-cap="butt" data-animate="1000" style="width: 130px; height: 130px; line-height: 130px; margin-left: auto; margin-right: auto;">' +
            '<div class          ="thumb-lg"><img src="../Content/Images/avatar_default.jpg" class="img-circle"></div><canvas width="130" height="130"></canvas></div>' +
            '<div class          ="h4 m-t m-b-xs"> {Doctor} </div>' +
            '<small class        ="text-muted m-b">{Clinic}</small></div></div></div></div></div>' +
            '<div class          ="panel-body Patient bg-dark bk">' +
            '<div>' +
            '<span class="text-muted">Çağırılan Hasta:</span>' +
            '<span class="h3 block">{LineNumber}</span>' +
            '<span class="h3 block">{Patient}</span></div>' +
            '<div class="line pull-in"></div>' +
            '<div class="row m-t-sm">' +
            '<div class="col-xs-4">' +
            '<small class="text-muted block">Toplam</small>' +
            '<span>{Total}</span></div>' +
            '<div class="col-xs-4">' +
                '<small class="text-muted block">Muayene</small>' +
                '<span>{Examined}</span></div>' +
            '<div class="col-xs-4"><small class="text-muted block">Kalan</small>' +
                '<span>{LeftOver}</span></div></div></div></section></div>';


    function formatLine(line) {
        return $.extend(line, {
            
            Doctor: line.Doctor.Name,
            LineNumber: line.Patient.Id,
            Patient: line.Patient.Name,
            Layout: $.getUrlVar('layout') == 3 ? 'col-md-3' : $.getUrlVar('layout') == 4 ? 'col-md-4' : $.getUrlVar('layout') == 5 ? 'col-md-5' :  'col-md-3'

            //Price: stock.Price.toFixed(2),
            //PercentChange: (stock.PercentChange * 100).toFixed(2) + '%',
            //Direction: stock.Change === 0 ? '' : stock.Change >= 0 ? up : down,
            //DirectionClass: stock.Change === 0 ? 'even' : stock.Change >= 0 ? 'up' : 'down'
        });
    }

    function pieChart() {
        //<pieChart
        $('.easyPieChart').each(function () {
            var $this = $(this),
            $data = $this.data(),
            $step = $this.find('.step'),
            $targetValue = parseInt($($data.target).text()),
            $value = 0;
            $data.barColor || ($data.barColor = function ($percent) {
                $percent /= 100;
                return "rgb(" + Math.round(200 * $percent) + ", 200, " + Math.round(200 * (1 - $percent)) + ")";
            });
            $data.onStep = function (value) {
                $value = value;
                $step.text(parseInt(value));
                $data.target && $($data.target).text(parseInt(value) + $targetValue);
            };
            $data.onStop = function () {
                $targetValue = parseInt($($data.target).text());
                $data.update && setTimeout(function () {
                    $this.data('easyPieChart').update(100 - $value);
                }, $data.update);
            };
            $(this).easyPieChart($data);
        });

        //pieChart>
    }
    $.extend({
        getUrlVars: function () {
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < hashes.length; i++) {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        },
        getUrlVar: function (name) {
            return $.getUrlVars()[name];
        }
    });

    function init() {
        var displayGroup = $.getUrlVar('group');
        
        if (displayGroup == null) {
            displayGroup = 4;
        }

        return ticker.server.getAllLines(displayGroup).done(function (lines) {
            $lineBody.empty(); // alanları boşaltalım
            $.each(lines, function () {
                var line = formatLine(this);
                $lineBody.append(sectionTemplate.supplant(line));
            });
        });
    }

    $.extend(ticker.client, {
        updateLineValues: function (line) {
            var displayline = formatLine(line),
                $row = $(sectionTemplate.supplant(displayline)),
                bg = '255,148,148';
            if ($lineBody.find('div[data-lineid=' + line.LineId + ']').length) {
                $lineBody.find('div[data-lineid=' + line.LineId + ']')
                    .replaceWith($row);
                $row.find('.Patient').flash(bg, 2000);
            }

            //var notifysound = $row.find('audio[data-soundid=' + line.LineId + ']');
            //notifysound[0].play();
            pieChart();

        },

        displayOpened: function () {
            $("#open").prop("disabled", true);
            $("#close").prop("disabled", false);
            $("#reset").prop("disabled", true);
            //scrollTicker();
        },
        displayClosed: function () {
            $("#open").prop("disabled", false);
            $("#close").prop("disabled", true);
            $("#reset").prop("disabled", false);
            //stopTicker();
        },
        displayReset: function () {
            return init();
        }
    });
    $.connection.hub.start()
        .then(init)
        .then(function () {
            return ticker.server.getDisplayState();
        })
        .done(
            function (state) {
                if (state === 'Open') {
                    ticker.client.displayOpened();

                } else {
                    ticker.server.openDisplay();
                    ticker.client.displayOpened();
                }
                pieChart();
                $('h1').fontFlex(20, 60, 40);
                $('h2').fontFlex(20, 60, 40);
                $('h3').fontFlex(20, 60, 40);
                $('h4').fontFlex(20, 60, 40);
                $('h5').fontFlex(20, 60, 40);
                $('h6').fontFlex(20, 60, 40);
                //FontSizeFlow();
            });

});