namespace LastSeenDemo.IntegrationTest.Mocks;

public class LoaderMock : ILoader
{
    private readonly Page[] _pages;
    public List<string> Urls { get; } = new();

    public LoaderMock(Page[] pages)
    {
        _pages = pages;
    }

    public Page Load(string url)
    {
        var result = _pages[Urls.Count];
        Urls.Add(url);
        return result;
    }
}
