namespace LastSeenDemo;
using System.Collections.Generic;
using System.Net;

// "https://sef.podkolzin.consulting/api/users/lastSeen"
// "https://sef.podkolzin.consulting/api/users/list"

public class ExamUser
{
    private string USERNAME;
    private string USERID;
    private string USER_FIRST_SEEN;

    public  ExamUser(string name, string id, string first_seen)
    {
        USERNAME = name;
        USERID = id;
        USER_FIRST_SEEN = first_seen;
    }

    string[] ReturnExamStats()
    {
        string[] exam_stats = new string[3];
        exam_stats[0] = USERNAME;
        exam_stats[1] = USERID;
        exam_stats[2] = USER_FIRST_SEEN;

        return exam_stats;
    }
}

public class Exam
{
    private User USER;
    private string JSON = "";

    public string get_json(string url)
    {
        using (WebClient wc = new WebClient())
        {
            JSON = wc.DownloadString(url);
        }
        return JSON;
    }

    public string[] CutJsonBySingleUserInfo(string json) //this big json from url
    {
        string[] cutted_json = json.Split('{');
        return cutted_json;
    }

    public string[] CutSingleUserByParameters(string single_user_stats) // ""userId":"0d0ac023-746e-5d8e-3229-4b049c6"···
    {
        string[] cutted_parameters = single_user_stats.Split(',');
        return cutted_parameters;
    }

    public string[] CutByPairs(string pair) // "userId":"0d0ac023-746e-5d8e-3229-4b049c6"
    {
        pair = pair.Replace("\"", "");
        string[] cutted_pairs = pair.Split(':'); 
        return cutted_pairs;
    }


    public List<ExamUser> CreateSomePeople(string json)
    {
        List<ExamUser> exam_users = new List<ExamUser>();
        string[] must_be_not_null = CutJsonBySingleUserInfo(json);
        string single_param_id;
        string single_param_name;
        string single_param_firstseen;
        foreach (var single_user_piece in must_be_not_null)
        {
            if (single_user_piece.Contains(','))
            {
                var single_user_params = CutSingleUserByParameters(single_user_piece);
                string name;
                string id;
                string firstseen;
                 single_param_id = single_user_params[0];
                 single_param_name = single_user_params[1];
                 single_param_firstseen = single_user_params[4];
                
                
                exam_users.Add(new ExamUser(single_param_name, single_param_id, single_param_firstseen));
            }
        }

        return exam_users;
    }

}




