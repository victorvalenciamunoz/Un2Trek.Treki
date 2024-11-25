namespace Un2Trek.Trekis.API.Controllers;

public class ActivityResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime ValidFromDate { get; set; }

    public DateTime? ValidToDate
    {
        get; set;
    }
}
