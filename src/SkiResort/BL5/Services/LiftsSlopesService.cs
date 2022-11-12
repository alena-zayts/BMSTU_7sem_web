using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IRepositories;
using BL.Models;

namespace BL.Services
{
    public class LiftsSlopesService
    {
        private ILiftsSlopesRepository _liftsSlopesRepository;
        private ILiftsRepository _liftsRepository;
        private ISlopesRepository _slopesRepository;
        private IUsersRepository _usersRepository;

        public LiftsSlopesService(ILiftsSlopesRepository liftsSlopesRepository, ILiftsRepository liftsRepository, ISlopesRepository slopesRepository, IUsersRepository usersRepository)
        {
            _liftsSlopesRepository = liftsSlopesRepository;
            _liftsRepository = liftsRepository;
            _slopesRepository = slopesRepository;
            _usersRepository = usersRepository;
        }

        public async Task<List<LiftSlope>> GetLiftsSlopesInfoAsync(uint requesterUserID, uint offset = 0, uint limit = 0)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _liftsSlopesRepository.GetLiftsSlopesAsync(offset, limit);
        }

        public async Task DeleteLiftSlopeAsync(uint requesterUserID, string liftName, string slopeName)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            Lift lift = await _liftsRepository.GetLiftByNameAsync(liftName);
            Slope slope = await _slopesRepository.GetSlopeByNameAsync(slopeName);
            await _liftsSlopesRepository.DeleteLiftSlopesByIDsAsync(lift.LiftID, slope.SlopeID);
        }

        public async Task AddLiftSlopeAsync(uint requesterUserID, uint recordID, uint liftID, uint slopeID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _liftsSlopesRepository.AddLiftSlopeAsync(recordID, liftID, slopeID);
        }

        public async Task<uint> AddAutoIncrementLiftSlopeAsync(uint requesterUserID, string liftName, string slopeName)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            Lift lift = await _liftsRepository.GetLiftByNameAsync(liftName);
            Slope slope = await _slopesRepository.GetSlopeByNameAsync(slopeName);
            return await _liftsSlopesRepository.AddLiftSlopeAutoIncrementAsync(lift.LiftID, slope.SlopeID);
        }
    }
}
