using Ardalis.GuardClauses;

namespace Un2Trek.Trekis.Domain.ValueObjects;

public class Location : ValueObject
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    private Location() { }
    public Location(double latitude, double longitude)
    {
        Latitude = Guard.Against.OutOfRange(latitude, nameof(latitude), -90, 90);
        Longitude = Guard.Against.OutOfRange(longitude, nameof(longitude), -180, 180);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Latitude;
        yield return Longitude;
    }
}