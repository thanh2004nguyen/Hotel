document.addEventListener("DOMContentLoaded", function () {
    const checkInDateInput = document.getElementById("date-in");
    const checkOutDateInput = document.getElementById("date-out");

    // Khởi tạo Flatpickr cho ngày nhận phòng
    const checkInPicker = flatpickr(checkInDateInput, {
        dateFormat: "Y-m-d",
        minDate: "today", // Vô hiệu hóa ngày trong quá khứ
        onChange: function (selectedDates) {
            if (selectedDates.length > 0) {
                const checkInDate = selectedDates[0];
                // Cập nhật ngày tối thiểu cho ngày trả phòng dựa trên ngày nhận phòng đã chọn
                checkOutPicker.set("minDate", checkInDate.setDate(checkInDate.getDate() + 1));
                console.log("Selected Check-In Date:", checkInDateInput.value);
            }
        }
    });

    // Khởi tạo Flatpickr cho ngày trả phòng
    const checkOutPicker = flatpickr(checkOutDateInput, {
        dateFormat: "Y-m-d",
        minDate: new Date().fp_incr(1) // Ngày tối thiểu mặc định là ngày mai
    });

    // Đính kèm trình xử lý sự kiện vào các biểu tượng để nhấp để mở bộ chọn ngày
    document.querySelectorAll('.fa-calendar').forEach((icon, index) => {
        icon.addEventListener("click", function () {
            if (index === 0) {
                checkInPicker.open(); // Open Check-In calendar
            } else {
                checkOutPicker.open(); // Open Check-Out calendar
            }
        });
    });

});