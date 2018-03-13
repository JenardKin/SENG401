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
    if (isNaN(createAccountForm.phonenumber.value)) {
        return false;
    }
    if (!createAccountForm.email.value.includes("@") || !createAccountForm.email.value.includes(".")) {
        return false;
    }
    return true;
}