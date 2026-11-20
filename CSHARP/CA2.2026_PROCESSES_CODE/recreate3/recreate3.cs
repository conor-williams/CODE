
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

// ==================================================
// CONFIGURATION
// ==================================================

const string JIRA_BASE_URL =
    "https://atu-team-yavca7kp.atlassian.net";

const string PROJECT_KEY = "CA2SPACECW";

const int BOARD_ID = 3;
const int WIP_LIMIT = 2;
const int COLUMN_WIDTH = 13;

// ==================================================
// DATE AND TIME
// ==================================================

if (args.Length != 1 ||
    !DateTime.TryParseExact(
        args[0],
        "yyyy-MM-dd HH:mm",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateTime cutoff))
{
    Console.WriteLine(
        "Usage: dotnet run -- \"yyyy-MM-dd HH:mm\"");
    return;
}

DateTimeOffset cutoffOffset = new(
    cutoff,
    TimeZoneInfo.Local.GetUtcOffset(cutoff));

// ==================================================
// AUTHENTICATION
// ==================================================

string email =
    Environment.GetEnvironmentVariable("JIRA_EMAIL") ?? "";

string token =
    Environment.GetEnvironmentVariable("JIRA_API_TOKEN") ?? "";

if (email == "" || token == "")
{
    Console.WriteLine(
        "ERROR: JIRA_EMAIL or JIRA_API_TOKEN is missing.");
    return;
}

string credentials = Convert.ToBase64String(
    Encoding.UTF8.GetBytes($"{email}:{token}"));

using HttpClient client = new();

client.BaseAddress = new Uri(JIRA_BASE_URL);

client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Basic", credentials);

client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

// ==================================================
// DEFINE BOARD COLUMNS
// ==================================================

List<BoardColumn> columns = new()
{
    new BoardColumn("Backlog", "10007"),
    new BoardColumn("To Do", "10008"),
    new BoardColumn("In Progress", "3"),
    new BoardColumn("Testing", "10009"),
    new BoardColumn("Done", "10010")
};

// ==================================================
// GET ALL PROJECT ISSUES
// ==================================================

List<JiraIssue> issues = new();

string jql =
    $"project = \"{PROJECT_KEY}\" ORDER BY created ASC";

string? nextPageToken = null;

do
{
    Dictionary<string, object> requestBody = new()
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

    using HttpResponseMessage response = await client.PostAsync(
        "/rest/api/3/search/jql",
        new StringContent(
            jsonBody,
            Encoding.UTF8,
            "application/json"));

    response.EnsureSuccessStatusCode();

    using JsonDocument document = JsonDocument.Parse(
        await response.Content.ReadAsStringAsync());

    JsonElement root = document.RootElement;

    foreach (JsonElement item in
             root.GetProperty("issues").EnumerateArray())
    {
        string key =
            item.GetProperty("key").GetString() ?? "";

        JsonElement fields = item.GetProperty("fields");

        DateTimeOffset created = DateTimeOffset.Parse(
            fields.GetProperty("created").GetString()!,
            CultureInfo.InvariantCulture);

        // Exclude issues created after the snapshot.

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
        root.TryGetProperty("nextPageToken", out JsonElement next)
            ? next.GetString()
            : null;

}
while (!string.IsNullOrEmpty(nextPageToken));

Console.WriteLine($"Issues retrieved: {issues.Count}");

// ==================================================
// RECONSTRUCT HISTORICAL STATUS
// ==================================================

foreach (JiraIssue issue in issues)
{
    await ReconstructStatus(issue);
}

// ==================================================
// ASSIGN ISSUES TO BOARD COLUMNS
// ==================================================

List<JiraIssue> unmappedIssues = new();

foreach (JiraIssue issue in issues)
{
    BoardColumn? matchingColumn = columns.FirstOrDefault(
        c => c.StatusId == issue.StatusId);

    if (matchingColumn != null)
    {
        matchingColumn.Issues.Add(issue);
    }
    else
    {
        unmappedIssues.Add(issue);
    }
}

// ==================================================
// DISPLAY BOARD
// ==================================================

PrintBoard(columns);

if (unmappedIssues.Count > 0)
{
    Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine("\nUNMAPPED ISSUES:");

    foreach (JiraIssue issue in unmappedIssues)
    {
        Console.WriteLine(
            $"{issue.Key} - Status ID: {issue.StatusId}");
    }

    Console.ResetColor();
}

// ==================================================
// METHODS
// ==================================================

// --------------------------------------------------
// GET JSON FROM JIRA
// --------------------------------------------------

async Task<JsonDocument> GetJson(string endpoint)
{
    using HttpResponseMessage response =
        await client.GetAsync(endpoint);

    response.EnsureSuccessStatusCode();

    string json =
        await response.Content.ReadAsStringAsync();

    return JsonDocument.Parse(json);
}

// --------------------------------------------------
// RECONSTRUCT HISTORICAL STATUS
// --------------------------------------------------

async Task ReconstructStatus(JiraIssue issue)
{
    const int PAGE_SIZE = 100;

    int startAt = 0;

    List<StatusChange> changes = new();

    // Retrieve all changelog pages.

    while (true)
    {
        string endpoint =
            $"/rest/api/3/issue/{issue.Key}/changelog" +
            $"?startAt={startAt}&maxResults={PAGE_SIZE}";

        using JsonDocument document = await GetJson(endpoint);

        JsonElement root = document.RootElement;

        foreach (JsonElement history in
                 root.GetProperty("values").EnumerateArray())
        {
            DateTimeOffset changeDate = DateTimeOffset.Parse(
                history.GetProperty("created").GetString()!,
                CultureInfo.InvariantCulture);

            // Only changes AFTER the requested snapshot
            // need to be reversed.

            if (changeDate <= cutoffOffset)
                continue;

            foreach (JsonElement item in
                     history.GetProperty("items").EnumerateArray())
            {
                string field =
                    item.GetProperty("field").GetString() ?? "";

                if (field != "status")
                    continue;

                string from = item.TryGetProperty(
                    "from", out JsonElement fromElement)
                    ? fromElement.GetString() ?? ""
                    : "";

                string to = item.TryGetProperty(
                    "to", out JsonElement toElement)
                    ? toElement.GetString() ?? ""
                    : "";

                if (from == "" || to == "")
                    continue;

                changes.Add(new StatusChange(
                    changeDate,
                    from,
                    to));
            }
        }

        int total = root.GetProperty("total").GetInt32();

        startAt += PAGE_SIZE;

        if (startAt >= total)
            break;
    }

    // Reverse changes in descending chronological order.
    // This restores the status at the requested date/time.

    foreach (StatusChange change in
             changes.OrderByDescending(c => c.Date))
    {
        if (issue.StatusId == change.To)
        {
            issue.StatusId = change.From;
        }
    }
}

// --------------------------------------------------
// PRINT BOARD
// --------------------------------------------------

void PrintBoard(List<BoardColumn> boardColumns)
{
    Console.WriteLine();
    Console.WriteLine(
        $"CA2SPACECW - Board {BOARD_ID}");

    Console.WriteLine(
        $"Historical snapshot: {cutoff:yyyy-MM-dd HH:mm}");

    Console.WriteLine();

    // Sort issues in each column.

    foreach (BoardColumn column in boardColumns)
    {
        column.Issues = column.Issues
            .OrderBy(i => i.Key)
            .ToList();
    }

    // ----------------------------------------------
    // COLUMN HEADINGS
    // ----------------------------------------------

    foreach (BoardColumn column in boardColumns)
    {
        Console.ForegroundColor =
            GetColumnColour(column.Name);

        Console.Write(column.Name.PadRight(COLUMN_WIDTH));
    }

    Console.ResetColor();
    Console.WriteLine();

    // ----------------------------------------------
    // WIP COUNTS
    // ----------------------------------------------

    foreach (BoardColumn column in boardColumns)
    {
        bool showWip =
            column.Name != "Backlog" &&
            column.Name != "Done";

        string wip = showWip
            ? $"WIP {column.Issues.Count}/{WIP_LIMIT}"
            : "";

        Console.ForegroundColor =
            GetColumnColour(column.Name);

        Console.Write(wip.PadRight(COLUMN_WIDTH));
    }

    Console.ResetColor();
    Console.WriteLine();

    // ----------------------------------------------
    // SEPARATOR
    // ----------------------------------------------

    Console.WriteLine(
        new string('-', COLUMN_WIDTH * boardColumns.Count));

    // ----------------------------------------------
    // ISSUES
    // ----------------------------------------------

    int maxRows = boardColumns.Max(
        c => c.Issues.Count);

    for (int row = 0; row < maxRows; row++)
    {
        foreach (BoardColumn column in boardColumns)
        {
            string value = row < column.Issues.Count
                ? column.Issues[row].Key
                : "";

            Console.ForegroundColor =
                GetColumnColour(column.Name);

            Console.Write(value.PadRight(COLUMN_WIDTH));
        }

        Console.ResetColor();
        Console.WriteLine();
    }

    // ----------------------------------------------
    // TOTAL
    // ----------------------------------------------

//    Console.WriteLine();

//    Console.WriteLine( $"Total issues: {boardColumns.Sum(c => c.Issues.Count)}");

    Console.ResetColor();
}

// --------------------------------------------------
// COLUMN COLOURS
// --------------------------------------------------

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
    public string StatusId { get; set; }

    public List<JiraIssue> Issues { get; set; }

    public BoardColumn(string name, string statusId)
    {
        Name = name;
        StatusId = statusId;
        Issues = new List<JiraIssue>();
    }
}

record StatusChange(
    DateTimeOffset Date,
    string From,
    string To);

