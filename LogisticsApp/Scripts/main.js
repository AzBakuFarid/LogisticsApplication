$(document).ready(function () {

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
            /* Toggle between adding and removing the "active" class,
            to highlight the button that controls the panel */
            this.classList.toggle("active");

            /* Toggle between hiding and showing the active panel */
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

        var id = $(this).attr("id");

        $.ajax({
            url: "/Message/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            console.log(res)
            $("#MessageModalDate").html(res.CreateDate);
            $("#MessageModalTitle").attr("value", res.MessageType);
            $("#MessageModalText").text(res.Text);
            $("#MessageModal").show();
        })

    })

    $(".inqueryDetailsBtn").on("click", function () {

        var id = $(this).attr("id");

        $.ajax({
            url: "/Inquerie/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
            console.log(res)
            $("#InqueryDetailsModalDate").html(res.CreateDate);
            $("#InqueryDetailsModalTitle").attr("value", res.MessageType);
            $("#InqueryDetailsModalText").text(res.Text);
            $("#InqueryDetailsModal").show();
        })

    })

    $("#alert button").on("click", function () {
        $("#alert").hide();
    })

    $(".AboutDetailsBtn").on("click", function (e) {
        var id = $(this).attr("data-id");
        $.ajax({
            url: "/Abouts/Details/",
            method: "POST",
            data: { id: id }
        }).done(function (res) {
       
            document.getElementById("AboutDetailsModalText").value=res.Text;
            document.getElementById("AboutDetailsModalId").value = res.Id;
            $("#AboutDetailsModal").show();
            })
       
       
    })

    $("#AdminAbout input[type='checkbox']").on("click", function (e) {
        var curr = $(this);
        if (this.checked) {
            $("input[type='checkbox']").prop("checked", false).removeAttr("checked");
            curr.attr("checked", "");
            this.checked = true;
        }
    })

    $(".AboutDeleteBtn").on("click", function () {
        var id = $(this).attr("data-id");
        console.log(id);
        $("#AboutDeleteModalId").val(id);
    })

    $("#SaveActiveAboutBtn").on("click", function () {
        var id = $("#AdminAbout input[type='checkbox'][checked='checked']").attr("data-id");
        console.log(id)
        $.ajax({
            url: "/Abouts/ChangeActivityStatus/",
            method: "POST",
            data: { id: id }
        })
    })








})