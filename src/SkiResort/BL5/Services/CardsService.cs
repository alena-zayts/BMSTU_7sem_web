using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IRepositories;
using BL.Models;

namespace BL.Services
{
    public class CardsService
    {
        private ICardsRepository _cardsRepository;
        private IUsersRepository _usersRepository;

        public CardsService(ICardsRepository cardsRepository, IUsersRepository usersRepository)
        {
            _cardsRepository = cardsRepository;
            _usersRepository = usersRepository;
        }

        public async Task AdminUpdateCardAsync(uint requesterUserID, uint cardID, DateTimeOffset newActivationTime, string newType)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _cardsRepository.UpdateCardByIDAsync(cardID, newActivationTime, newType);
        }

        public async Task AdminDeleteCardAsync(uint requesterUserID, uint cardID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _cardsRepository.DeleteCarByIDdAsync(cardID);
        }


        public async Task<uint> AdminAddAutoIncrementCardAsync(uint requesterUserID, DateTimeOffset activationTime, string type)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _cardsRepository.AddCardAutoIncrementAsync(activationTime, type);
        }

        public async Task<Card> AdminGetCardAsync(uint requesterUserID, uint messageID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _cardsRepository.GetCardByIdAsync(messageID);
        }
        public async Task<List<Card>> AdminGetCardsAsync(uint requesterUserID, uint offset = 0, uint limit = 0)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _cardsRepository.GetCardsAsync(offset, limit);
        }
    }
}
