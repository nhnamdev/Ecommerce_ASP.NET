$(document).ready(
    function () {
        $("#Toto").on("input", function () {
            var userName = $("#Toto").val();
            if (userName.length > 0) {
                $.ajax({
                    url="/KhachHang/checkUserName",
                    type="POST",
                    data={ userName: HoTen },
                    success: function (response) {
                        if (response.message === "OK") {
                            $("#To").prop("disabled", false);
                            alert(response.message);
                        }
                        if (response.message === "Tài khoản không tồn tại.") {
                            $("#To").prop("disabled", true);
                            alert(response.message);
                        }

                    },
                    error: function () {
                        $("#To").prop("disabled", true);
                        alert("Lỗi khi kiểm tra tài khoản.");
                    }
                })
            }

        });
    }
);