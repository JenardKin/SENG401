/**
 * These functions check whether text fields are blank or do not contain "invalid information"
 */
function validateLogInForm() {
    if (logInForm.username.value === "") {
        response.innerText = "Username cannot be blank";
        return false;
    }
    else if (logInForm.password.value == "") {
        response.innerText = "Password cannot be blank";
    }
    return true;
}

function validateCreateAccountForm() {
    var re1 = /.*\d.*/;
    var re2 = /.*[A-Z].*/;
    var re3 = /.*[a-z].*/;
    if (createAccountForm.username.value === "") {
        response.innerText = "Username cannot be blank";
        return false;
    }
    else if (createAccountForm.address.value === "") {
        response.innerText = "Address cannot be blank";
        return false;
    }
    else if (createAccountForm.phonenumber.value === "") {
        response.innerText = "Phonenumber cannot be blank";
        return false;
    }
    else if (createAccountForm.email.value === "") {
        response.innerText = "Email cannot be blank";
        return false;
    }
    else if (createAccountForm.password.value.length < 6) {
        response.innerText = "Password must be atleast 6 characters";
        return false;
    }
    else if (!createAccountForm.password.value.match(re1) || !createAccountForm.password.value.match(re2) || !createAccountForm.password.value.match(re3)) {
        response.innerText = "Password must contain a number, a captial letter, and a lowercase letter";
        return false;
    }
    //Check if phone number is of valid formats
    //Phones numbers must be structured as follows:
        // (ddd)wdddwdddd
            // ( is optional
            // ) is optional iff ( was not provided
            // d = a single digit
            // w = whitespace " " or a hypen "-" or empty ""
    var re1 = /((\d{3})|(\(\d{3}\)))(\-{0,1}|\s{0,1})\d{3}(\-{0,1}|\s{0,1})\d{4}/
    if (!(createAccountForm.phonenumber.value.match(re1))) {
        response.innerText = "Phone number must be of the following forms: (XXX)-XXX-XXXX,XXX-XXX-XXXX with hyphens (or spaces) optional"
        return false;
    }
    //Check if email is of the form <string>@<string>.<string>
    var re2 = /\S+@\S+\.\S+/;
    if (!(createAccountForm.email.value.match(re2))) {
        response.innerText = "Email must be of the form: <youremail>@<domain>"
        return false;
    }
    return true;
}