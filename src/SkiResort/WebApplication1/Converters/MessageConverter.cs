namespace WebApplication1.Converters
{
    public class MessageConverter
    {
        public static WebApplication1.Models.MessageDTO ConvertMessageToMessageDTO(BL.Models.Message message)
        {
            return new WebApplication1.Models.MessageDTO(message.MessageID, message.SenderID, message.CheckedByID, message.Text);
        }

        public static List<WebApplication1.Models.MessageDTO> ConvertMessagesToMessagesDTO(List<BL.Models.Message> messages)
        {
            List < WebApplication1.Models.MessageDTO > messagesDTO = new List < WebApplication1.Models.MessageDTO >();
            foreach (BL.Models.Message message in messages)
            {
                messagesDTO.Add(ConvertMessageToMessageDTO(message));
            }
            return messagesDTO;
        }
    }
}
