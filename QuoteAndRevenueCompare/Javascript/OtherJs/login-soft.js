
$(function () {
    $('#btnsubmit').on("click", function () {
        //进行表单验证
        var name = $('#username').val();
        var pwd = $('#password').val();
        if (name == '' || pwd == '') {
            $('.flash-error').show();
        }
        else {
            clientLogin();
        }
    });

})

function clientLogin() {
    var name = $('#username').val();
    var pwd = $('#password').val();

    var user = new Object();
    user.Account = name;
    user.PassWord = pwd;
    var app = JSON.stringify({ 'user': user, 'urlString': $.getUrlParam('url') });
    $.ajax({
        type: "POST",
        contentType: 'application/json',
        url: "/Login/UserLogin",
        data: JSON.stringify({ 'user':user,'urlString':$.getUrlParam('url')}),
        success: function (msg) {
            if (msg.Status) {
                window.location.href = msg.url;
            }
            else {
                $('.flash-error').show();
                $('#spanerror').html(msg.Message);
            }
        }
    })
}

