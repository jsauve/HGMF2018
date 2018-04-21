using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using System.Collections.Generic;
using System;

namespace HGMF2018.PushTestConsoleApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            try
            {
                Test().GetAwaiter().GetResult();
            }
            catch (ArgumentException aex)
            {
                Console.WriteLine($"Caught ArgumentException: {aex.Message}");
            }
        }

        private static async Task Test()
        { 
            var connectionString = "Endpoint=sb://duluthhomegrown.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=WLSiFyyzVCohnSpOc9YUzWqp4xUwociGQ95f5r5FWFU=";
            var hubName = "hgmf2018-push-hub-sandbox";

            var tweetContent = "Tweet Content blah blah blah";
            var tweetUrl = "Tweet Content blah blah blah";

            var hubClient = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);

            // Android JSON payload
            var androidPayload = $@"{{""notification"":{{""body"":""{tweetContent}""}},""data"":{{""tweetUrl"":""{tweetUrl}""}}}}";

            // iOS JSON payload
            //var iOSPayload = $"{{\"aps\":{{\"alert\":\"{tweetContent}\"}}";

            //Send push notification just to selected tags
            await hubClient.SendGcmNativeNotificationAsync(androidPayload);
            //hubClient.SendAppleNativeNotificationAsync(iOSPayload);
        }

        // {"notification":{"body":"{tweetContent}"},"data":{"tweetUrl":"{tweetUrl}"}}
    }
}
