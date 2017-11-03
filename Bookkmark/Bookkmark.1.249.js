window.onload = function () {
    var queryString = document.getElementById("bookkmark").src.split("?")[1];
    var querystrArray = queryString.split("&");
    var ifrm = document.createElement("iframe");
    var lnk = document.referrer;
    var ht = querystrArray[1].split("=")[1] + "px";
    var wd = querystrArray[2].split("=")[1] + "px";
    var sd = querystrArray[3].split("=")[1];
    ifrm.setAttribute("src", "http://localhost:49216/Bookkmark/Show?lnk=" + lnk + "&h=" + ht + "&w=" + wd + "&sd=" + sd + "");
    ifrm.style.border = "none";
    ifrm.style.overflow = "hidden";
    ifrm.scrolling = "no"
    document.body.appendChild(ifrm);
};
