
function CustomerPostAction(action, Id) {
    var data = {};
    if (action == "CREATE") {
        data.ActionType = action;
        data.FirstName = $('#first_Name').val();
        data.LastName = $('#last_Name').val();
        data.PhoneNumber = $('#tel').val();
        data.Email = $('#email').val();
        data.Address = $('#address').val();
        if (data.FirstName != "" && data.LastName != "" && data.Address != "" && data.PhoneNumber != "") {
            debugger;
            SendCustomerData2Kontrola(data);
        }
        else {
            errorAlert("Please fill the form correctly");
        }
    }
}

function EditCustomer(action) {
    var data = {};
    data.Id = $('#edit_Id').val();
    data.ActionType = action;
    data.FirstName = $('#edit_first_Name').val();
    data.LastName = $('#edit_last_Name').val();
    data.PhoneNumber = $('#edit_tel').val();
    data.Email = $('#edit_email').val();
    data.Address = $('#edit_address').val();

    if (data.Id != "" && data.FirstName != "" && data.LastName != "" && data.Address != "" && data.PhoneNumber != "") {
        let customerViewModel = JSON.stringify(data);
        if (customerViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Admin/EditCustomer',
                data:
                {
                    customerDetails: customerViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/Admin/Customer';
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
        errorAlert("Please fill the form correctly");
    }
}

function DeleteCustomer(action) {
    var data = {};
    if (action == "DELETE") {
        data.ActionType = action;
        data.Id = $('#delete_Id').val();
        if (data.Id != null) {
            let customerViewModel = JSON.stringify(data);
            if (customerViewModel != "") {
                $.ajax({
                    type: 'Post',
                    dataType: 'json',
                    url: '/Admin/DeleteCustomer',
                    data:
                    {
                        customerDetails: customerViewModel,
                    },
                    success: function (result) {
                        debugger;
                        if (!result.isError) {
                            var url = '/Admin/Customer';
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
            else {
                errorAlert("Incorrect Details");
            }
        }
    }
}


function SendCustomerData2Kontrola(data)
{
    let customerViewModel = JSON.stringify(data);
    if (customerViewModel != "") {
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: '/Admin/CustomerPostAction',
            data:
            {
                customerDetails: customerViewModel,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = '/Admin/Customer';
                    successAlertWithRedirect(result.msg, url)
                }
                else {
                    infoAlert(result.msg)
                }
            },
            error: function (ex) {
                errorAlert("Error occured try again");
            }
        });
    }
    else {
        errorAlert("Incorrect Details");
    }
}

function GetCustomerById(Id)
{
    let data = JSON.stringify(Id);
    $.ajax({
        type: 'GET',
        url: '/Admin/GetCustomerByID',
        data: { customerID: data },
        dataType: 'json',
        success: function (data)
        {
            if (!data.isError)
            {
                $("#delete_Id").val(data.id);
                $("#edit_Id").val(data.id);
                $("#edit_first_Name").val(data.firstName);
                $("#edit_last_Name").val(data.lastName);
                $("#edit_tel").val(data.phoneNumber);
                $("#edit_email").val(data.email);
                $("#edit_address").val(data.address);
            }
        }
    });
}

 function EditBookingGroup(action)
 {
    var data = {};
    data.Id = $('#edit_Id').val();
    data.ActionType = action;
     data.Name = $('#edit_Name').val();
     data.product = $('#edit_Name').val();
     data.ProductId = $('#edit_productId').val();
     data.ExpectedCostPrice = $('#edit_ExpectedCostPrice').val();
     if (data.Name != "" && data.ExpectedCostPrice != "" && data.ExpectedPrice != "" && data.product != "" && data.ProductId != "") {
    
         let BookingGroupViewModel = JSON.stringify(data);
         if (BookingGroupViewModel != "") {
             $.ajax({
                 type: 'Post',
                 dataType: 'json',
                 url: '/Admin/EditBookingGroup',
                 data:
                 {
                     bookingGroupDetails: BookingGroupViewModel,
                 },
                 success: function (result) {
                     debugger;
                     if (!result.isError) {
                         var url = '/Admin/BookingGroup';
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
         errorAlert("Please fill in the correct Details");
     }
 }
     
function DeleteBookingGroup(action) {
    debugger
    var data = {};
    if (action == "DELETE") {
        data.ActionType = action;
        data.Id = $('#delete_Id').val();
        if (data.Id != null) {
            debugger;
            let BookingGroupViewModel = JSON.stringify(data);
            if (BookingGroupViewModel != "") {
                $.ajax({
                    type: 'Post',
                    dataType: 'json',
                    url: '/Admin/DeleteBookingGroup',
                    data:
                    {
                        bookingGroupDetails: BookingGroupViewModel,
                    },
                    success: function (result) {
                        debugger;
                        if (!result.isError) {
                            var url = '/Admin/BookingGroup';
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
            else {
                errorAlert("Incorrect Details");
            }
        }
    }
}

function createBookingGroup() {
    var data = {};
    data.ProductId = $('#productId').val();
    data.Name = $('#Name').val();
    data.ExpectedCostPrice = $('#ExpectedCostPrice').val();
    data.ExpectedDateOfArrivalD = $('#ExpectedDateOfArrival').val();
    let BookingGroupViewModel = JSON.stringify(data);  $.ajax({
        type: 'Post',
        dataType: 'json',
        url: '/Admin/BookingGroup',
        data:
        {
            bookingGroupDetails: BookingGroupViewModel,
        },
        success: function (result) {
            if (!result.isError) {
                var url = '/Admin/BookingGroup';
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

function GetBookingGroupById(Id) {
    let data = JSON.stringify(Id);
    $.ajax({
        type: 'GET',
        url: '/Admin/GetBookingGroupById',
        data: { BookingGroupID: data },
        dataType: 'json',
        success: function (data) {
            if (!data.isError) {
                var expDate; 
                if (data.expectedDateOfArrival != null) {
                    expDate = data.expectedDateOfArrival.split("T");
                    $("#edit_ExpectedDateOfArrival").val(expDate[0]);
                }
                $("#delete_Id").val(data.id);
                $("#edit_Id").val(data.id);
                $("#edit_Name").val(data.name);
                $("#edit_ExpectedPrice").val(data.expectedPrice);
                $("#edit_ExpectedCostPrice").val(data.expectedCostPrice);
                $("#edit_productId").val(data.productId);
                $("#edit_product").val(data.product.name);
                $("#edit_MotalityRecorded").val(data.motalityRecorded);
                $("#edit_QuantityLeft").val(data.quantityLeft);
                $("#edit_QuantitySold").val(data.quantitySold);
                $("#edit_QuantityArrived").val(data.quantityArrived);

            }
        }
    });
}

function updateArrivalReceipt()
{
    var date = $('#arrivalReceipt_SupplyDate').val();
    var supplyDate = date.split("T");
    var data = {};
    data.MotalityRecorded = $('#arrivalReceipt_MotalityRecorded').val();
    data.ExpectedPrice = $('#arrivalReceipt_ExpectedPrice').val();
    data.QuantitySold = $('#arrivalReceipt_QuantitySold').val();
    data.QuantityArrived = $('#arrivalReceipt_QuantityArrived').val();
    data.QuantityLeft = $('#arrivalReceipt_QuantityLeft').val();
    data.SupplyDate =  supplyDate[0];
    data.SupplyPrice = $('#arrivalReceipt_SupplyPrice').val();
    data.Code = $('#myGroupId').val();
    var details = JSON.stringify(data);
    if (details != "") {
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: '/Admin/ArrivalReceipt',
            data:
            {
                details: details,
            },

            success: function (result) {
                debugger;
                if (!result.isError) {
                    var url = '/Admin/ManageGroup?bookingGroupId=' + result.data;
                    newSuccessAlert(result.msg, url)
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
    else {
        errorAlert("Incorrect Details");
    }

}

function getArrivalReceipt(id) {
    $.ajax({
        type: 'GET',
        url: '/Admin/ArrivalReceipt', // we are calling json method
        dataType: 'json',
        data:
        {
            groupId: id
        },
        success: function (data) {
            debugger;
            $("#myGroupId").val(id);
            $('#arrivalReceipt_MotalityRecorded').val(data.data.motalityRecorded);
            $('#arrivalReceipt_ExpectedPrice').val(data.data.expectedPrice);
            $('#arrivalReceipt_QuantitySold').val(data.data.quantitySold);
            $('#arrivalReceipt_QuantityArrived').val(data.data.quantityArrived);
            $('#arrivalReceipt_SupplyDate').val(data.data.supplyDate);
            $('#arrivalReceipt_SupplyPrice').val(data.data.supplyPrice);
            $('#arrivalReceipt_QuantityLeft').val(data.data.quantityLeft);
            
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    });

}



function AddRole() {
    var data = {};
    data.StaffId = $('#staffId').val();
    data.RoleId = $('#roleId').val();
   /* data.User = User; data.Role = Role;*/
    if (data.StaffId != "" && data.RoleId != "") {
        debugger;
        let roleViewModel = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            dataType: 'Json',
            url: '/Admin/AddUserRole',
            data:
            {
                roleViewModel: roleViewModel
            },
            success: function (result) {
                debugger;
                if (!result.isError) {
                    var url = '/Admin/Roles';
                    newSuccessAlert(result.msg, url);
                }
                else {
                    errorAlert(result.msg);
                }
            },
            Error: function (ex) {
                errorAlert(ex);
            }
        });
    }
}

function removeRole(id, userId) {
    $("#deletedRoleId").val(id);
    $("#userId").val(userId);
}

function roleRemove() {
    var roleId = $("#deletedRoleId").val();
    var userId = $("#userId").val();
    if (roleId != "" && userId != "") {
        $.ajax({
            type: 'POST',
            url: "/Admin/DeleteRole",
            dataType: 'json',
            data:
            {
                roleId: roleId,
                userId: userId,
            },
            success: function (result) {
                debugger;
                if (!result.isError) {
                    debugger;
                    var url = "/Admin/Roles";
                    newSuccessAlert(result.msg, url);

                }
                else {
                    $('#loader').show();
                    $('#loader-wrapper').fadeOut(3000);
                    errorAlert(result.msg);
                }
            }
        });
    }
}




 function ProductVaccine() {
   var data = {};
    data.productId = $('#productId').val();
    data.Name = $('#Name').val();
    data.Week = $('#Week').val();
   
    if (data.Name != "" && data.Week != "" && data.productId != 0) {
        let productVaccineViewModel = JSON.stringify(data);
        if (productVaccineViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Vaccine/ProductVaccine',
                data:
                {
                    productVaccineDetails: productVaccineViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/Vaccine/ProductVaccine';
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


function EditProductVaccine() {
    var data = {};
    data.Id = $('#edit_Id').val();
    data.Name = $('#edit_Name').val();
    data.Week = $('#edit_Week').val();
    data.productId = $('#edit_productId').val();

    if (data.Id != 0 && data.Name != "" && data.Week != "" && data.productId != "") {
       
        let productVaccineViewModel = JSON.stringify(data);
        if (productVaccineViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Vaccine/EditProductVaccine',
                data:
                {
                    productVaccineDetails: productVaccineViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/Vaccine/ProductVaccine';
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
        errorAlert("Please fill in the correct Details");
    }
}

function DeleteProductVaccine(Id) {
    debugger
    var data = {};
    data.Id = $('#delete_Id').val();
    if (data.Id != null) {
        debugger;
        let productVaccineViewModel = JSON.stringify(data);
        if (productVaccineViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Vaccine/DeleteProductVaccine',
                data:
                {
                    productVaccineDetails: productVaccineViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/Vaccine/ProductVaccine';
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
        else {
            errorAlert("Incorrect Details");
        }
    }
    
}


function GetProductVaccineByID(Id) {
    let data = Id;
    $.ajax({
        type: 'GET',
        url: '/Vaccine/GetProductVaccineById',
        data: { productVaccineID: data },
        dataType: 'json',
        success: function (data) {
            debugger;
            if (!data.isError) {
             
                $("#delete_Id").val(data.id);
                $("#edit_Id").val(data.id);
                $("#edit_Name").val(data.name);
                $("#edit_Week").val(data.week);
                $("#edit_productId").val(data.productId);
               
            }
        }
    });
}


function GetProductInfo() {
    let productId = $("#myProductId").val();;
    $.ajax({
        type: 'GET',
        url: '/Vaccine/ProductVaccineFillter',
        data:
        {
            productInfoId: productId
        },
        dataType: 'json',

        success: function (result) {
            debugger;
            $('#vaccine').empty();
            if (!result.isError) {
                $.each(result.data, function (i, productVaccine) {
                    debugger;
                    var action;
                    action = '<td class="text-right">' +
                        '<div class="dropdown dropdown-action">' +
                        '<a href="#" class="action-icon dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><i class="material-icons">more_vert</i></a>' +
                        '<div class="dropdown-menu dropdown-menu-right">' +
                        '<a class="dropdown-item" data-toggle="modal" data-target="#edit_termination" onclick="GetProductVaccineByID(' + productVaccine.id + ')"><i class="fa fa-pencil m-r-5"></i> Edit</a>' +
                        '<a class="dropdown-item" data-toggle="modal" data -target="#delete_termination" onclick="GetProductVaccineByID(' + productVaccine.id + ')"><i class="fa fa-trash-o m-r-5"></i> Delete</a>' +
                        '</div>' +
                        '</div>' +
                        '</td>'

                    debugger;
                    $("#vaccine").append('<tr>' +
                        '<td>' + '</td>' +
                        '<td>' + productVaccine.name + '</td>' +
                        '<td>' + productVaccine.week + '</td>' +
                        '<td>' + productVaccine.productName + '</td>' +
                        action
                    );
                });
            }
            else {
                errorAlert("No vaccine for this product");
            }
            
        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
	})




}


function MeasurementUnit() {
    var data = {};
    data.Name = $('#Name').val();
    data.DateCreated = $('#DateCreated').val();
    let MeasurementUnitViewModel = JSON.stringify(data); $.ajax({
        type: 'Post',
        dataType: 'json',
        url: '/MeasurementUnit/CreateMeasurementUnit',
        data:
        {
            UnitDetails: MeasurementUnitViewModel,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/MeasurementUnit/Index';
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

function CreateMeasurementUnit() {
    var data = {};
    data.Name = $('#Name').val();
    data.DateCreated = $('#DateCreated').val();
    if (data.Name != "" && data.DateCreated != "") {
        let MeasurementUnitViewModel = JSON.stringify(data);
        if (MeasurementUnitViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/MeasurementUnit/CreateMeasurementUnit',
                data:
                {
                    UnitDetails: MeasurementUnitViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/MeasurementUnit/Index';
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

function EditMeasurementUnit() {
    var data = {};
    data.Id = $('#edit_Id').val();
    data.Name = $('#edit_Name').val();
    data.DateCreated = $('#edit_DateCreated').val();
    if (data.Id != 0 && data.Name != "" && data.DateCreated != "" ) {
        let MeasurementUnitViewModel = JSON.stringify(data);
        if (MeasurementUnitViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/MeasurementUnit/EditMeasurementUnit',
                data:
                {
                    UnitDetails: MeasurementUnitViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/MeasurementUnit/Index';
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
        errorAlert("Please fill in the correct Details");
    }
}

function DeleteMeasurementUnit(Id) {
    var data = {};
    data.Id = $('#delete_Id').val();
    if (data.Id != null) {
        debugger;
        let MeasurementUnitViewModel = JSON.stringify(data);
        if (MeasurementUnitViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/MeasurementUnit/DeleteMeasurementUnit',
                data:
                {
                    UnitDetails: MeasurementUnitViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/MeasurementUnit/Index';
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
        else {
            errorAlert("Incorrect Details");
        }
    }
    
}

function GetMeasurementUnitByID(Id) {
    let data = Id;
    $.ajax({
        type: 'GET',
        url: '/MeasurementUnit/GetMeasurementUnitByID',
        data: { measurementUnitId: data },
        dataType: 'json',
        success: function (data) {
            debugger;
            if (!data.isError) {

                $("#delete_Id").val(data.data.id);
                $("#edit_Id").val(data.data.id);
                $("#edit_Name").val(data.data.name);
                $("#edit_DateCreated").val(data.data.dateCreated);
               

            }
        }
    });
}

function CreateModuleCost()
{
    var data = {};
    data.ModuleId = $('#moduleId').val();
    data.Price = $('#Price').val();
    data.NoOfDays = $('#noOfDays').val();
    data.Discription = $('#discription').val();
    if (data.ModuleId != "" && data.Price != "" && data.NoOfDays != "" && data.Discription) {
        debugger;

        let moduleCostViewModel = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: '/SuperAdmin/CreateModuleCost',
            data:
            {
                costDetails: moduleCostViewModel,
            },
            success: function (result) {

                if (!result.isError) {
                    var url = '/SuperAdmin/ModuleCost';
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
    else
    {
        errorAlert("please fill the form correctly");
    }
   
}

function EditmoduleCost() {
    debugger
    var data = {};
    data.Id = $('#edit_Id').val();
    data.ModuleId = $('#edit_moduleId').val();
    data.Price = $('#edit_Price').val();
    data.NoOfDays = $('#edit_noOfDays').val();
    data.Discription = $('#edit_discription').val();
    debugger;
    if (data.Id != 0 && data.ModuleId != "" && data.Price != "" && data.NoOfDays != "" && data.Discription) {
        debugger;

        let moduleCostViewModel = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: '/SuperAdmin/EditmoduleCost',
            data:
            {
                costDetails: moduleCostViewModel,
            },
            success: function (result) {

                if (!result.isError) {
                    var url = '/SuperAdmin/ModuleCost';
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
    else {
        errorAlert("please fill the form correctly");
    }
}

function GetModuleCostByID(Id)
{
    let data = JSON.stringify(Id);
    $.ajax({
        type: 'GET',
        url: '/SuperAdmin/GetModuleCostByID',
        data: { moduleCostId: data },
        dataType: 'json',
        success: function (data)
        {
            debugger;
            if (!data.isError) {
                $("#delete_Id").val(data.id);
                $("#edit_Id").val(data.id);
                $("#edit_moduleId").val(data.moduleId);
                $("#edit_Price").val(data.price);
                $("#edit_noOfDays").val(data.noOfDays);
                $("#edit_discription").val(data.discription);
            }
        }
    });
}

function DeleteModuleCost() {
    debugger
    var data = {};
    data.Id = $('#delete_Id').val();
    if (data.Id != null) {
        let moduleCostViewModel = JSON.stringify(data);
        if (moduleCostViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/SuperAdmin/DeleteModuleCost',
                data:
                {
                    costDetails: moduleCostViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/SuperAdmin/ModuleCost';
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
        else {
            errorAlert("Incorrect Details");
        }
    }

}


function addSalesCustomer(action, Id) {
    var data = {};
    if (action == "CREATE") {
        data.ActionType = action;
        data.FirstName = $('#first_Name').val();
        data.LastName = $('#last_Name').val();
        data.PhoneNumber = $('#tel').val();
        data.Email = $('#email').val();
        data.Address = $('#address').val();
        if (data.FirstName != "" && data.LastName != "" && data.Address != "" && data.PhoneNumber != "") {
            debugger;
            let customerViewModel = JSON.stringify(data);
            if (customerViewModel != "") {
                $.ajax({
                    type: 'Post',
                    dataType: 'json',
                    url: '/Admin/CustomerPostAction',
                    data:
                    {
                        customerDetails: customerViewModel,
                    },
                    success: function (result) {
                        if (!result.isError) {
                            var url = '/Sales/AddSales';
                            successAlertWithRedirect(result.msg, url)
                        }
                        else {
                            infoAlert(result.msg)
                        }
                    },
                    error: function (ex) {
                        errorAlert("Error occured try again");
                    }
                });
            }
            else {
                errorAlert("Incorrect Details");
            }
        }
        else {
            errorAlert("Please fill the form correctly");
        }
    }
}



function CreateRoutine() {
    var data = {};
    data.Purpose = $('#Purpose').val();
    data.CurrentDate = $('#CurrentDate').val();
    data.NextDate = $('#NextDate').val();
    if (data.Purpose != "" && data.CurrentDate != "" && data.NextDate != "") {
        debugger
        let RoutineViewModel = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: '/Routine/CreateRoutine',
            data:
            {
                routineDetails: RoutineViewModel,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = '/Routine/Index';
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
    else {
        errorAlert("please fill the form correctly");
    }
    
}



function EditRoutine() {
    debugger
    var data = {};
    data.Id = $('#edit_Id').val();
    data.Purpose = $('#edit_Purpose').val();
    data.CurrentDate = $('#edit_CurrentDate').val();
    data.NextDate = $('#edit_NextDate').val();
    debugger;
    if (data.Purpose != "" && data.CurrentDate != "" && data.NextDate != "") {
        debugger;

        let RoutineViewModel = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: '/Routine/EditRoutine',
            data:
            {
                routineDetails: RoutineViewModel,
            },
            success: function (result) {
                if (!result.isError) {
                    var url = '/Routine/Index';
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
    else {
        errorAlert("please fill the form correctly");
    }
}

function GetRoutineByID(Id) {
    let data = JSON.stringify(Id);
    $.ajax({
        type: 'GET',
        url: '/Routine/GetRoutineByID',
        data: { routineID: data },
        dataType: 'json',
        success: function (data) {
            debugger;
            var dateData;
            var dateData1;
            if (!data.isError) {
                $("#delete_Id").val(data.id);
                $("#edit_Id").val(data.id);
                if (data.currentDate != null && data.nextDate != null) {
                    dateData = data.currentDate.split(/[-,T]/);
                    dateData1 = data.nextDate.split(/[-,T]/);
                    var date = dateData[0] + "-" + dateData[1] + "-" + dateData[2];
                    var dates = dateData1[0] + "-" + dateData1[1] + "-" + dateData1[2];
                    $('#edit_CurrentDate').val(date);
                    $('#edit_NextDate').val(dates);
                }
             
                $("#edit_Purpose").val(data.purpose);
               /* $("#edit_CurrentDate").val(data.currentDate);*/
               /* $("#edit_NextDate").val(data.nextDate);*/
            }
        }
    });
}

function DeleteRoutine() {
    debugger
    var data = {};
    data.Id = $('#delete_Id').val();
    if (data.Id != null) {
        let RoutineViewModel = JSON.stringify(data);
        if (RoutineViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Routine/DeleteRoutine',
                data:
                {
                    routineDetails: RoutineViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/Routine/Index';
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
        else {
            errorAlert("Incorrect Details");
        }
    }

}




