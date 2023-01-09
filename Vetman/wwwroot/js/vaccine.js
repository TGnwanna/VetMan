
function ProductVaccine() {
    var data = {};
    data.Name = $('#product_Type_Name').val();
    data.Week = $('#Week').val();
    data.ProductId = $('#ProductId').val();
    debugger;
    if (data.Name != "" && data.Week != "" && data.ProductId != "") {
        let ProductVaccineViewModel = JSON.stringify(data);
        if (ProductVaccineViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Vaccine/ProductVaccine',
                data:
                {
                    ProductVaccineDetails: ProductVaccineViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/Vaccine/Index';
                        successAlertWithRedirect(result.msg, url)
                    }
                    else {
                        errorAlert(result.msg)
                    }
                },
                error: function (ex) {
                    errorAlert("Error occured try again");
                }
            });
        }
    }
    else {
        errorAlert("please fill the form correctly");
    }
}

