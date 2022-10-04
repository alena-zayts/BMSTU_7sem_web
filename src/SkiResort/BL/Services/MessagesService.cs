using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.IRepositories;
using BL.Models;
using BL.Exceptions.MessageExceptions;

namespace BL.Services
{
    public class MessagesService
    {
        private IMessagesRepository _messagesRepository;
        private IUsersRepository _usersRepository;

        public MessagesService(IMessagesRepository messagesRepository, IUsersRepository usersRepository)
        {
            _messagesRepository = messagesRepository;
            _usersRepository = usersRepository;
        }

        public async Task<Message> AdminGetMessageByIDAsync(uint requesterUserID, uint messageID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _messagesRepository.GetMessageByIdAsync(messageID);
        }

        public async Task<uint> SendMessageAsync(uint requesterUserID, string text)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);

            Message message = new(Message.MessageUniversalID, requesterUserID, Message.MessageCheckedByNobody, text);
            return await _messagesRepository.AddMessageAutoIncrementAsync(message.SenderID, message.CheckedByID, message.Text);
        }
        public async Task<Message> GetMessageAsync(uint requesterUserID, uint messageID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _messagesRepository.GetMessageByIdAsync(messageID);
        }
        public async Task<List<Message>> GetMessagesAsync(uint requesterUserID, uint offset = 0, uint limit = 0)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            return await _messagesRepository.GetMessagesAsync(offset, limit);
        }

        public async Task<Message> MarkMessageReadByUserAsync(uint requesterUserID, uint messageID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            Message message = await _messagesRepository.GetMessageByIdAsync(messageID);

            if (message.CheckedByID != Message.MessageCheckedByNobody)
            {
                throw new MessageCheckingException("Couldn't mark message checked because it is alredy checked", message);
            }

            Message checkedMessage = new(message.MessageID, message.SenderID, requesterUserID, message.Text);
            await _messagesRepository.UpdateMessageByIDAsync(checkedMessage.MessageID, checkedMessage.SenderID, checkedMessage.CheckedByID, checkedMessage.Text);

            return checkedMessage;
        }

        public async Task AdminUpdateMessageAsync(uint requesterUserID, uint messageID, uint senderID, uint checkedByID, string text)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _messagesRepository.UpdateMessageByIDAsync(messageID, senderID, checkedByID, text);
        }

        public async Task AdminDeleteMessageAsync(uint requesterUserID, uint messageID)
        {
            await CheckPermissionsService.CheckPermissionsAsync(_usersRepository, requesterUserID);
            await _messagesRepository.DeleteMessageByIDAsync(messageID);
        }

    }
}
