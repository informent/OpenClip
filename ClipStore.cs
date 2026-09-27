using System.IO;
using System.Text.Json;
namespace OpenClip;
public sealed record ClipItem(string Id, DateTime CapturedAt, string Text, bool IsPinned);
public sealed class ClipStore
{
    private readonly string path;
    public List<ClipItem> Items { get; private set; } = new();
    public ClipStore(string? storagePath = null) => path = storagePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OpenClip", "clips.json");
    public void Load() { try { if (File.Exists(path)) Items = JsonSerializer.Deserialize<List<ClipItem>>(File.ReadAllText(path)) ?? new(); } catch { Items = new(); } }
    public void Save() { Directory.CreateDirectory(Path.GetDirectoryName(path)!); File.WriteAllText(path, JsonSerializer.Serialize(Items, new JsonSerializerOptions { WriteIndented = true })); }
    public bool Add(string text) { if (string.IsNullOrWhiteSpace(text) || Items.Any(x => x.Text == text)) return false; Items.Insert(0, new ClipItem(Guid.NewGuid().ToString("N"), DateTime.Now, text, false)); Save(); return true; }
    public bool TogglePin(string id) { var index = Items.FindIndex(x => x.Id == id); if (index < 0) return false; Items[index] = Items[index] with { IsPinned = !Items[index].IsPinned }; Save(); return true; }
    public int RemoveUnpinned() { var old = Items.Count; Items.RemoveAll(x => !x.IsPinned); Save(); return old - Items.Count; }
    public int ClearAll(bool keepPinned) { var old = Items.Count; if (keepPinned) Items.RemoveAll(x => !x.IsPinned); else Items.Clear(); Save(); return old - Items.Count; }
    public IReadOnlyList<ClipItem> Search(string query) => Items.Where(x => string.IsNullOrWhiteSpace(query) || x.Text.Contains(query.Trim(), StringComparison.OrdinalIgnoreCase)).ToArray();
}
