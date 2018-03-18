/**
*   This function automatically runs when this javascript file is loaded.
*   This function sets the on click function of the send button to be the validate and send message function below.
*/
$(function () {
    $("#SendMessageButton").click(validateAndSendMessage);
});

/**
 * Ensures that the user has entered a valid message, then sends it.
 * If successful, the user will be forewarded to their message inbox
 * This function will fail to work until the Chat Service is implemented.
 */
function validateAndSendMessage() {

    var userData = $("#textUserMessage").val();
    if ($.trim(userData) == "") {
        return;
    }

    var recipient = $("#CompanyNameDisplay").text();
    var timestamp = Math.round((new Date()).getTime() / 1000);

    $.ajax({
        method: "POST",
        url: "/Chat/SendMessage",
        data: {
            receiver: recipient,
            timestamp: timestamp,
            message: userData
        },
        success: function () {
            window.location = "/Chat/Index";
        }
    });
}