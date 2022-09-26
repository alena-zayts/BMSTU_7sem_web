namespace WebApplication1.Models
{
    /// <summary>
    /// Information about readings of cards on turnstiles of the resort
    /// </summary>
    public record class CardReadingDTO
    {
        public CardReadingDTO(uint recordID, uint turnstileID, uint cardID, DateTimeOffset readingTime)
        {
            this.RecordID = recordID;
            this.TurnstileID = turnstileID;
            this.CardID = cardID;
            this.ReadingTime = readingTime;

        }
        /// <summary>
        /// The ID of the record about card reading
        /// </summary>
        public uint RecordID { get; }
        /// <summary>
        /// ID of the turnstile where the reading took place
        /// </summary>
        public uint TurnstileID { get; }
        /// <summary>
        /// ID of the card that was read
        /// </summary>
        public uint CardID { get; }
        /// <summary>
        /// The time of the card reading
        /// </summary>
        public DateTimeOffset ReadingTime { get; }


    }
}

