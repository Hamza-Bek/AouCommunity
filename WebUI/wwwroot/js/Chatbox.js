
function scrollToBottom(chatAreaId) {
    var chatArea = document.getElementById(chatAreaId);
    if (chatArea) {
        chatArea.scrollTop = chatArea.scrollHeight;
    }
}

function toggleSidebar() {
    const sidebar = document.getElementById('contact');
    sidebar.classList.toggle('open');
    const chat = document.getElementById("chat");
    chat.classList.toggle('hide');
}


