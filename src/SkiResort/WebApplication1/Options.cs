using System;
using System.Web;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace WebApplication1
{
	public static class Options
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

		public static BL.Facade createFacade()
        {
			BL.IRepositoriesFactory repositoriesFactory = new AccessToDB.TarantoolRepositoriesFactory();
			BL.Facade facade = new BL.Facade(repositoriesFactory);
			return facade;
        }

	}
}