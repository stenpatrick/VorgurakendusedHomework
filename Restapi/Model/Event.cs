namespace ITB2203Application.Model;

public class Event
{
    public int Id { get; set; }
    public int SpeakerId { get; set; }
    public required string Name { get; set; }
    public DateTime Date { get; set; }
    public required string Address { get; set; }
}
