(function () {
    var callWithJQuery;

    callWithJQuery = function (pivotModule) {
        if (typeof exports === "object" && typeof module === "object") {
            return pivotModule(require("jquery"));
        } else if (typeof define === "function" && define.amd) {
            return define(["jquery"], pivotModule);
        } else {
            return pivotModule(jQuery);
        }
    };

    callWithJQuery(function($) {
        var c3r, d3r, plFmt, plFmtInt, plFmtPct, gcr, nf, r, tpl;
        nf = $.pivotUtilities.numberFormat;
        tpl = $.pivotUtilities.aggregatorTemplates;
        r = $.pivotUtilities.renderers;
        gcr = $.pivotUtilities.gchart_renderers;
        d3r = $.pivotUtilities.d3_renderers;
        c3r = $.pivotUtilities.c3_renderers;
        sub = $.pivotUtilities.subtotal_renderers;

        plFmt = nf({
            thousandsSep: " ",
            decimalSep: ","
        });
        plFmtInt = nf({
            digitsAfterDecimal: 0,
            thousandsSep: " ",
            decimalSep: ","
        });
        plFmtPct = nf({
            digitsAfterDecimal: 2,
            scaler: 100,
            suffix: "%",
            thousandsSep: " ",
            decimalSep: ","
        });
        $.pivotUtilities.locales.pl = {
            localeStrings: {
                renderError: "Wystąpił błąd podczas renderowania wyników PivotTable.",
                computeError: "Wystąpił błąd podczas obliczania wyników PivotTable.",
                uiRenderError: "Wystąpił błąd podczas renderowania UI PivotTable.",
                selectAll: "Zaznacz wszystko",
                selectNone: "Odznacz wszystkie",
                tooMany: "(za dużo do wylistowania)",
                filterResults: "Filtruj wartości",
                apply: "Zastosuj",
                cancel: "Anuluj",
                totals: "Suma",
                vs: "vs",
                by: "przez"
            },
            aggregators: {
                "Liczba": tpl.count(plFmtInt),
                "Liczba Unikatowych Wartości": tpl.countUnique(plFmtInt),
                "Lista Unikatowych Wartości": tpl.listUnique(", "),
                "Suma": tpl.sum(plFmt),
                "Całkowita Suma": tpl.sum(plFmtInt),
                "Średnia": tpl.average(plFmt),
                "Minimum": tpl.min(plFmt),
                "Maksimum": tpl.max(plFmt),
                "Pierwszy": tpl.first(plFmt),
                "Ostatni": tpl.last(plFmt),
                "Suma po Sumie": tpl.sumOverSum(plFmt),
                "80% Kres Dolny": tpl.sumOverSumBound80(true, plFmt),
                "80% Kres Górny": tpl.sumOverSumBound80(false, plFmt),
                "Suma jako Ułamek Całości": tpl.fractionOf(tpl.sum(), "total", plFmtPct),
                "Suma jako Ułamek w Wierszach": tpl.fractionOf(tpl.sum(), "row", plFmtPct),
                "Suma jako Ułamek w Kolumnach": tpl.fractionOf(tpl.sum(), "col", plFmtPct),
                "Liczba jako Ułamek Całości": tpl.fractionOf(tpl.count(), "total", plFmtPct),
                "Liczba jako Ułamek w Wierszach": tpl.fractionOf(tpl.count(), "row", plFmtPct),
                "Liczba jako Ułamek w Kolumnach": tpl.fractionOf(tpl.count(), "col", plFmtPct)
            },
            renderers: {
                "Tabela": $.pivotUtilities.renderers["Table"],
                "Tabela z Wykresem Słupkowym": $.pivotUtilities.renderers["Table Barchart"],
                "Mapa cieplna": $.pivotUtilities.renderers["Heatmap"],
                "Mapa cieplna po Wierszach": $.pivotUtilities.renderers["Row Heatmap"],
                "Mapa cieplna po Kolumnach": $.pivotUtilities.renderers["Col Heatmap"]
            }
        };
        if (gcr) {
            $.pivotUtilities.locales.pl.gchart_renderers = {
                "Wykres liniowy": gcr["Line Chart"],
                "Wykres słupkowy": gcr["Bar Chart"],
                "Skumulowany wykres słupkowy": gcr["Stacked Bar Chart"],
                "Wykres warstwowy": gcr["Area Chart"]
            };
        }
        if (d3r) {
            $.pivotUtilities.locales.pl.d3_renderers = {
                "Mapa": d3r["Treemap"]
            };
        }
        if (sub) {
            $.pivotUtilities.subtotal_renderers = {
                "Table With Subtotal": sub["Table With Subtotal"]
            };
        }
        if (c3r) {
            $.pivotUtilities.locales.pl.c3_renderers = {
                "Wykres liniowy": c3r["Line Chart"],
                "Wykres słupkowy": c3r["Bar Chart"],
                "Skumulowany wykres słupkowy": c3r["Stacked Bar Chart"],
                "Wykres warstwowy": c3r["Area Chart"]
            };
        }
        return $.pivotUtilities.locales.pl;
    });

}).call(this);

//# sourceMappingURL=pivot.pl.js.map

