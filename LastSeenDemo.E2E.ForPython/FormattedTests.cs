namespace LastSeenDemo.E2E.ForPython;

public class FormattedTests
{
  [Fact]
  public void Formatted_ShouldReturn_ArrayOfStrings_WhenCalled()
  {
    var result = HttpHelper.Get("/formatted");
    Assert.NotEmpty(result);
  }
}
