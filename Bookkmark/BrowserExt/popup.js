document.getElementById("clickme").addEventListener("click", function(e) {
    chrome.runtime.sendMessage({ 'myPopupIsOpen': true });
});



chrome.runtime.onMessage.addListener(function (message, sender) {
    console.log(message.url);
    console.log(message.title);
    document.getElementById("divTitle").innerText = message.title;
    document.getElementById("divURL").innerText = message.url;
    var BookmarkModel = {
        'Bookmark':message.url,
        'title': message.title,
        'ipAddr': '10',
       'bookmarkImgSrc':'extension'
    };
    $.ajax({
        url: 'http://localhost/bookmark/bookmark/addbm',
        type: 'POST',
        dataType: "text",
        data: BookmarkModel,
        success: function (data) {
            if (data == null || data == "") {
                //alert("Bookmark not added");
                var left = (screen.width) - (0.30 * screen.width);
                var top = (screen.height) - (0.80 * screen.height);
                var height = screen.height * 0.50;
                var width = screen.height * 0.38;
               // window.open("http://localhost/bookmark/bookmark/account/quicklogin", "_blank", "toolbar=yes,scrollbars=no,resizable=no,top=" + top + ",left=" + left + ",width=" + width + ",height=" + height + "");
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