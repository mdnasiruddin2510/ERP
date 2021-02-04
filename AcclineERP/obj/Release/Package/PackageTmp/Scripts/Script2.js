var handleDataTableButtons = function () {
    //"use strict";
    //var table = $('#datatable-buttons').not('.initialized').addClass('initialized').show().DataTable({
    //    //0 !== $("#datatable-buttons").length && $("#datatable-buttons").DataTable({
    //        dom: "Bfrtip",
    //        buttons: [
    //            {
    //                extend: "copy",
    //                className: "btn-sm"
    //            }, {
    //                extend: "csv",
    //                className: "btn-sm"
    //            }, {
    //                extend: "excel",
    //                className: "btn-sm"
    //            }, {
    //                extend: "pdf",
    //                className: "btn-sm"
    //            }, {
    //                extend: "print",
    //                className: "btn-sm"
    //            }
    //        ],
    //        "order": [[0, 'asc']],
    //        "stateSave": false,
    //        "stateDuration": 60 * 60 * 24 * 365,
    //        "displayLength": 100,
    //        "sScrollX": "100%",
    //        "drawCallback": function (settings) {
    //            debugger;
    //            var api = this.api();
    //            var rows = api.rows({ page: 'current' }).nodes();
    //            var last = null;
    //            var colonne = api.row(0).data().length;
    //            var totale = new Array();
    //            totale['Totale'] = new Array();
    //            var groupid = -1;
    //            var subtotale = new Array();


    //            api.column(0, { page: 'current' }).data().each(function (group, i) {
    //                if (last !== group) {
    //                    groupid++;
    //                    $(rows).eq(i).before(
    //                        '<tr class="group"><td>' + group + '</td></tr>'
    //                    );
    //                    last = group;
    //                }


    //              var  val = api.row(api.row($(rows).eq(i)).index()).data();      //current order index
    //                $.each(val, function (index2, val2) {
    //                    if (typeof subtotale[groupid] == 'undefined') {
    //                        subtotale[groupid] = new Array();
    //                    }
    //                    if (typeof subtotale[groupid][index2] == 'undefined') {
    //                        subtotale[groupid][index2] = 0;
    //                    }
    //                    if (typeof totale['Totale'][index2] == 'undefined') { totale['Totale'][index2] = 0; }

    //                   var valore = Number(val2.replace('€', "").replace('.', "").replace(',', "."));
    //                    subtotale[groupid][index2] += valore;
    //                    totale['Totale'][index2] += valore;
    //                });



    //            });
    //            $('tbody').find('.group').each(function (i, v) {
    //                var rowCount = $(this).nextUntil('.group').length;
    //                $(this).find('td:first').append($('<span />', { 'class': 'rowCount-grid' }).append($('<b />', { 'text': ' (' + rowCount + ')' })));
    //                var subtd = '';
    //                for (var a = 2; a < colonne; a++) {
    //                    subtd += '<td>' + subtotale[i][a] + '</td>';
    //                }
    //                $(this).append(subtd);
    //            });

    //        }
           
    //    });
    //// Order by the grouping
    //    $('#datatable-buttons tbody').on('click', 'tr.group', function () {
    //        var currentOrder = table.order()[0];
    //        if (currentOrder[0] === 0 && currentOrder[1] === 'asc') {
    //            table.order([0, 'desc']).draw();
    //        }
    //        else {
    //            table.order([0, 'asc']).draw();
    //        }
    //    });
    },
    TableManageButtons = function () {
        "use strict";
        return {
            init: function () {
                handleDataTableButtons();
            }
        }
    }();








            