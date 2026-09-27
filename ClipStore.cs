using System.Text.Json;
using System.IO;
namespace OpenClip;
public sealed record ClipItem(string Id, DateTime CapturedAt, string Text, bool IsPinned);
public sealed class ClipStore
{
    private readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenClip", "clips.json");
    public List<ClipItem> Items { get; private set; } = new();
    public void Load() { try { if (File.Exists(path)) Items = JsonSerializer.Deserialize<List<ClipItem>>(File.ReadAllText(path)) ?? new(); } catch { Items = new(); } }
    public void Save() { Directory.CreateDirectory(Path.GetDirectoryName(path)!); File.WriteAllText(path, JsonSerializer.Serialize(Items, new JsonSerializerOptions { WriteIndented = true })); }
    public bool Add(string text) { if (string.IsNullOrWhiteSpace(text) || Items.Any(x => x.Text == text)) return false; Items.Insert(0, new ClipItem(Guid.NewGuid().ToString("N"), DateTime.Now, text, false)); Save(); return true; }
}
