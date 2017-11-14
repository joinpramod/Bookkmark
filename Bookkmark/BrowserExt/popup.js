document.getElementById("clickme").addEventListener("click", function(e) {
    chrome.runtime.sendMessage({ 'myPopupIsOpen': true });
});



chrome.runtime.onMessage.addListener(function (message, sender) {
    console.log(message.url);
    console.log(message.title);
    document.getElementById("divTitle").innerText = message.title;
    document.getElementById("divURL").innerText = message.url;
    var BookkmarkModel = {
        'Bookkmark':message.url,
        'title': message.title,
        'ipAddr': '10',
       'bookkmarkImgSrc':'extension'
    };
    $.ajax({
        url: 'http://localhost/bookkmark/bookkmark/addbm',
        type: 'POST',
        dataType: "text",
        data: BookkmarkModel,
        success: function (data) {
            if (data == null || data == "") {
                //alert("Bookkmark not added");
                var left = (screen.width) - (0.30 * screen.width);
                var top = (screen.height) - (0.80 * screen.height);
                var height = screen.height * 0.50;
                var width = screen.height * 0.38;
                window.open("/bookkmark/account/quicklogin", "_blank", "toolbar=yes,scrollbars=no,resizable=no,top=" + top + ",left=" + left + ",width=" + width + ",height=" + height + "");
            }
            else {
                alert(data);
            }
        },
        error: function (exception) {
            alert(exception);
        }
    });


}); 