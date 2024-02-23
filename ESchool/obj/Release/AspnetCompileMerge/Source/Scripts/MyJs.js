$(document).ready(function () {


      ////////////////// fill and Build Sauf dropdown
    $('#marhalaId').on({
        click: function (e) {
            e.preventDefault();
            getSauf();
        }
    });

    function getSauf() {
        $('#lblSauf').removeClass('d-none');
        $('#SaufId').removeClass('d-none');
        var marhalaId = $('#marhalaId').val();
        ClearSauf();
        return $.getJSON("/MemberClassYears/GetSauf?marhalaId=" + marhalaId, function (result) {
            $.each(result, function (i, field) {
                $("#SaufId").append('<option class = "SaufOption" value='+field.Id +' >'+field.Name+'</option>');
            });
        });
    }

    function ClearSauf()
    {
        $("#SaufId").empty();

      
    }
    ////////////////////////fill and build Fasl
    $('#SaufId').on({
        click: function (e) {
            e.preventDefault();
            getClass();
        }
    });

    function getClass() {
        $('#lblClass').removeClass('d-none');
        $('#ClassId').removeClass('d-none');
        var SaufId = $('#SaufId').val();
        ClearClasses();
        return $.getJSON("/MemberClassYears/GetClass?SaufId=" + SaufId, function (result) {
            $.each(result, function (i, field) {
                $("#ClassId").append('<option  value=' + field.Id + ' >' + field.Name + '</option>');
            });
        });
    }

    function ClearClasses() {
        $("#ClassId").empty();

    }

    ////   Search in Student Table 
    $("#StudentSearch").keyup(function () {
        var input, filter, table, tr, tdName,tdMarhala,tdSauf,tdFasl, tdMobile, tdMobile2, tdIdentity, tdStatus, i
        ,txtMarhalaValue,txtSaufValue,txtFaslValue , txtStatusValue, txtNameValue, txtMobileValue, txtMobile2Value, txtIdentityValue;
        input = document.getElementById("StudentSearch");
        filter = input.value.toUpperCase();
        table = document.getElementById("tableStudent");
        tr = table.getElementsByTagName("tr");
        for (i = 0; i < tr.length; i++) {
            tdIdentity = tr[i].getElementsByTagName("td")[0];
            tdName = tr[i].getElementsByTagName("td")[1];
            tdMarhala = tr[i].getElementsByTagName("td")[2];
            tdSauf = tr[i].getElementsByTagName("td")[3];
            tdFasl = tr[i].getElementsByTagName("td")[4];
            tdMobile = tr[i].getElementsByTagName("td")[5];
            tdMobile2 = tr[i].getElementsByTagName("td")[6];
            tdStatus = tr[i].getElementsByTagName("td")[9];
           
            if (tdIdentity || tdName || tdMarhala || tdSauf || tdFasl || tdMobile || tdMobile2 || tdStatus) {
                txtIdentityValue = tdIdentity.textContent || tdIdentity.innerText;
                txtNameValue = tdName.textContent || tdName.innerText;
                txtMarhalaValue = tdMarhala.textContent || tdMarhala.innerText;
                txtSaufValue = tdSauf.textContent || tdSauf.innerText;
                txtFaslValue = tdFasl.textContent || tdFasl.innerText;
                txtMobileValue = tdMobile.textContent || tdMobile.innerText;
                txtMobile2Value = tdMobile2.textContent || tdMobile2.innerText;
                txtStatusValue = tdStatus.textContent || tdStatus.innerText;

                if (txtNameValue.toUpperCase().indexOf(filter) > -1 || txtStatusValue.toUpperCase().indexOf(filter) > -1 || txtMobileValue.toUpperCase().indexOf(filter) > -1 || txtMobile2Value.toUpperCase().indexOf(filter) > -1 || txtIdentityValue.toUpperCase().indexOf(filter) > -1 || txtMarhalaValue.toUpperCase().indexOf(filter) > -1 || txtSaufValue.toUpperCase().indexOf(filter) > -1 || txtFaslValue.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = "";
                } else {
                    tr[i].style.display = "none";
                }
            }
        }
    });


    ////// Update total Amount And Vat

    var nathionality = document.getElementById('Nathionality').innerText;
    if (nathionality.includes( "مقيم")) {
      
        $('#Vat').val(15);
    } else {
        $('#Vat').val(0);
    }

    var amount = $('#Amount');
    var Vat = $('#Vat');
    var Total = $('#TotalAmount');

    $('#Amount').on('change', function(){
        CalculateTotal();
    });
    //$('#Vat').on('change', function () {
    //    CalculateTotal();
    //});

    $('#TotalAmount').on('change', function () {
        CalculateAmount();
    });
    function CalculateTotal() {
        if (Vat.val() == 0) {

            Total.val(amount.val());
        } else {

            var VatValue = parseFloat(Vat.val()) / 100 * parseFloat(amount.val());
            Total.val(VatValue + parseFloat(amount.val()))
        }
    }

    function CalculateAmount() {

        if (Vat.val() == 0) {

            amount.val(Total.val());
        } else {

            var amountValue = parseFloat(Total.val()) / (1+ (Vat.val()/100));
            amount.val(  amountValue.toFixed(2));
        }
    }
   

});