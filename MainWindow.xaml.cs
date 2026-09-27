using System.Windows;
using System.Windows.Threading;
using Forms = System.Windows.Forms;
namespace OpenClip;
public partial class MainWindow : Window
{
    private readonly ClipStore store = new(); private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromMilliseconds(700) }; private ClipItem? selected; private string lastClipboard = "";
    public MainWindow() { InitializeComponent(); store.Load(); Refresh(); timer.Tick += (_, _) => CaptureClipboard(); timer.Start(); }
    private void CaptureClipboard() { try { if (!Forms.Clipboard.ContainsText()) return; var text = Forms.Clipboard.GetText(); if (text != lastClipboard) { lastClipboard = text; if (store.Add(text)) Refresh(); } } catch { } }
    private void Refresh() { var q = SearchBox?.Text?.Trim() ?? ""; ClipList.ItemsSource = store.Items.Where(x => string.IsNullOrEmpty(q) || x.Text.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(x => $"{x.CapturedAt:g}    {x.Text.Replace(Environment.NewLine, " ")}").ToArray(); CountText.Text = $"{store.Items.Count:N0} items"; }
    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => Refresh();
    private void ClipList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { if (ClipList.SelectedIndex < 0) return; selected = store.Items.Where(x => string.IsNullOrEmpty(SearchBox.Text) || x.Text.Contains(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase)).ElementAtOrDefault(ClipList.SelectedIndex); if (selected is not null) { SelectedMeta.Text = selected.CapturedAt.ToString("dddd, MMM d · h:mm tt"); PreviewText.Text = selected.Text; } }
    private void Copy_Click(object sender, RoutedEventArgs e) { if (selected is null) return; Forms.Clipboard.SetText(selected.Text); StatusText("Copied to clipboard"); }
    private void StatusText(string text) => Title = $"OpenClip  ·  {text}";
}
