document.getElementById("clickme").addEventListener("click", function (e) {
    chrome.runtime.sendMessage({ 'myPopupIsOpen': true });
});


var emailId = "";
chrome.runtime.onMessage.addListener(function (message, sender) {
    if (message.url != null || message.url != undefined) {
        var strRedirect = "http://booqmarqs.com/bookmark/addBmrk?url=" + message.url + "&name=" + message.title + "";
        var creating = chrome.tabs.create({
            url: strRedirect
        });
    }
    else if (message.booqmarqsCookieValue != null || message.booqmarqsCookieValue != undefined) {
        emailId = message.booqmarqsCookieValue;
        chrome.bookmarks.getTree(process_bookmark);
    }
    else
    {
        alert("Please login to continue");
    }
}); 




function process_bookmark(bookmarks) {
    var BookmarkModel = {
        'Id': emailId,
        'Bookmarks': JSON.stringify(bookmarks)
    };

    var data = BookmarkModel;
    var http = new XMLHttpRequest();
    var url = "http://booqmarqs.com/bookmark/extImport";
    http.open("POST", url, true);
    //Send the proper header information along with the request
    http.setRequestHeader("Content-type", "application/json");
    http.send(JSON.stringify(data));
    alert("Imported successfully, please click Booqmarqs icon again to refresh");
}

document.getElementById("import").addEventListener("click", function (e) {
    chrome.runtime.sendMessage({ 'readCookie': true });
});




//function getCookies(domain, name, callback) {
//    debugger;
//    chrome.cookies.get({ "url": domain, "name": name }, function (cookie) {
//        if (callback) {
//            callback(cookie.value);
//        }
//    });
//}







//usage:

//function process_bookmark(bookmarks) {

//    for (var i = 0; i < bookmarks.length; i++) {

//        var cb = new ChromeBookmark(bookmark.title, bookmark.url, bookmark.children);

//        var bookmark = bookmarks[i];
//        if (bookmark.url) {
//            console.log("bookmark: " + bookmark.title + " ~  " + bookmark.url);
//        }
//        if (bookmark.children) {
//            process_bookmark(bookmark.children);
//        }
//    }
//}

//function ChromeBookmark(title, url, children) {
//    // Add object properties like this
//    this.title = title;
//    this.url = url;
//    this.children = children;

//}