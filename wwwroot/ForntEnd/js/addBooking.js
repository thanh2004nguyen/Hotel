$(document).ready(function () {
    var countdownTime = 600; // 10 minutes in seconds
    var countdownTimer = setInterval(function () {
        var minutes = Math.floor(countdownTime / 60);
        var seconds = countdownTime % 60;

        $('#timer').text(
            (minutes < 10 ? '0' : '') + minutes + ':' +
            (seconds < 10 ? '0' : '') + seconds
        );

        if (countdownTime <= 0) {
            clearInterval(countdownTimer);
        }
        countdownTime--;
    }, 1000);

    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
            $('.countdown').addClass('fixed');
        } else {
            $('.countdown').removeClass('fixed');
        }
    });

    // show hide select request
    $('#toggleSpecialRequest').click(function () {
        $('#specialRequestContent').slideToggle();
    });
    //bước tiếp theo thanh toán
    document.getElementById('nextStepButton').addEventListener('click', function (e) {
        e.preventDefault(); // Ngăn chặn hành động mặc định

        const customerName = document.getElementById('customerName').value.trim();
        const customerEmail = document.getElementById('customerEmail').value.trim();
        const customerPhone = document.getElementById('customerPhone').value.trim();

        // Kiểm tra thông tin người dùng
        if (!customerName || !customerEmail || !customerPhone) {
            return alert("Vui lòng điền đầy đủ thông tin!");
        }

        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(customerEmail)) {
            return alert("Vui lòng nhập email hợp lệ!");
        }

        // Cập nhật thanh tiến trình và chuyển bước
        document.querySelector('.progress').style.width = "66%";
        document.getElementById('step1').style.display = 'none';
        document.getElementById('step2').style.display = 'block';
    });

    // Lắng nghe sự kiện 'input' cho các trường thông tin
    document.getElementById('customerName').addEventListener('input', checkFormValidity);
    document.getElementById('customerEmail').addEventListener('input', checkFormValidity);
    document.getElementById('customerPhone').addEventListener('input', checkFormValidity);

    function checkFormValidity() {
        const customerName = document.getElementById('customerName').value.trim();
        const customerEmail = document.getElementById('customerEmail').value.trim();
        const customerPhone = document.getElementById('customerPhone').value.trim();

        // Kiểm tra tính hợp lệ
        const isValid = customerName && customerEmail && customerPhone &&
            /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(customerEmail);

        document.getElementById('nextStepButton').disabled = !isValid;
    }

    // Vô hiệu hóa nút khi tải trang
    document.getElementById('nextStepButton').disabled = true;

    // Nút Quay lại từ bước 2 về bước 1
    document.getElementById('backToStep1').addEventListener('click', function () {
        var progress = document.getElementsByClassName('progress')[0];
        if (progress) {
            progress.style.width = "33%"; // Đưa progress bar về 33%
        }

        // Ẩn bước 2, hiện bước 1
        var step1 = document.getElementById('step1');
        var step2 = document.getElementById('step2');

        if (step1 && step2) {
            step2.style.display = 'none';
            step1.style.display = 'block';
        }
    });


    //checkint-checkout-date
    // Check-in and check-out dates
    var today = new Date();
    var tomorrow = new Date(today);
    tomorrow.setDate(today.getDate() + 1);

    // Date display format
    var options = { weekday: 'short', month: 'long', day: 'numeric' };

    // Store the dates as ISO for calculations
    var checkinISO = today.toISOString();
    var checkoutISO = tomorrow.toISOString();

    // Update the displayed dates
    document.getElementById('checkinDate').querySelector('h3').textContent = today.toLocaleDateString('vi-VN', options);
    document.getElementById('checkoutDate').querySelector('h3').textContent = tomorrow.toLocaleDateString('vi-VN', options);
    // Function to calculate nights between check-in and check-out
    function calculateNights(checkinISO, checkoutISO) {
        var checkin = new Date(checkinISO);
        var checkout = new Date(checkoutISO);
        var timeDiff = checkout - checkin; // Time difference
        var numberOfNights = Math.ceil(timeDiff / (1000 * 3600 * 24)); // Convert to days
        return numberOfNights;
    }

    // Function to update price and nights display
    function updatePriceAndNights() {
        var nights = calculateNights(checkinISO, checkoutISO);

        var discount = document.getElementById('discount-price').innerText
        document.getElementById('totalNights').innerText = nights > 0 ? nights : '0';
        document.getElementById('total-nights').innerText = nights > 0 ? nights : '0';
        document.getElementById('total-nights1').innerText = nights > 0 ? nights : '0';
        // Get price per night from the input element
        var pricePerNight = parseFloat(document.getElementById('pricePerNight').value);

        // Calculate total price
        var totalPrice = pricePerNight * nights;
        var totalPriceDiscount = (pricePerNight - pricePerNight * (discount / 100)) * nights;//giá gốc - giá phòng + discount /100
        
        // Update the price and total fields
        document.getElementById('totalpricediscount').innerText = totalPriceDiscount;
        document.getElementById('total-nights-price').innerText = nights;
        document.getElementById('total-nights-price1').innerText = nights;
        document.getElementById('old-price').textContent = totalPrice.toLocaleString('vi-VN') + " ₫";
        document.getElementById('old-price1').textContent = totalPrice.toLocaleString('vi-VN') + " ₫";
        document.getElementById('room-price2').textContent = totalPriceDiscount.toLocaleString('vi-VN') + " ₫";
        document.getElementById('room-price3').textContent = totalPriceDiscount.toLocaleString('vi-VN') + " ₫";
        document.getElementById('duocgiam').textContent = (totalPrice - totalPriceDiscount).toLocaleString('vi-VN') + " ₫";
        document.getElementById('duocgiam1').textContent = (totalPrice - totalPriceDiscount).toLocaleString('vi-VN') + " ₫";

        // Cập nhật ngày nhận phòng và trả phòng cho giao diện thứ hai
        var checkinDateText = document.getElementById('checkinDate').querySelector('h3').innerText;
        var checkoutDateText = document.getElementById('checkoutDate').querySelector('h3').innerText;

        document.getElementById('checkin-payment').innerText = checkinDateText;
        document.getElementById('checkout-payment').innerText = checkoutDateText;
        document.getElementById('pernight-payment').innerText = nights;
    }

    // Initialize Check-in calendar with rome
    var checkinRome = rome(inline_cal_from, {
        time: false,
        inputFormat: 'MMMM DD, YYYY',
        min: today,
        dateValidator: rome.val.beforeEq(inline_cal_to)
    }).on('data', function (value) {
        checkinISO = new Date(value).toISOString(); // Update ISO value
        document.getElementById('checkinDate').querySelector('h3').textContent = new Date(value).toLocaleDateString('vi-VN', options);
        $("#checkin-calendar").hide();
        updatePriceAndNights(); // Update price and nights
    });

    // Initialize Check-out calendar with rome
    var checkoutRome = rome(inline_cal_to, {
        time: false,
        inputFormat: 'MMMM DD, YYYY',
        min: tomorrow,
        dateValidator: rome.val.afterEq(inline_cal_from)
    }).on('data', function (value) {
        checkoutISO = new Date(value).toISOString(); // Update ISO value
        document.getElementById('checkoutDate').querySelector('h3').textContent = new Date(value).toLocaleDateString('vi-VN', options);
        $("#checkout-calendar").hide();
        updatePriceAndNights(); // Update price and nights
    });

    // Toggle calendars visibility
    $("#checkinDate").click(function () {
        $("#checkout-calendar").hide();
        $("#checkin-calendar").toggle();
    });

    $("#checkoutDate").click(function () {
        $("#checkin-calendar").hide();
        $("#checkout-calendar").toggle();
    });

    // Hide calendars when clicking outside
    $(document).click(function (event) {
        if (!$(event.target).closest("#checkinDate, #checkoutDate, #checkin-calendar, #checkout-calendar").length) {
            $("#checkin-calendar").hide();
            $("#checkout-calendar").hide();
        }
    });

    // Initial price and nights update when the page loads
    updatePriceAndNights();
});