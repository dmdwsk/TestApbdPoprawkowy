using TestApbdPoprawkowy.Dto;
using TestApbdPoprawkowy.Models;

namespace TestApbdPoprawkowy.Repository;

public interface IClientRepository
{
    Task<ClientRentalDto?> FetchClientWithRentals(int clientId);
    Task<Cars?> GetCarById(int carId);
    Task AddClientWithRental(Clients client, CarRentals rental);
}