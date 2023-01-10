$("#petSpec").change(function () {
    debugger;
    var id = $(this).val();
    if (id != "") {
        $("#breedId").empty();
        $.ajax({
            type: 'GET',
            url: '/Patient/GetBreed',
            data: { id: id },
            success: function (result) {
                debugger;

                if (!result.isError) {
                    $.each(result.data, function (i, mean) {
                        debugger;
                        $("#breedId").append('<option value=' + mean.id + '>' + mean.name + '</option>');
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

$("#petSpecExisting").change(function () {
    debugger;
    var id = $(this).val();
    if (id != "") {
        $("#breedIdExisting").empty();
        $.ajax({
            type: 'GET',
            url: '/Patient/GetBreed',
            data: { id: id },
            success: function (result) {
                debugger;

                if (!result.isError) {
                    $.each(result.data, function (i, mean) {
                        debugger;
                        $("#breedIdExisting").append('<option value=' + mean.id + '>' + mean.name + '</option>');
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

function NewPatient_Customer() {
    debugger;
    var patientData = {};
    patientData.FirstName = $("#FnameP").val();
    patientData.LastName = $("#LnameP").val();
    patientData.PhoneNumber = $("#phoneNumberP").val();
    patientData.IsExistingClient = false;
    patientData.Email = $("#emailP").val();
    patientData.Address = $("#addressP").val();
    patientData.Profession = $("#profession").val();
    patientData.Colour = $("#colour").val();
    patientData.SpecialMarking = $("#petMarking").val();
    patientData.BreedId = $("#breedId").val();
    patientData.GenderId = $("#petGenderId").val();
    patientData.CustomerGenderId = $("#cusGenderId").val();
    patientData.BirthDay = $("#petBirth").val();
    patientData.SpecieId = $("#petSpec").val();
    patientData.Name = $("#petName").val();
    var picture = document.getElementById("patientImg").files[0];
    if (parseInt(patientData.BreedId) > 0 && patientData.SpecialMarking != "" && patientData.FirstName != ""
        && patientData.BirthDay != "" && patientData.PhoneNumber != "" && patientData.Address != "") {
        let details = JSON.stringify(patientData);
        var formData = new FormData();
        formData.append("patient", details);
        formData.append("picture", picture);
        debugger;
        $.ajax({
            type: 'POST',
            url: '/Patient/CreatePatient',
            dataType: 'JSON',
            data: formData,
            processData: false,
            contentType: false,
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

function existingPatient_Customer() {
    debugger;
    var patientData = {};
    patientData.CustomerId = $("#existCustomerid").val();
    patientData.IsExistingClient = true;
    patientData.Colour = $("#colourExisting").val();
    patientData.SpecialMarking = $("#markingExisting").val();
    patientData.BreedId = $("#breedIdExisting").val();
    patientData.GenderId = $("#petGenderExisting").val();
    patientData.BirthDay = $("#petBirthExisting").val();
    patientData.SpecieId = $("#petSpecExisting").val();
    patientData.Name = $("#petNameExisting").val();
    var picture = document.getElementById("patientImgExisting").files[0];
    if (parseInt(patientData.GenderId) > 0 && patientData.CustomerId != "" && patientData.BreedId != "" &&
        patientData.BirthDay != "" && patientData.SpecialMarking != "") {
        let details = JSON.stringify(patientData);
       
        var formData = new FormData();
        formData.append("patient", details);
        formData.append("picture", picture);
       
        debugger;
        $.ajax({
            type: 'POST',
            url: '/Patient/CreatePatient',
            dataType: 'json',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                debugger;
                if (!result.isError) {
                    var url = '/Patient/CreatePatient';
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

var picture;

$("#editImg").change(function () {
    debugger;
    $("#editShowPix").html("");
   
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
    if (regex.test($(this).val().toLowerCase())) {  
        if (typeof (FileReader) != "undefined") {
            
            var dats = $("#editShowPix").attr("src");
            var reader = new FileReader();
            reader.onload = function (e) {
                picture = e.target.result;
                $("#editShowPix").attr("src", picture);
            }
            reader.readAsDataURL($(this)[0].files[0]);
        } else {
            alert("This browser does not support FileReader.");
        }
    } else {
        alert("Please upload a valid image file.");
    }
});

function getCustomerPatientToEdit(id) {
    debugger;
    $.ajax({
        type: 'GET',
        url: '/Patient/EditPatient',
        dataType: 'json',
        data: { id: id },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var dob;
                if (result.data.patient.dob != null) {
                    var birthDate = result.data.patient.dob.split("T");
                    dob = birthDate[0];
                }
                $("#editPatientId").val(result.data.patient.id);
                $("#delete_Id").val(result.data.patient.id);
                $("#nameEdit").val(result.data.patient.name);
                $("#dobEdit").val(dob);
                $("#editShowPix").attr("src", result.data.patient.picture);
                $("#editColour").val(result.data.patient.colour);
                $("#genderEdit").val(result.data.patient.genderId);
                $("#edit_petSpec").val(result.data.patient.speciesId);
               
                $("#editUnique").val(result.data.patient.specialMarking);
                $("#editCaseNo").val(result.data.patient.caseNumber);
                $("#breed_Edit").empty();
                $.each(result.data.breed, function (i, bred) {  
                    $("#breed_Edit").append('<option value=' + bred.id + '>' + bred.name + '</option>');
                });
                $("#breed_Edit").val(result.data.patient.breedId)
            }
            else
                errorAlert(result.msg);
        }
    });
}

$(function () {
    $(".filter").select2();
});


function EditPatients() {
    debugger
    var data = {};
    data.Id = $('#editPatientId').val();
    data.Name = $('#nameEdit').val();
    data.SpecieId = $('#edit_petSpec').val();
    data.GenderId = $('#genderEdit').val();
    data.BreedId = $('#breed_Edit').val();
    data.BirthDay = $('#dobEdit').val();
    data.Colour = $('#editColour').val();
    data.SpecialMarking = $('#editUnique').val();
    data.Picture = picture;
    debugger;
    if (data.Id != "" && data.Name != "" && data.SpecieId != "" && data.GenderId != "" && data.Breed != "" && data.DOB != "" && data.Colour != "" && data.Picture != "") {
        debugger;

        let PatientViewModel = JSON.stringify(data);
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: '/Patient/EditPatients',
            data:
            {
                patientDetails: PatientViewModel,
            },
            success: function (result) {
                if (!result.isError) {
                    newSuccessAlert(result.msg, result.url)
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



function DeletePatient() {
    debugger
    var data = {};
    data.Id = $('#delete_Id').val();
    if (data.Id != null) {
        let PatientViewModel = JSON.stringify(data);
        if (PatientViewModel != "") {
            $.ajax({
                type: 'Post',
                dataType: 'json',
                url: '/Patient/DeletePatient',
                data:
                {
                    patientDetails: PatientViewModel,
                },
                success: function (result) {
                    debugger;
                    if (!result.isError) {
                        var url = '/Patient/Index';
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



$("#edit_petSpec").change(function () {
    debugger;
    var id = $(this).val();
    if (id != "") {
        $("#breed_Edit").empty();
        $.ajax({
            type: 'GET',
            url: '/Patient/GetBreed',
            data: { id: id },
            success: function (result) {
                debugger;

                if (!result.isError) {
                    $.each(result.data, function (i, mean) {
                        debugger;
                        $("#breed_Edit").append('<option value=' + mean.id + '>' + mean.name + '</option>');
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





//function GetPatientByID(Id) {
//    let data = JSON.stringify(Id);
//    $.ajax({
//        type: 'GET',
//        url: '/Patient/GetPatientByID',
//        data: { patientID: data },
//        dataType: 'json',
//        success: function (data) {
//            debugger;
//            if (!data.isError) {
//                $("#delete_Id").val(data.id);
//                $("#edit_Id").val(data.id);
//                $("#edit_Name").val(data.name);
//                $("#edit_Specie").val(data.specie);
//                $("#edit_Breed").val(data.breed);
//                $("#edit_Dob").val(data.dob);
//            }
//        }
//    });
//}

