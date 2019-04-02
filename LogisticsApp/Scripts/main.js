
$(document).ready(function () {
    window.onscroll = function () { scrollFunction() };
    function scrollFunction() {
        if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
            document.getElementById("goUp").style.display = "block";
        } else {
            document.getElementById("goUp").style.display = "none";
        }
    }

    $("#Calculate").on("click", function () {
        let model = {
            CountryId: $("#calculator #countryId").val(),
            BundleCount: $("#calculator #BundleCount").val(),
            Length: $("#calculator #Length").val(),
            Width: $("#calculator #Width").val(),
            Height: $("#calculator #Height").val(),
            Weight: $("#calculator #Weight").val()
        }
        $.ajax({
            url: "/Taariff/Calculator/",
            method: "Post",
            data: { model:model }
        }).done(function (res) {
            $("#calculator #calculatorResult").text(res.Value);
        })
    })

    $(".card-header a").on("click", function () {
        $(".card-header a").removeClass("active");
        $(this).addClass("active");
        var div = $(this).attr("data-id");
        $(".card-body>div").hide();
        $("#" + div).show();
    })

    var acc = document.getElementsByClassName("accordion");
    var i;
    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.display === "block") {
                panel.style.display = "none";
            } else {
                panel.style.display = "block";
            }
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
        });
    }

    $("#markets .list-group-item").on("click", function () {
        $("#markets .list-group-item").removeClass("activemarketcategory");
        $(this).addClass("activemarketcategory");
    })

    $(".messageDetailsBtn").on("click", function () {

        let id = $(this).attr("id");

        $.ajax({
            url: "/Message/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            $("#MessageModalDate").html(res.CreateDate);
            $("#MessageModalTitle").attr("value", res.MessageType);
            $("#MessageModalText").text(res.Text);
            $("#MessageModal").show();
        })

    })

    $(".inqueryDetailsBtn").on("click", function () {

        let id = $(this).attr("id");

        $.ajax({
            url: "/Inquery/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            $("#InqueryDetailsModal .modal-body").html(res);
            $("#InqueryDetailsModal").show();
        })

    })
    $("#inqueryInformations button[data-target='#InqueryCreateModal'").on("click", function () {
        $.ajax({
            url: "/Inquery/Create/",
            method: "GET",
        }).done(function (res) {
            $("#InqueryCreateModal .modal-body").html(res);
            $("#InqueryCreateModal").modal("show");
        })
    })

    $("#alert button").on("click", function () {
        $("#alert").hide();
    })

    $(".AboutDetailsBtn").on("click", function (e) {
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/Abouts/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {

            document.getElementById("AboutDetailsModalText").value = res.Text;
            document.getElementById("AboutDetailsModalId").value = res.Id;
            $("#AboutDetailsModal").show();
        })


    })
    $("#AdminAbout input[type='checkbox']").on("click", function (e) {
        let curr = $(this);
        if (this.checked) {
            $("input[type='checkbox']").prop("checked", false).removeAttr("checked");
            curr.attr("checked", "");
            this.checked = true;
        }
    })
    $(".AboutDeleteBtn").on("click", function () {
        let id = $(this).attr("data-id");
        $("#AboutDeleteModalId").val(id);
    })
    $("#SaveActiveAboutBtn").on("click", function () {
        let id = $("#AdminAbout input[type='checkbox']:checked").attr("data-id");
        $.ajax({
            url: "/Abouts/ChangeActivityStatus/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            location.reload();
        })
    })

    $("#SaveActiveCaruselsBtn").on("click", function () {
        let selecteds = [];
        $.each($("#AdminCarusel input[type='checkbox']:checked"), function () {
            selecteds.push($(this).attr("data-id"));
        });

        $.ajax({
            url: "/Carusels/ChangeActivityStatus/",
            method: "POST",
            dataType: 'json',
            data: { id: selecteds }
        }).done(function (res) {
            window.location.href = res;
        })
    })
    $(".CaruselDetailsBtn").on("click", function (e) {
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/Carusels/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            document.getElementById("CaruselDetailsModalId").value = res.Id;
            document.getElementById("CaruselDetailsModalTitle").value = res.Title;
            document.getElementById("CaruselDetailsModalText").value = res.Text;
            $("#CaruselDetailsModalImage").attr("src", res.Image);
            $("#CaruselDetailsModalLong").show();
        })


    })
    $(".CaruselDeleteBtn").on("click", function () {
        let id = $(this).attr("data-id");
        $("#CaruselDeleteModalId").val(id);
    })

    $("#SaveActiveForumsBtn").on("click", function () {
        let selecteds = [];
        $.each($("#AdminForum input[type='checkbox']:checked"), function () {
            selecteds.push($(this).attr("data-id"));
        });

        $.ajax({
            url: "/FAQ/ChangeActivityStatus/",
            method: "POST",
            dataType: 'json',
            data: { id: selecteds }
        }).done(function (res) {
            window.location.href = res;
        })
    })
    $(".ForumDetailsBtn").on("click", function (e) {
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/FAQ/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            document.getElementById("ForumDetailsModalId").value = res.Id;
            document.getElementById("ForumDetailsModalQuestion").value = res.Question;
            document.getElementById("ForumDetailsModalAnswer").value = res.Answer;
            $("#ForumDetailsModalLong").show();
        })


    })
    $(".ForumDeleteBtn").on("click", function () {
        let id = $(this).attr("data-id");
        $("#ForumDeleteModalId").val(id);
    })

    $(".countryInfoBtn").on("click", function () {
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/Countrie/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            $("#AdminCountryInformations").html(res);
        })

    })

    $(".addNewLinkToOrder").on("click", function () {
        var where = $(this).parent().parent();
        var btn = where.children().last();
        var row = where.children()[1].outerHTML;
        if (where.find($("div[id^='orderlinkrow']")).length < 10) {
            btn.remove();
            var id = where.children().last().attr("id");
            var counter = Number(id.substring(id.length - 1));
            counter++;
            where.append(row).children().last().attr("id", "orderlinkrow" + counter)
            where.children().last().find(".deleteOrderBtn").attr("data-counter", "#orderlinkrow" + counter).removeAttr("disabled")
            where.append(btn)
        }
        
    })
    $(document).on("click", ".deleteOrderBtn", function () {
        var item = $(this).attr("data-counter");
        $(item).remove();
    })
    $("#orderInformations .card-header a ").on("click", function () {
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/Order/ordersPerSteps/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            $("#ordersPerSteps").html(res);
        })
    })
    $(document).on("click", "button.OrderDetailsBtn", function (e) {

        let id = $(this).attr("data-id");
        $.ajax({
            url: "/Order/Details/",
            method: "GET",
            data: { id: id }
        }).done(function (res) {
            $("#OrderDetailsModalLong .modal-body").html(res);
            $("#OrderDetailsModalLong").modal("show");
        })
    })
    $(".OrderDeleteBtn").on("click", function () {
        let id = $(this).attr("data-id");
        $("#OrderDeleteModalId").val(id);
    })
    $("#selectAllUnpaids").on("click", function () {
        if ($(".UsersUnpaidOrders:checked").length == $(".UsersUnpaidOrders").length) {
            $(".UsersUnpaidOrders").prop("checked", false).removeAttr("checked")
            $("#payForOrder").attr("disabled", "")
        }
        else {
            $(".UsersUnpaidOrders").prop("checked", true).attr("checked", "")
            $("#payForOrder").removeAttr("disabled")
        }
    })
    $(document).on("change", ".UsersUnpaidOrders", function (e) {
        
        if ($(".UsersUnpaidOrders:checked").length > 0) {
            $("#payForOrder").removeAttr("disabled")
        }
        else { $("#payForOrder").attr("disabled","")}
    })
    $("#payForOrder").on("click", function () {
        let selecteds = [];
        let sumprice = 0;
        $.each($("#orderInformations .UsersUnpaidOrders:checked"), function () {
            selecteds.push($(this).attr("data-id"));
            sumprice += Number($(this).attr("data-sumPrice"))
            
        });
        sumprice = Number(sumprice.toFixed(2));
        let final=(sumprice+Number((sumprice*5/100).toFixed(2))).toFixed(2)
        $("#OrderPaymentModal #OrderPaymentModalText").html("<p>Secdiyiniz sifarislerin deyeri <span style='font-size:20px; color:red'>" + sumprice + "</span> manatdir. " +
            "Nezerinize catdiriq ki, saytimizda mehsulun sifarisli odenisi ucun 5% mebleginde xidmet haqqi odenilmelidir(cemi " +
            "<span style='font-size:20px; color:red'>" + final + "</span> manat).</p>"+
            "<p>Bundan elave, sifarisin tecili olmasini secmisinizse hemin meblegin 2%-i hecminde elave odenis etmeli olacaqsiniz.</p> " +
            "<p>Balansinizda kifayet qeder pulun oldugundan emin olduqdan sonra ODEMEK duymesini sixa bilersiniz. "+
            "Eks halda evvelce <a href = '/Payment/AddToBalance' style='color:blue;'> bu linkden</a> istifade ederek balansinizi " +
            "artirmaginiz xahis olunur </p>")
        for (let i = 0; i < selecteds.length; i++)
        {
            $("#OrderPaymentModalId").append("<input type='hidden' name='selecteds' value='" + selecteds[i] + "' />")
        }
    })

    $("#actions button[data-target='#declarationModal']").on("click", function () {
        $.ajax({
            url: "/Bundle/Create/",
            method: "GET"
        }).done(function (res) {
            $("#declarationModal .modal-content").html(res);
            })
    })

    $("#BundleSearch, #AccountSearch").on("keyup", function () {
        let filter = $(this).val();
        let url = "";
        let target = "";
        if ($(this).attr("id") == "BundleSearch") {
            url = "/Bundle/Search";
            target = "#BundleSearchResult";
        }
        else {
            url = "/Account/Search";
            target = "#AccountSearchResult";
        }
        $.ajax({
            url: url,
            method: "POST",
            data: {filter:filter}
        }).done(function (res) {
            console.log(res);
            $(target).html(res);
            $(target).show();
            })
    })
    $(document).on("click", function () {
        $(".adminHeaderSearchResult, .AdminOrderActionBtns").hide();
    })
    $(".adminHeaderSearch").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(".adminHeaderSearchResult").hide();
       $(this).next("ul").show();
    })

    $(".adminOrderShowActionsBtn").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(".AdminOrderActionBtns").hide();
        $(this).siblings("div").show();
    })
    $(document).on("click", ".AdminOrderProcessBtn", function () {
        let id = $(this).attr("data-id");
        $.ajax({
            url: "/Order/AdminOrderInfo/",
            method: "GET",
            data: { id: id }
        }).done(function (res) {
            $("#AdminOrderProcessLong .modal-body").html(res);
            $("#AdminOrderProcessLong").show();
        })

    })
    $(".AdminOrderDeleteBtn").on("click", function () {
        let id = $(this).attr("data-id");
        $("#AdminOrderDeleteModalId").val(id);
    })
    $("#ShowAllOrders").on("click", function () {
        $("#ListAllOrders").toggle();
    })
    $("#AdminOrdersAll span[data-id='AdminSearchOrder']").on("click", function () {
        let filter = $("#AdminOrdersAll input[placeholder='search for order']").val();
        $.ajax({
            url: "/Order/Search/",
            method: "POST",
            data: { filter: filter }
        }).done(function (res) {
            $("#AdminOrdersAll #ListAllOrders").html(res);
            $("#AdminOrdersAll #ListAllOrders").show();
        })

    })
    $(".AdminOrderPaymentBtn").on("click", function () {
        let id = $(this).attr("data-id");
        console.log(id)
        $.ajax({
            url: "/Payment/AdminPaymentForOrder/",
            method: "GET",
            data: { id: id }
        }).done(function (res) {
            $("#AdminOrderPaymentModal .modal-body").html(res);
            $("#AdminOrderPaymentModal").show();
        })

    })



})