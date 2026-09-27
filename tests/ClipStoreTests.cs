using OpenClip;
var root = Path.Combine(Path.GetTempPath(), "openclip-" + Guid.NewGuid().ToString("N")); var store = new ClipStore(Path.Combine(root, "clips.json"));
if (!store.Add("alpha") || store.Add("alpha")) throw new Exception("Duplicate clipboard content was accepted.");
var id = store.Items[0].Id; if (!store.TogglePin(id) || !store.Items[0].IsPinned) throw new Exception("Pin toggle failed.");
store.Add("beta"); if (store.Search("ALP").Count != 1) throw new Exception("Search failed."); if (store.RemoveUnpinned() != 1 || store.Items.Count != 1) throw new Exception("Unpinned cleanup failed.");
var reloaded = new ClipStore(Path.Combine(root, "clips.json")); reloaded.Load(); if (reloaded.Items.Count != 1 || !reloaded.Items[0].IsPinned) throw new Exception("Persistence failed."); Directory.Delete(root, true); Console.WriteLine("PASS: clipboard storage, search, pinning, cleanup, and persistence");
