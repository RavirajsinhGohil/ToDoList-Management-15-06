$(document).on('click', '#forgotPasswordLink', function (e) {
    e.preventDefault();
    var email = $("#email").val();
    var url = `Auth/ForgotPassword`;
    if (email != "") {
        url += `?email=${encodeURIComponent(email)}`;
    }
    window.location.href = url;
});