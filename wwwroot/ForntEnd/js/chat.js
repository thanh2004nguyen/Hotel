    // thông tin user và employee chat 
    var userRole = document.getElementById("userRole").value;
    var userId = document.getElementById("userId") ? parseInt(document.getElementById("userId").value) : null;
    var employeeId = document.getElementById("employeeId") ? parseInt(document.getElementById("employeeId").value) : null;
    console.log("Role:", userRole," - Id: ", userId, " - Emp: ", employeeId)


    // kết nối SignalR
    const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


    /*connection.on("ReceiveMessage", (senderId, content, dateSend) => {
        console.log(`Message received from ${senderId}: ${content}`);
        const messageElement = document.createElement("div");
        messageElement.classList.add("message");
        const currentUserId = userId || employeeId;

        if (parseInt(senderId) === currentUserId) {
            messageElement.style.background = '#83acc5';
            messageElement.style.color = 'black';
            messageElement.style.textAlign = 'right';
        } else {
            messageElement.style.background = '#ededed';
            messageElement.style.color = '#212121';
            messageElement.style.textAlign = 'left';
        }
        messageElement.innerHTML = `${content}<br/><small>${new Date(dateSend).toLocaleTimeString()}</small>`;

        if (userRole === "user") {
            document.getElementById("messages").appendChild(messageElement);
        } else {
            document.getElementById("employee-messages").appendChild(messageElement);
        }
    });*/
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
            messageElement.style.textAlign = 'right';
        } else {
            messageElement.style.background = '#ededed';
            messageElement.style.color = '#212121';
            messageElement.style.textAlign = 'left';
        }

        messageElement.innerHTML = `${content} <br/><small>${new Date(dateSend).toLocaleTimeString()}</small>`;

        if (userRole === "user") {
            document.getElementById("messages").appendChild(messageElement);
        } else {
            document.getElementById("employee-messages").appendChild(messageElement);
        }

        // Cuộn xuống cuối chat box
        const messagesDiv = userRole === "user" ? document.getElementById("messages") : document.getElementById("employee-messages");
        messagesDiv.scrollTop = messagesDiv.scrollHeight;
    });

    connection.start().then(() => {
        console.log("SignalR connected.");
    }).catch(err => {
        console.error("SignalR connection error: ", err.toString());
    });
    if (userRole === "user") {
        loadMessages();

        document.getElementById("chatButton").addEventListener("click", function () {
            document.getElementById("chat-section").style.display = "block";
        });
        document.getElementById("closeChat").addEventListener("click", function () {
            document.getElementById("chat-section").style.display = "none";
        });
    
        document.getElementById("Send").addEventListener("click", event => {
            const senderId = userId;
            const receiverId = employeeId;
            const content = document.getElementById("messageInput").value;

            if (content.trim() === "") return;

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
            messageElement.innerHTML = `${content}<br/><small>${new Date().toLocaleTimeString()}</small>`;

            document.getElementById("messages").appendChild(messageElement);
            document.getElementById("messageInput").value = "";
            event.preventDefault();
        });

        // Hàm để lấy tin nhắn từ API 
        function loadMessages() {
            $.ajax({
                url: `/api/messages/${userId}`, // Thay thế bằng URL API của bạn
                type: 'GET',
                success: function (messages) {
                    console.log('Messages received:', messages);
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

                        messageElement.innerHTML = `${message.content}<br/> <small class="dateSend">${new Date(message.dateSend).toLocaleTimeString()}</small>`;
                        document.getElementById("messages").appendChild(messageElement);
                    });
                
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching messages:', error);
                }
            });
        }

    } else {
        loadUsers();
        const senderId = employeeId;  // senderId là employeeId
        let receiverId = ''; // Khai báo receiverId là rỗng ban đầu

        console.log("senderId:", senderId, " receiverId:", receiverId);

        document.getElementById("chatButton").addEventListener("click", function () {
            document.getElementById("wrapper-chat-emp").style.display = "block";
        });

        document.getElementById("closeChatEmp").addEventListener("click", function () {
            document.getElementById("wrapper-chat-emp").style.display = "none";
        });

        // Xử lý gửi tin nhắn
        document.getElementById("EmpSend").addEventListener("click", event => {
            const content = document.getElementById("employeeMessageInput").value;

            if (!receiverId) {
                console.error("ReceiverId is not selected!");
                alert("Please select a user to chat with.");
                return;
            }
            const messageElement = document.createElement("div");
                messageElement.classList.add("message");
                messageElement.style.background = '#83acc5 none repeat scroll 0 0';
                messageElement.style.color = 'black';
                messageElement.style.right = '-109px';
                messageElement.style.padding = '5px';
                messageElement.style.borderRadius = '8px';
                messageElement.style.marginBottom = '10px';
                messageElement.innerHTML = `${content}<br/><small class="dateSend">${new Date().toLocaleTimeString()}</small>`;
                document.getElementById("employee-messages").appendChild(messageElement);
                console.log("Sending message from", senderId, "to", receiverId, "with content:", content);
            // Gửi tin nhắn qua SignalR
            connection.invoke("SendMessage", receiverId, content).catch(err => {
                console.error("Error while sending message: ", err.toString());
            });

            document.getElementById("employeeMessageInput").value = ""; // Xóa nội dung sau khi gửi
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
                        const userElement = `
                                <li class="clearfix" style="list-style-type: none;">
                                    <img style="width: 45px;border-radius: 50%;padding: 5px;" src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="avatar">
                                    <div class="about">
                                        <div class="name">${user.username}</div>
                                        <div class="status"> <i class="fa fa-circle offline"></i> online </div>
                                    </div>
                                </li>
                        `;
                        const $userElement = $(userElement).on("click", function () {
                            selectUserForChat(user.id, user.username);
                            receiverId = user.id; // Cập nhật receiverId khi chọn user
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
            loadMessagesEmpWithUser(senderId, userId); 
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

                        if (parseInt(message.senderId) === employeeId) {
                            // Tin nhắn của nhân viên
                            messageElement.style.background = '#83acc5 none repeat scroll 0 0';
                            messageElement.style.color = 'black';
                            messageElement.style.alignSelf = 'flex-end';
                            messageElement.style.textAlign = 'left';
                            messageElement.style.padding = '10px';
                            messageElement.style.borderRadius = '8px';
                            messageElement.style.marginBottom = '10px'; 
                        } else {
                            // Tin nhắn của người dùng
                            messageElement.style.background = '#ededed none repeat scroll 0 0';
                            messageElement.style.color = 'black';
                            messageElement.style.alignSelf = 'flex-start';
                            messageElement.style.textAlign = 'left';
                            messageElement.style.padding = '10px';
                            messageElement.style.borderRadius = '8px';
                            messageElement.style.marginBottom = '10px'; 
                        }

                        messageElement.style.padding = '5px 8px';
                        messageElement.style.borderRadius = '8px';
                        messageElement.style.marginBottom = '10px';
                        messageElement.innerHTML = `${message.content}<br/><small class="dateSend">${new Date(message.dateSend).toLocaleTimeString()}</small>`;

                        document.getElementById("employee-messages").appendChild(messageElement);
                    });
                
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching messages:', error);
                }
            });
        }
    }






