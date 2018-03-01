
/**
 * This function checks to make sure the as is echo text is not empty
 * If it is not empty, it will return true
 */
function validateAsIsEchoForm() {
    if (asIsEchoForm.asIsText.value === "") {
        return false;
    }
    return true;
}

function validateReverseEchoForm() {
    if (reverseEchoForm.reverseText.value === "") {
        return false;
    }
    return true;
}