using AirPad.Models;
using System.Runtime.InteropServices;

namespace AirPad.Services;

public class WindowsSimulator : IMouseSimulator
{
    // ----------------------------------------------------
    // استيراد دوال ويندوز الأساسية من مكتبة user32.dll
    // ----------------------------------------------------

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    // هيكل بيانات موقع الماوس
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    // ثوابت أوامر الماوس في ويندوز
    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const int MOUSEEVENTF_RIGHTUP = 0x10;
    private const int MOUSEEVENTF_WHEEL = 0x0800; // عجلة التمرير (Scroll)
    private const int MOUSEEVENTF_HWHEEL = 0x1000; // التمرير الأفقي (أضف هذا)

    // معامل سرعة الماوس (Sensitivity) - يمكنك زيادته أو تقليله
    private readonly double _sensitivity = 1.5;

    // ----------------------------------------------------
    // تنفيذ الواجهة
    // ----------------------------------------------------

    public void ProcessMovement(TouchPayload payload)
    {
        if (payload.FingerCount == 1)
        {
            // تحريك المؤشر (إصبع واحد)
            if (GetCursorPos(out POINT currentPos))
            {
                // ضرب فرق المسافة في معامل السرعة لتسريع الحركة
                int newX = currentPos.X + (int)(payload.DeltaX * _sensitivity);
                int newY = currentPos.Y + (int)(payload.DeltaY * _sensitivity);

                SetCursorPos(newX, newY);
            }
        }
        else if (payload.FingerCount == 2)
        {
            if (Math.Abs(payload.DeltaY) > 0.5) // تجاهل الحركات الطفيفة جداً
            {
                int scrollY = (int)(payload.DeltaY * 2);
                mouse_event(MOUSEEVENTF_WHEEL, 0, 0, scrollY, 0);
            }

            // التمرير الأفقي (Horizontal Scroll)
            if (Math.Abs(payload.DeltaX) > 0.5)
            {
                int scrollX = (int)(-payload.DeltaX * 2);
                mouse_event(MOUSEEVENTF_HWHEEL, 0, 0, scrollX, 0);
            }
        }
    }

    public void ProcessCommand(MacroCommand command)
    {
        switch (command.Command)
        {
            case CommandType.LeftClick:
                // النقرة تتكون من ضغط الزر ثم إفلاته
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                break;

            case CommandType.RightClick:
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                break;

            case CommandType.LeftMouseDown:
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                break;

            case CommandType.LeftMouseUp:
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                break;

                // يمكنك لاحقاً إضافة باقي الأوامر هنا
        }
    }
}