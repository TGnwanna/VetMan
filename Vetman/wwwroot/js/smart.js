function bookForExistingClient() {
	debugger;
	var clientAndBookingData = {};
	clientAndBookingData.CustomerId = $("#CustomerIdExistingUser").val();
	clientAndBookingData.IsExistingClient = true;
	clientAndBookingData.Id = $("#bookingGroupExistingUserId").val();
	clientAndBookingData.ProductPrice = $("#pricePerProductExistingUser").val();
	clientAndBookingData.InitialAmount = $("#amountExistingUser").val();
	clientAndBookingData.Quantity = $("#quantityExistingUser").val();
	if (parseInt(clientAndBookingData.InitialAmount) > 0 && clientAndBookingData.Id != "" && clientAndBookingData.CustomerId != "" &&
		clientAndBookingData.Quantity != "") {
		let details = JSON.stringify(clientAndBookingData);
		debugger;
		$.ajax({
			type: 'POST',
			url: '/Booking/UserBookings',
			dataType: 'json',
			data:
			{
				clientInfoAndBookings: details,
			},
			success: function (result) {
				debugger;
				if (!result.isError) {
					var url = "/Booking/BookProduct";
					newSuccessAlert(result.msg, url);
				}
				else {
					errorAlert(result.msg);
				}
			},
			error: function (ex) {
				errorAlert(ex);
			}
		});
	} else {
		errorAlert("Please fill the form properly");
	}
	
	   
}

function bookForNewClient() {
	debugger;
	var clientAndBookingData = {};
	clientAndBookingData.FirstName = $("#Fname").val();
	clientAndBookingData.LastName = $("#Lname").val();
	clientAndBookingData.PhoneNumber = $("#phoneNumber").val();
	clientAndBookingData.IsExistingClient = false;
	clientAndBookingData.Email = $("#email").val();
	clientAndBookingData.Address = $("#address").val();
	clientAndBookingData.Id = $("#bookingGroupId").val();
	clientAndBookingData.ProductPrice = $("#pricePerProduct").val();
	clientAndBookingData.InitialAmount = $("#amount").val();
	clientAndBookingData.Quantity = $("#quantity").val();
	if (parseInt(clientAndBookingData.InitialAmount) > 0 && clientAndBookingData.BookingGroupId != "" && clientAndBookingData.FirstName != ""
		&& clientAndBookingData.Quantity != "" && clientAndBookingData.PhoneNumber != "" && clientAndBookingData.Address != "") {
		let details = JSON.stringify(clientAndBookingData);
		debugger;
		$.ajax({
			type: 'POST',
			url: '/Booking/UserBookings',
			dataType: 'json',
			data:
			{
				clientInfoAndBookings: details,
			},
			success: function (result) {
				debugger;
				if (!result.isError) {
					var url = location.href;
					newSuccessAlert(result.msg, url);
				}
				else {
					errorAlert(result.msg);
				}
			},
			error: function (ex) {
				errorAlert(ex);
			}
		});
	} else {
		errorAlert("Please fill the form properly");
	}


}

$("#quantity").keyup(function () {
	var quantity = $("#quantity").val();
	var amount = $("#pricePerProduct").val();
	var initialAmount = $("#amount").val();
	var result = parseFloat(quantity) * parseFloat(amount);
	$("#totalPrice").val(result);
	var totalPriceCalculated = $("#totalPrice").val();
	if (initialAmount != "")
	{
		var amountResult = parseFloat(totalPriceCalculated) - parseFloat(initialAmount);

		$("#balanceAfterPayment").val(amountResult);
    }
})

$("#quantityExistingUser").keyup(function () {
	var quantity = $("#quantityExistingUser").val();
	var amount = $("#pricePerProductExistingUser").val();
	var initialAmount = $("#amountExistingUser").val();
	var result = parseFloat(quantity) * parseFloat(amount);
	$("#totalPriceExistingUser").val(result);
	var totalPriceCalculated = $("#totalPriceExistingUser").val();
	if (initialAmount != "") {
		var amountResult = parseFloat(totalPriceCalculated) - parseFloat(initialAmount);

		$("#balanceAfterPaymentExistingUser").val(amountResult);
	}
})


$("#bookingGroupId").change(function () {
	debugger;
	var groupId = $("#bookingGroupId").val();
	$.ajax({
		type: 'GET',
		url: '/Booking/GetGroupPrice', //we are calling json method
		dataType: 'json',
		data: { id: groupId },
		success: function (result) {
			if (!result.isError) {
				$("#pricePerProduct").val(result.data);
			}
		},
		error: function (ex) {
			alert('Failed to retrieve State.' + ex);
		}
	});
});


function updatePrice() {

	debugger;
	var data = {};
	data.ExpectedPrice = $('#newPrice').val();
	data.Id = $('#bookingGroupId').val();
	data.IsPrice = true;
	var sendSMS = $('#sendSMS').is(":checked");
	if (sendSMS) {
		data.SendSMS = true;
	}
	else {
		data.SendSMS = false;
	}
	var sendEmail = $('#sendEmail').is(":checked");
	if (sendEmail) {
		data.SendEmail = true;
	} else {
		data.SendEmail = false;
	}
	if (data.ExpectedPrice != "") {
		var priceDetails = JSON.stringify(data);
		$.ajax({
			type: 'Post',
			url: '/CustomerPayment/UpdatePrice', // we are calling json method
			dataType: 'json',
			data:
			{
				priceDetails: priceDetails
			},
			success: function (result) {
				debugger;
				if (!result.isError) {
					var url = '/CustomerPayment/ManageGroup?bookingGroupId=' + result.data.id;
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
		errorAlert("Please fill the form correctly");
	}
}

function addPayment() {
	debugger;
	var data = {};
	data.Amount = $('#amount').val();
	data.DatePaid = $('#date').val();
	data.UpdatedByUserId = $('#updatedByUserId').val();
	data.CustomerBookingId = $('#customerBookingId').val();
	var paymentDetails = JSON.stringify(data);
	$.ajax({
		type: 'Post',
		url: '/CustomerPayment/AddPayment',
		dataType: 'json',
		data: {
			paymentDetails: paymentDetails
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				var url = 'CustomerPayment/CustomerPayment?customerBookingId=' + result;
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
$("#quantityExistingUser").keyup(function () {
	debugger;
	var quantity = $("#quantityExistingUser").val();
	var amount = $("#pricePerProductExistingUser").val();
	var initialAmount = $("#amountExistingUser").val();
	var result = parseFloat(quantity) * parseFloat(amount)
	$("#totalPriceExistingUser").val(result);

	var totalPriceCalculated = $("#totalPriceExistingUser").val();
	if (initialAmount != "") {
		var amountResult = parseFloat(totalPriceCalculated) - parseFloat(initialAmount);

		$("#balanceAfterPayment").val(amountResult);
	}
})


$("#bookingGroupExistingUserId").change(function () {
	var groupId = $("#bookingGroupExistingUserId").val();
	debugger;
	$.ajax({
		type: 'GET',
		url: '/Booking/GetGroupPrice', //we are calling json method
		dataType: 'json',
		data: { id: groupId },
		success: function (result) {
			if (!result.isError) {
				$("#pricePerProductExistingUser").val(result.data);
			}
		},
		error: function (ex) {
			alert('Failed to retrieve State.' + ex);
		}
	});
});
function registerUser() {
	debugger;
	var data = {};
	data.Email = $('#email').val();
	data.PassWord = $('#password').val();
	data.ConfirmPassword = $('#confirmPassword').val();
	data.FirstName = $('#firstName').val();
	data.LastName = $('#lastName').val();
	let userDetails = JSON.stringify(data);
	$.ajax({
	type: 'Post',
		url: '/Account/Register',
		dataType: 'json',
		data:
		{
		   userDetails: userDetails,
		},
		success: function (result)
		{
			debugger;
			if (!result.isError)
			{
				var url = '/Account/Login';
				successAlertWithRedirect(result.msg, url);
			}
			else
			{
				errorAlert(result.msg);
			}
		},
		error: function (ex)
		{
			"Something went wrong, contact support - " + errorAlert(ex);
		}
	});
}
function login() {
	var email = $('#email').val();
	var password = $('#password').val();
	$.ajax({
		type: 'Post',
		url: '/Account/Login',
		dataType: 'json',
		data:
		{
			 email: email,
			 password: password
		},
		success: function (result)
		{
			debugger;
			if (!result.isError)
			{
				var n = 1;
				localStorage.clear();
				localStorage.setItem("on_load_counter", n);
				location.href = result.dashboard;
			}
			else
			{
				errorAlert(result.msg);
			}
		},
		error: function (ex)
		{
			"Something went wrong, contact support - " + errorAlert(ex);
		}
	});
}
function getExistingPrice(id) {
	debugger;
	$.ajax({
	type: 'GET',
		url: '/CustomerPayment/UpdatePrice', // we are calling json method
		dataType: 'json',
		data:
		{
			bookingGroupId: id
		},
		success: function (data) {
			debugger;
			$('#existingPrice').val(data.data.expectedPrice);
			$('#bookingGroupId').val(data.data.id);
		 },
		error: function (ex) {
			"Something went wrong, contact support - " + errorAlert(ex);
		}
	});
}

function getCustomerBookingToEdit(id) {
	debugger;
	$.ajax({
		type: 'GET',
		url: '/Booking/EditBooking',
		dataType: 'json',
		data: { id: id },
		success: function (result) {
			debugger;
			if (!result.isError) {
				$("#editId").val(result.data.id);
				$("#editCustomerId").val(result.data.customerId);
				$("#editName").val(result.data.customerName);
				$("#editBookingGroupId").val(result.data.bookingGroupId);
				$("#editQuantity").val(result.data.quantityBooked);
				$("#editproductGroup").val(result.data.bookingGroupName)
				$("#editAmount").val(result.data.totalPaid);
				$("#editPhone").val(result.data.phoneNumber);
				var datePaid = result.data.bookingDate.split("T");
				$("#editBookedDate").val(datePaid[0]);
				$("#editBalance").val(result.data.balance);
				$("#editTotal").val(result.data.totalAmount);
				$("#editExpectedPrice").val(result.data.expectedPrice);

			}
			else
				errorAlert(result.msg);
		}
	});
}

$("#editQuantity").change(function () {
	var quantity = $("#editQuantity").val();
	var amount = $("#editExpectedPrice").val();
	var result = parseFloat(quantity) * parseFloat(amount)
	var amountPaid = $("#editAmount").val();
	var balanceToPay = parseFloat(result) - parseFloat(amountPaid)
	$("#editBalance").val(balanceToPay);
	$("#editTotal").val(result);
})

$("#editAmount").change(function () {
	var amountPaid = $("#editAmount").val();
	var totalToPay = $("#editTotal").val();
	var result = parseFloat(totalToPay) - parseFloat(amountPaid)
	$("#editBalance").val(result);
})

function SaveEdit() {
	debugger;
	var customerGroup = {};
	customerGroup.Id =  $("#editId").val();
	customerGroup.CustomerId =  $("#editCustomerId").val();
	customerGroup.BookingGroupId = $("#editBookingGroupId").val();
	customerGroup.QuantityBooked =  $("#editQuantity").val();
	
	let group = JSON.stringify(customerGroup);
	$.ajax({
		type: 'POST',
		url: '/Booking/SaveEditedBooking',
		dataType: 'json',
		data:
		{
			group: group,
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				var url = '/Booking/Index';
				newSuccessAlert(result.msg, url);
			}
			else {
				errorAlert(result.msg);
			}
		},
		error: function (ex) {
			errorAlert(ex);
		}
	});
}

function getCustomerBookingToDelete(id) {
	$('#DeleteCustomerBookId').val(id);
}

function CustomerBookingToDelete() {

	var customerBookId = $("#DeleteCustomerBookId").val();
	$.ajax({
		type: 'POST',
		dataType: 'JSON',
		url: "/Booking/DeleteCustomerBooking",
		data:
		{
			id: customerBookId
		},
		success: function (result) {
			debugger;
			if (!result.isError) {

				var url = "/Booking/Index";
				newSuccessAlert(result.msg, url);

			}
			else {
				errorAlert(result.msg);
			}
		}
	});

}
function updateExpectedDate(id) {
	debugger;
	$.ajax({
		type: 'GET',
		url: '/CustomerPayment/UpdatePrice', // we are calling json method
		dataType: 'json',
		data:
		{
			bookingGroupId: id
		},
		success: function (data) {
			debugger;
			$('#existingDate').val(data.date);
			$('#groupId').val(data.data.id);
		},
		error: function (ex) {
			"Something went wrong, contact support - " + errorAlert(ex);
		}
	});
}

function updateDate() {
	debugger;
	var data = {};
	data.ExpectedDateOfArrival = $('#newDate').val();
	data.Id = $('#groupId').val();
	data.IsDate = true;
	var sendSMS = $('#sendSMSForDate').is(":checked");
	if (sendSMS) {
		data.SendSMS = true;
	}
	else {
		data.SendSMS = false;
	}
	var sendEmail = $('#sendEmailForEmail').is(":checked");
	if (sendEmail) {
		data.SendEmail = true;
	} else {
		data.SendEmail = false;
	}
	if (data.ExpectedDateOfArrival != "") {
		var dateDetails = JSON.stringify(data);
		$.ajax({
			type: 'Post',
			url: '/CustomerPayment/UpdateDate', // we are calling json method
			dataType: 'json',
			data:
			{
				dateDetails: dateDetails
			},
			success: function (result) {
				debugger;
				if (!result.isError) {
					var url = '/CustomerPayment/ManageGroup?bookingGroupId=' + result.data.id;
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
		errorAlert("Please fill the form correctly");
    }
	
}

$("#amountExistingUser").keyup(function () {
	debugger;
	var totalToPay = $("#totalPriceExistingUser").val();
	var initialToPay = $("#amountExistingUser").val();
	if (initialToPay != "" && totalToPay != "") {
		var result = parseFloat(totalToPay) - parseFloat(initialToPay)
		$("#balanceAfterPaymentExistingUser").val(result);
	}
})

$("#amount").keyup(function () {
	debugger;
	var totalToPay = $("#totalPrice").val();
	var initialToPay = $("#amount").val();
	if (initialToPay != "" && totalToPay != "") {
		var result = parseFloat(totalToPay) - parseFloat(initialToPay)
		$("#balanceAfterPayment").val(result);
	}
})

$("#bookingGroupId").change(function () {
	debugger;
	var bookingId = $("#bookingGroupId").val();
	if (bookingId != "00000000-0000-0000-0000-000000000000")
		$("#quantity").attr("readonly", false);
	else
		$("#quantity").attr("readonly", true); 
})
	
$("#bookingGroupExistingUserId").change(function () {
	debugger;
	var bookingId = $("#bookingGroupExistingUserId").val();
	if (bookingId != "00000000-0000-0000-0000-000000000000")
		$("#quantityExistingUser").attr("readonly", false);
	else
		$("#quantityExistingUser").attr("readonly", true);
})

function registerCompany() {
	debugger;
	var data = {};
	data.Name = $('#companyName').val();
	data.Email = $('#email').val();
	data.CompanyAddress = $('#companyAddress').val();
	data.Address = $('#address').val();
	data.Phone = $('#phoneNumber').val();
	data.Mobile = $('#mobileNumber').val();
	data.FirstName = $('#firstName').val();
	data.LastName = $('#lastName').val();
	data.Password = $('#password').val();
	data.IsAdmin = true;
	data.ConfirmPassword = $('#confirmpassword').val();
	var companyDetails = JSON.stringify(data);
	$.ajax({
		type: 'Post',
		url: '/Account/CompanyRegistration', // we are calling json method
		dataType: 'json',
		data:
		{
			companyDetails: companyDetails
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				var url = '/Account/Login';
				successAlertWithRedirect(result.msg, url);
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
var checkedFeature = [];
var uncheckedFeature = [];
function getCheckedAccess() {
	debugger;
	var allCheckedFeatures = document.getElementsByClassName("feature");

	for (var i = 0; i < allCheckedFeatures.length; i++) {

		if (allCheckedFeatures[i].checked) {
			checkedFeature.push(allCheckedFeatures[i].name);
		}
		else {
			uncheckedFeature.push(allCheckedFeatures[i].name)
		}
	}
}

function SaveData() {
	debugger;
	$('#loader').show();
	$('#loader-wrapper').show();
	getCheckedAccess();
	var companyId = $('#companyId').val();
	var checkedListFeaturesJs = checkedFeature;
	var unCheckedListFeaturesJs = uncheckedFeature;

	$.ajax({
		type: 'POST',
		dataType: 'Json',
		url: '/SuperAdmin/CompanyCustomSetting',
		data:
		{
			companyId: companyId,
			checkedCompanySettings: checkedListFeaturesJs,
			uncheckedCompanySettings: unCheckedListFeaturesJs
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var reloadUrl = "/SuperAdmin/CompanySetting?companyId=" + companyId;
				newSuccessAlert(result.msg, reloadUrl);
			}

			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				errorAlert(result.msg);
			}
		},
		error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}
	
function makePayment() {
    debugger;
    var data = {};
    data.UserId = $("#userId").val();
	data.CompanyBranchId = $("#companyBranchIdWallet").val();
	data.CompanyId = $("#companyId").val();
	data.walletId = $("#walletId").val();
    data.Amount = $("#amountDeposit").val();
    
    if (data.Amount != "0" && data.Amount != "") {
        let wallet = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            url: '/Wallet/MakePayment',
            data:
            {
                wallet: wallet,
            },
            success: function (result) {

                if (result.isError) {
                    errorAlert(result.msg);
                }
                else if (result.isPayStack) {


					Swal.fire
						({
							title: "Success",
							text: "You're good to go,Let's make the payment now",
							icon: "success",
							timer: "30000",
							confirmButtonColor: "#FF9B44",

                        })
                        .then(function () {
                            location.href = result.paystackUrl;
                        });
                }
            },
            error: function (ex) {
                errorAlert(ex + " internal error occured");
            }
        });
    }
    else {
        errorAlert("Invalid Payment Method,Please Select how you want to pay for your order.")
    }
    
    
}


$("#amountDeposit").keyup(function () {
	debugger;
	var amount = $("#amountDeposit").val();
	var payStackRate = ((amount * 1.5) / 100) + 100;
	var result = amount - payStackRate;
	if (result > 0) {
		$("#displayAmount").text(result);
    }
});

$('#sendSMSForDate').change(function () {
	debugger;
	var check = $('#sendSMSForDate').is(":checked");
	if (check) {
		$.ajax({
			type: 'POST',
			url: '/Wallet/CheckBalance',
			success: function (result) {
				var numberOfCustomer = $("#customerCount").val();
				var smsCost = $("#smsCost").val();
				if (numberOfCustomer > 0 && smsCost > 0) {
					var charge = parseInt(numberOfCustomer) * smsCost;
					if (result.data > charge) {
						$("#infomation").append('<span> You will be charged <span class="text-success">&#x20A6;' + charge + ' </span>for the sms.</span>');
					}
					else {
						$("#infomation").append('<span class="text-danger"> Insufficient fund please fund your wallet.</span>');
					}
				}
				
			},
			error: function (ex) {
				errorAlert(ex + " internal error occured");
			}
		});
	} else {
		$("#infomation").empty();
	}
});

$('#sendSMS').change(function () {
	debugger;

	var check = $('#sendSMS').is(":checked");
	if (check) {
		$.ajax({
			type: 'POST',
			url: '/Wallet/CheckBalance',
			success: function (result) {
				var numberOfCustomer = $("#customerCount").val();
				var smsCost = $("#smsCost").val();
				if (numberOfCustomer > 0 && smsCost > 0 ) {
					var charge = parseInt(numberOfCustomer) * smsCost;
					if (result.data > charge) {
						$("#infomationPrice").append('<span> You will be charged <span class="text-success">&#x20A6;' + charge + ' </span>for the sms.</span>');
					}
					else {
						$("#infomationPrice").append('<span class="text-danger"> Insufficient fund please fund your wallet.</span>');
					}
                }
				

			},
			error: function (ex) {
				errorAlert(ex + " internal error occured");
			}
		});

	} else {
		$("#infomationPrice").empty();
	}
});

function updateCustomerBooking()
{ 
	var data = {};
	/*data.Email = $('#sub_email_module').val();*/
	/*data.Sms = $('#sub_sms_module').val();*/
	data.Id = $("#CustomerBookingId").val();
	data.TextArea = $("#TextArea").val();
	var sendSMS = $('#SmsSubscribed').is(":checked");
	if (sendSMS) {
		data.SmsSubscribed = true;
	}
	
	var sendEmail = $('#EmailSubscribed').is(":checked");
	if (sendEmail) {
		data.EmailSubscribed = true;
	} 

	var subscriberOption = JSON.stringify(data);
	debugger;
	$.ajax({
		type: 'POST',
		dataType: 'Json',
		url: '/CustomerPayment/UpdateCustomerBookingStatus',
		data:
		{
			subscriberOption: subscriberOption,
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var reloadUrl = "/CustomerPayment/ManageGroup?bookingGroupId=" + result.bookingGroupId;
				newSuccessAlert(result.msg, reloadUrl);
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				infoAlertWithStyles(result.msg);
			}
		},
		error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}
function moveToNewGrouperBooking(id) {
	debugger;
	$.ajax({
		type: 'Get',
		dataType: 'Json',
		url: '/CustomerPayment/MoveUserToNewGroup',
		data:
		{
			customerBookingId: id,
		},
		success: function (data) {
			debugger;
			if (!data.isError) {
				$('#existingGroup').val(data.data.bookingGroupName);
				$('#customerBookingId').val(data.data.id);
				$('#update-Group').modal();
			}
			else
			{
				infoAlert(data.msg)
            }
			
		},
		error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}
function updateGroup() {
	debugger;
	var data = {};
	data.Id = $('#customerBookingId').val();
	data.BookingGroupId = $('#newGroup').val();
	var groupDetails = JSON.stringify(data);
	debugger;
	$.ajax({
		type: 'POST',
		dataType: 'Json',
		url: '/CustomerPayment/MoveUserToNewGroup',
		data:
		{
			groupDetails: groupDetails,
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var reloadUrl = "/CustomerPayment/ManageGroup?bookingGroupId=" + result.bookingGroupId;
				newSuccessAlert(result.msg, reloadUrl);
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				infoAlertWithStyles(result.msg);
			}
		},
		error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}
function moveUserToNewGroup(id) {
	debugger;
	var newGroupId = $('#UserNewGroupId').val();
	$.ajax({
		type: 'POST',
		dataType: 'Json',
		url: '/CustomerPayment/MoveCustomersToNewGroup',
		data:
		{
			bookingGroupId: id,
			newGroupId: newGroupId
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var reloadUrl = "/CustomerPayment/ManageGroup?bookingGroupId=" + result.bookingGroupId;
				newSuccessAlert(result.msg, reloadUrl);
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				errorAlert(result.msg);
			}
		},
		error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}

function SaveAccess() {
	debugger;
	var data = {};
	data.Id = $('#roleId').val();
	data.BranchId = $('#branchId').val();
	var getAllAccess = document.getElementsByClassName("access");
	var checkedRoleSettings = [];
	var uncheckedRoleSettings = [];
	$.each(getAllAccess, function (i, access) {
		
		if (access.checked) {
			checkedRoleSettings.push(access.name);
		} else {
			uncheckedRoleSettings.push(access.name);
        }
	});
	var details = JSON.stringify(data);
	$.ajax({
		type: 'POST',
		dataType: 'Json',
		url: '/Admin/UpdateAccess',
		data:
		{
			details: details,
			checkedRoleSettings: checkedRoleSettings,
			uncheckedRoleSettings: uncheckedRoleSettings,
		},
		success: function (result) {
			debugger;
			
			if (!result.isError) {
				var url = "/Admin/CompanySetting/" + result.data;
				newSuccessAlert(result.msg, url);
			}
			else {
				errorAlert(result.msg);
			}
		},
		error: function (ex) {
			errorAlert(ex);
		}
	});
} 
function impersonateCompany(email) {
	debugger;
	$.ajax({
		type: 'POST',
		url: '/SuperAdmin/ImpersonateCompanyAdmin',
		data:
		{
			email: email
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				var url = "/Admin/Index";
				$("#superAdminId").val(result.data);
				successAlertWithRedirect(result.msg, url);
			}
			else {
				errorAlert(result.msg)
			}
		},
		error: function (ex) {
			errorAlert(ex);
		}
	});
}
function companyToBeDeleted(id)
{
	debugger;
	$('#companyId').val(id);
}
function deleteCompany() {
	debugger;
	var companyId = $('#companyId').val();
	$.ajax({
		type: 'Post',
		url: "/SuperAdmin/DeleteCompany",
		dataType: 'Json',
		data:
		{
			companyId: companyId
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				debugger;
				var url = "/SuperAdmin/Index";
				newSuccessAlert(result.msg, url);

			} else {
				errorAlert(result.msg);
			}
		}
	});
}
function resetPassword() {
	debugger;
	$('#loader').show();
	$('#loader-wrapper').show();
	var newLogInPassword = {};
	newLogInPassword.Password = $('#password').val();
	newLogInPassword.ConfirmPassword = $('#confirmPassword').val();
	newLogInPassword.Token = $('#token').val();
	let passwordViewModel = JSON.stringify(newLogInPassword);
	$.ajax({
		type: 'POST',
		url: '/Account/ResetPassword/',
		data:
		{
			passwordResetViewmodel: passwordViewModel
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var url = "/Account/Login";
				successAlertWithRedirect(result.msg, url);
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				newErrorAlert(result.msg, url);
			}
		},
		Error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}
function confirmTransaction() {
	debugger;
	var transactionId = $('#transactionId').val();
	var paymentReceipt = $('#receipt').val();
	debugger;
	$.ajax({
		type: 'POST',
		dataType: 'Json',
		url: '/Transaction/ConfirmTransaction',
		data:
		{
			transactionId: transactionId,
			paymentReceipt: paymentReceipt,
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				var reloadUrl = "/Transaction/Index";
				successAlertWithRedirect(result.mgs, reloadUrl);
			}
		},
		error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}
function transactionToBeConfirmed(id) {
	debugger;
	$('#transactionId').val(id);
}
function DeliveryBtn(id) {
	debugger;
	$.ajax({
		type: 'GET',
		url: '/CustomerPayment/UpdateCustomerBookingDetails', // we are calling json method
		dataType: 'json',
		data:
		{
			customerPaymentId: id
		},
		success: function (data) {
			debugger;
			if (!data.isError) {

				$("#add_deliver").modal();
				$("#CustomerBookingId").val(data.data.id)
			}
			else if (data.isError && data == 'Delivered')
			{
				infoAlertWithStyles(data.msg)
			}
			else
			{
				infoAlert(data.msg)
			}
		},
		error: function (ex) {
			"Something went wrong, contact support - " + errorAlert(ex);
		}
	});
}
function impersonateBranch() {
	debugger;
	var companyBranchId = $('#companyBranchId').val();
	$.ajax({
		type: 'POST',
		dataType: 'Json',
		url: '/Admin/ImpersonateCompanyBranch',
		data:
		{
			companyBranchId: companyBranchId,
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				var url = location.reload;
				newSuccessAlert(result.msg, url);
			}
			else {
				errorAlert(result.msg);
			}
		},
		error: function (ex) {
			errorAlert(ex);
		}
	});
}

function bookingsByProductId() {
	productId = $('#bookingProductId').val();
	debugger;
	var url = '/Booking/ProductBookings?projectId=' + productId;
	window.location = url;
}

function filterBookingByDateRange() {
	debugger
	var dateFrom = $("#dateFrom").val();
	var dateTo = $("#dateTo").val();
	$.ajax({
		type: 'Post',
		url: '/Booking/GetBookingsByDateRange',
		dataType: 'json',
		data:
		{
			dateFrom : dateFrom,
			dateTo : dateTo,
		},
		success: function (result) {
			debugger;
			$("#bookingDetails").empty();
			if (!result.isError) {
				$.each(result.data, function (i, bookingGroup) {
					debugger;
					var action;
					var manageGroup;
					action = '<td class="text-right">' +
						'<div class="dropdown dropdown-action">' +
						'<a href="#" class="action-icon dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><i class="material-icons">more_vert</i></a>' +
						'<div class="dropdown-menu dropdown-menu-right">' +
						'<a class="dropdown-item" data-toggle="modal" data-target="#edit_termination" onclick="GetBookingGroupById(' + bookingGroup.id + ')"><i class="fa fa-pencil m-r-5"></i> Edit</a>' +
						'<a class="dropdown-item" data-toggle="modal" data -target="#delete_termination" onclick="GetBookingGroupById(' + bookingGroup.id + ')"><i class="fa fa-trash-o m-r-5"></i> Delete</a>' +
						'</div>' +
						'</div>' +
						'</td>'
					manageGroup = '<td class="float-left"><a class="btn add-btn" asp-action="ManageGroup" asp-controller="CustomerPayment" asp-route-bookingGroupId="' + bookingGroup.id +'">Manage</a></td>'
					debugger;
					$("#bookingDetails").append('<tr>' +
						'<td>' + '</td>' +
						'<td>' + bookingGroup.name + '</td>' +
						'<td>' + bookingGroup.expectedCostPrice + '</td>' +
						'<td>' + bookingGroup.expectedDateOfArrival + '</td>' +
						'<td>' + bookingGroup.totalQuantityBooked + '</td>' +
						manageGroup +
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

function transactionSetUp() {
	debugger;
	var data = {};
	data.SenderName = $('#sender').val();
	data.KeyWord = $('#keyword').val();
	data.ValidMessageChecker = $('#validMessageChecker').val();
	var setupDetails = JSON.stringify(data);
	$.ajax({
		type: 'POST',
		url: '/Transaction/TransactionSetUp',
		data:
		{
			setupDetails: setupDetails
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var url = "/Transaction/TransactionSettings";
				successAlertWithRedirect(result.msg, url);
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				newErrorAlert(result.msg, url);
			}
		},
		Error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}

function transactionSetUpToBeEdidted(transactionSettingId) {
	debugger;
	$.ajax({
		type: 'get',
		url: '/Transaction/EditTransactionSetUp',
		data:
		{
			transactionSettingId: transactionSettingId
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				$("#editedSender").val(result.data.name);
				$("#editedKeyword").val(result.data.keyWord);
				$("#editedValidMessageChecker").val(result.data.validMessageChecker);
				$("#transactionSettingId").val(result.data.id);
			}
		},
		Error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}

function edittransactionSetUp() {
	debugger;
	var data = {};
	data.Id = $('#transactionSettingId').val();
	data.SenderName = $('#editedSender').val();
	data.KeyWord = $('#editedKeyword').val();
	data.ValidMessageChecker = $('#editedValidMessageChecker').val();
	var setupDetails = JSON.stringify(data);
	$.ajax({
		type: 'Post',
		url: '/Transaction/EditTransactionSetUp',
		data:
		{
			setupDetails: setupDetails
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var url = "/Transaction/TransactionSettings";
				successAlertWithRedirect(result.msg, url);
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				newErrorAlert(result.msg, url);
			}
		},
		Error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}

function transactionDetailsToBeDeleted(id) {
	debugger;
	$('#transactionSettingId').val(id);
}

function deleteTransactionSetUp() {
	debugger;
	var transactionSettingId = $('#transactionSettingId').val();
	$.ajax({
		type: 'POST',
		url: '/Transaction/DeleteTransactionSetUp',
		data:
		{
			transactionSettingId: transactionSettingId
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var url = "/Transaction/TransactionSettings";
				successAlertWithRedirect(result.msg, url);
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				newErrorAlert(result.msg, url);
			}
		},
		Error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}

$("#getOldRecord").change(function () {
	debugger;
	var isChecked = $(this).is(":checked");
	if (isChecked) {
		$.ajax({
			type: 'GET',
			url: '/Admin/GetAllBooking',
			success: function (result) {
				debugger;
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				if (!result.isError) {
					$("#getOldRecord").val(true);
					var resul = $("#addTableBody  tbody").attr("id", "bookingDetails");
					$("#dataBoard").addClass("boardShape");
					$("#bookingDetails").empty();
					$.each(result.data, function (i, group) {

						var action = '<div class="dropdown dropdown-action">' +
							'<a href="#" class="action-icon dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><i class="material-icons">more_vert</i></a>' +
							'<div class="dropdown-menu dropdown-menu-right">' +
							'<a class="dropdown-item" data-toggle="modal" data-target="#edit_termination"' + 'onclick="GetBookingGroupById(' + "'" + group.id + "'" + ')"><i class="fa fa-pencil m-r-5"></i> Edit</a>' +
							'<a class="dropdown-item" data-toggle="modal" data-target="#delete_termination" onclick="GetBookingGroupById(' + "'" + group.id + "'" + ')"><i class="fa fa-trash-o m-r-5"></i> Delete</a>' +
							'</div>' +
							'</div>';

						$("#bookingDetails").append('<tr class="even">' +
							'<td class="sorting_1"></td >' +
							'<td>' + group.name + '</td>' +
							'<td>' + group.expectedCostPrice + '</td>' +
							'<td>' + group.expectedDateOfArrival + '</td>' +
							'<td>' + group.totalQuantityBooked + '</td>' +
							'<td class="float-left"><a class="btn add-btn" href="/CustomerPayment/ManageGroup?bookingGroupId=' + group.id + '">Manage</a></td>' +
							'<td class="text-right">' + action + '</td>' +
							'</tr>');
					});
				}
				else {
					$('#loader').show();
					$('#loader-wrapper').fadeOut(3000);
					newErrorAlert(result.msg);
				}
			},
			Error: function (ex) {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				errorAlert(ex);
			}
		});
	} else {
		$("#bookingDetails").empty();
    }
	
});

$("#link").on("click", function () {
	debugger;
	$.ajax({
		type: 'GET',
		url: '/Admin/GetBookingLink',
		success: function (data) {
			var text = data;
			debugger;
			var $temp = $("<input>");
			$("body").append($temp);
			$temp.val(text).select();
			document.execCommand("copy");
			$temp.remove();
			successAlert("Booking Link copied Successfuly");
		}
	});
});
function guestBooking() {
	$('#loader').show();
	$('#loader-wrapper').fadeOut(3000);
	debugger;
	var bookingDeatils = {};
	bookingDeatils.FirstName = $("#Fname").val();
	bookingDeatils.LastName = $("#Lname").val();
	bookingDeatils.PhoneNumber = $("#phoneNumber").val();
	bookingDeatils.Email = $("#email").val();
	bookingDeatils.Address = $("#address").val();
	bookingDeatils.BookingId = $("#bookingGroupId").val();
	bookingDeatils.CompanyBranchId = $("#companyBranchId").val();
	bookingDeatils.Quantity = $("#quantity").val();
	bookingDeatils.TotalPrice = $("#totalPrice").val();
	let details = JSON.stringify(bookingDeatils);
	debugger;
	$.ajax({
		type: 'POST',
		url: '/Guest/GuestBooking',
		dataType: 'json',
		data:
		{
			guestBookingViewModel: details,
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				Swal.fire
					({
						title: "Success",
						text: "You're good to go,Let's make the payment now",
						icon: "success",
						timer: "30000",
						overlay: "background - color: rgba(43, 165, 137, 0.45)",
						confirmButtonColor: "#FF9B44",
					})
					.then(function () {
						$('#loader').show(3000);
						$('#loader-wrapper').fadeOut(3000);
						location.href = result;
					})
			}
			else {
				errorAlert(result.msg);
				$('#loader').show(3000);
				$('#loader-wrapper').fadeOut(3000);
			}
		},
		error: function (ex) {
			$('#loader').show(3000);
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
	
}

function addPatitientVisit() {
	debugger;
	var data = {};
	data.TreatmentHistory = $('#treatmentHistoryId').val();
	data.PatientId = $('#patientId').val();
	data.VaccinationHistory = $('#vaccinationHistoryId').val();
	data.EnvironmentalHistory = $('#environmentalHistoryId').val();
	data.FeedingHistory = $('#feedingHistoryId').val();
	data.GeneralExamination = $('#generalExaminationId').val();
	data.PhysicalExamination = $('#physicalExaminationId').val();
	data.PhysiologicalExamination = $('#physiologicalExaminationId').val();
	data.LaboratoryExamination = $('#laboratoryExaminationId').val();
	data.LaboratorySamples = $('#laboratorySamplesId').val();
	data.LaboratoryResults = $('#laboratoryResultsId').val();
	data.DifferentialDiagnosis = $('#differentialDiagnosisId').val();
	data.DefinitiveDiagnosis = $('#definitiveDiagnosisId').val();
	data.PrimaryComplaint = $('#primaryComplainId').val();
	data.Purpose = $('#purposeId').val();
	data.NextDate = $('#nextDateId').val();
	var sendSMS = $('#sendSMS').is(":checked");
	if (sendSMS) {
		data.SMSAllowed = true;
	}
	else {
		data.EmailAllowed = false;
	}
	var sendEmail = $('#sendEmail').is(":checked");
	if (sendEmail) {
		data.SendEmail = true;
	} else {
		data.SendEmail = false;
	}
	let details = JSON.stringify(data);
	$.ajax({
		type: 'POST',
		url: '/Visit/AddPatientVisit',
		data:
		{
			patientVisitDeta: details
		},
		success: function (result) {
			debugger;
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			if (!result.isError) {
				var url = result.returnUrl;
				newSuccessAlert(result.msg, url)
			}
			else {
				$('#loader').show();
				$('#loader-wrapper').fadeOut(3000);
				newErrorAlert(result.msg, url);
			}
		},
		Error: function (ex) {
			$('#loader').show();
			$('#loader-wrapper').fadeOut(3000);
			errorAlert(ex);
		}
	});
}


$(function () {
	$(".makeFilter").select2();
});

$("#patientId").change(function () {
	var patientId = $('#patientId').val();
	debugger;
	$.ajax({
		type: 'Get',
		url: '/Visit/GetPatientDetails',
		data:
		{
			patientId: patientId
		},
		success: function (result) {
			debugger;
			$('#caseNumber').val(result);
		},
	});
});
function append() {
	debugger;
	var total;
	var treatmentDetails = [];
	var treatmentDetail = {};
	var cost = parseFloat($('#costId').val());
	var treatmentName = $('#treatmentName').val();
	var discount = parseFloat($('#discountId').val());
	if (discount.toString() == "NaN") {
		discount = 0;
	}
	var total = cost - discount;
	var action =
		'<span class="action-circle large sale-delete-btn" title = "Delete Task">' +
		'<i class="material-icons">delete</i>' +
		'</span> ';
	var template = '<tr class="task">' +
		'<td></td>' +
		'<td>' + treatmentName + '</td>' +
		'<td>' + cost + '</td>' +
		'<td>' + discount + '</td>' +
		'<td>' + total + '</td>' +
		'<td>' + action + '</td>';
	// Make a new task template
	var newTemplate = $(template).clone();
	$('#addItem').append(newTemplate);
	var amt = parseFloat(total);
	var prevAmount = $("#totalAmountToPay").text();
	if (prevAmount != "" && prevAmount != "0") {
		var realValue = prevAmount.replace(",", "");
		var initialAmount = parseFloat(realValue);
		var total = initialAmount + amt;
		$("#totalAmountToPay").text(total);
	} else {
		$("#totalAmountToPay").text(amt);
	}
	treatmentDetail.Cost = cost;
	treatmentDetail.Discount = discount;
	treatmentDetail.TreatmentId = $('#treamentId').val();
	treatmentDetail.VisitId = $('#visitId').val();
	updateNotification('added to list');
	var treatments = localStorage.getItem("treatments");
	if (treatments != "" && treatments != "[]" && treatments != null) {
		treatmentDetails = JSON.parse(treatments);
		if (treatments.includes(treatmentDetail.Id)) {
			var count = 0;
			$.each(treatmentDetails, function (i, index) {
				if (index.Id === treatmentDetail.Id) {
					var data = JSON.stringify(treatmentDetails);
					localStorage.setItem("treatments", data);
					count++;
				}
			});
			debugger;
			if (count === 0) {
				treatmentDetails.push(treatmentDetail);
				var data = JSON.stringify(treatmentDetails);
				localStorage.setItem("treatments", data);
			}
		}
		else {
			treatmentDetails.push(treatmentDetail);
			var data = JSON.stringify(treatmentDetails);
			localStorage.setItem("treatments", data);
		}
	}
	else {
		treatmentDetails.push(treatmentDetail);
		var data = JSON.stringify(treatmentDetails);
		localStorage.setItem("treatments", data);
	}
	$('#costId').val("");
	$('#discountId').val("");
	$('#treatmentName').val("");
	$('#treamentId').val("");
}
$("#treamentId").change(function () {
	var treamentId = $('#treamentId').val();
	debugger;
	$.ajax({
		type: 'Get',
		url: '/Visit/GetTreatmentDetails',
		data:
		{
			treamentId: treamentId
		},
		success: function (result) {
			debugger;
			$('#costId').val(result.price);
			$('#treatmentName').val(result.name);
		},
	});
});
$('#saveTreatment').on("click", function () {
	debugger;
	var carts = localStorage.getItem("treatments");
	$.ajax({
		type: 'POST',
		url: "/Visit/AddVisitTreatment",
		data: {
			visitTreatments: carts,
		},
		success: function (result) {
			debugger;
			if (!result.isError) {
				localStorage.removeItem("treatments");
				var url = "/Visit/Index/";
				successAlertWithRedirect(result.msg, url)
			}
			else {
				errorAlert(result.msg);
			}
		},
	});	
});
// Deletes task on click of delete button
$('#addItem').on('click', '.sale-delete-btn', function () {
	debugger;
	var task = $(this).closest('.task');
	var taskId = task.find('.idSelected').text();
	var taskAmt = task.find('.amtSelected').text();
	var salesAmt = parseFloat(taskAmt);
	var visitTreatId = parseInt(taskId);
	var prevAmount = $("#totalAmountToPay").text();
	var realValue = prevAmount.replace(",", "");
	var initialAmount = parseFloat(realValue);

	var carts = localStorage.getItem("treatments");
	if (carts != "" && carts != "[]" && carts != null) {
		treatmentDetails = JSON.parse(carts);
		if (treatmentDetails.includes(visitTreatId)) {
			$.each(treatmentDetails, function (i, index) {
				if (index.Id === visitTreatId) {
					var data = JSON.stringify(treatmentDetails);
					localStorage.setItem("treatments", data);
				}
			});
		}
	}
	var result = initialAmount - salesAmt;
	$("#totalAmountToPay").text(result);
	task.remove();
	updateNotification('item deleted.');
});