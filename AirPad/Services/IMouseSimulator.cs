using AirPad.Models;

namespace AirPad.Services;

public interface IMouseSimulator
{
    // التعامل مع حركة الماوس أو الإيماءات بناءً على عدد الأصابع
    void ProcessMovement(TouchPayload payload);

    // التعامل مع النقرات والأوامر الثابتة
    void ProcessCommand(MacroCommand command);
}