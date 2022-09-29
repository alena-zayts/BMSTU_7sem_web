namespace WebApplication1.Converters
{
    public class MessageConverter
    {
        public static WebApplication1.Models.Message ConvertMessageToMessageDTO(BL.Models.Message message)
        {
            return new WebApplication1.Models.Message(message.MessageID, message.SenderID, message.CheckedByID, message.Text);
        }

        public static List<WebApplication1.Models.Message> ConvertMessagesToMessagesDTO(List<BL.Models.Message> messages)
        {
            List < WebApplication1.Models.Message > messagesDTO = new List < WebApplication1.Models.Message >();
            foreach (BL.Models.Message message in messages)
            {
                messagesDTO.Add(ConvertMessageToMessageDTO(message));
            }
            return messagesDTO;
        }
    }
}
