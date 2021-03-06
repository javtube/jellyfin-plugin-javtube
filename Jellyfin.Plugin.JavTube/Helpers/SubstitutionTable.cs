namespace Jellyfin.Plugin.JavTube.Helpers;

public class SubstitutionTable : Dictionary<string, string>
{
    private SubstitutionTable() : base(StringComparer.OrdinalIgnoreCase)
    {
    }

    public static SubstitutionTable Parse(string text)
    {
        var dictionary = new SubstitutionTable();

        var reader = new StringReader(text ?? string.Empty);
        while (reader.ReadLine() is { } line)
        {
            var kvp = line.Split('=', 2).Select(s => s.Trim()).ToList();
            if (string.IsNullOrWhiteSpace(kvp.First()))
                continue;
            dictionary[kvp[0]] = kvp.Count switch
            {
                1 => null,
                2 => kvp[1],
                _ => dictionary[kvp[0]]
            };
        }

        return dictionary;
    }

    public override string ToString()
    {
        var table = this;
        return table.Any() != true
            ? string.Empty
            : string.Join('\n',
                table.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                    .Select(kvp => $"{kvp.Key?.Trim()}={kvp.Value?.Trim()}"));
    }
}