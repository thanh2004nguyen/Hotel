document.addEventListener("DOMContentLoaded", function () {
    const currentPath = window.location.pathname;
    const targetPath = "/Room";

    if (!currentPath.startsWith(targetPath)) {
        return;
    }

    const rangeInput = document.getElementById("customRange1");
    const maxNumberInput = document.getElementById("typeNumberMax");
    const minNumberInput = document.getElementById("typeNumberMin");
    const maxLabel = maxNumberInput.nextElementSibling;
    const minLabel = minNumberInput.nextElementSibling;
    
    function formatNumber(num) {
        return Number(num).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 }).replace(/,/g, ".");
    }
    function updateMaxValue() {
        const newValue = parseInt(rangeInput.value);
        maxNumberInput.value = newValue;

        if (newValue > 0) {
            maxNumberInput.classList.add("active");
            maxLabel.innerText = `${formatNumber(newValue)}đ`;
            minNumberInput.max = newValue;

            const minValue = parseInt(minNumberInput.value.replace(/\./g, ""));
            if (minValue >= newValue) {
                minNumberInput.value = newValue;
                minLabel.innerText = `${formatNumber(newValue)}đ`;
            }
        } else {
            maxNumberInput.classList.remove("active");
            maxLabel.innerText = `$0`;
            maxNumberInput.value = "";
            minNumberInput.value = "";
            minNumberInput.classList.remove("active");
            minLabel.innerText = `$0`;
        }
    }

    function updateMinValue() {
        const minValue = parseInt(minNumberInput.value.replace(/\./g, ""));
        if (minValue) {
            minNumberInput.classList.add("active");
            minLabel.innerText = `${formatNumber(minValue)}đ`;
        } else {
            minLabel.classList.remove("active");
        }

        const maxValue = parseInt(maxNumberInput.value.replace(/\./g, ""));
        if (minValue >= maxValue) {
            minNumberInput.value = maxValue;
            minLabel.innerText = `${formatNumber(maxValue)}đ`;
        }
    }

    function updateMaxLabel() {
        const maxValue = parseInt(maxNumberInput.value.replace(/\./g, ""));
        if (maxValue) {
            maxLabel.classList.add("active");
            maxLabel.innerText = `${formatNumber(maxValue)}đ`;
        } else {
            maxLabel.classList.remove("active");
        }
    }

    rangeInput.addEventListener("input", updateMaxValue);
    minNumberInput.addEventListener("input", updateMinValue);
    maxNumberInput.addEventListener("input", updateMaxLabel);

    minNumberInput.addEventListener("focus", function () {
        minLabel.classList.add("active");
    });

    maxNumberInput.addEventListener("focus", function () {
        maxLabel.classList.add("active");
    });

    minNumberInput.addEventListener("blur", function () {
        if (!minNumberInput.value) {
            minLabel.classList.remove("active");
        }
    });

    maxNumberInput.addEventListener("blur", function () {
        if (!maxNumberInput.value) {
            maxLabel.classList.remove("active");
        }
    });

    updateMaxValue();
    updateMinValue();
});




// Lọc bằng ajax
$(document).ready(function () {
    const selectMost = document.getElementById("selectMost");
    const filterBtn = document.getElementById("filterBtn");
    const minPriceInput = document.getElementById("typeNumberMin");
    const maxPriceInput = document.getElementById("typeNumberMax");
    const ratingCheckboxes = document.querySelectorAll('.accordion-item:nth-child(3) .form-check input[type="checkbox"]');
    function getFilterValues() {
        // Lấy tất cả các checkbox roomType đã được chọn (chỉ checkbox có class 'roomTypeCheckbox')
        const roomTypes = Array.from(document.querySelectorAll('.roomTypeCheckbox:checked'))
            .map(checkbox => parseInt(checkbox.value));

        const minPrice = parseInt(minPriceInput.value) || 0;
        const maxPrice = parseInt(maxPriceInput.value) || 10000000;

        const ratings = Array.from(document.querySelectorAll('.accordion-item:nth-child(3) .form-check input[type="checkbox"]:checked')).map(checkbox => parseInt(checkbox.value));
        const minRating = ratings.length > 0 ? Math.min(...ratings) : 0; 
        
        // Tạo đối tượng dữ liệu để gửi
        const filterData = {
            roomTypes: roomTypes,
            minPrice: minPrice,
            maxPrice: maxPrice,
            minRating: minRating
        };
        console.log("Filter Data to send:", filterData);

        $.ajax({
            url: '/Room/Index', 
            type: 'GET',
            contentType: 'application/json',
            data: filterData,
            traditional: true,
            success: function (data) {
                $('#room-list').html($(data).find('#room-list').html());
                // Cập nhật số lượng phòng (totalRooms)
                $('#totalRooms').html($(data).find('#totalRooms').html());

                // Cập nhật lại phân trang (pagination)
                $('.pagination').html($(data).find('.pagination').html());

            },
            error: function (xhr, status, error) {
                console.error("Error occurred: ", error);
            }
        });
    }
    filterBtn.addEventListener("click", function (event) {
        getFilterValues();
    });

    $('selectMost').on('change', function () {
        getFilterValues();
    });
});
