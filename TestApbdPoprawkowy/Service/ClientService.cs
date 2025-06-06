using TestApbdPoprawkowy.Dto;
using TestApbdPoprawkowy.Models;
using TestApbdPoprawkowy.Repository;

namespace TestApbdPoprawkowy.Service;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }
    public async Task<ClientRentalDto?> GetClientWithRentalsAsync(int id)
    {
        return await _repository.FetchClientWithRentals(id);
    }

    public async Task<bool> CreateClientWithRentalAsync(CreateClientWithRentalRequestDto request)
    {
        var car = await _repository.GetCarById(request.CarId);
        if (car == null) return false;
        var days = (request.DateTo - request.DateFrom).Days;
        var totalPrice = days * car.PricePerDay;

        var client = new Clients
        {
            FirstName = request.Client.FirstName,
            LastName = request.Client.LastName,
            Address = request.Client.Address
        };
        var rental = new CarRentals
        {
            CarId = request.CarId,
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            TotalPrice = totalPrice
        };
        await _repository.AddClientWithRental(client, rental);
        return true;
    }
}