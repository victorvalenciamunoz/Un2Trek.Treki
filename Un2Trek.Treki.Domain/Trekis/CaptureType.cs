using Ardalis.SmartEnum;

namespace Un2Trek.Trekis.Domain;

public class CaptureType : SmartEnum<CaptureType>
{
    public static readonly CaptureType Direct = new(nameof(Direct), 0);
    public static readonly CaptureType Qr = new(nameof(Qr), 1);

    protected CaptureType(string name, int value) : base(name, value)
    {

    }
}
