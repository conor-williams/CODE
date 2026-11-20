
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

const string BASE_URL = "https://atu-team-yavca7kp.atlassian.net";
//const string PROJECT = "CA2 - Software Processes - SpaceCW";
const string PROJECT = "CA2SPACECW";

// Change these dates for each reporting period.
// End date is EXCLUSIVE.
DateTimeOffset periodStart =
    DateTimeOffset.Parse("2026-11-08 00:00");

DateTimeOffset periodEnd =
    DateTimeOffset.Parse("2026-11-10 23:59");

// Read credentials from environment variables.
string email = Environment.GetEnvironmentVariable("JIRA_EMAIL")
    ?? throw new Exception("JIRA_EMAIL not set.");

string token = Environment.GetEnvironmentVariable("JIRA_API_TOKEN")
    ?? throw new Exception("JIRA_API_TOKEN not set.");

using HttpClient client = new();

string credentials = Convert.ToBase64String(
    Encoding.UTF8.GetBytes($"{email}:{token}"));

client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Basic", credentials);

client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

// Movement counters.
int todo = 0;
int inProgress = 0;
int testing = 0;
int done = 0;

// --------------------------------------------------
// 1. Retrieve all issues in the project.
// --------------------------------------------------

Console.WriteLine("Retrieving Jira issues...");

List<string> issueKeys = new();

string? nextPageToken = null;

do
{
    var requestBody = new
    {
        jql = $"project = \"{PROJECT}\"",
        fields = new[] { "key" },
        maxResults = 100,
        nextPageToken
    };

    string json = await PostJson(
        client,
        $"{BASE_URL}/rest/api/3/search/jql",
        requestBody);

    var result = JsonSerializer.Deserialize<SearchResult>(
        json, options)!;

    issueKeys.AddRange(
        result.Issues.Select(i => i.Key));

    nextPageToken = result.IsLast
        ? null
        : result.NextPageToken;

} while (nextPageToken != null);

Console.WriteLine($"Found {issueKeys.Count} issues.");

// --------------------------------------------------
// 2. Retrieve the changelog for every issue.
// --------------------------------------------------

List<(string, string)> dates = new();
dates.Add(("2026-11-08 00:00", "2026-11-10 23:59"));
dates.Add(("2026-11-11 00:00", "2026-11-13 23:59"));
dates.Add(("2026-11-14 00:00", "2026-11-16 23:59"));
foreach ((string from, string to) in dates) {
DateTimeOffset dFrom = DateTimeOffset.Parse(from);
DateTimeOffset dTo = DateTimeOffset.Parse(to);
foreach (string issueKey in issueKeys)
{
    //Console.WriteLine($"Checking {issueKey}...");

    int startAt = 0;

    while (true)
    {
        string url =
            $"{BASE_URL}/rest/api/3/issue/{issueKey}/changelog" +
            $"?startAt={startAt}&maxResults=100";

        string json = await GetJson(client, url);

        var result = JsonSerializer.Deserialize<ChangelogResult>(
            json, options)!;
        //Console.WriteLine($"Issue: {issueKey}");
        //Console.WriteLine($"Changelog entries: {result.Values.Count}");
        //Console.WriteLine($"IsLast: {result.IsLast}");


        foreach (var history in result.Values)
        {
            
            if (!DateTimeOffset.TryParse(
                history.Created, out DateTimeOffset changedAt))
                continue;

            // Only count changes within the reporting period.
            //if (changedAt < periodStart || changedAt >= periodEnd)
            if (changedAt < dFrom || changedAt > dTo)
                continue;

            foreach (var item in history.Items)
            {
                    //Console.WriteLine( $"{issueKey} | {changedAt:dd/MM/yyyy HH:mm} | " + $"Field: {item.Field} | " + $"From: {item.FromString} | " + $"To: {item.ToStringValue}");
                if (!item.Field.Equals(
                    "status", StringComparison.OrdinalIgnoreCase))
                    continue;

                string newStatus = item.ToStringValue ?? "";

                switch (newStatus.ToLowerInvariant())
                {
                    case "to do":
                        todo++;
                        break;

                    case "in progress":
                        inProgress++;
                        break;

                    case "testing":
                        testing++;
                        break;

                    case "done":
                        done++;
                        break;
                }
            }
                 //Console.WriteLine($"totals here..{done}");
        }
        //Console.WriteLine($"Finished checking {issueKey}");

        if (result.IsLast || result.Values.Count == 0)
            break;

        startAt += result.Values.Count;
    }
}
// --------------------------------------------------
// 3. Display results.
// --------------------------------------------------

Console.WriteLine();
Console.WriteLine("----------------------------------");
Console.WriteLine("       JIRA MOVEMENT REPORT");
Console.WriteLine("----------------------------------");

Console.WriteLine($"Period: {dFrom:dd/MM/yyyy} - " +
                  $"{dTo:dd/MM/yyyy}");

Console.WriteLine();

Console.WriteLine($"Moved to To Do:       {todo}");
Console.WriteLine($"Moved to In Progress: {inProgress}");
Console.WriteLine($"Moved to Testing:     {testing}");
Console.WriteLine($"Moved to Done:        {done}");

Console.WriteLine("----------------------------------");
todo = 0;
inProgress = 0;
testing = 0;
done = 0;
}


// --------------------------------------------------
// HTTP helper methods.
// --------------------------------------------------

static async Task<string> GetJson(HttpClient client, string url)
{
    using var response = await client.GetAsync(url);
    string content = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
        throw new Exception(
            $"HTTP {(int)response.StatusCode}: {content}");

    return content;
}

static async Task<string> PostJson(
    HttpClient client, string url, object data)
{
    string json = JsonSerializer.Serialize(data);

    using var response = await client.PostAsync(
        url,
        new StringContent(json, Encoding.UTF8, "application/json"));

    string content = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
        throw new Exception(
            $"HTTP {(int)response.StatusCode}: {content}");

    return content;
}

// --------------------------------------------------
// JSON classes.
// --------------------------------------------------

class SearchResult
{
    [JsonPropertyName("issues")]
    public List<JiraIssue> Issues { get; set; } = [];

    [JsonPropertyName("nextPageToken")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("isLast")]
    public bool IsLast { get; set; }
}

class JiraIssue
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = "";
}

class ChangelogResult
{
    [JsonPropertyName("values")]
    public List<History> Values { get; set; } = [];

    [JsonPropertyName("isLast")]
    public bool IsLast { get; set; }
}

class History
{
    [JsonPropertyName("created")]
    public string Created { get; set; } = "";

    [JsonPropertyName("items")]
    public List<HistoryItem> Items { get; set; } = [];
}

class HistoryItem
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = "";

    [JsonPropertyName("toString")]
    public string? ToStringValue { get; set; }
  
    [JsonPropertyName("fromString")]
    public string? FromString { get; set; }
}

