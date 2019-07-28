using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sample.Tests.Helpers
{
	public static class ContentHelper
	{
		public static StringContent GetStringContent(object obj)
				=> new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");

		public static async Task<string> GetContentAsString(HttpContent content) => await content.ReadAsStringAsync();

		public static T GetContentAsJson<T>(HttpContent content)
		{
			var stringContent = GetContentAsString(content);
			return JsonConvert.DeserializeObject<T>(stringContent.Result);
		}
	}
}
