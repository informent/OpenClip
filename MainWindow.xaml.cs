using System.Windows;
using System.Windows.Threading;
using Forms = System.Windows.Forms;
using StackPanel = System.Windows.Controls.StackPanel;
namespace OpenClip;
public partial class MainWindow : Window
{
    private readonly ClipStore store = new(); private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromMilliseconds(700) }; private ClipItem? selected; private string lastClipboard = "";
    public MainWindow() { InitializeComponent(); store.Load(); Refresh(); timer.Tick += (_, _) => CaptureClipboard(); timer.Start(); if (ClipList.Parent is System.Windows.Controls.Panel panel) { var pin = new System.Windows.Controls.Button { Content = "Pin / unpin selected", Margin = new Thickness(0, 10, 8, 0) }; pin.Click += Pin_Click; var clear = new System.Windows.Controls.Button { Content = "Clear unpinned", Margin = new Thickness(0, 10, 0, 0) }; clear.Click += Clear_Click; panel.Children.Add(new StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal, Children = { pin, clear } }); } }
    private void CaptureClipboard() { try { if (!Forms.Clipboard.ContainsText()) return; var text = Forms.Clipboard.GetText(); if (text != lastClipboard) { lastClipboard = text; if (store.Add(text)) Refresh(); } } catch { } }
    private void Refresh() { var q = SearchBox?.Text?.Trim() ?? ""; ClipList.ItemsSource = store.Items.Where(x => string.IsNullOrEmpty(q) || x.Text.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(x => $"{x.CapturedAt:g}    {x.Text.Replace(Environment.NewLine, " ")}").ToArray(); CountText.Text = $"{store.Items.Count:N0} items"; }
    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => Refresh();
    private void ClipList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) { if (ClipList.SelectedIndex < 0) return; selected = store.Search(SearchBox.Text).ElementAtOrDefault(ClipList.SelectedIndex); if (selected is not null) { SelectedMeta.Text = $"{(selected.IsPinned ? "Pinned · " : "")}{selected.CapturedAt:dddd, MMM d · h:mm tt}"; PreviewText.Text = selected.Text; } }
    private void Pin_Click(object sender, RoutedEventArgs e) { if (selected is not null && store.TogglePin(selected.Id)) { Refresh(); StatusText("Pin updated"); } }
    private void Clear_Click(object sender, RoutedEventArgs e) { var removed = store.RemoveUnpinned(); selected = null; Refresh(); StatusText($"Removed {removed} unpinned items"); }
    private void Copy_Click(object sender, RoutedEventArgs e) { if (selected is null) return; Forms.Clipboard.SetText(selected.Text); StatusText("Copied to clipboard"); }
    private void StatusText(string text) => Title = $"OpenClip  ·  {text}";
}
