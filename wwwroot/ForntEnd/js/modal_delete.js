
$("#btn-delete-room-type").click(function () {
    let data_id = $(this).attr("data-id")

    $("#a-link-delete-room-type").attr("href", `/admin/RoomType/delete/?id=${data_id}`)
});


$(document).ready(() => {
    $(".paginate_button.next").text("trang tiep theo ")
    $(".paginate_button.previous").text("trang truoc ")
    $(".dataTables_info").attr("style", "display:none")

})




const convert = (data) => {
    var parts = data.split(" ");
    var datePart = parts[0];
    var timePart = parts[1];

    // Split the date into day, month, and year
    var dateParts = datePart.split("/");
    var month = dateParts[0];
    var day = dateParts[1];
    var year = dateParts[2];

    // Reformat the date string in dd/mm/yyyy format
    var formattedDate = `ngày ${day}/${month}/${year}`;
    return formattedDate
}
const showDetail = (id, name, phone, days) => {


    $(".cs_de-phone").text(phone)
    $(".cs_de-name").text(name)
    $(".cs_de-day").text(convert(days))

    $(".btn_update-order").attr("data-id", id)
}

const updateOrder = () => {
    let id = $(".btn_update-order").attr("data-id")

    $.post(
        `/admin/order/update?id=${id}`,
        { id: id }, () => {
            $(document).ready(function () {
                let tem = localStorage.getItem('tempTimes')
                localStorage.setItem('orderTimes', tem)
                location.reload(true);
            });
        }

    );
}