$(document).ready(function () {
    // ================= INVOICE DATE PICKER =================
    var today = new Date();
    today.setHours(0, 0, 0, 0); // normalize

    $('#dtInvoiceDate, #txtInvoiceDate').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        endDate: today      // 🚫 blocks tomorrow & future dates
    });

    // ✅ Default value = today
    $('#txtInvoiceDate').datepicker('setDate', today);

    // ================= INITIAL LOAD =================
    toggleGSTSummaryByType();
    calculateGSTTotals();

    // ================= GST TYPE CHANGE =================
    $('#ddlGSTType').on('change', function () {
        toggleGSTSummaryByType();
        calculateGSTTotals();
    });

    // ================= PROCEED =================
    $('#btnGSTProceed').on('click', function () {

        if ($('#ddlParty').val() === '') {
            alert('Please select Party');
            return;
        }
        if ($('#ddlGSTSlab').val() === '') {
            alert('Please select GST Slab');
            return;
        }
        if ($('#ddlGSTType').val() === '') {
            alert('Please select GST Type');
            return;
        }

        $('#tblGSTItems tbody').empty();
        $('#lblGSTTotal').text('0.00');

        $('#dvAddGSTItem').show();
        $('#dvGSTItemTable').hide();
        $('#btnGSTProceed').hide();

        calculateGSTTotals();
    });

    // ================= ADD ITEM =================
    $('#btnAddGSTItem').on('click', function () {
        debugger;
        var itemId = $('#ddlGSTItems').val();
        var itemText = $('#ddlGSTItems option:selected').text().trim();
        var baseAmount = parseFloat($('#txtItemAmount').val());

        // ✅ Item validation
        if (!itemId || itemId === '' || itemId === '0' || itemText === 'Select Item') {
            alert('Please select Item');
            return;
        }

        // ✅ Amount validation
        if (isNaN(baseAmount) || baseAmount <= 0) {
            alert('Please Enter Amount');
            return;
        }
        var slab = parseFloat($('#ddlGSTSlab').val());
        var type = $('#ddlGSTType').val();

        var cgst = 0, sgst = 0, igst = 0;

        if (type === 'CGST & SGST') {
            cgst = (baseAmount * slab / 100) / 2;
            sgst = cgst;
        } else if (type === 'IGST') {
            igst = (baseAmount * slab / 100);
        }

        var totalGST = cgst + sgst + igst;

        // ✅ HSN from map (no UI label)
        var hsnCode = (typeof itemHSNMap !== 'undefined' && itemHSNMap[itemId])
                      ? itemHSNMap[itemId]
                      : '-';
        $('#dvGSTItemTable').show();

        var markup =
        "<tr " +
        "data-base='" + baseAmount.toFixed(2) + "' " +
        "data-cgst='" + cgst.toFixed(2) + "' " +
        "data-sgst='" + sgst.toFixed(2) + "' " +
        "data-igst='" + igst.toFixed(2) + "'>" +
        "<td><input type='checkbox' class='chkGSTRow'></td>" +
        "<td>" + itemText + "</td>" +
        "<td>" + hsnCode + "</td>" +
        "<td class='align-right'>" + baseAmount.toFixed(2) + "</td>" +
        "</tr>";

        $('#tblGSTItems tbody').append(markup);
        // ✅ Reset after adding
        calculateGSTTotals();
    });

    // ================= CHECKBOX CHANGE =================
    $(document).on('change', '.chkGSTRow', function (e) {
        e.stopPropagation();
        calculateGSTTotals();
    });

    // ================= ROW CLICK TOGGLE =================
    $(document).on('click', '#tblGSTItems tbody tr', function (e) {

        if ($(e.target).is('input[type="checkbox"]')) {
            return;
        }

        var $checkbox = $(this).find('.chkGSTRow');
        $checkbox.prop('checked', !$checkbox.is(':checked'));

        calculateGSTTotals();
    });

    // ================= FUNCTIONS =================

    function calculateGSTTotals() {

        var totalBase = 0;
        var totalCGST = 0;
        var totalSGST = 0;
        var totalIGST = 0;

        var checkedCount = $('#tblGSTItems tbody .chkGSTRow:checked').length;

        $('#tblGSTItems tbody tr').each(function () {

            var base = Number($(this).attr('data-base')) || 0;
            var cgst = Number($(this).attr('data-cgst')) || 0;
            var sgst = Number($(this).attr('data-sgst')) || 0;
            var igst = Number($(this).attr('data-igst')) || 0;
            var isChecked = $(this).find('.chkGSTRow').is(':checked');

            if (checkedCount > 0 && !isChecked) {
                return;
            }

            totalBase += base;
            totalCGST += cgst;
            totalSGST += sgst;
            totalIGST += igst;
        });

        var totalGST = totalCGST + totalSGST + totalIGST;
        var grandTotal = totalBase + totalGST;

        // ----- TABLE FOOTER -----
        $('#lblGSTTotal').text(totalBase.toFixed(2));

        // ----- SUMMARY -----
        $('#lblBaseAmount').text(totalBase.toFixed(2));
        $('#lblCGST').text(totalCGST.toFixed(2));
        $('#lblSGST').text(totalSGST.toFixed(2));
        $('#lblIGST').text(totalIGST.toFixed(2));
        $('#lblTotalGST').text(totalGST.toFixed(2));
        $('#lblGrandTotal').text(grandTotal.toFixed(2));

        // ----- PERCENTAGES -----
        var gstPct = totalBase > 0 ? (totalGST / totalBase) * 100 : 0;
        var cgstPct = totalBase > 0 ? (totalCGST / totalBase) * 100 : 0;
        var sgstPct = totalBase > 0 ? (totalSGST / totalBase) * 100 : 0;
        var igstPct = totalBase > 0 ? (totalIGST / totalBase) * 100 : 0;

        $('#lblTotalGSTPct').text(gstPct.toFixed(2) + '%');
        $('#lblCGSTPct').text(cgstPct.toFixed(2) + '%');
        $('#lblSGSTPct').text(sgstPct.toFixed(2) + '%');
        $('#lblIGSTPct').text(igstPct.toFixed(2) + '%');
    }

    function toggleGSTSummaryByType() {

        var gstType = ($('#ddlGSTType').val() || '').toUpperCase();

        // Hide all first
        $('#lblCGST').closest('div').hide();
        $('#lblSGST').closest('div').hide();
        $('#lblIGST').closest('div').hide();

        if (gstType.includes('IGST')) {
            $('#lblIGST').closest('div').show();
        } else {
            $('#lblCGST').closest('div').show();
            $('#lblSGST').closest('div').show();
        }
    }
    
    // ================= ITEM → AUTO-FILL HSN =================
    $('#ddlGSTItems').on('change', function () {
        var selectedId = $(this).val();
        clearError('ddlGSTItems');
    });
});
