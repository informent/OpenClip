using OpenClip;
var root = Path.Combine(Path.GetTempPath(), "openclip-" + Guid.NewGuid().ToString("N")); var store = new ClipStore(Path.Combine(root, "clips.json"));
if (!store.Add("alpha") || store.Add("alpha")) throw new Exception("Duplicate clipboard content was accepted.");
var id = store.Items[0].Id; if (!store.TogglePin(id) || !store.Items[0].IsPinned) throw new Exception("Pin toggle failed.");
store.Add("beta"); if (store.Search("ALP").Count != 1) throw new Exception("Search failed."); if (store.RemoveUnpinned() != 1 || store.Items.Count != 1) throw new Exception("Unpinned cleanup failed.");
var reloaded = new ClipStore(Path.Combine(root, "clips.json")); reloaded.Load(); if (reloaded.Items.Count != 1 || !reloaded.Items[0].IsPinned) throw new Exception("Persistence failed."); Directory.Delete(root, true); Console.WriteLine("PASS: clipboard storage, search, pinning, cleanup, and persistence");
var privacyRoot = Path.Combine(Path.GetTempPath(), "openclip-privacy-" + Guid.NewGuid().ToString("N")); var privacyPath = Path.Combine(privacyRoot, "clips.json"); var privacy = new ClipStore(privacyPath); privacy.Add("pinned"); privacy.TogglePin(privacy.Items[0].Id); privacy.Add("private"); if (privacy.ClearAll(true) != 1 || privacy.Items.Count != 1 || !privacy.Items[0].IsPinned) throw new Exception("Privacy clear did not protect pinned content."); if (File.Exists(privacyPath + ".bak")) throw new Exception("Privacy clear left deleted text in a recovery copy."); if (privacy.ClearAll(false) != 1 || privacy.Items.Count != 0) throw new Exception("Privacy clear-all failed."); Directory.Delete(privacyRoot, true); Console.WriteLine("PASS: privacy clear actions protect pinned content and purge recovery copies");

var secureRoot = Path.Combine(Path.GetTempPath(), "openclip-secure-" + Guid.NewGuid().ToString("N")); var securePath = Path.Combine(secureRoot, "clips.json"); var secure = new ClipStore(securePath, 2);
if (secure.Add("password=hunter2") || secure.Add("-----BEGIN PRIVATE KEY-----\nsecret") || secure.Add("4111 1111 1111 1111")) throw new Exception("Sensitive clipboard content was persisted.");
secure.Add("first"); secure.TogglePin(secure.Items[0].Id); secure.Add("second"); secure.Add("third");
if (secure.Items.Count != 2 || secure.Items.All(x => x.Text != "first") || secure.Items.All(x => x.Text != "third")) throw new Exception("Bounded history did not preserve pinned content.");
secure.Add("fourth"); File.WriteAllText(securePath, "{ damaged json"); var recovered = new ClipStore(securePath, 2); recovered.Load();
if (recovered.Items.Count == 0 || recovered.Items.All(x => x.Text != "first")) throw new Exception("Backup recovery failed.");
Directory.Delete(secureRoot, true); Console.WriteLine("PASS: sensitive-content exclusion, bounded history, atomic save, and backup recovery");
