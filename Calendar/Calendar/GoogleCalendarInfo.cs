using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace Calendar
{
    class GoogleCalendarInfo
    {
        private static string[] Scopes={ CalendarService.Scope.CalendarReadonly };
        private UserCredential credential;
        EventsResource.ListRequest request;

        public GoogleCalendarInfo()
        {
            Access();
            CreateAPIService();
        }

        public string ApplicationName { get; private set; }

        // 接続設定
        private void Access()
        {
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/calendar-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
        }

        // API使用許可とデータ取得設定
        private void CreateAPIService()
        {
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // データ取得設定
            request = service.Events.List("primary");
            request.TimeMin = DateTime.Now; // 今日から
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 20;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        }

        // 予定の取得
        public string[][] GetGoogleCalPlan()
        {
            Events events = request.Execute();
            List<string[]> plans = new List<string[]>(); // 返すための配列 1:イベント、2:時刻

            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string[] planinfo = new string[2]; // 1つの予定の情報
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                    planinfo[0] = eventItem.Summary; planinfo[1] = when.Substring(0,10);
                    plans.Add(planinfo);
                }
            }
            else
            {
                return null;
                //Console.WriteLine("No upcoming events found.");
            }
            return plans.ToArray();
        }
    }
}
