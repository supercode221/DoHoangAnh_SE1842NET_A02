const MAX_NOTIFICATIONS = 10;
let notifications = [];

function loadNotifications() {
    const stored = localStorage.getItem('notifications');
    if (stored) {
        try {
            notifications = JSON.parse(stored);
            updateNotificationUI();
        } catch (e) {
            notifications = [];
        }
    }
}

function saveNotifications() {
    localStorage.setItem('notifications', JSON.stringify(notifications));
}

function addNotification(message) {
    const notification = {
        id: Date.now(),
        message: message,
        timestamp: new Date().toISOString(),
        read: false
    };

    notifications.unshift(notification);

    if (notifications.length > MAX_NOTIFICATIONS) {
        notifications = notifications.slice(0, MAX_NOTIFICATIONS);
    }

    saveNotifications();
    updateNotificationUI();
    showToast(message);
}

function updateNotificationUI() {
    const notificationList = document.getElementById('notificationList');
    const emptyState = document.getElementById('emptyNotifications');
    const notificationDot = document.getElementById('notificationDot');
    const notificationCount = document.getElementById('notificationCount');

    if (!notificationList) return;

    const unreadCount = notifications.filter(n => !n.read).length;

    if (unreadCount > 0) {
        notificationDot.classList.remove('hidden');
        notificationCount.classList.remove('hidden');
        notificationCount.textContent = unreadCount;
    } else {
        notificationDot.classList.add('hidden');
        notificationCount.classList.add('hidden');
    }

    if (notifications.length === 0) {
        emptyState.classList.remove('hidden');
        notificationList.innerHTML = '';
        return;
    }

    emptyState.classList.add('hidden');

    notificationList.innerHTML = notifications.map(notif => `
        <div class="notification-item p-4 border-b border-gray-100 hover:bg-gray-50 transition-colors ${notif.read ? 'bg-white' : 'bg-blue-50'}" data-id="${notif.id}">
            <div class="flex items-start space-x-3">
                <div class="flex-shrink-0">
                    ${notif.read ?
            '<div class="w-2 h-2 rounded-full bg-gray-300 mt-2"></div>' :
            '<div class="w-2 h-2 rounded-full bg-blue-500 mt-2"></div>'
        }
                </div>
                <div class="flex-1 min-w-0">
                    <p class="text-sm text-gray-800 break-words">${escapeHtml(notif.message)}</p>
                    <p class="text-xs text-gray-500 mt-1">${formatNotificationTime(notif.timestamp)}</p>
                </div>
                <button class="delete-notification flex-shrink-0 text-gray-400 hover:text-red-500 transition-colors" data-id="${notif.id}">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                    </svg>
                </button>
            </div>
        </div>
    `).join('');

    document.querySelectorAll('.notification-item').forEach(item => {
        item.addEventListener('click', function (e) {
            if (!e.target.closest('.delete-notification')) {
                const id = parseInt(this.dataset.id);
                markAsRead(id);
            }
        });
    });

    document.querySelectorAll('.delete-notification').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.stopPropagation();
            const id = parseInt(this.dataset.id);
            deleteNotification(id);
        });
    });
}

function markAsRead(id) {
    const notification = notifications.find(n => n.id === id);
    if (notification && !notification.read) {
        notification.read = true;
        saveNotifications();
        updateNotificationUI();
    }
}

function deleteNotification(id) {
    notifications = notifications.filter(n => n.id !== id);
    saveNotifications();
    updateNotificationUI();
}

function clearAllNotifications() {
    if (notifications.length === 0) return;

    if (confirm('Are you sure you want to clear all notifications?')) {
        notifications = [];
        saveNotifications();
        updateNotificationUI();
    }
}

function showToast(message) {
    const toast = document.createElement('div');
    toast.className = 'fixed top-20 right-5 bg-white border-l-4 border-purple-500 rounded-lg shadow-xl p-4 max-w-sm z-50 animate-slide-in';
    toast.innerHTML = `
        <div class="flex items-start space-x-3">
            <div class="flex-shrink-0">
                <svg class="w-6 h-6 text-purple-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
                </svg>
            </div>
            <div class="flex-1">
                <p class="text-sm font-medium text-gray-900">New Notification</p>
                <p class="text-sm text-gray-600 mt-1">${escapeHtml(message)}</p>
            </div>
        </div>
    `;

    document.body.appendChild(toast);

    setTimeout(() => {
        toast.classList.add('animate-slide-out');
        setTimeout(() => toast.remove(), 300);
    }, 3000);

    if (Notification.permission === "granted") {
        new Notification("New Notification", {
            body: message,
            icon: "/images/notification-icon.png"
        });
    }
}

function formatNotificationTime(timestamp) {
    const date = new Date(timestamp);
    const now = new Date();
    const diffMs = now - date;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins}m ago`;
    if (diffHours < 24) return `${diffHours}h ago`;
    if (diffDays < 7) return `${diffDays}d ago`;

    return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

const notificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7000/notificationHub", {
        withCredentials: true
    })
    .withAutomaticReconnect()
    .build();

notificationConnection.on("ReceiveNotification", (message) => {
    addNotification(message);
});

async function startNotificationConnection() {
    try {
        await notificationConnection.start();
        console.log("SignalR Connected for notifications");
    } catch (err) {
        console.error("SignalR Connection Error: ", err);
        setTimeout(startNotificationConnection, 5000);
    }
}

notificationConnection.onreconnecting((error) => {
    console.log("Reconnecting...", error);
});

notificationConnection.onreconnected((connectionId) => {
    console.log("Reconnected!", connectionId);
});

notificationConnection.onclose((error) => {
    console.log("Connection closed", error);
    startNotificationConnection();
});

document.addEventListener('DOMContentLoaded', () => {
    if (Notification.permission === "default") {
        Notification.requestPermission();
    }

    loadNotifications();

    startNotificationConnection();

    const notificationBtn = document.getElementById('notificationBtn');
    const notificationDropdown = document.getElementById('notificationDropdown');

    if (notificationBtn && notificationDropdown) {
        notificationBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            notificationDropdown.classList.toggle('hidden');
        });

        document.addEventListener('click', (e) => {
            if (!notificationDropdown.contains(e.target) && !notificationBtn.contains(e.target)) {
                notificationDropdown.classList.add('hidden');
            }
        });
    }

    const clearAllBtn = document.getElementById('clearAllNotifications');
    if (clearAllBtn) {
        clearAllBtn.addEventListener('click', clearAllNotifications);
    }
});