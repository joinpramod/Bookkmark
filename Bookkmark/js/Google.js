
function onSuccess(googleUser) {
    //    debugger;
    var profile = googleUser.getBasicProfile();
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    //form.setAttribute("action", "/Bookkmark/Account/ViewUser");
    form.setAttribute("action", "/Account/ViewUser");

    var hfEMail = document.createElement("input");
    hfEMail.setAttribute("type", "hidden");
    hfEMail.setAttribute("name", profile.getEmail());
    hfEMail.setAttribute("value", profile.getEmail());


    var hfName = document.createElement("input");
    hfName.setAttribute("type", "hidden");
    hfName.setAttribute("name", profile.getName());
    hfName.setAttribute("value", profile.getName());


    var hfImageURL = document.createElement("input");
    hfImageURL.setAttribute("type", "hidden");
    hfImageURL.setAttribute("name", profile.getImageUrl());
    hfImageURL.setAttribute("value", profile.getImageUrl());


    var hfType = document.createElement("input");
    hfType.setAttribute("type", "hidden");
    hfType.setAttribute("name", "Google");
    hfType.setAttribute("value", "Google");

    form.appendChild(hfEMail);
    form.appendChild(hfName);
    form.appendChild(hfImageURL);
    form.appendChild(hfType);

    document.body.appendChild(form);
    form.submit();
}

function onFailure(error) {
    alert("Sign in with Google failed. Please try again");
}

function renderButton() {

    gapi.signin2.render('my-signin2', {
        'scope': 'https://www.googleapis.com/auth/plus.login',
        'width': 200,
        'height': 35,
        'longtitle': true,
        'theme': 'dark',
        'onsuccess': onSuccess,
        'onfailure': onFailure
    });
}



window.onbeforeunload = function (e) {
    try{
        var auth2 = gapi.auth2.getAuthInstance();
        auth2.signOut().then(function () {
            console.log('User signed out.');
        });
    }
    catch(err){
    }


};
