function ValidateUserReg() { var e = document.getElementById("FirstName").value; var b = document.getElementById("LastName").value; var d = document.getElementById("Email").value; var a = document.getElementById("Details").value; var c = document.getElementById("txtPassword").value; var f = document.getElementById("txtConfirmPassword").value; var g = document.getElementById("Address").value; if (e == "" || b == "" || d == "" || a == "" || c == "" || f == "" || g == "") { alert("Please enter all details. First and Last names, EMail, Password, Addess and Details"); return false } else { if (c != f) { alert("Confirm password not matching with Password"); return false } else { if (c.length < 8) { alert("Password is expected to be 8 charectors."); return false } else { if (ValidateEMail(d)) { return true } else { alert("Please enter valid email"); return false } } } } }

function ValidateSuggestion() { var b = document.getElementById("txtSuggestion").value; var a = document.getElementById("hfUserEMail").value; if (a != "") { if (b == "") { alert("Please enter all details"); return false } else { return true } } else { alert("To avoid spams and robots please login to contact us. We appreciate your patience."); return false } }

function ValidateLogin()
{
    var a = document.getElementById("txtEMailId").value;
    var b = document.getElementById("txtPassword").value;
    if (a != "" && b != "") {
        if (ValidateEMail(a)) {
            return true;
        }
        else {
            document.getElementById("lblAck").innerText = "Please enter valid email";
            return false;
        }
    }
    else {
        document.getElementById("lblAck").innerText = "Please enter username and password";
        return false;
    }
}

function ValidateEMail(a) { var b = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i; return b.test(a) }

function ValidateForgotPassword() { var a = document.getElementById("txtEMailId").value; if (a != "") { if (ValidateEMail(a)) { return true } else { return false } } else { alert("Please enter email"); return false } }

function ValidatePasswords() { var b = document.getElementById("HFOldPassword").value; var c = document.getElementById("txtPassword").value; var a = document.getElementById("txtNewPassword").value; var e = document.getElementById("txtConfirmPassword").value; var d = document.getElementById("hfUserEMail").value; if (d != "") { if (b != c) { alert("Please enter correct Old Password"); return false } else { if (a != e) { alert("Password and ConfirmPassword does not match"); return false } else { if (a.length < 8) { alert("Password is expected to be 8 charectors."); return false } else { return true } } } } else { alert("Please login to post"); return false } }
