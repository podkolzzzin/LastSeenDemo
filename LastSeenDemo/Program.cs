// See https://aka.ms/new-console-template for more information

using LastSeenDemo;

#pragma warning disable S1075
var userLoader = new UserLoader(new Loader(), "https://sef.podkolzin.consulting/api/users/lastSeen");
#pragma warning restore S1075
var application = new LastSeenApplication(userLoader);
var result = application.Show(DateTimeOffset.Now);

foreach (var item in result)
    Console.WriteLine(item);
