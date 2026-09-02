// 1. إعداد SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/airpadHub")
    .withAutomaticReconnect()
    .build();

const statusDiv = document.getElementById('status');

connection.start()
    .then(() => statusDiv.innerText = "✅ متصل بالخادم")
    .catch(err => statusDiv.innerText = "❌ فشل الاتصال");

// 2. التقاط الإحداثيات وحساب الـ Delta
let lastX = 0;
let lastY = 0;
const touchpad = document.getElementById('touchpad');

touchpad.addEventListener('touchstart', (e) => {
    // التقاط موقع الإصبع الأول عند بداية اللمس
    lastX = e.touches[0].clientX;
    lastY = e.touches[0].clientY;
});

touchpad.addEventListener('touchmove', (e) => {
    // حساب الموقع الجديد
    const currentX = e.touches[0].clientX;
    const currentY = e.touches[0].clientY;
    const fingerCount = e.touches.length;

    // حساب فرق المسافة (Delta)
    const deltaX = currentX - lastX;
    const deltaY = currentY - lastY;

    // إرسال البيانات إلى السيرفر (فقط إذا كان متصلاً)
    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendMovement", {
            DeltaX: deltaX,
            DeltaY: deltaY,
            FingerCount: fingerCount
        }).catch(err => console.error(err));
    }

    // تحديث الموقع السابق ليكون هو الموقع الحالي للحركة القادمة
    lastX = currentX;
    lastY = currentY;
});

// 3. دالة لاختبار إرسال أمر النقرة (MacroCommand)
function testClick() {
    if (connection.state === signalR.HubConnectionState.Connected) {
        // Command: 0 يعادل LeftClick في الـ Enum الذي أنشأناه
        connection.invoke("SendCommand", { Command: 0 })
            .catch(err => console.error(err));
    }
}