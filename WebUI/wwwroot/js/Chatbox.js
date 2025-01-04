
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

sendButton.addEventListener('click', () => {
    const message = messageInput.value.trim();
    if (message) {
        // Add the message to the chat display
        const messageElement = document.createElement('div');
        messageElement.textContent = message;
        chatDisplay.appendChild(messageElement);

        // Clear the input field
        messageInput.value = '';

        // Keep the focus on the input field
        messageInput.focus();
    }
});

// Optional: Send message when pressing Enter
messageInput.addEventListener('keypress', (event) => {
    if (event.key === 'Enter') {
        sendButton.click();
    }
});
