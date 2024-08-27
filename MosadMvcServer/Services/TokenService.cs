using System.Diagnostics;

namespace MosadMvcServer.Services
{
    public  class TokenService
    {
        public static string Token { get; private set; } = "debug";
        public readonly static string Id = "MVCServer";

        public static async void InitToken(HttpJsonService httpJsonService)
        {
            try
            {
                var token = await httpJsonService.GetToken(Id);
                Token = token.Token;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
