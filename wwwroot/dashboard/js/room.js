$(document).on('click', '.delete-room', function () {
    var roomId = $(this).data('room-id');

    if (confirm("Are you sure you want to delete this room?")) {
        $.ajax({
            url: '/admin/room/deleteroom',
            type: 'POST',
            data: { roomId: roomId },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    $('.room-item-' + response.roomId).closest('.products-row').remove();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert("An error occurred while trying to delete the room.");
            }
        });
    }
});
document.addEventListener('DOMContentLoaded', function () {
    const currentPath = window.location.pathname;
    const targetPath = "/admin/room/index"; 
    if (currentPath !== targetPath) {
        return; 
    }
    const roomList = document.getElementById('roomList');
    const rooms = roomList.getElementsByClassName('room');

    document.getElementById("search-room").addEventListener("keyup", function () {
        const searchTerm = this.value.toLowerCase();
        const productRows = document.querySelectorAll("#room");
        productRows.forEach(row => {
            const roomName = row.querySelector(".roomName").textContent.toLowerCase(); 
            if (roomName.includes(searchTerm)) {
                row.style.display = ""; 
            } else {
                row.style.display = "none";
            }
        });
    });

});

document.addEventListener("DOMContentLoaded", function () {
    const currentPath = window.location.pathname;
    const targetPath = "/admin/room/create";
    if (currentPath !== targetPath) {
        return;
    }
    const selectAllAmenitiesThemes = document.getElementById('selectAllAmenitiesThemes');
    const amenitiesThemesSelect = $('#amenitiesThemes');

    // Xử lý sự kiện cho checkbox "Select All Amenities Themes"
    selectAllAmenitiesThemes.addEventListener('change', function () {
        const options = amenitiesThemesSelect[0].options; 
        for (let i = 0; i < options.length; i++) {
            options[i].selected = selectAllAmenitiesThemes.checked;
        }
        // Cập nhật select2
        amenitiesThemesSelect.val([...options].filter(option => option.selected).map(option => option.value)).trigger('change');
    });

    const selectAllAmenities = document.getElementById('selectAllAmenities');
    const amenitiesSelect = $('#amenities');
    selectAllAmenities.addEventListener('change', function () {
        const options = amenitiesSelect[0].options;
        for (let i = 0; i < options.length; i++) {
            options[i].selected = selectAllAmenities.checked;
        }
        // Cập nhật select2
        amenitiesSelect.val([...options].filter(option => option.selected).map(option => option.value)).trigger('change');
    });
});

