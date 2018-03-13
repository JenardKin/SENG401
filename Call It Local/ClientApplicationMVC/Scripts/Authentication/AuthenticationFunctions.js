function validateLogInForm() {
    if (logInForm.username.value === "" || logInForm.password.value === "") {
        return false;
    }
    return true;
}