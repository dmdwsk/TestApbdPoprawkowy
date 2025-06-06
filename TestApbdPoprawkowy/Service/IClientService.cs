using TestApbdPoprawkowy.Dto;

namespace TestApbdPoprawkowy.Service;

public interface IClientService
{
    Task<ClientRentalDto?> GetClientWithRentalsAsync(int id);
    Task<bool> CreateClientWithRentalAsync(CreateClientWithRentalRequestDto request);
}