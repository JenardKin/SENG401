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
    //Check if phone number is not a number
    if (isNaN(createAccountForm.phonenumber.value)) {
        return false;
    }
    //Check if email is of the form <string>@<string>.<string>
    var re = /\S+@\S+\.\S+/;
    if (!(createAccountForm.email.value.match(re))) {
        return false;
    }
    return true;
}