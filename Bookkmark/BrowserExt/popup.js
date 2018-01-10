document.getElementById("clickme").addEventListener("click", function(e) {
    chrome.runtime.sendMessage({ 'myPopupIsOpen': true });
});



chrome.runtime.onMessage.addListener(function (message, sender) {
    //document.getElementById("divTitle").innerText = message.title;
    //document.getElementById("divURL").innerText = message.url;
    var strRedirect = "http://booqmarqs.com/bookmark/addBmrk?url=" + message.url + "&name=" + message.title + "";
    var creating = browser.tabs.create({
        url: strRedirect
    });
 
}); 