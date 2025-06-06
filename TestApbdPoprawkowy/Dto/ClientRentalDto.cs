namespace TestApbdPoprawkowy.Dto;

public class ClientRentalDto
{
    public int Id {get;set;}
    public String FirstName {get;set;}
    public String LastName {get;set;}
    public String adress {get;set;}
    public List<RentalInfoDto> Rentals { get; set; }
}
public class RentalInfoDto
{
    public string Vin { get; set; }
    public string Color { get; set; }
    public string Model { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int TotalPrice { get; set; }
}