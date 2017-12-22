window.onload = function () {
     var queryString = document.getElementById("bqmrq").src.split("?")[1];
    var ifrm = document.createElement("iframe");
    var lnk = document.referrer;
    var did = queryString.substring(queryString.indexOf("=") + 1);
    ifrm.setAttribute("src", "http://localhost/Bookmark/Bookmark/Show?did=" + did + "&lnk=" + lnk + "&t=" + document.title);
    ifrm.style.border = "none";
    ifrm.style.overflow = "hidden";
    ifrm.scrolling = "no"
    document.getElementById("divBqmrq").appendChild(ifrm);
};
