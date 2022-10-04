using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IRepositories;
using BL.Models;

namespace BL.Services
{
    public class CardReadingsService
    {
        private ICardReadingsRepository _cardReadingsRepository;
        private IUsersRepository _usersRepository;

        public CardReadingsService(ICardReadingsRepository cardReadingsRepository, IUsersRepository usersRepository)
        {
            _cardReadingsRepository = cardReadingsRepository;
            _usersRepository = usersRepository;
        }

        public async Task AdminAddCardReadingAsync(uint requesterUserID, uint recordID, uint turnstileID, uint cardID, DateTimeOffset readingTime)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _cardReadingsRepository.AddCardReadingAsync(recordID, turnstileID, cardID, readingTime);
        }

        public async Task<List<CardReading>> AdminGetCardReadingsAsync(uint requesterUserID, uint offset = 0, uint limit = 0)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _cardReadingsRepository.GetCardReadingsAsync(offset, limit);
        }

        public async Task<CardReading> AdminGetCardReadingAsync(uint requesterUserID, uint recordID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            CardReading cardReading = await _cardReadingsRepository.GetCardReadingByIDAsync(recordID);

            return cardReading;
        }

        public async Task AdminDeleteCardReadingAsync(uint requesterUserID, uint recordID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _cardReadingsRepository.DeleteCardReadingAsync(recordID);
        }


        public async Task<uint> AdminAddAutoIncrementCardReadingAsync(uint requesterUserID, uint turnstileID, uint cardID, DateTimeOffset readingTime)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _cardReadingsRepository.AddCardReadingAutoIncrementAsync(turnstileID, cardID, readingTime);
        }

        public async Task AdminUpdateCardReadingAsync(uint requesterUserID, uint recordID, uint newTurnstileID, uint newCardID, DateTimeOffset newReadingTime)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _cardReadingsRepository.UpdateCardReadingByIDAsync(recordID, newTurnstileID, newCardID, newReadingTime);
        }
    }
}
