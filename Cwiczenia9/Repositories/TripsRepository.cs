using Cwiczenia9.Models;
using Cwiczenia9.ModelsDtos;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia9.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly MasterContext _context = new();

    public async Task<IEnumerable<TripsDto>> GetTripsDto
    (bool paging,
        CancellationToken cancellationToken,
        int pageNum = 1,
        int pageSize = 10)
    {
        var query = _context
            .Trips
            .Include(trip => trip.ClientTrips)
            .ThenInclude(clientTrip => clientTrip.IdClientNavigation)
            .Include(trip => trip.IdCountries)
            .OrderByDescending(trip => trip.DateFrom)
            .AsQueryable();

        var tripsCount = await query.CountAsync(cancellationToken);
        var allPages = (int)Math.Ceiling(tripsCount / (double)pageSize);

        if (paging)
        {
            query = query.Skip((pageNum - 1) * pageSize).Take(pageSize);
        }

        var trips = await query.Select(trip => new TripsDto
        {
            PageNum = pageNum,
            PageSize = pageSize,
            AllPages = allPages,
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            MaxPeople = trip.MaxPeople,
            Countries = trip.IdCountries.Select(country => new CountryDto
            {
                Name = country.Name
            }).ToList(),
            Clients = trip.ClientTrips.Select(client => new ClientDto
            {
                FirstName = client.IdClientNavigation.FirstName,
                LastName = client.IdClientNavigation.LastName,
            }).ToList()
        }).ToListAsync(cancellationToken);

        return trips;
    }

    public async Task<int> DeleteClient(int clientId, CancellationToken cancellationToken)
    {
        if (await DoesClientHaveAssignedTrips(clientId, cancellationToken))
        {
            return -1;
        }

        var clientToRemove = await _context
            .Clients
            .Where(client => client.IdClient == clientId)
            .FirstAsync(cancellationToken);

        _context.Clients.Remove(clientToRemove);

        var res = await _context.SaveChangesAsync(cancellationToken);

        return res;
    }

    private async Task<bool> DoesClientHaveAssignedTrips(int clientId, CancellationToken cancellationToken)
    {
        var query = await _context
            .ClientTrips
            .Where(client => client.IdClient == clientId)
            .FirstOrDefaultAsync(cancellationToken);

        return query != null;
    }

    public async Task<int> AssignClientToTrip(ClientTripDto clientTripDto, CancellationToken cancellationToken)
    {
        if (await DoesClientWithProvidedPeselExist(clientTripDto.Pesel, cancellationToken) == false)
        {
            return -1;
        }

        if (await DoesTripExist(clientTripDto.IdTrip, cancellationToken) == false)
        {
            return -2;
        }

        if (await IsProvidedTripInFuture(clientTripDto.IdTrip, cancellationToken))
        {
            return -3;
        }

        if (await IsClientWithProvidedPeselAlreadyAssignedToProvidedTrip(clientTripDto.Pesel, clientTripDto.IdTrip,
                cancellationToken))
        {
            return -4;
        }

        await _context
            .ClientTrips
            .AddAsync(new ClientTrip
                {
                    IdClient = await _context.Clients.Where(client => client.Pesel.Equals(clientTripDto.Pesel))
                        .Select(client => client.IdClient).FirstOrDefaultAsync(cancellationToken),
                    IdTrip = clientTripDto.IdTrip,
                    RegisteredAt = DateTime.Now,
                    PaymentDate = clientTripDto.PaymentDate
                }
                , cancellationToken);

        var res = await _context.SaveChangesAsync(cancellationToken);

        return res;
    }

    private async Task<bool> DoesClientWithProvidedPeselExist(string pesel, CancellationToken cancellationToken)
    {
        var client = await _context
            .Clients
            .Where(client => client.Pesel.Equals(pesel))
            .FirstOrDefaultAsync(cancellationToken);

        return client != null;
    }

    private async Task<bool> IsClientWithProvidedPeselAlreadyAssignedToProvidedTrip(string pesel, int tripId,
        CancellationToken cancellationToken)
    {
        // Klient na pewno nie będzie null, bo wcześniej przeprowadzamy walidację,
        // która nie przepuszcza działania metody w repozytorium dalej.
        var client = await _context
            .Clients
            .Where(client => client.Pesel.Equals(pesel))
            .FirstOrDefaultAsync(cancellationToken);

        var clientTrip = await _context
            .ClientTrips
            .Where(trip => trip.IdClient == client.IdClient && trip.IdTrip == tripId)
            .FirstOrDefaultAsync(cancellationToken);

        return clientTrip != null;
    }

    private async Task<bool> DoesTripExist(int idTrip, CancellationToken cancellationToken)
    {
        var trip = await _context
            .Trips
            .Where(trip => trip.IdTrip == idTrip)
            .FirstOrDefaultAsync(cancellationToken);

        return trip != null;
    }

    private async Task<bool> IsProvidedTripInFuture(int idTrip, CancellationToken cancellationToken)
    {
        var tripDateFrom = await _context
            .Trips
            .Where(trip => trip.IdTrip == idTrip)
            .Select(trip => trip.DateFrom)
            .FirstOrDefaultAsync(cancellationToken);

        return DateTime.Today.Ticks - tripDateFrom.Ticks > 0;
    }
}