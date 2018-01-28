chrome.runtime.onMessage.addListener(function (message, sender) {
    if (message.myPopupIsOpen) {
        chrome.tabs.query({ active: true, currentWindow: true }, function (arrayOfTabs) {

            // since only one tab should be active and in the current window at once
            // the return variable should only have one entry
            var activeTab = arrayOfTabs[0];
            var activeTabId = activeTab.url; // or do whatever you need
            //console.log(arrayOfTabs[0].url);
            //console.log(arrayOfTabs[0].title);
            chrome.runtime.sendMessage({ 'title': arrayOfTabs[0].title, 'url': arrayOfTabs[0].url });
        });
    }
    else if (message.readCookie) {
        chrome.cookies.get({ "url": "http://booqmarqs.com", "name": "BooqmarqsLogin" }, function (cookie) {
               chrome.runtime.sendMessage({ 'booqmarqsCookieValue': cookie.value });
    });

    }
    // Do your stuff
});

