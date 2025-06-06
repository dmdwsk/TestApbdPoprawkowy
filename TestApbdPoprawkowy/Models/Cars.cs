namespace TestApbdPoprawkowy.Models;

public class Cars
{
    public int Id { get; set; }
    public string VIN { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Seats { get; set; }
    public int PricePerDay { get; set; }
    public int ModelId { get; set; }
    public int ColorId { get; set; }
}