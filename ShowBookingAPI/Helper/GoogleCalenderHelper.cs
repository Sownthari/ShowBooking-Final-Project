using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

class GoogleCalendarHelper
{
    static string[] Scopes = { CalendarService.Scope.Calendar };
    static string ApplicationName = "ShowSpot";

    public static void CreateEvent()
    {
        UserCredential credential;

        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        // Create Calendar service.
        var service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // Define an event
        Event newEvent = new Event()
        {
            Summary = "Meeting with John",
            Location = "123 Main St",
            Description = "Discuss the project",
            Start = new EventDateTime()
            {
                DateTime = DateTime.Now.AddDays(1), // Set start time
                TimeZone = "America/Los_Angeles",
            },
            End = new EventDateTime()
            {
                DateTime = DateTime.Now.AddDays(1).AddHours(1), // Set end time
                TimeZone = "America/Los_Angeles",
            },
            Attendees = new EventAttendee[] {
                new EventAttendee() { Email = "sownthari01052003@gmail.com" }
            },
        };

        // Insert the event into the user's calendar
        var calendarId = "primary"; // Use "primary" for the default calendar
        EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
        Event createdEvent = request.Execute();
        Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
    }
}
