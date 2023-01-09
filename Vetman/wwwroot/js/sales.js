
function AppendField() {
    debugger;
    var productDetailss = [];
    var productDetail = {};
    var shopProduct = $('#salesProductName').val();
    var quantity = parseInt($('#quantity').val());
    var unit = $('#unit').val();
    var unitPrice = parseFloat($('#unitPrice').val());
    var discount = parseFloat($("#discount").val());
    if (discount.toString() == "NaN") {
        discount = 0;
    }
    var amount = $("#amountShop").val();
    var productId = parseInt($("#salesProductId").val());
    
    // If new task is blank show error message
    if (productId.toString() === "NaN" || quantity.toString() === "NaN" || unit === '' || amount === '') {
           
        $('#quantity').addClass('error');
        $('.new-task-wrapper .error-message').removeAttr('hidden');
    }
    else {
        var action = 
            '<span class="action-circle large sale-delete-btn" title = "Delete Task">' +
            '<i class="material-icons">delete</i>' +
            '</span> ';
        var template = '<tr class="task">' +
            
           	                '<td></td>' +
                        	'<td>' + shopProduct + '</td>' +
            	            '<td class="qtySelected">' + quantity + '</td>' +
                            '<td>' + unit + '</td>' +
            	            '<td>' + unitPrice + '</td>' +
                            '<td class="idSelected" hidden>' + productId + '</td>' +
                        	'<td class="discountSelected">' + discount + '</td>' +
                        	'<td class="amtSelected">' + amount + '</td>' +
                            '<td class="text-right">' + action  + '</td>' +
            	    '</tr>';
        // Make a new task template
        var newTemplate = $(template).clone();
        $('#addItem').append(newTemplate);
        var amt = parseFloat(amount);
        var prevAmount = $("#totalAmountToPay").text();

        if (prevAmount != "" && prevAmount != "0") {
            var realValue = prevAmount.replace(",","");
            var initialAmount = parseFloat(realValue);
            var total = initialAmount + amt;
            var result = total.toLocaleString('en-US');
            $("#totalAmountToPay").text(result);
        } else {
            var totalAmt = amt.toLocaleString('en-US');
            $("#totalAmountToPay").text(totalAmt);
        }
        debugger;
        productDetail.Id = productId;
        productDetail.Quantity = quantity;
        productDetail.Discount = discount;
        productDetail.UnitPrice = unitPrice;

        updateNotification('added to list');

        var carts = localStorage.getItem("sales");
        if (carts != "" && carts != "[]" && carts != null) {
            productDetailss = JSON.parse(carts);
            if (carts.includes(productDetail.Id)) {
                var count = 0;
                $.each(productDetailss, function (i, product) {

                    if (product.Id === productDetail.Id) {

                        product.Quantity += productDetail.Quantity;
                        product.Discount += productDetail.Discount;

                        var products = JSON.stringify(productDetailss);
                        localStorage.setItem("sales", products);
                        count++;
                    }
                   
                });
                debugger;
                if (count === 0) {
                    productDetailss.push(productDetail);
                    var product = JSON.stringify(productDetailss);
                    localStorage.setItem("sales", product);
                }
            }
            else {
                productDetailss.push(productDetail);
                var product = JSON.stringify(productDetailss);
                localStorage.setItem("sales", product);
            }
        }
        else {
            productDetailss.push(productDetail);
            var product = JSON.stringify(productDetailss);
            localStorage.setItem("sales", product);
        }
        

        $('#quantity').val("");
        $('#salesProductName').val("");
        $('#unit').val("");
        $('#unitPrice').val("");
        $("#amountShop").val("");
        $("#discount").val("");
        $("#salesProduct").val(0);
        $('#showQuantity').attr("hidden");
        $('#qtyLeft').text("");
        $("#salesProductId").val("");
    }
}


$("#salesProduct").change(function () {
        debugger;
        var productId = $(this).val();
    if (productId == '0') {
        $('#unit').val("");
        $('#unitPrice').val("");
        $('#salesProductName').val("");
        // $('#salesProductId').val(result.data.amountPerProduct);
        $('#salesProductId').val("");
        $('#salesProductQty').val("");
    } else
    {
        $.ajax({
            type: 'POST',
            url: '/Sales/GetProductDetail',
            dataType: 'json',
            data:
            {
                id: productId,
            },
        	success: function (result) {
            		debugger;
            		if (!result.isError) {
                        var prevAmount = $("#totalAmountToPay").text();
                        if (prevAmount === "" || prevAmount === "0") {
                            localStorage.removeItem("sales");
                        }
                        var productDetailss = [];
                        var carts = localStorage.getItem("sales");
                        if (carts != "" && carts != "[]" && carts != null) {
                            productDetailss = JSON.parse(carts);
                            var count = 0;
                            $.each(productDetailss, function (i, product) {
                                debugger;
                                if (product.Id === result.data.id) {
                                    var qty = parseInt(product.Quantity);
                                    var dbQty = parseInt(result.data.quantity);
                                    var leftQuantity = dbQty - qty;
                                    $('#qtyLeft').text(leftQuantity);
                                    $('#salesProductQty').val(leftQuantity);
                                    $('#unit').val(result.data.measurementunit.name);
                                    $('#unitPrice').val(result.data.amountPerProduct);
                                    $('#salesProductName').val(result.data.name);
                                    $('#showQuantity').removeAttr("hidden");
                                    $('#salesProductId').val(result.data.id);
                                    count++;
                                }
                            });
                            debugger;
                            if (count === 0) {
                                $('#unit').val(result.data.measurementunit.name);
                                $('#unitPrice').val(result.data.amountPerProduct);
                                $('#salesProductName').val(result.data.name);
                                $('#showQuantity').removeAttr("hidden");
                                $('#qtyLeft').text(result.data.quantity);
                                $('#salesProductId').val(result.data.id);
                                $('#salesProductQty').val(result.data.quantity);
                                
                            }
                        }
                        else {
                            debugger;
                            $('#unit').val(result.data.measurementunit.name);
                            $('#unitPrice').val(result.data.amountPerProduct);
                            $('#salesProductName').val(result.data.name);
                            $('#showQuantity').removeAttr("hidden");
                            $('#qtyLeft').text(result.data.quantity);
                            $('#salesProductId').val(result.data.id);
                            $('#salesProductQty').val(result.data.quantity);
                            
                        }
                	}
                else {
                    $('#unit').empty();
                        $('#unitPrice').empty();
                        $('#salesProductName').empty();
                        // $('#salesProductId').val(result.data.amountPerProduct);
                        $('#salesProductId').empty();
                        $('#salesProductQty').empty();
                	}
            },
        	error: function (ex) {
            	errorAlert(ex);
            }
        });
        }
    });

function NewProductSale () {
    debugger;
    var addProduct= {};
    addProduct.SupplierId = $("#supplierId").val();
    addProduct.Name = $("#shopProduct").val();
    addProduct.ShopCategoryId = $("#shopCategoryId").val();
    addProduct.IsNewProduct = true;
    addProduct.MeasurementunitId = $("#unit").val();
    addProduct.Quantity = $("#qtyBought").val();
    addProduct.TotalAmountPaid = $("#priceOfQtyBought").val();
    addProduct.AmountPerProduct = $("#pricePerUnitProduct").val();
    addProduct.Tag = $("#productTag").val();
 

    if (parseInt(addProduct.Quantity) > 0 && parseFloat(addProduct.TotalAmountPaid) > 0 && parseFloat(addProduct.AmountPerProduct) > 0
        && addProduct.MeasurementunitId != "" && addProduct.SupplierId != "" && addProduct.ShopCategoryId != "" && addProduct.Name != "") {
        let details = JSON.stringify(addProduct);
        debugger;
        $.ajax({
            type: 'POST',
            url: '/ShopProducts/addProduct',
            dataType: 'json',
            data:
            {
                addShopProduct: details,
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

function shopRestock () {
    debugger;
    var addProduct = {};
    addProduct.SupplierId = $("#restockSupplierId").val();
    addProduct.Id = $("#restockProductId").val();
    //addProduct.ShopCategoryId = $("#shopCategoryId").val();
   // addProduct.MeasurementunitId = $("#unit").val();
    addProduct.Quantity = $("#restockQty").val();
    addProduct.TotalAmountPaid = $("#restockTotalAmountPaid").val();
    addProduct.AmountPerProduct = $("#restockPerUnitPrice").val();
    addProduct.Tag = $("#restockProductTag").val();


    if (parseInt(addProduct.Quantity) > 0 && parseFloat(addProduct.TotalAmountPaid) > 0 && parseFloat(addProduct.AmountPerProduct) > 0
        && addProduct.MeasurementunitId != "" && addProduct.SupplierId != "" && addProduct.ShopCategoryId != "" && addProduct.Name != "") {
        let details = JSON.stringify(addProduct);
        debugger;
        $.ajax({
            type: 'POST',
            url: '/ShopProducts/addProduct',
            dataType: 'json',
            data:
            {
                addShopProduct: details,
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

function updateStockDetail() {
    debugger;
    var supplierId = $("#restockSupplierId").val();
    var productId = $("#restockProductId").val();
    if (supplierId != '0' && productId != '0') {
        $.ajax({
            type: 'GET',
            url: '/ShopProducts/GetProductDetail',
            dataType: 'json',
            data:
            {
                id : productId,
                supplierId : supplierId
            },
            success: function (result) {
                debugger;
                if (!result.isError) {
                    $('#restockPerUnitPrice').val(result.data.amountPerProduct);
                    $('#restockProductTag').val(result.data.unit);
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
}

$("#quantity").keyup(function () {
    debugger;
    var desiredQty = parseInt($(this).val());
    var avaliableQty = parseInt($("#salesProductQty").val());
    if (avaliableQty == 0) {
        infoAlert("No avaliable Quantity");
        $(this).val("");
    }
    else {
        if (avaliableQty > 0 && desiredQty > 0) {
            if (desiredQty > avaliableQty) {
                infoAlert("Quantity required Exceeded the avaliable ");
                $(this).val("");
                $("#amountShop").val("");
            }
            else {
                var productPerPrice = parseFloat($('#unitPrice').val());
                var amount = desiredQty * productPerPrice;
                $("#amountShop").val(amount);
            }
        }
        else {
            $("#amountShop").val("");
        }
    }
});
   

$("#discount").keyup(function () {
    debugger;
    var desiredQty = $("#quantity").val()
    var productPerPrice = parseFloat($('#unitPrice').val());
    if (desiredQty > 0 && productPerPrice > 0) {
        var result = desiredQty * productPerPrice;
        var discount = parseFloat($(this).val());
        if (discount > 0 && result > 0) {
            var result = result - discount;
            $("#amountShop").val(result);
        }
        else {
            var result = desiredQty * productPerPrice;
            $("#amountShop").val(result);
        }
    }
});

// Deletes task on click of delete button
$('#addItem').on('click', '.sale-delete-btn', function () {
    debugger;
    var task = $(this).closest('.task');

    var taskQty = task.find('.qtySelected').text();
    var taskAmt = task.find('.amtSelected').text();
    var taskId = task.find('.idSelected').text();
    var taskdiscount = task.find('.discountSelected').text();

    var salesAmt = parseFloat(taskAmt);
    var salesQty = parseInt(taskQty);
    var salesId = parseInt(taskId);
    var salesDiscount = parseFloat(taskdiscount);

    var prevAmount = $("#totalAmountToPay").text();
    var realValue = prevAmount.replace(",", "");
    var initialAmount = parseFloat(realValue);

    var carts = localStorage.getItem("sales");
    if (carts != "" && carts != "[]" && carts != null) {
        productDetailss = JSON.parse(carts);
        if (carts.includes(salesId)) {
            $.each(productDetailss, function (i, product) {
                if (product.Id === salesId) {
                    if (i > -1) {
                        var leftQty = product.Quantity - salesQty;
                        if (leftQty <= 0) {
                            productDetailss.splice(i, 1); // 2nd parameter means remove one item only
                        }
                        product.Quantity = leftQty;
                        product.Discount = product.Discount - salesDiscount;
                        $("#salesProduct").val(0);
                    }
                    var products = JSON.stringify(productDetailss);
                    localStorage.setItem("sales", products);
                   
                }

            });
        }
    }

    var total = initialAmount - salesAmt;
    var result = total.toLocaleString('en-US');
    $("#totalAmountToPay").text(result);
    task.remove();
    updateNotification('item deleted.');
});

$('#saveAndSend').on("click", function () {
    debugger;
    var carts = localStorage.getItem("sales");
    if (carts != "" && carts != "[]" && carts != null) {
        var customerId = $("#CustomerIdExistingUser").val();
        var totalAmountPaid = $("#totalAmountToPay").text();
        if (customerId != "00000000-0000-0000-0000-000000000000" && totalAmountPaid != "") {
     
            $.ajax({
                type: 'POST',
                url: "/Sales/SaveSales",
                data: {
                    salesDetails: carts,
                    customerId: customerId,
                    totalAmountPaid: totalAmountPaid
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        localStorage.removeItem("sales");
                        var url = "/SalesPayment/Index/" + result.data.salesLogsId + "&" + result.data.amount;
                        successAlertWithRedirect(result.msg, url)
                    }
                    else {
                        errorAlert(result.msg);
                    }
                },
            });
        }
        else {
            infoAlert("customer is required ");
        }
    }
});

function viewDetailModal (id) {
    debugger;
    if (id != "") {
        $.ajax({
            type: 'POST',
            url: '/Sales/CustomerSalesHistory',
            dataType: 'json',
            data:
            {
                id: id,
            },
            success: function (result) {
                debugger;
                if (!result.isError) {
                    $("#customerName").text(result.data.customer);
                    $("#customerPhone").text(result.data.phone);
                    var total = result.data.totalAmountPaid;
                    $("#totalPaidByCustomer").text(total.toLocaleString('en-US'));
                    $("#salesStaff").text(result.data.staff);
                    $("#customerAddress").text(result.data.customerAddress);
                    $("#shoppedItems").empty();
                    $.each(result.data.saleDetailsModel, function (i, salesItem) {
                        debugger;
                        var tableInfo = '<tr>' +
                            '<td hidden></td>' +
                            '<td>' + salesItem.productName + '</td>' +
                            '<td>' + salesItem.discount + '</td>' +
                            '<td>' + salesItem.unitPrice + '</td>' +
                            '<td>' + salesItem.quantity + '</td>' +
                            '<td>' + salesItem.supplierName + '</td>' +
                            '<td>' + salesItem.measurementunit + '</td>' +
                            '</tr>';
                        
                        $("#shoppedItems").append(tableInfo);

                    })
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
}

var notificationTimeout;
//Shows updated notification popup 
var updateNotification = function (notificationText) {
    var notificationPopup = $('.notification-popup ');
    
    notificationPopup.find('.notification-text').text(notificationText);
    notificationPopup.removeClass('hide success');
    
    // If there is already a timeout running for hiding current popup, clear it.
    if (notificationTimeout)
        clearTimeout(notificationTimeout);
    // Init timeout for hiding popup after 3 seconds
    notificationTimeout = setTimeout(function () {
        notificationPopup.addClass('hide');
    }, 1000);
};


function GetItemDetails() {
    debugger
    let details = {};
    var customerId = $('#customerId').val();
    var dateGet = $('#dateFrom').val();
    var dateGetFrom;
    var dateTo = $('#dateTo').val();
    var dateToEnd;

    if (dateGet != "" && dateTo != "") {
        var date1 = dateGet.split("/");
        dateGetFrom = date1[1] + "/" + date1[0] + "/" + date1[2];
        var date2 = dateTo.split("/");
        dateToEnd = date2[1] + "/" + date2[0] + "/" + date2[2];
    }

    $.ajax({
        dataType: 'json',
        type: 'GET',
        url: '/Sales/Itemfilter',
        data:
        {
             customerId : customerId,
            dateGetFrom: dateGetFrom,
            dateToEnd: dateToEnd
        },

        success: function (result) {
            debugger;
           
            if (!result.isError) {
                var body = $('#headColumn').next();
                body.attr("id", "ItemList");
                $('#ItemList').empty();
                $.each(result.data, function (i, itemList) {
                    debugger;
                    var action;
                    action = '<td>' +
                        '<a href="#" class="text-center no-sort" data-toggle="modal" data-target="#add_subscriber" data-backdrop="static" onclick="viewDetailModal('+"'" +itemList.id +"'"+')" title="View Details">' +
                        '<i class="fa fa-eye" aria-hidden="true"></i>' +
                        '</a>' +
                        '</td>'

                    debugger;
                    $("#ItemList").append('<tr>' +
                        '<td>' + '</td>' +
                        '<td>' + itemList.customer + '</td>' +
                        '<td>' + itemList.staff + '</td>' +
                        '<td>' + itemList.dateFromD + '</td>' +
                        '<td>' + itemList.totalAmountPaid + '</td>' +
                        action
                    );
                });
            }
            else {
                errorAlert("No  Item bought on the Selected Date");
            }

        },
        error: function (ex) {
            "Something went wrong, contact support - " + errorAlert(ex);
        }
    })
}


$(".checkbox-all").on('click', function () {
    var check = $(this).is(":checked");
    if (check) {
        $('.checkmail').not(this).prop('checked', true);
    }
    else {
        $('.checkmail').not(this).prop('checked', false);
    }  
    getCheckRecords();
});


var ids = [];
function getCheckRecords() {
    debugger;
    ids.length = 0;
    $(".checkmail:checked").each(function () {
        debugger;
        if ($(this).prop("checked")) {
            const data = '' + $(this).attr("data-id") + '';
            ids.push(data);
        }
    });
}

$(".checkmail").on('change', function () {
    debugger;
    var selectedId = $(this).attr("data-id");
    if ($(this).is(":checked")) {
        if (!$(".checkbox-all").prop("checked")) {
            ids.push(selectedId);
        } 
    } 
    else {
        $.each(ids, function (i, id) {
            debugger;
            if (selectedId == id) {
                ids.splice(i, 1);
                $('.checkbox-all').prop('checked', false);
            }
        });
    }
}); 

function subscriber() {
    debugger;
    getCheckRecords();
    if (ids.length > 0) {
        debugger;
        var moduleIds = JSON.stringify(ids);
        $.ajax({
            type: 'POST',
            url: '/Subscription/Create',
            data: { ids: moduleIds },
            success: function (result) {
                debugger;
                if (!result.isError) {
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
            error: function () {

            }
        });
    } else {
        errorAlert("Please select the module your want to subscribe to.");
    }
    
}

$(function () {
    $("#CustomerIdExistingUser").select2();
    $("#customerId").select2();
    $("#salesProduct").select2();
});


function editedShopProduct(id)
{
    debugger;
    $.ajax({
        type: 'Get',
        dataType: 'Json',
        url: '/ShopProducts/EditedproductInventory',
        data: {
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError)
            {
                $('#shopProductInventoryId').val(result.id);
                $('#editSupplierId').val(result.supplierId);
                $('#editShopProductName').val(result.productName);
                $('#editUnit').val(result.measurementunitId);
                $('#editQtyBought').val(result.quantityBought);
                $('#editPriceOfQtyBought').val(result.totalAmountofQuantityBought);
                $('#editPricePerUnitProduct').val(result.pricePerUnit);
                $('#editshopProductInventoryId').val(result.shopCategoryId);
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function editedShopProductToSave() {
    debugger;
    var data = {};
    data.Id = $("#shopProductInventoryId").val();
    data.SupplierId = $("#editSupplierId").val();
    data.ProductName = $("#editShopProductName").val();
    data.ShopCategoryId = $("#editshopProductInventoryId").val();
    data.MeasurementunitId = $("#editUnit").val();
    data.QuantityBought = $("#editQtyBought").val();
    data.TotalAmountofQuantityBought = $("#editPriceOfQtyBought").val();
    data.PricePerUnit = $("#editPricePerUnitProduct").val();
    let details = JSON.stringify(data);
    debugger;
    $.ajax({
        type: 'POST',
        url: '/ShopProducts/EditedShopProductInventory',
        dataType: 'json',
        data:
        {
            shopdetails: details,
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/ShopProducts/Views'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg);
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    });
   
}

function DeleteProductInventory() {
    var id = $('#deleteProductInventoryId').val();
    debugger;
    $.ajax({
        type: 'Post',
        dataType: 'Json',
        url: '/ShopProducts/DeleteShopProductInventoty',
        data: {
            id: id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var url = '/ShopProducts/Views'
                successAlertWithRedirect(result.msg, url)
            }
            else {
                errorAlert(result.msg)
            }
        },
        error: function (ex) {
            errorAlert("Error occured try again");
        }
    })
}

function deleteShopProduc(id) {
    debugger;
    $('#deleteProductInventoryId').val(id);
}