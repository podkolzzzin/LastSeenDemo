using System.Net;

namespace LastSeenDemo.E2E;


public static class HttpHelper
{
    public static (HttpStatusCode statusCode, string response) Get(string apiUrl)
    {
        using var client = new HttpClient();
        var url = new Uri(Configuration.BaseUrl + apiUrl);
        using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, url));
        using var reader = new StreamReader(result.Content.ReadAsStream());

        return (result.StatusCode, reader.ReadToEnd());
    }
}
