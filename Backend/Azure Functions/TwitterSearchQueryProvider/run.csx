using System.Net;
using System.Text;
using System.Threading.Tasks;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    // see the Twitter search API docs for details
    var twitterSearchQuery = "#hgmf17 OR @dhgmf OR from:dhgmf -filter:retweets";

    var response = new HttpResponseMessage();
    response.Content = new StringContent(twitterSearchQuery, Encoding.Unicode);
    return response;
}