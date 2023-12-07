using System.Net.Http;
using System.Text.Json;

namespace LastSeenDemo;

public class Page
{
    public int Total { get; set; }
    public User[] Data { get; set; }
}

public class User
{
    public Guid UserId { get; set; }
    public DateTimeOffset? LastSeenDate { get; set; }
    public string Nickname { get; set; }
    public bool IsOnline { get; set; }
}

public interface ILoader
{
    Page Load(string url);
}

public class Loader : ILoader
{
    private readonly HttpClient _client;

    public Loader(HttpClient client)
    {
        _client = client;
    }

    public Page Load(string url)
    {
        var response = _client.Send(new HttpRequestMessage(HttpMethod.Get, url));
        using var reader = new StreamReader(response.Content.ReadAsStream());
        var stringContent = reader.ReadToEnd();
        return JsonSerializer.Deserialize<Page>(stringContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        })!;
    }
}
