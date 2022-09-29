namespace WebApplication1.Converters
{
    public class TurnstileConverter
    {
        public static WebApplication1.Models.Turnstile ConvertTurnstileToTurnstileDTO(BL.Models.Turnstile turnstile)
        {
            return new WebApplication1.Models.Turnstile(turnstile.TurnstileID, turnstile.LiftID, turnstile.IsOpen);
        }

        public static List<WebApplication1.Models.Turnstile> ConvertTurnstilesToTurnstilesDTO(List<BL.Models.Turnstile> turnstiles)
        {
            List < WebApplication1.Models.Turnstile > turnstilesDTO = new List < WebApplication1.Models.Turnstile >();
            foreach (BL.Models.Turnstile turnstile in turnstiles)
            {
                turnstilesDTO.Add(ConvertTurnstileToTurnstileDTO(turnstile));
            }
            return turnstilesDTO;
        }
    }
}
