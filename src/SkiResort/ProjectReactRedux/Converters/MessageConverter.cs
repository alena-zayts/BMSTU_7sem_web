using System.Collections.Generic;
namespace ProjectReactRedux.Converters
{
    public class MessageConverter
    {
        public static ProjectReactRedux.Models.Message ConvertMessageToMessageDTO(BL.Models.Message message)
        {
            return new ProjectReactRedux.Models.Message(message.MessageID, message.SenderID, message.CheckedByID, message.Text);
        }

        public static List<ProjectReactRedux.Models.Message> ConvertMessagesToMessagesDTO(List<BL.Models.Message> messages)
        {
            List < ProjectReactRedux.Models.Message > messagesDTO = new List < ProjectReactRedux.Models.Message >();
            foreach (BL.Models.Message message in messages)
            {
                messagesDTO.Add(ConvertMessageToMessageDTO(message));
            }
            return messagesDTO;
        }
    }
}
