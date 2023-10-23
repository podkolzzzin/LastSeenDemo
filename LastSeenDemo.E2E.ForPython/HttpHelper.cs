namespace LastSeenDemo.E2E.ForPython;


public static class HttpHelper
{
  public static string Get(string apiUrl)
  {
    using var client = new HttpClient();
    using var result = client.Send(new HttpRequestMessage(HttpMethod.Get, Configuration.BaseUrl + apiUrl));
    using var reader = new StreamReader(result.Content.ReadAsStream());

    result.EnsureSuccessStatusCode();
    
    return reader.ReadToEnd();
  }
}