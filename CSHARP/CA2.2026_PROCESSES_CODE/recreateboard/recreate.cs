
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Globalization;
using System.Text.RegularExpressions;

class Program
{
    const string ProjectKey = "CA2SPACECW";
    const string JIRA_BASE_URL = "https://atu-team-yavca7kp.atlassian.net";

    static readonly HttpClient client = new();

    record BoardColumn(string Name);
    record StatusChange(
        DateTimeOffset Created,
        string? FromId,
        string? FromName
    );

    record WorkItem(
        string Key,
        string Summary,
        string Priority,
        DateTimeOffset Created,
        string? StatusId,
        string? StatusName
    );

    static async Task Main(string[] args)
    {
        try
        {
if (args.Length != 1 ||
    !DateTime.TryParseExact(
        args[0],
        "yyyy-MM-dd HH:mm",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateTime snapshotDate))
{
    Console.WriteLine(
        "Usage: dotnet run -- \"yyyy-MM-dd HH:mm\"");
    return;
}
            string baseUrl = JIRA_BASE_URL;

            string email =
                Environment.GetEnvironmentVariable("JIRA_EMAIL")
                ?? throw new Exception("JIRA_EMAIL is missing.");

            string token =
                Environment.GetEnvironmentVariable("JIRA_API_TOKEN")
                ?? throw new Exception("JIRA_API_TOKEN is missing.");

            client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");

            string credentials =
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{email}:{token}"));

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // The requested date is inclusive.
            // Use the following local midnight as the exclusive cutoff.
DateTimeOffset cutoff = new(
    snapshotDate,
    TimeZoneInfo.Local.GetUtcOffset(snapshotDate));
            Console.WriteLine($"Reconstructing board: {snapshotDate}");
            Console.WriteLine();

            // 1. Find the Kanban board.
            string boardId = await GetBoardId();

            // 2. Retrieve its configuration.
            using JsonDocument config =
                await GetJson(
                    $"rest/agile/1.0/board/{boardId}/configuration");

            JsonElement root = config.RootElement;

            // 3. Retrieve the saved filter used by the board.
            string filterId =
                GetString(root, "filter", "id")
                ?? throw new Exception("Board filter ID not found.");

            using JsonDocument filter =
                await GetJson($"rest/api/3/filter/{filterId}");

            string filterJql =
                GetString(filter.RootElement, "jql")
                ?? throw new Exception("Board filter JQL not found.");

            // Remove ORDER BY from the saved filter before combining JQL.
            filterJql = Regex.Replace(
                filterJql,
                @"\s+ORDER\s+BY\s+[\s\S]*$",
                "",
                RegexOptions.IgnoreCase);

            string subQuery =
                GetString(root, "subQuery") ?? "";

            string jql = string.IsNullOrWhiteSpace(subQuery)
                ? filterJql
                : $"({filterJql}) AND ({subQuery})";

            jql += " ORDER BY key ASC";

            // 4. Read the board's columns and status mappings.
            var columns = new List<string>();

            var statusToColumn =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

            var statusNameToColumn =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

            JsonElement columnArray =
                root.GetProperty("columnConfig")
                    .GetProperty("columns");

            foreach (JsonElement column in columnArray.EnumerateArray())
            {
                string columnName =
                    GetString(column, "name") ?? "Unnamed";

                columns.Add(columnName);

                if (!column.TryGetProperty(
                    "statuses", out JsonElement statuses))
                    continue;

                foreach (JsonElement status in statuses.EnumerateArray())
                {
                    string? id = GetString(status, "id");
                    string? name = GetString(status, "name");

                    if (id != null)
                        statusToColumn[id] = columnName;

                    if (name != null)
                        statusNameToColumn[name] = columnName;
                }
            }

            // 5. Retrieve all work items matching the board filter.
            List<WorkItem> issues =
                await GetAllIssues(jql, cutoff);

            Console.WriteLine(
                $"Work items found: {issues.Count}");

            // 6. Reconstruct each item's status at the requested date.
            var board =
                new Dictionary<string, List<WorkItem>>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (string column in columns)
                board[column] = new List<WorkItem>();

            var unmapped = new List<WorkItem>();

            foreach (WorkItem issue in issues)
            {
                string? statusId = issue.StatusId;
                string? statusName = issue.StatusName;

                List<StatusChange> changes =
                    await GetStatusChanges(issue.Key);

                // Reverse every status transition occurring AFTER
                // the requested snapshot date.
                foreach (StatusChange change in changes
                    .Where(c => c.Created >= cutoff)
                    .OrderByDescending(c => c.Created))
                {
                    if (change.FromId != null)
                        statusId = change.FromId;

                    statusName = change.FromName ?? statusName;
                }

                string? columnName = null;

                if (statusId != null)
                    statusToColumn.TryGetValue(
                        statusId, out columnName);

                if (columnName == null && statusName != null)
                    statusNameToColumn.TryGetValue(
                        statusName, out columnName);

                if (columnName != null &&
                    board.ContainsKey(columnName))
                {
                    board[columnName].Add(issue);
                }
                else
                {
                    unmapped.Add(issue);
                }
            }

            // 7. Display the reconstructed board.
            Console.WriteLine();
            Console.WriteLine(
                $"========== BOARD: {snapshotDate} ==========");

            foreach (string column in columns)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"--- {column} ({board[column].Count}) ---");

                foreach (WorkItem issue in board[column])
                {
                    PrintIssue(issue);
                }
            }

            if (unmapped.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"--- UNMAPPED ({unmapped.Count}) ---");

                foreach (WorkItem issue in unmapped)
                    PrintIssue(issue);
            }

            Console.WriteLine();
            Console.WriteLine("========== END OF BOARD ==========");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    // Find the Kanban board associated with the project.

static async Task<string> GetBoardId()
{
    return "3";
}
//////////////////

    // Retrieve all issues matching the board's JQL filter.
    static async Task<List<WorkItem>> GetAllIssues(
        string jql, DateTimeOffset cutoff)
    {
        var result = new List<WorkItem>();

        string? nextPageToken = null;

        do
        {
            var requestBody = new Dictionary<string, object>
            {
                ["jql"] = jql,
                ["fields"] = new[]
                {
                    "summary", "status", "priority", "created"
                },
                ["maxResults"] = 100
            };

            if (nextPageToken != null)
                requestBody["nextPageToken"] = nextPageToken;

            using var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response =
                await client.PostAsync(
                    "rest/api/3/search/jql", content);

            using JsonDocument doc =
                await ReadResponse(response);

            JsonElement root = doc.RootElement;

            foreach (JsonElement issue in
                root.GetProperty("issues").EnumerateArray())
            {
                JsonElement fields = issue.GetProperty("fields");

                string key = GetString(issue, "key") ?? "";
                string summary =
                    GetString(fields, "summary") ?? "";

                string priority =
                    GetString(fields, "priority", "name")
                    ?? "Unassigned";

                string? statusId =
                    GetString(fields, "status", "id");

                string? statusName =
                    GetString(fields, "status", "name");

                string createdText =
                    GetString(fields, "created") ?? "";

                DateTimeOffset created =
                    DateTimeOffset.Parse(
                        createdText,
                        CultureInfo.InvariantCulture);

                // Exclude items that did not yet exist.
                if (created >= cutoff)
                    continue;

                result.Add(new WorkItem(
                    key, summary, priority, created,
                    statusId, statusName));
            }

            nextPageToken =
                GetString(root, "nextPageToken");

        } while (!string.IsNullOrEmpty(nextPageToken));

        return result;
    }

    // Retrieve every page of an issue's changelog.
    static async Task<List<StatusChange>> GetStatusChanges(
        string issueKey)
    {
        var changes = new List<StatusChange>();

        int startAt = 0;
        const int pageSize = 100;

        while (true)
        {
            using JsonDocument doc = await GetJson(
                $"rest/api/3/issue/{issueKey}/changelog" +
                $"?startAt={startAt}&maxResults={pageSize}");

            JsonElement root = doc.RootElement;

            JsonElement histories =
                root.GetProperty("values");

            int count = 0;

            foreach (JsonElement history in
                histories.EnumerateArray())
            {
                count++;

                DateTimeOffset created =
                    DateTimeOffset.Parse(
                        GetString(history, "created")!,
                        CultureInfo.InvariantCulture);

                foreach (JsonElement item in
                    history.GetProperty("items").EnumerateArray())
                {
                    if (!string.Equals(
                        GetString(item, "field"),
                        "status",
                        StringComparison.OrdinalIgnoreCase))
                        continue;

                    changes.Add(new StatusChange(
                        created,
                        GetString(item, "from"),
                        GetString(item, "fromString")
                    ));
                }
            }

            startAt += count;

            if (count == 0 ||
                (root.TryGetProperty("total", out JsonElement total)
                 && startAt >= total.GetInt32()) ||
                (root.TryGetProperty("isLast", out JsonElement last)
                 && last.GetBoolean()))
                break;
        }

        return changes;
    }

    static async Task<JsonDocument> GetJson(string endpoint)
    {
        using HttpResponseMessage response =
            await client.GetAsync(endpoint);

        return await ReadResponse(response);
    }

    static async Task<JsonDocument> ReadResponse(
        HttpResponseMessage response)
    {
        string json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Jira API error {(int)response.StatusCode}: {json}");
        }

        return JsonDocument.Parse(json);
    }

    static string? GetString(
        JsonElement element, params string[] path)
    {
        foreach (string property in path)
        {
            if (!element.TryGetProperty(
                property, out JsonElement next))
                return null;

            element = next;
        }

        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.ToString(),
            _ => null
        };
    }

    static void PrintIssue(WorkItem issue)
    {
        Console.WriteLine(
            $"{issue.Key,-20} | {issue.Priority,-12} | " +
            $"{issue.Summary}");
    }
}
