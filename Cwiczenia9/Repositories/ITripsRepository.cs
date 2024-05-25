using Cwiczenia9.ModelsDtos;

namespace Cwiczenia9.Repositories;

public interface ITripsRepository
{
    public Task<IEnumerable<TripsDto>> GetTripsDto
    (bool paging,
        CancellationToken cancellationToken,
        int pageNum = 1,
        int pageSize = 10);

    public Task<int> DeleteClient(int clientId, CancellationToken cancellationToken);

    public Task<int> AssignClientToTrip(ClientTripDto clientTripDto, CancellationToken cancellationToken);
}