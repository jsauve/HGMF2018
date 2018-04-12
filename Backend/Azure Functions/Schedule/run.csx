#r "Newtonsoft.Json"

using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    try
    {
        using (var reader = File.OpenText(Path.Combine(GetFunctionPath("Schedule"), "Schedule.json")))
        {
            var text = await reader.ReadToEndAsync();
            var response = req.CreateResponse(HttpStatusCode.OK, "", System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json"));
            response.Content = new StringContent(text, Encoding.Unicode);
            return response;
        }
    } 
    catch (Exception ex)
    {
        return req.CreateResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
    }
}

private static string GetFunctionPath(string functionName)
    => Path.Combine(GetEnvironmentVariable("HOME"), String.Format(@"site\wwwroot\{0}", functionName));

private static string GetEnvironmentVariable(string name)
    => System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);