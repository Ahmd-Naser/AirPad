namespace AirPad.Models;

public class TouchPayload
{
    public double DeltaX { get; set; }
    public double DeltaY { get; set; }

    // عدد الأصابع (1 للحركة، 2 للتمرير، 3 و 4 للإيماءات المتقدمة)
    public int FingerCount { get; set; }
}
