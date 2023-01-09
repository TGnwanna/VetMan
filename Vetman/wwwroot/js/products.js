function ProductPostAction()
{
    var data = {};
    data.Name = $('#product_Name').val();
    data.productTypeId = $('#productTypeId').val();
   
    if (data.Name != "" && data.productTypeId != "0") {
        let productViewModel = JSON.stringify(data);
        if (productViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Product/ProductPostAction',
                data:
                {
                    productDetails: productViewModel,
                },
                success: function (result) {
                    if (!result.isError) {
                        var url = '/Product/Index';
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
        errorAlert("Incorrect Details");
    }
}

function ProductTypePostAction() {
    var data = {};
    data.Name = $('#product_Type_Name').val();
    data.DateCreated = $('#DateCreated').val();
    if (data.Name != "") {
        let productTypeViewModel = JSON.stringify(data);
        if (productTypeViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/ProductType/ProductTypePostAction',
                data:
                {
                    productTypeDetails: productTypeViewModel,
                },
                success: function (result) {
                    if (!result.isError) {
                        var url = '/ProductType/Index';
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

function EditedProduct(id) {
    $.ajax({
        type: 'GET',
        url: '/Product/EditedProduct', // we are calling json method
        dataType: 'json',
        data:
        {
            productId: id
        },
        success: function (data) {
            if (!data.isError) {

                $("#editId").val(data.data.id);
                $("#deleteId").val(data.data.id);
                $("#Editedproduct_Name").val(data.data.name);
                $("#EditedproductTypeId").val(data.data.editedproductTypeId);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function EditedProductType(id) {
    $.ajax({
        type: 'GET',
        url: '/ProductType/EditedProductType', // we are calling json method
        dataType: 'json',
        data:
        {
            productTypeId: id
        },
        success: function (data) {
            if (!data.isError) {

                $("#editProductTypeIds").val(data.data.id);
                $("#deleteProductTypeId").val(data.data.id);
                $("#EditedproductType_Name").val(data.data.name);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function SaveEditProduct() {
    var data = {};
    data.id = $('#editId').val();
    data.Name = $('#Editedproduct_Name').val();
    data.ProductTypeId = $('#EditedproductTypeId').val();
    if (data.Name != "" && (data.ProductTypeId != null || data.ProductTypeId != undefined)) {
        let product = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Product/EditedProduct', // we are calling json method,
            dataType: 'json',
            data:
            {
                product: product,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/Product/Index";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    } else {
        errorAlert("Incorrect Details");
    }
    
}

function SaveEditProductType() {

    var data = {};
    data.id = $('#editProductTypeIds').val();
    data.Name = $('#EditedproductType_Name').val();
    if (data.Name != "") {
        let product = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/ProductType/EditedProductType', // we are calling json method,
            dataType: 'json',
            data:
            {
                productType: product,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/ProductType/Index";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    } else {
        errorAlert("Incorrect Details");
    }
}

function DeleteProduct() {

    var data = {};
    data.id = $('#deleteId').val();

    let product = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/Product/DeleteProduct', // we are calling json method,
        dataType: 'json',
        data:
        {
            product: product,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/Product/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
}

function DeleteProductType() {

    var data = {};
    data.id = $('#deleteProductTypeId').val();

    let product = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/ProductType/DeleteProductType', // we are calling json method,
        dataType: 'json',
        data:
        {
            productType: product,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/ProductType/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
} 

function addStaff() {
    var data = {};
    data.FirstName = $('#staff_first_Name').val();
    data.LastName = $('#staff_Last_Name').val();
    data.Email = $('#staff_Email').val();
    data.CompanyBranchId = $('#companyBranchIdStaff').val();
    data.PassWord = $('#staff_Password').val();
    data.ConfirmPassword = $('#staff_Confirm_Password').val();

    if (data.Email != "" && data.CompanyBranchId != "0") {


        let staffViewModel = JSON.stringify(data);
        if (staffViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Staff/CreateStaff',
                data:
                {
                    staffDetails: staffViewModel,
                },
                success: function (result) {
                    if (!result.isError) {
                        var url = '/Staff/CompanyStaff';
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
        errorAlert("Incorrect Details");
    }
}

function EditedStaff(id) {
    $.ajax({
        type: 'GET',
        url: '/Staff/EditedStaff', // we are calling json method
        dataType: 'json',
        data:
        {
            staffId: id
        },
        success: function (data) {
            if (!data.isError) {

                $("#staff_editId").val(data.data.id);
                $("#staff_deleteId").val(data.data.id);
                $("#staff_edited_first_name").val(data.data.firstName);
                $("#staff_edited_last_name").val(data.data.lastName);
                $("#staff_edited_email").val(data.data.email);
                $("#edit_companyBranchId").val(data.data.companyBranchId);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function SaveEditStaff() {

    var data = {};
    data.id = $('#staff_editId').val();
    data.FirstName = $('#staff_edited_first_name').val();
    data.LastName = $('#staff_edited_last_name').val();
    data.Email = $('#staff_edited_email').val();
    data.CompanyBranchId = $('#edit_companyBranchId').val();

    if (data.FirstName != "" && data.LastName != "" && data.Email != "" && data.CompanyBranchId != "") {
        let Staff = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Staff/EditedStaffs', // we are calling json method,
            dataType: 'json',
            data:
            {
                staff: Staff,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/Staff/CompanyStaff";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    }
    else {
        errorAlert("Incorrect Details");
    }
}

function DeleteStaff() {

    var data = {};
    data.id = $('#staff_deleteId').val();

    let staff = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/Staff/DeleteStaff', // we are calling json method,
        dataType: 'json',
        data:
        {
            staffDetails: staff,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/Staff/CompanyStaff";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
} 

function CompanyBranchPostAction() {
    var data = {};
    data.Name = $('#branch_Name').val();
    data.Address = $('#Address').val();
    data.CompanyId = $('#CompanyId').val();
    if (data.Name != "" && data.Address != "" && data.CompanyId != "") {
        let CompanyBranchViewModel = JSON.stringify(data);
        if (CompanyBranchViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Branches/CompanyBranchPostAction',
                data:
                {
                    branchDetails: CompanyBranchViewModel,
                },
                success: function (result) {
                    if (!result.isError) {
                        var url = '/Branches/Index';
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
        errorAlert("Incorrect Details");
    }
}

function EditedCompanyBranch(id) {
    $.ajax({
        type: 'GET',
        url: '/Branches/EditedCompanyBranch', // we are calling json method
        dataType: 'json',
        data:
        {
            CompanyId: id
        },
        success: function (data) {
            if (!data.isError) {

                $("#Company_edit_Id").val(data.data.id);
                $("#Company_delete_Id").val(data.data.id);
                $("#edit_branch_Name").val(data.data.name);
                $("#edit_Address").val(data.data.address);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function SaveEditedBranch() {

    var data = {};
    data.id = $('#Company_edit_Id').val();
    data.Name = $('#edit_branch_Name').val();
    data.Address = $('#edit_Address').val();
    if (data.id != "" && data.Name != "" && data.Address != "") {
        let branches = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Branches/EditedCompanyBranch', // we are calling json method,
            dataType: 'json',
            data:
            {
                branch: branches,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/Branches/Index";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    } else {
        errorAlert("Incorrect Details");
    }
}

function DeleteBranch() {

    var data = {};
    data.id = $('#Company_delete_Id').val();

    let branch = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/Branches/DeleteCompanyBranch', // we are calling json method,
        dataType: 'json',
        data:
        {
            branchDetails: branch,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/Branches/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function GetVaccineSubscriptionId(customerId) {
    $.ajax({
        type: 'GET',
        url: '/Vaccine/GetVaccineSubscriptionId', // we are calling json method
        dataType: 'json',
        data:
        {
            SubDetails: customerId
        },
        success: function (data) {
            if (!data.isError) {

                $("#editId").val(data.data.id);
                $("#cancelId").val(data.data.id);
                $("#deleteId").val(data.data.id);
                $("#completedId").val(data.data.id);
              
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function ShowVaccineSubscriber(customerId) {
    $.ajax({
        type: 'GET',
        url: '/Vaccine/GetVaccineSubscriptionId', // we are calling json method
        dataType: 'json',
        data:
        {
            SubDetails: customerId
        },
        success: function (data) {
            debugger
            var dateData;
            if (!data.isError) {
                if (data.data.dateDelivered != null) {
                    dateData = data.data.dateDelivered.split(/[-,T]/);
                    var date = dateData[0] + "-" + dateData[1] + "-" + dateData[2] ;
                    $('#edit_date').val(date);
				}
                $('#editId').val(data.data.id);
                
                if (data.data.smsSubscribed) {
                    $('#editSmsSubscribed').prop('checked', true);
                }
                if (data.data.emailSubscribed) {
                    $('#editEmailSubscribed').prop('checked', true);
                }
                $("#edit_subscriber").modal();

            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function SaveEditedSubscriber() {
   
    var data = {};
    debugger
    data.Id = $('#editId').val();
    data.DateDelivered = $('#edit_date').val();
    var sendSMS = $('#editSmsSubscribed').is(":checked");
    if (sendSMS) {
        data.SmsSubscribed = true;
    }
    else {
        data.SmsSubscribed = false;
    } 
    var sendEmail = $('#editEmailSubscribed').is(":checked");
    if (sendEmail) {
        data.EmailSubscribed = true;
    } else {
        data.EmailSubscribed = false;
    }

    if (data.SmsSubscribed != false || data.EmailSubscribed != false ) {
        let subMode = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Vaccine/EditedSubscriber', // we are calling json method,
            dataType: 'json',
            data:
            {
                editSubMode: subMode,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/Vaccine/Index";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    }
    else
    {
        errorAlert("Incorrect Details or Check One");
    }

};

function CancelSubscriber() {
    var data = {};
    data.id = $('#cancelId').val();
    let subData = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/Vaccine/VaccineStatusCancel', // we are calling json method,
        dataType: 'json',
        data:
        {
            action: subData,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/Vaccine/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function CompletedSubscriber() {

    var data = {};
    data.id = $('#completedId').val();

    let subId = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/Vaccine/VaccineStatusCompleted', // we are calling json method,
        dataType: 'json',
        data:
        {
            action: subId,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/Vaccine/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function VaccineStatusDeleted() {

    var data = {};
    data.id = $('#deleteId').val();

    let subData = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/Vaccine/VaccineStatusDeleted', // we are calling json method,
        dataType: 'json',
        data:
        {
            action: subData,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/Vaccine/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function AddVaccineSubscribers() {

    $("#add_subscriber").modal();
};

$(document).ready(function () {
    $(".userAutoComp").autocomplete({
        minLength: 2,
        appendTo: "#searchedNames",
        source: function (request, response) {
            $.ajax({
                url: '/Vaccine/GetAllcustomer',
                dataType: 'json',
                data:
                {
                    term: request.term,
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        var customer = { label: item.fullName, value: item.fullName };
                        $('#save').val(item.id)
                        return customer;
                    }));
                },
                error: function (err) {
                    errorAlert("Request not completed, Try again.", err);
                }
            });
        },
    });
});

function CreateSubscriber() {
    var data = {};
    data.productId = $('#productId').val();
    data.customerId = $('#save').val();
    data.FullName = $('#customerNameSearch').val();
    data.DateDelivered = $('#date').val();
    var sendSMS = $('#SmsSubscribed').is(":checked");
    if (sendSMS) {
        data.SmsSubscribed = true;
    }
    else {
        data.SmsSubscribed = false;
    }
    var sendEmail = $('#EmailSubscribed').is(":checked");
    if (sendEmail) {
        data.EmailSubscribed = true;
    } else {
        data.EmailSubscribed = false;
    }

    if (data.FullName != "" && data.product != "0") {


        let customerViewModel = JSON.stringify(data);
        if (customerViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Vaccine/CreateCustomer',
                data:
                {
                    customerDetails: customerViewModel,
                },
                success: function (result) {
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
        errorAlert("Incorrect Details");
    }
};

function RegisterSupplier() {
    var data = {};
    data.Name = $('#supplier_Name').val();
    data.address = $('#supplier_Address').val();
    data.phoneNumber = $('#supplier_PhoneNumber').val();
    data.email = $('#supplier_Email').val();
    data.companyName = $('#supplier_CompanyName').val();

    if (data.Name != "" && data.phoneNumber != "" && data.address != "") {
        if (data.email == "") {

            RegisterSupplier2();
        }
        else
        {
            EmailValidation();
        }
    }
    else {
        errorAlert("Incorrect Details");
    }
};

function EditedSupplier(id) {
    $.ajax({
        type: 'GET',
        url: '/Suppliers/EditedSupplier', // we are calling json method
        dataType: 'json',
        data:
        {
            id: id
        },
        success: function (data) {
            if (!data.isError) {

                $("#edit_supplier_id").val(data.data.id);
                $("#delete_supplier_id").val(data.data.id);
                $("#edit_supplier_Email").val(data.data.email);
                $("#edit_supplier_PhoneNumber").val(data.data.phoneNumber);
                $("#edit_supplier_CompanyName").val(data.data.companyName);
                $("#edit_supplier_Address").val(data.data.address);
                $("#edit_supplier_Name").val(data.data.name);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function SaveEditedSupplier() {

    var data = {};
    data.id = $('#edit_supplier_id').val();
    data.Name = $('#edit_supplier_Name').val();
    data.address = $('#edit_supplier_Address').val();
    data.phoneNumber = $('#edit_supplier_PhoneNumber').val();
    data.email = $('#edit_supplier_Email').val();
    data.companyName = $('#edit_supplier_CompanyName').val();

    if (data.Name != "" && data.phoneNumber != "" && data.address != "")
    {
        let details = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Suppliers/SaveEditedSupplier', // we are calling json method,
            dataType: 'json',
            data:
            {
                supplierDetails: details,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/Suppliers/Index";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    } else {
        errorAlert("Incorrect Details");
    }

};

function DeleteSupplier() {

    var data = {};
    data.id = $('#delete_supplier_id').val();

    let Supplier = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/Suppliers/DeleteSupplier', // we are calling json method,
        dataType: 'json',
        data:
        {
            supplierDetails: Supplier,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/Suppliers/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function ProductCategory() {
   
    var data = {};
    data.Name = $('#product_Category_Name').val();
    if (data.Name != "") {
        let productCategoryViewModel = JSON.stringify(data);
        if (productCategoryViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/ShopProducts/ProductCategory',
                data:
                {
                    productCatDetails: productCategoryViewModel,
                },
                success: function (result) {
                    if (!result.isError) {
                        var url = '/ShopProducts/Index';
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
};

function EditedProductCategory(id) {

    $.ajax({
        type: 'GET',
        url: '/ShopProducts/EditedShopProduct', // we are calling json method
        dataType: 'json',
        data:
        {
            productCatId: id
        },
        success: function (data) {
            if (!data.isError) {

                $("#editProductCategoryId").val(data.data.id);
                $("#deleteProductCategoryId").val(data.data.id);
                $("#EditedproductCategory_Name").val(data.data.name);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
};

function ProductCatToSave() {

    var data = {};
    data.id = $('#editProductCategoryId').val();
    data.Name = $('#EditedproductCategory_Name').val();
    if (data.Name != "") {
        let productCat = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/ShopProducts/ProductCatToSave', // we are calling json method,
            dataType: 'json',
            data:
            {
                productCategory: productCat,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/ShopProducts/Index";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    } else {
        errorAlert("Incorrect Details");
    }

};

function DeleteProductCategory() {

    var data = {};
    data.id = $('#deleteProductCategoryId').val();

    let product = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        url: '/ShopProducts/DeleteProductCategory', // we are calling json method,
        dataType: 'json',
        data:
        {
            productCat: product,
        },
        success: function (result) {
            if (!result.isError) {
                var url = "/ShopProducts/Index";
                newSuccessAlert(result.msg, url);
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });
} 

function EmailValidation() {
    var email = $('#supplier_Email').val();
    var re = /([A-Z0-9a-z_-][^@])+?@[^$#<>?]+?\.[\w]{2,4}/.test(email);
    if (!re) {
        errorAlert(msg = "invalid Email")
    }
    else
    {
        RegisterSupplier2();
    }
}

function RegisterSupplier2() {
    var data = {};
    data.Name = $('#supplier_Name').val();
    data.address = $('#supplier_Address').val();
    data.phoneNumber = $('#supplier_PhoneNumber').val();
    data.email = $('#supplier_Email').val();
    data.companyName = $('#supplier_CompanyName').val();

    if (data.Name != "" && data.phoneNumber != "" && data.address != "") {
      
        let supplierViewModel = JSON.stringify(data);
        if (supplierViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Suppliers/RegisterSupplier',
                data:
                {
                    supplierDetails: supplierViewModel,
                },
                success: function (result) {
                    if (!result.isError) {
                        var url = '/Suppliers/Index';
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
        errorAlert("Incorrect Details");
    }
};

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});


$(document).ready(function () {
    (function () {
        var xstopDate = parseInt($("#stopDate").val());
        if (xstopDate == 10 || xstopDate == 5 || xstopDate == 2 || xstopDate == 1) {
            var n = localStorage.getItem('on_load_counter');
            if (parseInt(n) === 1) {
                $("#moduleInfo").modal({ backdrop: 'static', keyboard: false }, 'show');
                n++;
                localStorage.setItem("on_load_counter", n );
            }
        }
    })();
});


function enable() {
    var cash = $('#paid_cash').prop("checked");
    var bank = $('#paid_bank').prop('checked');
    if (cash == true) {
        $('#select_cash').removeClass("d-none");
    }
    else {
        $('#select_cash').addClass("d-none");
    }
    if (bank == true) {
        $('#select_device').removeClass("d-none");
        $('#select_device2').removeClass("d-none");
        $('#select_device3').removeClass("d-none");
    }
    else {
        $('#select_device').addClass("d-none");
        $('#select_device2').addClass("d-none");
        $('#select_device3').addClass("d-none");
    }
};

function RegisterBanks() {
    var data = {};
    data.Name = $('#bank_Name').val();
    data.SalesLogId = $('#salesLogsId').val();
    data.Amount = $('#totalPaid').val();
    if (data.Name != "" ) {

        let salesPaymentViewModel = JSON.stringify(data);
        if (salesPaymentViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/SalesPayment/CreateBankPayment',
                data:
                {
                    bankDetails: salesPaymentViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var id = result.data;
                        var url = '/SalesPayment/Index/'+id;
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
        errorAlert("Incorrect Details");
    }
};

function DeleteBank(Id) {
    var data = {};
    data.id = Id;
    if (data.id != "" && data.id > 0) {
        let bank = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/SalesPayment/DeleteBank', // we are calling json method,
            dataType: 'json',
            data:
            {
                bankId: bank,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = "/SalesPayment/Index";
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                "Something went wrong, contact support - " + errorAlert(ex);
            }
        });
    } else {
        errorAlert("Incorrect Details");
    }
}

function PaymentMeans() {
    var data = {};
    data.Name = $('#bank_payment_means').val();
    data.CategoryId = $('#payment_meansBank').val();
    data.SalesLogId = $('#salesLogsId').val();
    data.Amount = $('#totalPaid').val();

    if (data.Name != "") {
        let paymentMeans = JSON.stringify(data);
        if (paymentMeans != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/SalesPayment/CreatePaymentMeans',
                data:
                {
                    paymentDetails: paymentMeans,
                },
                success: function (result) {
                    if (!result.isError) {
                        var id = result.data;
                        var url = '/SalesPayment/Index/'+id;
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
        errorAlert("Incorrect Details");
    }
}

$("#amountPaid").keyup(function () {
    var totalPaid = $("#totalPaid").val();
    var amount = $("#amountPaid").val();
    $("#amount_to_apply").val(amount);

    var result = parseFloat(totalPaid) - parseFloat(amount);
    $("#balance").val(result);
 
})
$("#amountEntered").keyup(function () {
    var totalPaid = $("#totalPaid").val();
    var amount = $("#amountEntered").val();
    $("#amount_to_apply").val(amount);

    var result = parseFloat(totalPaid) - parseFloat(amount);
    $("#balance").val(result);
 
})

$("#amountEntered").blur(function () {
    var totalPaid = $("#totalPaid").val();
    var amount = $("#amountEntered").val();
    var bal = parseFloat(amount) > parseFloat(totalPaid)
    if (bal) {
        $('#amountEntered').val('');
        $('#amount_to_apply').val('');
        $('#balance').val('');
    }
})

function CreatePayment() {
    debugger;
    var data = {};
    data.dateCreated = $('#date_created').val();
    data.orderId = $('#OrderId').val();
    data.totalPaid = $('#totalPaid').val();
    data.paidCash = $('#paid_cash').prop("checked");
    data.amountPaid = $('#amountEntered').val();
    data.balance = $('#balance').val();
    data.salesLogsId = $('#salesLogsId').val();
    if (!data.paidCash) {
        data.SelectBank = $('#Select_bank').val();
        data.SelectPayment = $('#Select_payment').val();
    }
        
    if (data.totalPaid != "" && data.amountPaid != "" && data.dateCreated != "" && data.salesLogsId != "") {

        let SalesPaymentViewModel = JSON.stringify(data);
        if (SalesPaymentViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/SalesPayment/CreatePayment',
                data:
                {
                    paymentDetails: SalesPaymentViewModel,
                },
                success: function (result) {
                    if (!result.isError) {
                        var url = '/Sales/AddSales';
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
        errorAlert("Incorrect Details");
    }
}

$("#Select_bank").change(function () {
    debugger;
    var id = $(this).val();
    if (id != "") {
        $("#Select_payment").empty();
        $.ajax({
            type: 'GET',
            url: '/SalesPayment/GetPaymentMeans',
            data: {id:id},
            success: function (result) {
                debugger;
                
                if (!result.isError) {
                    $.each(result.data, function (i, mean) {
                        debugger;
                        $("#Select_payment").append('<option value=' + mean.id +'>'+mean.name+'</option>');
                    });
                }
                else {
                   
                    newErrorAlert(result.msg);
                }
            },
            Error: function (ex) {
               
                errorAlert(ex);
            }
        });
    } 
});
