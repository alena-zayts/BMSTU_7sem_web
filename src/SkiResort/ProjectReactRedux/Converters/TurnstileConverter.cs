using System.Collections.Generic;
namespace ProjectReactRedux.Converters
{
    public class TurnstileConverter
    {
        public static ProjectReactRedux.Models.Turnstile ConvertTurnstileToTurnstileDTO(BL.Models.Turnstile turnstile)
        {
            return new ProjectReactRedux.Models.Turnstile(turnstile.TurnstileID, turnstile.LiftID, turnstile.IsOpen);
        }

        public static List<ProjectReactRedux.Models.Turnstile> ConvertTurnstilesToTurnstilesDTO(List<BL.Models.Turnstile> turnstiles)
        {
            List < ProjectReactRedux.Models.Turnstile > turnstilesDTO = new List < ProjectReactRedux.Models.Turnstile >();
            foreach (BL.Models.Turnstile turnstile in turnstiles)
            {
                turnstilesDTO.Add(ConvertTurnstileToTurnstileDTO(turnstile));
            }
            return turnstilesDTO;
        }
    }
}
