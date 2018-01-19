window.onload = function () {
    var queryString = document.getElementById("bqmrq").src.split("?")[1];
    var querystrArray = queryString.split("&");
    var ifrm = document.createElement("iframe");
    var lnk = document.URL;
    var ht = querystrArray[0].split("=")[1];
    var wd = querystrArray[1].split("=")[1];
    var did = querystrArray[2].substring(4);
    ifrm.setAttribute("src", "http://booqmarqs.com/Bookmark/Show?did=" + did + "&lnk=" + lnk + "&t=" + document.title);
    ifrm.style.border = "none";
    ifrm.style.overflow = "hidden";
    ifrm.scrolling = "no";
    ifrm.id = "frmBqmrq";
    ifrm.height = (parseInt(ht) + 10).toString() + "px";
    ifrm.width = (parseInt(wd) + 10).toString() + "px";
    document.getElementById("divBqmrq").appendChild(ifrm);
};
