
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
    const navbar = document.getElementById('navbar');
    navbar.classList.toggle('open');
}


const messageInput = document.getElementById('messageInput');
const sendButton = document.getElementById('sendButton');
const chatDisplay = document.getElementById('chatDisplay');

function showPage(page) {
    // Hide all pages
    document.getElementById('myContactsPage').style.display = 'none';
    document.getElementById('newContactsPage').style.display = 'none';
    document.getElementById('notification').style.display = 'none';
    

    // Show the selected page
    if (page === 'myContacts') {
        document.getElementById('myContactsPage').style.display = 'block';
    } else if (page === 'newContact') {
        document.getElementById('newContactsPage').style.display = 'block';
    } else if (page === 'NotificationsPage') {
        document.getElementById('notification').style.display = 'block';
    }
}

// Optionally: Set default page
document.addEventListener('DOMContentLoaded', () => {
    showPage('myContacts');
});
