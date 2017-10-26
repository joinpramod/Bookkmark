
(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));
// Init the SDK upon load

window.fbAsyncInit = function () {
    FB.init({
        appId: '848792638492356', // Write your own application id
        channelUrl: '//' + window.location.hostname + '/channel', // Path to your Channel File
        scope: 'email,id,name,gender,user_birthday',
        status: false, // check login status
        //cookie: false, // enable cookies to allow the server to access the session
        version: 'v2.5',
        xfbml: true  // parse XFBML
    });
}


function FaceBookLogin() {
    FB.login(function (response) {
        if (response.authResponse) {
            debugger;
            // user has auth'd your app and is logged into Facebook
            var uid = "http://graph.facebook.com/" + response.authResponse.userID + "/picture";
            FB.api('/me', function (me) {
                if (me.name) {
                    // debugger;

                    var form = document.createElement("form");
                    form.setAttribute("method", "post");
                    form.setAttribute("action", "/Account/ViewUser");
                    //form.setAttribute("action", "/Account/ViewUser");

                    var hfEMail = document.createElement("input");
                    hfEMail.setAttribute("type", "hidden");
                    hfEMail.setAttribute("name", me.email);
                    hfEMail.setAttribute("value", me.email);


                    var hfName = document.createElement("input");
                    hfName.setAttribute("type", "hidden");
                    hfName.setAttribute("name", me.name);
                    hfName.setAttribute("value", me.name);


                    var hfImageURL = document.createElement("input");
                    hfImageURL.setAttribute("type", "hidden");
                    hfImageURL.setAttribute("name", me.hfImageUrl);
                    hfImageURL.setAttribute("value", me.hfImageUrl);


                    var hfType = document.createElement("input");
                    hfType.setAttribute("type", "hidden");
                    hfType.setAttribute("name", "Facebook");
                    hfType.setAttribute("value", "Facebook");

                    form.appendChild(hfEMail);
                    form.appendChild(hfName);
                    form.appendChild(hfImageURL);
                    form.appendChild(hfType);

                    document.body.appendChild(form);
                    form.submit();

                }
            })
        }
    });
}


