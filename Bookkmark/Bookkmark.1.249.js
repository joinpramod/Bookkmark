window.onload = function () {
    var queryString = document.getElementById("bookmark").src.split("?")[1];
    var querystrArray = queryString.split("&");
    var ifrm = document.createElement("iframe");
    var lnk = document.referrer;
    var ht = querystrArray[1].split("=")[1] + "px";
    var wd = querystrArray[2].split("=")[1] + "px";
    var sd = querystrArray[3].split("=")[1];
    ifrm.setAttribute("src", "http://localhost/Bookmark/Bookmark/Show?lnk=" + lnk + "&h=" + ht + "&w=" + wd + "&sd=" + sd + "&t=" + document.title
);
    ifrm.style.border = "none";
    ifrm.style.overflow = "hidden";
    ifrm.scrolling = "no"
    ifrm.style.width = parseInt(wd, 10) + 1;
    ifrm.style.height = parseInt(ht, 10) + 1;
    document.getElementById("divBookmark").appendChild(ifrm);
};
