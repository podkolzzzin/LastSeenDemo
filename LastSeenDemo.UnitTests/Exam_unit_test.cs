namespace LastSeenDemo.UnitTests;

public class Exam_unit_test
{
    [Fact]
    public void test_if_we_get_something_from_Url()
    {
        Exam exam = new Exam();
        string must_be_not_null = exam.get_json("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
        Assert.False(null  == must_be_not_null);
    }
    //str example
    // {"userId":"df60f3f8-0e13-b64a-76b5-863bae54478e","nickname":"Patty.Metz31","firstName":"Patty","lastName":"Metz","registrationDate":"2023-07-19T06:45:41.4403458+00:00","lastSeenDate":"2023-11-02T06:38:09.9063968+00:00","isOnline":false}
    [Fact]
    public void split_json_correctly()
    {
        Exam exam = new Exam();
        string must_be_not_null = exam.get_json("https://sef.podkolzin.consulting/api/users/lastSeen?offset=40");
        var single_user_piece = exam.CutJsonBySingleUserInfo(must_be_not_null);
        var single_user_params = exam.CutSingleUserByParameters(single_user_piece[2]);
        var pair = exam.CutByPairs(single_user_params[0]);
        
        string expected = "df60f3f8-0e13-b64a-76b5-863bae54478e";
        Assert.Equal(expected, pair[1]);
    }  
    
    
    
}
