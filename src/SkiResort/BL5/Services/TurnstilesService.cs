using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IRepositories;
using BL.Models;

namespace BL.Services
{
    public class TurnstilesService
    {
        private ITurnstilesRepository _turnstilesRepository;
        private IUsersRepository _usersRepository;

        public TurnstilesService(ITurnstilesRepository turnstilesRepository, IUsersRepository usersRepository)
        {
            _turnstilesRepository = turnstilesRepository;
            _usersRepository = usersRepository;
        }

        public async Task AdminUpdateTurnstileAsync(uint requesterUserID, uint turnstileID, uint newLiftID, bool newIsOpen)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _turnstilesRepository.UpdateTurnstileByIDAsync(turnstileID, newLiftID, newIsOpen);
        }

        public async Task AdminDeleteTurnstileAsync(uint requesterUserID, uint turnstileID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _turnstilesRepository.DeleteTurnstileByIDAsync(turnstileID);
        }

        public async Task AdminAddTurnstileAsync(uint requesterUserID, uint turnstileID, uint liftID, bool isOpen)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _turnstilesRepository.AddTurnstileAsync(turnstileID, liftID, isOpen);
        }

        public async Task<uint> AdminAddAutoIncrementTurnstileAsync(uint requesterUserID, uint liftID, bool isOpen)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _turnstilesRepository.AddTurnstileAutoIncrementAsync(liftID, isOpen);
        }

        public async Task<Turnstile> AdminGetTurnstileAsync(uint requesterUserID, uint turnstileID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _turnstilesRepository.GetTurnstileByIdAsync(turnstileID);
        }
        public async Task<List<Turnstile>> AdminGetTurnstilesAsync(uint requesterUserID, uint offset = 0, uint limit = 0)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _turnstilesRepository.GetTurnstilesAsync(offset, limit);
        }
    }
}
