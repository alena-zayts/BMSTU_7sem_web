namespace Project1.Models
{
    /// <summary>
    /// Information about turnstiles
    /// </summary>
    public record class Turnstile
    {
        /// <summary>
        /// Turnstile ID
        /// </summary>
        public uint TurnstileID { get; }
        /// <summary>
        /// ID of the lift to which the turnstile is connected
        /// </summary>
        public uint LiftID { get; }
        /// <summary>
        /// Is the turnstile currently working or not
        /// </summary>
        public bool IsOpen { get; }

        public Turnstile(uint turnstileID, uint liftID, bool isOpen)
        {
            this.TurnstileID = turnstileID;
            this.LiftID = liftID;
            this.IsOpen = isOpen;

        }
    }
}

