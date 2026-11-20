
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

const string JIRA_BASE_URL = "https://atu-team-yavca7kp.atlassian.net";
const string PROJECT_KEY = "CA2SPACECW";
const int BOARD_ID = 3;
const int WIP_LIMIT = 2;

const int MAX_COLUMN_WIDTH = 13;

if (args.Length != 1 ||
    !DateTime.TryParseExact(
        args[0],
        "yyyy-MM-dd HH:mm",
        null,
        System.Globalization.DateTimeStyles.None,
        out DateTime cutoff))
{
    Console.WriteLine(
        "Usage: dotnet run -- \"yyyy-MM-dd HH:mm\"");
    return;
}

string email = Environment.GetEnvironmentVariable("JIRA_EMAIL") ?? "";
string token = Environment.GetEnvironmentVariable("JIRA_API_TOKEN") ?? "";

if (email == "" || token == "")
{
    Console.WriteLine("JIRA_EMAIL or JIRA_API_TOKEN is missing.");
    return;
}

var credentials = Convert.ToBase64String(
    Encoding.UTF8.GetBytes($"{email}:{token}"));

using HttpClient client = new();

client.BaseAddress = new Uri(JIRA_BASE_URL);
client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Basic", credentials);

client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

var cutoffOffset = new DateTimeOffset(
    cutoff,
    TimeZoneInfo.Local.GetUtcOffset(cutoff));

var columns = new List<BoardColumn>();
var issues = new List<JiraIssue>();

/*
// --------------------------------------------------
// GET BOARD CONFIGURATION
// --------------------------------------------------

JsonDocument boardConfig = await GetJson(
    $"/rest/agile/1.0/board/{BOARD_ID}/configuration");

var columnConfig = boardConfig.RootElement
    .GetProperty("columnConfig")
    .GetProperty("columns");

Console.WriteLine(
    JsonSerializer.Serialize(
        boardConfig.RootElement,
        new JsonSerializerOptions { WriteIndented = true }));
foreach (var column in columnConfig.EnumerateArray())
{
    string name = column.GetProperty("name").GetString() ?? "";

    var statuses = new HashSet<string>();

    if (column.TryGetProperty("statuses", out var statusArray))
    {
        foreach (var status in statusArray.EnumerateArray())
        {
            statuses.Add(
                status.GetProperty("id").GetString() ?? "");
        }
    }

    columns.Add(new BoardColumn(name, statuses));
}

boardConfig.Dispose();
*/
string jql =
    $"project = \"{PROJECT_KEY}\" " +
    "ORDER BY created ASC";
// --------------------------------------------------
// GET BOARD FILTER
// --------------------------------------------------

/*
JsonDocument board = await GetJson(
    $"/rest/agile/1.0/board/{BOARD_ID}");

if (!board.RootElement.TryGetProperty("filter", out var filterElement))
{
    Console.WriteLine("ERROR: Jira board response has no filter property.");
    Console.WriteLine("Actual Jira response:");
    Console.WriteLine(
        JsonSerializer.Serialize(
            board.RootElement,
            new JsonSerializerOptions { WriteIndented = true }));

    board.Dispose();
    return;
}

if (!filterElement.TryGetProperty("id", out var filterIdElement))
{
    Console.WriteLine("ERROR: Jira board filter has no ID.");
    board.Dispose();
    return;
}

int filterId = filterIdElement.GetInt32();

board.Dispose();

JsonDocument filter = await GetJson(
    $"/rest/api/3/filter/{filterId}");

string filterJql = filter.RootElement
    .GetProperty("jql")
    .GetString() ?? "";

filter.Dispose();

// Include the project restriction and cutoff date.
// The cutoff is applied again locally below.

string jql =
    $"({filterJql}) AND project = \"{PROJECT_KEY}\" " +
    $"AND created <= \"{cutoff:yyyy-MM-dd HH:mm}\" " +
    "ORDER BY created ASC";
*/

// --------------------------------------------------
// GET ALL ISSUES
// --------------------------------------------------

string? nextPageToken = null;

do
{
    var requestBody = new Dictionary<string, object>
    {
        ["jql"] = jql,
        ["maxResults"] = 100,
        ["fields"] = new[]
        {
            "summary",
            "status",
            "priority",
            "created"
        }
    };

    if (nextPageToken != null)
        requestBody["nextPageToken"] = nextPageToken;

    string jsonBody = JsonSerializer.Serialize(requestBody);

    using var response = await client.PostAsync(
        "/rest/api/3/search/jql",
        new StringContent(
            jsonBody,
            Encoding.UTF8,
            "application/json"));

    response.EnsureSuccessStatusCode();

    using var document = JsonDocument.Parse(
        await response.Content.ReadAsStringAsync());

    var root = document.RootElement;

    foreach (var item in root.GetProperty("issues").EnumerateArray())
    {
        string key = item.GetProperty("key").GetString() ?? "";

        var fields = item.GetProperty("fields");

        DateTimeOffset created = DateTimeOffset.Parse(
            fields.GetProperty("created").GetString()!);

        if (created > cutoffOffset)
            continue;

        string summary =
            fields.GetProperty("summary").GetString() ?? "";

        string statusId =
            fields.GetProperty("status")
                .GetProperty("id").GetString() ?? "";

        string priority =
            fields.GetProperty("priority").ValueKind ==
                JsonValueKind.Null
                ? "None"
                : fields.GetProperty("priority")
                    .GetProperty("name").GetString() ?? "None";

        issues.Add(new JiraIssue(
            key,
            summary,
            priority,
            statusId,
            created));
    }

    nextPageToken =
        root.TryGetProperty("nextPageToken", out var next)
            ? next.GetString()
            : null;

}
while (!string.IsNullOrEmpty(nextPageToken));


Console.WriteLine("\n--- JIRA STATUS LOOKUP ---");

foreach (string statusId in issues
             .Select(i => i.StatusId)
             .Distinct())
{
    using var statusDocument = await GetJson(
        $"/rest/api/3/status/{statusId}");

    var status = statusDocument.RootElement;

    Console.WriteLine(
        $"{statusId} = {status.GetProperty("name").GetString()}");
}

Console.WriteLine("--- END STATUS LOOKUP ---\n");

Console.WriteLine("\n--- JIRA STATUS LOOKUP ---");

string[] statusIds = { "10010", "3", "10008", "10007" };

foreach (string statusId in statusIds)
{
    using var statusDocument = await GetJson(
        $"/rest/api/3/status/{statusId}");

    var status = statusDocument.RootElement;

    Console.WriteLine(
        $"{statusId} = {status.GetProperty("name").GetString()}");
}

Console.WriteLine("--- END STATUS LOOKUP ---\n");
Console.WriteLine("\n--- PROJECT STATUSES ---");

using var projectStatuses = await GetJson(
    $"/rest/api/3/project/{PROJECT_KEY}/statuses");

foreach (var issueType in projectStatuses.RootElement.EnumerateArray())
{
    Console.WriteLine(
        $"\nIssue type: {issueType.GetProperty("name").GetString()}");

    foreach (var status in issueType.GetProperty("statuses").EnumerateArray())
    {
        Console.WriteLine(
            $"{status.GetProperty("id").GetString()} - " +
            $"{status.GetProperty("name").GetString()}");
    }
}
// --------------------------------------------------
// RECONSTRUCT HISTORICAL STATUS
// --------------------------------------------------

foreach (var issue in issues)
{
    await ReconstructStatus(issue);
}

// --------------------------------------------------
// ASSIGN ISSUES TO COLUMNS
// --------------------------------------------------

Console.WriteLine("\n--- DIAGNOSTICS ---");

Console.WriteLine($"Issues retrieved: {issues.Count}");
Console.WriteLine($"Board columns retrieved: {columns.Count}");

Console.WriteLine("\nBOARD COLUMNS:");

foreach (var column in columns)
{
    Console.WriteLine(
        $"{column.Name}: {string.Join(", ", column.StatusIds)}");
}

Console.WriteLine("\nISSUE STATUSES:");

foreach (var issue in issues)
{
    Console.WriteLine(
        $"{issue.Key}: Status ID = {issue.StatusId}");
}

Console.WriteLine("--- END DIAGNOSTICS ---\n");
foreach (var issue in issues)
{
    string columnName = "Backlog";

    foreach (var column in columns)
    {
        if (column.StatusIds.Contains(issue.StatusId))
        {
            columnName = column.Name;
            break;
        }
    }

    // Add any board columns not represented in the
    // configuration only if required by the board.

    var matchingColumn = columns.FirstOrDefault(
        c => c.Name.Equals(
            columnName,
            StringComparison.OrdinalIgnoreCase));

    if (matchingColumn != null)
        matchingColumn.Issues.Add(issue);
}

// --------------------------------------------------
// OUTPUT
// --------------------------------------------------

PrintBoard(columns);

// ==================================================
// METHODS
// ==================================================

async Task<JsonDocument> GetJson(string endpoint)
{
    using var response = await client.GetAsync(endpoint);

    response.EnsureSuccessStatusCode();

    return JsonDocument.Parse(
        await response.Content.ReadAsStringAsync());
}

async Task ReconstructStatus(JiraIssue issue)
{
    int startAt = 0;
    const int pageSize = 100;

    var changes = new List<StatusChange>();

    while (true)
    {
        string endpoint =
            $"/rest/api/3/issue/{issue.Key}/changelog" +
            $"?startAt={startAt}&maxResults={pageSize}";

        using var document = await GetJson(endpoint);

        var root = document.RootElement;

        foreach (var history in root.GetProperty("values")
                     .EnumerateArray())
        {
            DateTimeOffset changeDate =
                DateTimeOffset.Parse(
                    history.GetProperty("created").GetString()!);

            if (changeDate <= cutoffOffset)
                continue;

            foreach (var item in history
                         .GetProperty("items").EnumerateArray())
            {
                if (item.GetProperty("field")
                    .GetString() != "status")
                    continue;

                string from = item.TryGetProperty("from", out var f)
                    ? f.GetString() ?? ""
                    : "";

                string to = item.TryGetProperty("to", out var t)
                    ? t.GetString() ?? ""
                    : "";

                changes.Add(new StatusChange(
                    changeDate,
                    from,
                    to));
            }
        }

        int total = root.GetProperty("total").GetInt32();

        startAt += pageSize;

        if (startAt >= total)
            break;
    }

    // Reverse changes made after the requested time.
    // The newest change must be undone first.

    foreach (var change in changes
                 .OrderByDescending(c => c.Date))
    {
        if (issue.StatusId == change.To)
            issue.StatusId = change.From;
    }
}

void PrintBoard(List<BoardColumn> boardColumns)
{
    Console.WriteLine();
    Console.WriteLine(
        $"CA2SPACECW - Board {BOARD_ID}");
    Console.WriteLine(
        $"Historical snapshot: {cutoff:yyyy-MM-dd HH:mm}");
    Console.WriteLine();

    foreach (var column in boardColumns)
    {
        int count = column.Issues.Count;

        bool showWip =
            !column.Name.Equals(
                "Backlog",
                StringComparison.OrdinalIgnoreCase)
            &&
            !column.Name.Equals(
                "Done",
                StringComparison.OrdinalIgnoreCase);

        string heading = showWip
            ? $"{column.Name} ({count}/{WIP_LIMIT})"
            : column.Name;

        Console.ForegroundColor = GetColumnColour(column.Name);

        Console.WriteLine(
            heading.PadRight(MAX_COLUMN_WIDTH));

        Console.WriteLine(
            new string('-', MAX_COLUMN_WIDTH));

        Console.ResetColor();

        if (count == 0)
        {
            Console.WriteLine("(empty)");
        }
        else
        {
            foreach (var issue in column.Issues
                         .OrderBy(i => i.Key))
            {
                Console.ForegroundColor =
                    GetColumnColour(column.Name);

                Console.WriteLine(
                    issue.Key.PadRight(MAX_COLUMN_WIDTH));

                Console.ResetColor();
            }
        }

        Console.WriteLine();
    }

    Console.WriteLine(
        $"Total issues: {boardColumns.Sum(c => c.Issues.Count)}");

    Console.ResetColor();
}

ConsoleColor GetColumnColour(string columnName)
{
    return columnName.ToLowerInvariant() switch
    {
        "backlog" => ConsoleColor.DarkGray,
        "to do" => ConsoleColor.Cyan,
        "in progress" => ConsoleColor.Yellow,
        "testing" => ConsoleColor.Magenta,
        "done" => ConsoleColor.Green,
        _ => ConsoleColor.White
    };
}

// ==================================================
// CLASSES
// ==================================================

class JiraIssue
{
    public string Key { get; set; }
    public string Summary { get; set; }
    public string Priority { get; set; }
    public string StatusId { get; set; }
    public DateTimeOffset Created { get; set; }

    public JiraIssue(
        string key,
        string summary,
        string priority,
        string statusId,
        DateTimeOffset created)
    {
        Key = key;
        Summary = summary;
        Priority = priority;
        StatusId = statusId;
        Created = created;
    }
}

class BoardColumn
{
    public string Name { get; set; }
    public HashSet<string> StatusIds { get; set; }
    public List<JiraIssue> Issues { get; set; }

    public BoardColumn(
        string name,
        HashSet<string> statusIds)
    {
        Name = name;
        StatusIds = statusIds;
        Issues = new List<JiraIssue>();
    }
}

record StatusChange(
    DateTimeOffset Date,
    string From,
    string To);

