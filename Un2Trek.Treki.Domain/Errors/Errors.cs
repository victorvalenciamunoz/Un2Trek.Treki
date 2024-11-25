using ErrorOr;

namespace Un2Trek.Trekis.Domain;

public class Errors
{
    public static readonly Error InvalidDistance = Error.Validation(code: "T002", description: "Invalid distance to capture treki");
    public static readonly Error TrekiAlreadyCaptured = Error.Validation(code: "T004", description: "Treki already capture by user");
}
