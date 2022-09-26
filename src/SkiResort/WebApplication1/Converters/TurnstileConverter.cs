namespace WebApplication1.Converters
{
    public class TurnstileConverter
    {
        public static WebApplication1.Models.TurnstileDTO ConvertTurnstileToTurnstileDTO(BL.Models.Turnstile turnstile)
        {
            return new WebApplication1.Models.TurnstileDTO(turnstile.TurnstileID, turnstile.LiftID, turnstile.IsOpen);
        }

        public static List<WebApplication1.Models.TurnstileDTO> ConvertTurnstilesToTurnstilesDTO(List<BL.Models.Turnstile> turnstiles)
        {
            List < WebApplication1.Models.TurnstileDTO > turnstilesDTO = new List < WebApplication1.Models.TurnstileDTO >();
            foreach (BL.Models.Turnstile turnstile in turnstiles)
            {
                turnstilesDTO.Add(ConvertTurnstileToTurnstileDTO(turnstile));
            }
            return turnstilesDTO;
        }
    }
}
