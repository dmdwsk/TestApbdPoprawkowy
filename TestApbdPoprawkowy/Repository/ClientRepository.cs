using TestApbdPoprawkowy.Dto;
using TestApbdPoprawkowy.Models;
using Microsoft.Data.SqlClient;
namespace TestApbdPoprawkowy.Repository;

public class ClientRepository : IClientRepository
{
    private readonly SqlConnection _conn;

    public ClientRepository(IConfiguration config)
    {
        _conn = new SqlConnection(config.GetConnectionString("Default"));
    }

    public async Task<ClientRentalDto?> FetchClientWithRentals(int clientId)
    {
        const string sql = @"
        SELECT c.ID, c.FirstName, c.LastName, c.Address,
               ca.VIN, co.Name AS Color, m.Name AS Model,
               cr.DateFrom, cr.DateTo, cr.TotalPrice
        FROM clients c
        LEFT JOIN car_rentals cr ON c.ID = cr.ClientID
        LEFT JOIN cars ca ON cr.CarID = ca.ID
        LEFT JOIN colors co ON ca.ColorID = co.ID
        LEFT JOIN models m ON ca.ModelID = m.ID
        WHERE c.ID = @id";

        var cmd = new SqlCommand(sql, _conn);
        cmd.Parameters.AddWithValue("@id", clientId);

        await _conn.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();

        ClientRentalDto? client = null;
        var rentals = new List<RentalInfoDto>();

        while (await reader.ReadAsync())
        {
            if (client == null)
            {
                client = new ClientRentalDto
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    adress = reader.GetString(3),
                    Rentals = rentals
                };
            }

            if (!reader.IsDBNull(4))
            {
                rentals.Add(new RentalInfoDto
                {
                    Vin = reader.GetString(4),
                    Color = reader.GetString(5),
                    Model = reader.GetString(6),
                    DateFrom = reader.GetDateTime(7),
                    DateTo = reader.GetDateTime(8),
                    TotalPrice = reader.GetInt32(9)
                });
            }
        }

        await _conn.CloseAsync();
        return client;
    }

    public async Task<Cars?> GetCarById(int carId)
    {
        const string sql = "SELECT * FROM cars WHERE ID = @id";
        var cmd = new SqlCommand(sql, _conn);
        cmd.Parameters.AddWithValue("@id", carId);

        await _conn.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();

        Cars? car = null;
        if (await reader.ReadAsync())
        {
            car = new Cars
            {
                Id = reader.GetInt32(0),
                VIN = reader.GetString(1),
                Name = reader.GetString(2),
                Seats = reader.GetInt32(3),
                PricePerDay = reader.GetInt32(4),
                ModelId = reader.GetInt32(5),
                ColorId = reader.GetInt32(6)
            };
        }

        await _conn.CloseAsync();
        return car;
    }

    public async Task AddClientWithRental(Clients client, CarRentals rental)
    {
        const string insertClient = @"
            INSERT INTO clients (FirstName, LastName, Address)
            OUTPUT INSERTED.ID
            VALUES (@fn, @ln, @addr)";
        var cmd = new SqlCommand(insertClient, _conn);
        cmd.Parameters.AddWithValue("@fn", client.FirstName);
        cmd.Parameters.AddWithValue("@ln", client.LastName);
        cmd.Parameters.AddWithValue("@addr", client.Address);

        await _conn.OpenAsync();
        var clientId = (int)await cmd.ExecuteScalarAsync();

        const string insertRental = @"
            INSERT INTO car_rentals (ClientID, CarID, DateFrom, DateTo, TotalPrice)
            VALUES (@cid, @carid, @from, @to, @price)";
        var rentalCmd = new SqlCommand(insertRental, _conn);
        rentalCmd.Parameters.AddWithValue("@cid", clientId);
        rentalCmd.Parameters.AddWithValue("@carid", rental.CarId);
        rentalCmd.Parameters.AddWithValue("@from", rental.DateFrom);
        rentalCmd.Parameters.AddWithValue("@to", rental.DateTo);
        rentalCmd.Parameters.AddWithValue("@price", rental.TotalPrice);

        await rentalCmd.ExecuteNonQueryAsync();
        await _conn.CloseAsync();
    }
}
