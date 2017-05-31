
$(function () {

    $('#modal_offer').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        ownerid = btn.data("channel-ownerid");
        $("#modal_offer_body").load("/SendMessage/Sendmessage/" + ownerid);
    })

});
$(function () {

    $('#modal_subscribe').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        ownerid = btn.data("channel-ownerid");
        $("#modal_subscribe_body").load("/CreditCard/CreditcardSubscribe/" + ownerid);
    })

});
$(function () {

    $('#modal_domain').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        ownerid = btn.data("channel-ownerid");
        $("#modal_domain_body").load("/CreditCard/Creditcard/" + ownerid);
    })

});

$(function () {

    var channelids = [];

    $("div[data-channel-id]").each(function (i, e) {
        channelids.push($(e).data("channel-id"));
    });
    $.ajax({
        method: "POST",
        url: "/Other/Followed",
        data: { ids: channelids }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var followedchannel = $("div[data-channel-id=" + id + "]");
                var btn = followedchannel.find("button[data-followed]");
                var texta = "Takip Ediliyor";


                btn.data("followed", true);
                btn.text(texta);

            }

        }

    }).fail(function () {

    });

    $("button[data-followed]").click(function () {

        var btn = $(this);
        var followed = btn.data("followed");
        var channelid = btn.data("channel-id");
        var text = btn.children().first().html();

        $.ajax({
            method: "POST",
            url: "/Other/Follow",
            data: { "channelid": channelid, "followed": !followed }
        }).done(function (data) {

            console.log(data);

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                followed = !followed;
                btn.data("followed", followed);

                if (followed) {
                    btn.text("Takip Ediliyor");
                } else {
                    btn.text("Takip Et");
                }

            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        })


    });


});


$(function () {

    var channelids = [];

    $("div[data-channel-id]").each(function (i, e) {
        channelids.push($(e).data("channel-id"));
    });
    $.ajax({
        method: "POST",
        url: "/Other/Complained",
        data: { ids: channelids }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var complainedchannel = $("div[data-channel-id=" + id + "]");
                var btn = complainedchannel.find("button[data-complained]");
                var textb = "Şikayet Edildi";


                btn.data("complained", true);
                btn.text(textb);

            }

        }

    }).fail(function () {

    });

    $("button[data-complained]").click(function () {

        var btn = $(this);
        var complained = btn.data("complained");
        var channelid = btn.data("channel-id");
        var text = btn.children().first().html();

        $.ajax({
            method: "POST",
            url: "/Other/Complain",
            data: { "channelid": channelid, "complained": !complained }
        }).done(function (data) {

            console.log(data);

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                complained = !complained;
                btn.data("complained", complained);

                if (complained) {
                    btn.text("Şikayet Edildi");
                } else {
                    btn.text("Şikayet Et");
                }

            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        })


    });


});