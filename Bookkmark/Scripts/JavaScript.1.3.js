function ValidateUserReg() {
    var e = document.getElementById("txtFirstName").value;
    var b = document.getElementById("txtLastName").value;
    var d = document.getElementById("txtEMailId").value;    
    var c = document.getElementById("txtNewPassword").value;
    var f = document.getElementById("txtConfirmPassword").value;
    if (e == "" || b == "" || d == "" || c == "" || f == "") {
        document.getElementById("divAck").innerText = "Please enter all details. First and Last names, EMail, Password";
        return false;
    }
    else {
        if (c != f) {
            document.getElementById("divAck").innerText = "Confirm password not matching with Password";
            return false;
        }
        else {
            if (c.length < 8) {
                document.getElementById("divAck").innerText = "Password is expected to be 8 charectors";
                return false;
            }
            else {
                if (ValidateEMail(d)) {
                    return true;
                }
                else {
                    document.getElementById("divAck").innerText = "Please enter valid email";
                    return false;
                }
            }
        }
    }
}

//function ValidateEditUser() {
//    var e = document.getElementById("FirstName").value;
//    var b = document.getElementById("LastName").value;
//    var d = document.getElementById("Email").value;
//    if (e == "" || b == "" || d == "") {
//        document.getElementById("divAck").innerText = "Please enter all details. First and Last names, EMail";
//        return false;
//    }
//    else {
//                if (ValidateEMail(d)) {
//                    return true;
//                }
//                else {
//                    document.getElementById("divAck").innerText = "Please enter valid email";
//                    return false;
//            }
//    }
//}


//function ValidateSuggestion() {
//    var b = document.getElementById("txtSuggestion").value;
//    var a = document.getElementById("hfUserEMail").value;
//    if (a != "") {
//        if (b == "") {
//            document.getElementById("divAck").innerText = "Please enter all details";
//            return false;
//        }
//        else {
//            return true;
//        }
//    }
//    else {
//        document.getElementById("divAck").innerText = "To avoid spams and robots please login to contact us. We appreciate your patience";
//        return false;
//    }
//}

//function ValidateLogin()
//{
//    var a = document.getElementById("txtEMailId").value;
//    var b = document.getElementById("txtPassword").value;
//    if (a != "" && b != "") {
//        if (ValidateEMail(a)) {
//            return true;
//        }
//        else {
//            document.getElementById("divAck").innerText = "Please enter valid email";
//            return false;
//        }
//    }
//    else {
//        document.getElementById("divAck").innerText = "Please enter username and password";
//        return false;
//    }
//}


function ValidatePasswords() {
    var c = document.getElementById("txtPassword").value;
    var a = document.getElementById("txtNewPassword").value;
    var e = document.getElementById("txtConfirmPassword").value;
    var d = document.getElementById("hfUserEMail").value;
            if (a != e) {
                document.getElementById("divAck").innerText = "Password and ConfirmPassword does not match";
                return false;
            }
            else {
                if (a.length < 8) {
                    document.getElementById("divAck").innerText = "Password is expected to be 8 charectors";
                    return false;
                }
                else {
                    return true;
                }
            }
}



//function ValidateEMail(a) {
//    var b = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
//    return b.test(a);
//}

//function ValidateForgotPassword() {
//    var a = document.getElementById("txtEMailId").value;
//    if (a != "") {
//        if (ValidateEMail(a)) {
//            return true;
//        }
//        else {
//            document.getElementById("divAck").innerText = "Please enter valid email";
//            return false;
//        }
//    }
//    else {
//        document.getElementById("divAck").innerText = "Please enter email";
//        return false;
//    }
//}
