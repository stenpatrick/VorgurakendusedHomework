namespace ITB2203Application.Model;

public class Attendee
{
    public int Id { get; set; }
    public int SpeakerId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime RegistrationTime { get; set; }
}
