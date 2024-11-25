namespace Un2Trek.Trekis.API.Controllers;

public class CreateActivityRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ValidFromDate { get; set; }
    public DateTime? ValidToDate { get; set; }
}
