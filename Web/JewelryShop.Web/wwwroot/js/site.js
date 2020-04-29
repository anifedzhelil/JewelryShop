// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function sort(category, page) {
    var sortValue = $("#selectSort").val();
    var searchValue = $("#inputSearch").val();
    window.location.href = '/Home/Index' + '?category=' + category + '&search=' + searchValue + '&sort=' + sortValue + '&page=' + page;
}

$("#addProduct").click(function (e) {
    // Stop the normal navigation
    e.preventDefault();

    //Build the new URL
    var url = $(this).attr("href");
    // Stop the normal navigation
    var quantity = $("#quantity").val();
    url = url.replace("quantityval", quantity);
    //Navigate to the new URL
    window.location.href = url;

});
function changeImage(image) {
    mainImage = document.getElementById("mainImage").src;
    document.getElementById("mainImage").src = image.src;
    image.src = mainImage;
}

function showRateError()
{
    $("#alertLogin").css('visibility', 'visible');
}

function rate(id) {

    var radios = document.getElementsByName('rating');
    var length = radios.length;
    var selected_value = -1;
    for (var i = 0; i < length; ++i) {
        if (radios[i].checked) {
            selected_value = radios[i].value;
            break;
        }
    }

    var review = document.getElementById("review").value;
    var token = $("#ratingsForm input[name=__RequestVerificationToken]").val();
    var json = { jewelId: id, rating: parseInt(selected_value), review: review };
    $.ajax({
        url: "/api/ratings",
        type: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {

            $("#review").val("");
            $(".media").remove();
            $(".average-rating-checked").remove();

            var averageRatingHtml = '<div class="average-rating-checked">';
            for (var i = 0; i < 5; i++) {
                if (i < parseInt(data.avarageRating)) {
                    averageRatingHtml += ' <span class="fa fa-star checked"></span>';
                }
                else {
                    averageRatingHtml += '<span class="fa fa-star"></span>';
                }
            }

            averageRatingHtml += '</div>';
            $(".average-rating").append(averageRatingHtml);

            var htmlMedia = "";
            for (var item in data.jewelryRatings) {
                var date = new Date(data.jewelryRatings[item].createdOn);
                var mydate = date.toLocaleDateString();

                htmlMedia = ' <li class="media">';

                htmlMedia += '<div class="media-body">';
                htmlMedia += '   <span class="text-muted pull-right">';
                htmlMedia += '       <small class="text-muted">' + mydate + '</small>';
                htmlMedia += '    </span>';
                htmlMedia += '    <span class="text-muted pull-left">';
                htmlMedia += '       <strong>' + data.jewelryRatings[item].userUserName + '</strong>';
                htmlMedia += '    </span><br/>';
                htmlMedia += '    <div class="rating-checked">';

                for (var i = 0; i < 5; i++) {
                    if (i < parseInt(data.jewelryRatings[item].type)) {
                        htmlMedia += ' <span class="fa fa-star checked"></span>';
                    }
                    else {
                        htmlMedia += '<span class="fa fa-star"></span>';
                    }
                }

                htmlMedia += ' </div>';
                htmlMedia += '        <p>';
                htmlMedia += data.jewelryRatings[item].review;
                htmlMedia += '       </p>';

                htmlMedia += '     </div>';
                htmlMedia += '     </li >';
                htmlMedia += '     </li >';
                $(".media-list").append(htmlMedia);
            }


        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (errorThrown == "Unauthorized") {
                $("#alertLogin").css('visibility', 'visible');
            }
        }
    });
}
$(function () {
    var $tabButtonItem = $('#tab-button li'),
        $tabSelect = $('#tab-select'),
        $tabContents = $('.tab-contents'),
        activeClass = 'is-active';

    $tabButtonItem.first().addClass(activeClass);
    $tabContents.not(':first').hide();

    $tabButtonItem.find('a').on('click', function (e) {
        var target = $(this).attr('href');

        $tabButtonItem.removeClass(activeClass);
        $(this).parent().addClass(activeClass);
        $tabSelect.val(target);
        $tabContents.hide();
        $(target).show();
        e.preventDefault();
    });

    $tabSelect.on('change', function () {
        var target = $(this).val(),
            targetSelectNum = $(this).prop('selectedIndex');

        $tabButtonItem.removeClass(activeClass);
        $tabButtonItem.eq(targetSelectNum).addClass(activeClass);
        $tabContents.hide();
        $(target).show();
    });
});

$(document).ready(function () {
    $('.quantity-right-plus').click(function (e) {

        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        var quantity = parseInt($('#quantity').val());

        // If is not undefined
        var maxQuantity = parseInt($('#quantity').attr('max'));
        if (quantity < maxQuantity) {
            $('#quantity').val(quantity + 1);
        }
    });

    $('.quantity-left-minus').click(function (e) {
        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        var quantity = parseInt($('#quantity').val());

        if (quantity > 1) {
            $('#quantity').val(quantity - 1);
        }
    });
});


$('#myForm input').on('change', function () {
    var value = $('input[name=delivery]:checked', '#myForm').val();
    if (value == "Econt") {
        $("#divSpeedy").hide();
        $("#speedyMessage").text("");
    }
    else {
        $("#divSpeedy").show();
    }
});

$("#completeOrder").click(function () {
    var addresValue = $('input[name="address"]:checked').val();
    var deliveryValue = $('input[name=delivery]:checked', '#myForm').val();

    var select = $('.select2').val();
    var firstName = $("#firstName").val();
    var lastName = $("#lastName").val();
    var phone = $("#phone").val();

    var isComplete = changeAddress(addresValue);
    var pattern = new RegExp("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-+\s\./0-9]*$");

    if (addresValue == "Speedy" && deliveryValue == "Speedy") {
        if (select == "-1") {
            $("#speedyMessage").text("Моля изберете офис на Спиди.");
            isComplete = false;
        }
        else if (phone == "") {
            $("#speedyMessage").text("Моля въведете телефон.");
            isComplete = false;
        }
        else if (!pattern.test(phone)) {
            $("#speedyMessage").text("Моля въведете валиден телефон.");
            isComplete = false;
        }
        else if (firstName == "") {
            $("#speedyMessage").text("Моля въведете име.");
            isComplete = false;
        }
        else if (lastName == "") {
            $("#speedyMessage").text("Моля въведете фамилия.");
            isComplete = false;
        }
        else {
            $("#speedyMessage").text("");
            isComplete = true;
        }

    }

    if (isComplete) {
        var orderId = $("#orderId").val();
        var fullAddress;
        if (addresValue == "Speedy") {
            fullAddress = $(".select2 option:selected").text();
        }
        else {
            fullAddress = null;
        }
        var deliveryValue = $('input[name=delivery]:checked', '#myForm').val();
        var shippingAddressId = addresValue != "Speedy" ? parseInt(addresValue) : null;
        var shippingPrice = addresValue != "Speedy" ? 6 : 4;

        var json = {
            orderId: parseInt(orderId),
            deliveryMethod: deliveryValue,
            shippingAddressId: shippingAddressId,
            speedyOfficeAddress: fullAddress,
            shippingPrice: shippingPrice,
            firstName: firstName,
            lastName: lastName,
            phone: phone
        };
        var token = $("#formSpeedy input[name=__RequestVerificationToken]").val();

        $.ajax({
            url: "/api/orderComplete",
            type: "POST",
            data: JSON.stringify(json),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'X-CSRF-TOKEN': token },
            success: function (data) {
                window.location.href = '/ShippingAddress/CompleteSuccess';
            }
        });
    }
});

function changeAddress(addresValue) {
    var selectMessage = $("#selectMessage");
    var addresValue = $('input[name="address"]:checked').val();
    var deliveryValue = $('input[name=delivery]:checked', '#myForm').val();
    var deliveryPrice = $("#deliveryPrice");
    var totalPrice = $("#totalPrice");
    var subPrice = $("#subPrice");
    var price = parseInt($("#subPrice").text());
    var isComplete = false;

    if (addresValue == undefined || (addresValue == "Speedy" && deliveryValue == "Econt")) {
        selectMessage.text("Моля изберете адрес за доставка.");
    }
    else if (deliveryValue == undefined) {
        selectMessage.text("Моля изберете метод за доставка.");
    }
    else {
        selectMessage.text("");

        if (addresValue > 0) {
            deliveryPrice.text("(доставка 6 лв)");
            price += 6;
            totalPrice.text(price.toString() + " лв");
        }
        else {
            deliveryPrice.text("(доставка 4 лв)");
            price += 4;
            totalPrice.text(price.toString() + " лв");
        }

        isComplete = true;
    }

    return isComplete;
}


$(document).ready(function () {
    if ($("#mySelect2").length) {
        $('.select2').select2();

        // Fetch the preselected item, and add to the control
        var token = $("#formSpeedy input[name=__RequestVerificationToken]").val();
        var officeSelect = $('#mySelect2');

        $.ajax({
            url: "/api/speedy",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: { 'X-CSRF-TOKEN': token },
        }).then(function (data) {
            // create the option and append to Select2
            for (var num = 0; num < data.addresses.length; num++) {
                var option = new Option(data.addresses[num].fullAddress, data.addresses[num].id, false, false);
                officeSelect.append($("<option />").val(data.addresses[num].id).text(data.addresses[num].fullAddress));
            }

        });


    }
});


$(document).ready(function () {
    $("#newAddress").click(function () {
        $("#partialView").show();
    })
});

var imageUrl = "";
var jewelid = "";
var divId = "";
$("[data-modal-action=openconfimdialog]").click(function () {
    imageUrl = $(this).attr("data-url");
    jewelid = $(this).attr("data-jewelId");
    divId = $(this).attr("data-divId");
    $("#myModal").modal("show");
});

/*  <button type="button" class="btn btn-primary" data-modal-action="yes"> Yes</button> */
// when user click yes, already you stored the value in id, you can pass the vales in ajax and delete action

$("[data-modal-action=yes]").click(function () {
    //call the delete ajax method
    deleteAward(imageUrl, jewelid, divId);  //calling delete method
    $("#myModal").modal("hide");
});


function deleteAward(imageUrl, jewelid, divId) {
    var divElement = $("#" + divId);

    var json = {
        imageUrl: imageUrl,
        jewelId: parseInt(jewelid)
    };
    var token = $("#editForm input[name=__RequestVerificationToken]").val();

    $.ajax({
        url: "/api/images",
        type: "POST",
        data: JSON.stringify(json),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: { 'X-CSRF-TOKEN': token },
        success: function (data) {
            $("#" + divId).remove()
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (errorThrown == "Unauthorized") {
            }
        }
    });
}