// See https://aka.ms/new-console-template for more information

using LastSeenDemo;

var userLoader = new UserLoader(new Loader(), "https://sef.podkolzin.consulting/api/users/lastSeen");
var application = new LastSeenApplication(userLoader);
var result = application.Show(DateTimeOffset.Now);

foreach (var item in result)
    Console.WriteLine(item);


/*
my password
LVPtPJgr_01b
https://github.com/Zakhar-Khmurych/LastSeenDemoAssignments.git
dotnet run -urls=http://[::]/
maybe anonymous messenger chat? spme mind game
sebassignmentstaskdomain
dotnet run --urls=http://[::]/

167.172.189.115 my IP

my password
*/

