document.getElementById("clickme").addEventListener("click", function(e) {
    chrome.runtime.sendMessage({ 'myPopupIsOpen': true });
});



chrome.runtime.onMessage.addListener(function (message, sender) {
    console.log(message.url);
    console.log(message.title);
    document.getElementById("divURL").innerText = message.url + message.title;
}); 