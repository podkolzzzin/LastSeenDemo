namespace LastSeenDemo.IntegrationTest;

public class Exam_integration_tests
{
    [Fact]
    public void fill_user_list()
    {
        Exam exam = new Exam();
        string must_be_not_null = exam.get_json("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
        var some_people = exam.CreateSomePeople("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
        
        Assert.NotEmpty(some_people);
    }

}
