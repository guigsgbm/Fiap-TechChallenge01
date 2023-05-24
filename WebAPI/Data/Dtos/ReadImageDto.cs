namespace WebAPI.Data.Dtos;

public class ReadImageDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime AppointmentTime { get; set; } = DateTime.Now;
}
