using Cwiczenia9.ModelsDtos;
using Cwiczenia9.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia9.Services;

public class TripsService : ITripsService
{
    private ITripsRepository _tripsRepository;

    public TripsService(ITripsRepository tripsRepository)
    {
        _tripsRepository = tripsRepository;
    }

    public async Task<IEnumerable<TripsDto>> GetTripsDto
    (bool paging,
        CancellationToken cancellationToken,
        int pageNum = 1,
        int pageSize = 10)
    {
        var tripsDto = await _tripsRepository.GetTripsDto(paging, cancellationToken, pageNum, pageSize);
        
        return tripsDto;
    }

    public async Task<int> DeleteClient(int clientId, CancellationToken cancellationToken)
    {
        var res = await _tripsRepository.DeleteClient(clientId, cancellationToken);

        return res;
    }

    public async Task<int> AssignClientToTrip(ClientTripDto clientTripDto, CancellationToken cancellationToken)
    {
        var res = await _tripsRepository.AssignClientToTrip(clientTripDto, cancellationToken);

        return res;
    }
}