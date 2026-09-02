// 1. إعداد SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/airpadHub")
    .withAutomaticReconnect()
    .build();

const statusDiv = document.getElementById('status');

connection.start()
    .then(() => statusDiv.innerText = "✅ متصل بالخادم")
    .catch(err => statusDiv.innerText = "❌ فشل الاتصال");

// 2. التقاط الإحداثيات والإيماءات الذكية
let lastX = 0, lastY = 0;
let touchStartTime = 0;
let hasMoved = false;
let maxFingers = 0;
let lastTapTime = 0;
let isDragging = false;

const touchpad = document.getElementById('touchpad');

touchpad.addEventListener('touchstart', (e) => {
    lastX = e.touches[0].clientX;
    lastY = e.touches[0].clientY;
    touchStartTime = new Date().getTime();
    hasMoved = false;
    maxFingers = e.touches.length;

    // اكتشاف النقر المزدوج للسحب (Double Tap & Hold)
    if (maxFingers === 1) {
        let timeSinceLastTap = touchStartTime - lastTapTime;
        if (timeSinceLastTap < 300) {
            isDragging = true;
            sendCommand(2); // CommandType.LeftMouseDown
        }
    }
});

touchpad.addEventListener('touchmove', (e) => {
    // تحديث أقصى عدد للأصابع على الشاشة
    if (e.touches.length > maxFingers) {
        maxFingers = e.touches.length;
    }

    const currentX = e.touches[0].clientX;
    const currentY = e.touches[0].clientY;

    const deltaX = currentX - lastX;
    const deltaY = currentY - lastY;

    // إذا تحرك الإصبع أكثر من 3 بيكسل، نعتبرها حركة وليست نقرة
    if (Math.abs(deltaX) > 3 || Math.abs(deltaY) > 3) {
        hasMoved = true;
    }

    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendMovement", {
            DeltaX: deltaX,
            DeltaY: deltaY,
            FingerCount: maxFingers // نرسل عدد الأصابع (1 للحركة، 2 للـ Scroll)
        }).catch(err => console.error(err));
    }

    lastX = currentX;
    lastY = currentY;
});

touchpad.addEventListener('touchend', (e) => {
    const touchDuration = new Date().getTime() - touchStartTime;

    // إنهاء السحب (Drag & Drop)
    if (isDragging && e.touches.length === 0) {
        isDragging = false;
        sendCommand(3); // CommandType.LeftMouseUp
        return;
    }

    // اكتشاف النقرات (إذا لم يتحرك الإصبع وكانت المدة قصيرة)
    if (!hasMoved && touchDuration < 250) {
        if (maxFingers === 1) {
            sendCommand(0); // LeftClick
            lastTapTime = new Date().getTime();
        } else if (maxFingers === 2) {
            sendCommand(1); // RightClick
        }
    }
});

// دالة مساعدة لإرسال الأوامر
function sendCommand(commandId) {
    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendCommand", { Command: commandId })
            .catch(err => console.error(err));
    }
}

// 3. دالة لاختبار إرسال أمر النقرة (MacroCommand)
function testClick() {
    if (connection.state === signalR.HubConnectionState.Connected) {
        // Command: 0 يعادل LeftClick في الـ Enum الذي أنشأناه
        connection.invoke("SendCommand", { Command: 0 })
            .catch(err => console.error(err));
    }
}