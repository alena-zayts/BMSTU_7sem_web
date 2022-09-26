using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    /// <summary>
    /// Information about cards used in the resort
    /// </summary>
    public class MessageDTO
    {
        public MessageDTO(uint messageID, uint senderID, uint checkedByID, string text)
        {
            MessageID = messageID;
            SenderID = senderID;
            CheckedByID = checkedByID;
            Text = text;
        }

        /// <summary>
        /// Message ID
        /// </summary>
        public uint MessageID { get; }
        /// <summary>
        /// Sender ID
        /// </summary>
        public uint SenderID { get; }
        /// <summary>
        /// ID of the ski patrol worker who checked the message
        /// </summary>
        public uint CheckedByID { get; }
        /// <summary>
        /// Text of the message
        /// </summary>
        public string Text { get; }
    }
}
