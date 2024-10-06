$(document).ready(function () {

    $(".js-example-placeholder-single").select2({
        placeholder: "Chọn Icon",
        allowClear: true,
        templateResult: formatIcon, // Định dạng hiển thị trong dropdown
        templateSelection: formatIcon // Định dạng hiển thị khi chọn
    });

    function formatIcon(icon) {
        if (!icon.id) { return icon.text; }
        const iconName = icon.text.split('-').pop().split('"')[0]; // Lấy giá trị sau dấu - cuối cùng
        var $icon = $('<span><i class="' + $(icon.element).data('icon') + '"></i> ' + icon.text + '  ' + iconName + '</span>');
        return $icon;
    }

});