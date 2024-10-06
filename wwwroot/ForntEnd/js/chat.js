// thông tin user và employee chat 
var userRole = document.getElementById("userRole").value;
var userId = document.getElementById("userId") ? parseInt(document.getElementById("userId").value) : null;
var employeeId = document.getElementById("employeeId") ? parseInt(document.getElementById("employeeId").value) : null;

function formatTime(dateString) {
    const date = new Date(dateString);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');
    return `${hours}:${minutes}:${seconds}`; // Returns "HH:mm:ss"
}
console.log("Role:", userRole, " - Id: ", userId, " - Emp: ", employeeId)


// kết nối SignalR
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
// Listen for user status changes
connection.on("UserStatusChanged", function (userId, isOnline) {
    const statusIcon = isOnline ? "fa-circle online" : "fa-circle offline";
    const onlineStatus = isOnline ? "Online" : "Offline";

    // Find the user element in the list and update the status
    const userElement = $(`#user-${userId} .status`);
    if (userElement.length > 0) {
        userElement.html(`<i class="fa ${statusIcon}"></i> ${onlineStatus}`);
    } else {
        console.log(`User element with ID user-${userId} not found`);
    }
});
// Đăng ký sự kiện nhận tin nhắn
connection.on("ReceiveMessage", (senderId, content, dateSend) => {
    console.log(`Message received from ${senderId}: ${content}`);

    const messageElement = document.createElement("div");
    messageElement.classList.add("message");

    let isSender = false;

    if (userRole === "user") {
        isSender = (parseInt(senderId) === userId);
    } else { // employee
        isSender = (parseInt(senderId) === employeeId);
    }

   
    if (isSender) {
        messageElement.style.background = '#83acc5';
        messageElement.style.color = 'black';
        messageElement.style.alignSelf = 'flex-end';
    } else {
        messageElement.style.background = '#ededed';
        messageElement.style.color = '#212121';
        messageElement.style.alignSelf = 'flex-start';
    }                                                           
    messageElement.innerHTML = `${content} <br/><small>${formatTime(dateSend)}</small>`;
    if (userRole === "user") {
        document.getElementById("messages").appendChild(messageElement);
        lastmessage("chat-box"); 
    } else {
        document.getElementById("employee-messages").appendChild(messageElement);
        lastmessage("employee-chat-box"); 
    }
    
});

connection.start().then(() => {
    console.log("SignalR connected.");
}).catch(err => {
    console.error("SignalR connection error: ", err.toString());
});
function lastmessage(divId) { var objDiv = document.getElementById(divId); objDiv.scrollTop = objDiv.scrollHeight; }
document.getElementById('chatButton').addEventListener('click', function () {
    if (userRole === "user") {
        const chatSection = document.getElementById("chat-section");
        const messages = document.getElementById("messages");
        //show chatbox for USER
        chatSection.style.display = "block";
        loadMessages();

        //Đóng Chatbox
        document.getElementById("closeChat").addEventListener("click", function () {
            document.getElementById("chat-section").style.display = "none";
        });

        //Handle Send message for user to employee
        document.getElementById("Send").addEventListener("click", event => {
            const senderId = userId;
            const receiverId = employeeId;
            const content = document.getElementById("messageInput").value;
            if (content.trim() === "") return;
            //Gọi hàm send message
            connection.invoke("SendMessage", receiverId, content).catch(err => {
                console.error("Error while sending message: ", err.toString());
            });

            const messageElement = document.createElement("div");
            messageElement.classList.add("message");
            messageElement.style.background = '#83acc5';
            messageElement.style.color = 'black';
            messageElement.style.alignSelf = 'flex-end';
            messageElement.style.textAlign = 'left';
            messageElement.style.padding = '5px 10px';
            messageElement.style.borderRadius = '8px';
            messageElement.style.marginBottom = '10px';
            messageElement.innerHTML = `${content}<br/><small>${formatTime(new Date())}</small>`;
            messages.appendChild(messageElement);
            lastmessage("chat-box")
            document.getElementById("messageInput").value = "";
            event.preventDefault();

        });

        // get messages for user API
        function loadMessages() {
            $.ajax({
                url: `/api/messages/${userId}`, // Thay thế bằng URL API của bạn
                type: 'GET',
                success: function (messages) {
                    $('#messages').empty();
                    messages.sort((a, b) => new Date(a.dateSend) - new Date(b.dateSend));
                    messages.forEach(message => {
                        const messageElement = document.createElement("div");
                        messageElement.classList.add("message");

                        const currentUserId = userId;
                        if (parseInt(message.senderId) === currentUserId) {
                            messageElement.style.background = '#83acc5';
                            messageElement.style.color = 'black';
                            messageElement.style.alignSelf = 'flex-end';
                            messageElement.style.textAlign = 'left';
                            messageElement.style.padding = '10px';
                            messageElement.style.borderRadius = '8px';
                            messageElement.style.marginBottom = '10px';
                        } else {
                            messageElement.style.background = '#ededed';
                            messageElement.style.color = 'black';
                            messageElement.style.alignSelf = 'flex-start';
                            messageElement.style.textAlign = 'left';
                            messageElement.style.padding = '10px';
                            messageElement.style.borderRadius = '8px';
                            messageElement.style.marginBottom = '10px';

                        }

                        messageElement.style.padding = '5px';
                        messageElement.style.borderRadius = '8px';
                        messageElement.style.marginBottom = '10px';
                        messageElement.innerHTML = `${message.content}<br/><small class="dateSend">${formatTime(new Date(message.dateSend))}</small>`;
                        document.getElementById("messages").appendChild(messageElement);
                    });
                    lastmessage("chat-box")
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching messages:', error);
                }
            });
            console.log("load messages for user success");
        }

    } else if (userRole == "employeeChat" || userRole == "admin") {
        document.getElementById("employee-message-input").style.display = "none";
        const chatSectionEmp = document.getElementById("wrapper-chat-emp");
        const messagesEmp = document.getElementById("employee-messages");
        //show chatbox for EMP
        chatSectionEmp.style.display = "block";
        loadUsers();
        const senderId = employeeId;
        let receiverId = '';
        document.getElementById("closeChatEmp").addEventListener("click", function () {
            chatSectionEmp.style.display = "none";
        });

        // Xử lý gửi tin nhắn
        document.getElementById("EmpSend").addEventListener("click", event => {
            const content = document.getElementById("employeeMessageInput").value;
            const messageElement = document.createElement("div");
            messageElement.classList.add("message");
            messageElement.style.background = '#83acc5';
            messageElement.style.color = 'black';
            messageElement.style.alignSelf = 'flex-end';
            messageElement.style.padding = '5px 10px';
            messageElement.style.borderRadius = '8px';
            messageElement.style.marginBottom = '10px';
            messageElement.innerHTML = `${content}<br/><small>${formatTime(new Date())}</small>`;
            messagesEmp.appendChild(messageElement);
            lastmessage("employee-chat-box")
            console.log("Sending message from", senderId, "to", receiverId, "with content:", content);
            // Gửi tin nhắn qua SignalR
            connection.invoke("SendMessage", receiverId, content).catch(err => {
                console.error("Error while sending message: ", err.toString());
            });
            document.getElementById("employeeMessageInput").value = "";
            event.preventDefault();
        });


        // Tải danh sách user
        function loadUsers() {
            $.ajax({
                url: `/api/users`,
                type: 'GET',
                success: function (users) {
                    console.log("Users fetched:", users);
                    $('#user-list').empty();

                    users.forEach(user => {
                        const onlineStatus = user.isOnline ? 'online' : 'offline';
                        const statusIcon = user.isOnline ? 'fa-circle online' : 'fa-circle offline';
                        const userElement = `
                    <li id="user-${user.id}" class="clearfix" style="list-style-type: none;">
                        <img style="width: 45px; border-radius: 50%; padding: 5px;" src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="avatar">
                        <div class="about">
                            <div class="name">${user.username}</div>
                            <div class="status"> <i class="fa ${statusIcon}"></i> ${onlineStatus} </div>
                        </div>
                    </li>
                `;
                        const $userElement = $(userElement).on("click", function () {
                            selectUserForChat(user.id, user.username);
                            receiverId = user.id; // Update receiverId when a user is selected
                        });
                        $('#user-list').append($userElement);
                    });
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching users:', error);
                }
            });
        }



        // Chọn user để chat và tải tin nhắn
        function selectUserForChat(userId, username) {
            document.getElementById("employeeChatWith").textContent = `Chat với ${username}`;
            document.getElementById("employee-message-input").style.display = "flex";
            loadMessagesEmpWithUser(senderId, userId); // Ensure senderId is defined in scope
        }

        // Hàm để tải tin nhắn giữa employee và user
        function loadMessagesEmpWithUser(employeeId, userId) {
            $.ajax({
                url: `/api/messages/${employeeId}/${userId}`,
                type: 'GET',
                success: function (messages) {
                    console.log('Messages received:', messages);
                    $('#employee-messages').empty();
                    messages.sort((a, b) => new Date(a.dateSend) - new Date(b.dateSend));

                    messages.forEach(message => {
                        const messageElement = document.createElement("div");
                        messageElement.classList.add("message");

                        // Set common styles
                        messageElement.style.padding = '5px 8px';
                        messageElement.style.borderRadius = '8px';
                        messageElement.style.marginBottom = '10px';

                        if (parseInt(message.senderId) === employeeId) {
                            // Tin nhắn của nhân viên
                            messageElement.style.background = '#83acc5';
                            messageElement.style.color = 'black';
                            messageElement.style.alignSelf = 'flex-end';
                            messageElement.style.textAlign = 'left';
                        } else {
                            // Tin nhắn của người dùng
                            messageElement.style.background = '#ededed';
                            messageElement.style.color = 'black';
                            messageElement.style.alignSelf = 'flex-start';
                            messageElement.style.textAlign = 'left';
                        }

                        messageElement.innerHTML = `${message.content}<br/><small class="dateSend">${formatTime(message.dateSend)}</small>`;
                        document.getElementById("employee-messages").appendChild(messageElement);
                    });

                    // Call the last message function if defined
                    if (typeof lastmessage === 'function') {
                        lastmessage("employee-chat-box");
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching messages:', error);
                    alert('Could not load messages. Please try again later.'); // Notify user about the error
                }
            });
        }
    }
    else {
        if (confirm("Please log in to continue.")) {
            window.location.href = "/login";
        }
    }

});
