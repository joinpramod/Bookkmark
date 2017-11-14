chrome.runtime.onMessage.addListener(function (message, sender) {
    if (!message.myPopupIsOpen) return;
    chrome.tabs.query({ active: true, currentWindow: true }, function (arrayOfTabs) {

        // since only one tab should be active and in the current window at once
        // the return variable should only have one entry
        var activeTab = arrayOfTabs[0];
        var activeTabId = activeTab.url; // or do whatever you need
        //console.log(arrayOfTabs[0].url);
        //console.log(arrayOfTabs[0].title);
        chrome.runtime.sendMessage({ 'title': arrayOfTabs[0].title, 'url': arrayOfTabs[0].url });
    });
    // Do your stuff
});