/**
 * These functions check whether text fields are blank or do not contain "invalid information"
 */
function validateLogInForm() {
    if (logInForm.username.value === "" || logInForm.password.value === "") {
        return false;
    }
    return true;
}

function validateCreateAccountForm() {
    if (createAccountForm.username.value === "" || createAccountForm.password.value === "" ||
        createAccountForm.address.value === "" || createAccountForm.phonenumber.value === "" ||
        createAccountForm.email.value === "") {
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
        return false;
    }
    //Check if email is of the form <string>@<string>.<string>
    var re2 = /\S+@\S+\.\S+/;
    if (!(createAccountForm.email.value.match(re2))) {
        return false;
    }
    return true;
}