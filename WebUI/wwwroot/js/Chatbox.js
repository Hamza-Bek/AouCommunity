
toggleBtn = document.getElementById('toggleBtn');
SideMenu = document.getElementById('SideMenu');

toggleBtn.addEventListener('click', () => {
    SideMenu.classList.toggle('active');
});

function toggleSidebar() {
    const sidebar = document.getElementById('contact');
    sidebar.classList.toggle('open');
    const chat = document.getElementById("chat");
    chat.classList.toggle('hide');
}