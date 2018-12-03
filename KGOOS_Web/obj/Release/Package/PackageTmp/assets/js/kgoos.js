function openwin(title, url, width, height) {
    var _width = !width ? "800px" : width + "px";
    var _height = !height ? "550px" : height + "px";

    layui.use('layer', function () {
        var layer = layui.layer;
        layer.open({
            type: 2,
            title: title,
            shadeClose: false,
            shade: 0.8,
            maxmin: false, //开启最大化最小化按钮
            area: [_width, _height],
            content: url
        });
    });

}

function alert_layui(msg, icon, time) {
    var _time = !time ? "2000" : time
    layui.use('layer', function () {
        var layer = layui.layer;
        layer.msg(msg, {
            icon: icon,
            time: _time
        });
    });
}

function IsLogin() {
    $.post('../Login/getLoginData', function (data) {
        if (data.code == 0) {
        }
        else {
            alert_layui(data.msg, 2);
            window.location = '../Login/Login';
            return false;
        }
    });
}