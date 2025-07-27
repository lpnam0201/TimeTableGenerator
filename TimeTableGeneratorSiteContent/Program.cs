using Google.Apis.Services;

namespace TimeTableGeneratorSiteContent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            var service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer
            {
                ApplicationName = "Discovery Sample",
                ApiKey = "AIzaSyCfl7Q8KdO1TuwjSRW7Yp0wKSv_8iPNiO4",
            });
            var request = service.Files.List();
            request.Q = "name = 'ulaw_automation_khoa47_20250627'";
            request.Fields = "files(id, name)";
            var result = await request.ExecuteAsync();
        }
    }
}
