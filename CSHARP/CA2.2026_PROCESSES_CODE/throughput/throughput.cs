using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
//JIRA_EMAIL=L00206159@atu.ie
//JIRA_API_TOKEN=ATATT3xFfGF0Nno_j2pYkIiWJisTp5S0OYIIEqs1_oabXvfz26B6gDerqDYZ-Lf2-uGOcSYyqHsl-6kocVKo1szFjLqg745ci9Qy1JROsfVrjpwTKH9IJnuhGaqbS5jrkdEwFZx_NLqlLtxVN0kFvN8FbBqJSFRkqDGF2Zc8q1Tu7IB_xI8BORU=3EC955E8

class Program
{
    private const string JIRA_URL = "https://atu-team-yavca7kp.atlassian.net";
    private const string PROJECT = "CA2 - Software Processes - SpaceCW";
    static async Task Main()
    {
        string email = Environment.GetEnvironmentVariable("JIRA_EMAIL") ?? "";
        string apiToken = Environment.GetEnvironmentVariable("JIRA_API_TOKEN") ?? "";
        if (email == "" || apiToken == "") {
            Console.WriteLine("JIRA_EMAIL and JIRA_API_TOKEN environment variables are required.");
            return;
        } try {
            int tot = 0;
            Console.WriteLine($"\x1B[33mThroughput Period                    | Throughput\x1B[0m");
            Console.WriteLine($"\x1B[33m-------------------------------------------------\x1B[0m");
	    List<(string, string)> dates = new();
            dates.Add(("2026-11-08 00:00", "2026-11-10 23:59"));
            dates.Add(("2026-11-11 00:00", "2026-11-13 23:59"));
            dates.Add(("2026-11-14 00:00", "2026-11-16 23:59"));
            foreach ((string from, string to) in dates) {
                int count = await GetResolvedCount(email, apiToken, from, to);
                //Console.WriteLine();
                Console.WriteLine($"\x1B[34m{from} -> {to} \x1B[33m|\x1B[0m\x1B[34m {count}\x1B[0m");
                tot += count;
            }
          
            Console.WriteLine($"\x1B[34m                                     \x1B[33m|\x1B[34m \x1B[0m");
            Console.WriteLine($"\x1B[34mTotal Throughput                     \x1B[33m|\x1B[34m {tot}\x1B[0m");
            double avg = (double)tot/(double)dates.Count();
            Console.WriteLine($"\x1B[34mAverage Throughput Per Period        \x1B[33m|\x1B[34m {avg:F2}\x1B[0m");
            
        }
        catch (Exception ex) {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static async Task<int> GetResolvedCount(string email, string apiToken, string from, string to)
    {
        using HttpClient client = new();
        // Jira Basic Authentication
        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}:{apiToken}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        // JQL query
        string jql =
            $"project = \"{PROJECT}\" " +
            "AND status = Done " +
            $"AND resolved >= \"{from}\" " +
            $"AND resolved < \"{to}\"";
//            "AND resolved >= \"2026-11-08\" " +
//            "AND resolved < \"2026-11-11\"";

        int totalCount = 0;
        string? nextPageToken = null;
        bool isLast = false;

        while (!isLast)
        {
            string url = $"{JIRA_URL}/rest/api/3/search/jql" + $"?jql={Uri.EscapeDataString(jql)}" + "&maxResults=100";
            if (nextPageToken != null) { url += $"&nextPageToken={Uri.EscapeDataString(nextPageToken)}"; }

            // Request next page
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            // Count issues on this page
            int pageCount = root.GetProperty("issues").GetArrayLength();
            totalCount += pageCount;
            // Read pagination information
            isLast = root.GetProperty("isLast").GetBoolean();
            //Console.WriteLine($"Retrieved {pageCount} issues. Running total: {totalCount}");

            if (!isLast) {
                if (!root.TryGetProperty("nextPageToken", out JsonElement token)) {
                    throw new Exception("No nextPageToken returned by Jira.");
                }
                nextPageToken = token.GetString();
                if (string.IsNullOrEmpty(nextPageToken)) {
                    throw new Exception("Invalid nextPageToken returned by Jira.");
                }
            }
        }
        return totalCount;
    }
}
