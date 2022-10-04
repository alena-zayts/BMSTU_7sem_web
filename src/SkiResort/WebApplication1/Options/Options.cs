using System;
using System.Web;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Options
{
	public static class OtherOptions
	{
		public static JsonSerializerOptions JsonOptions()
		{
			var options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
				WriteIndented = true
			};

			return options;
		}

		//public static BL.Facade createFacade()
  //      {
		//	BL.IRepositoriesFactory repositoriesFactory = new AccessToDB.TarantoolRepositoriesFactory();
		//	BL.Facade facade = new BL.Facade(repositoriesFactory);
		//	return facade;
  //      }

		public static uint getUserIDFromToken(HttpRequest request)
        {
			var _bearer_token = request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
			JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(_bearer_token);
			uint userID = (uint)(long)token.Payload["userID"];
			return userID;
		}

	}
}