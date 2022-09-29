using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    /// <summary>
    /// Information about cards used in the resort
    /// </summary>
    public class Card
    {
        public Card(uint cardID, DateTimeOffset activationTime, string type)
        {
            CardID = cardID;
            ActivationTime = activationTime;
            Type = type;
        }

        /// <summary>
        /// Card ID
        /// </summary>
        public uint CardID { get; }
        /// <summary>
        /// The time when the card was activated
        /// </summary>
        public DateTimeOffset ActivationTime { get; }
        /// <summary>
        /// Type of the card
        /// </summary>
        public string Type { get; }
    }
}
