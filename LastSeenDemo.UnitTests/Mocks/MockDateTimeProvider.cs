namespace LastSeenDemo.UnitTests.Mocks;

class MockDateTimeProvider : IDateTimeProvider
{
    public static readonly DateTime DefaultMockDate = new(2000, 01, 31);

    private readonly DateTime _dateTime;

    public MockDateTimeProvider()
    {
        _dateTime = DefaultMockDate;
    }

    public MockDateTimeProvider(DateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public DateTimeOffset GetCurrentTime()
    {
        return _dateTime;
    }
}
