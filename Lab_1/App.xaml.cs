using CodingSeb.Localization;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Lab_1
{
    public static class TranslationLoader
    {
        public static void Load(string path = "dictionary.json", string startLanguage = "ru")
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"Файл словаря не найден: {path}");

            string json = File.ReadAllText(path);
            var translations = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

            if (translations == null) return;

            foreach (var lang in translations)
            {
                foreach (var pair in lang.Value)
                {
                    if (!Loc.Instance.TranslationsDictionary.ContainsKey(pair.Key))
                    {
                        Loc.Instance.TranslationsDictionary[pair.Key] = new SortedDictionary<string, LocTranslation>();
                    }

                    Loc.Instance.TranslationsDictionary[pair.Key][lang.Key] = new LocTranslation { TranslatedText = pair.Value };
                }
            }
            Loc.Instance.CurrentLanguage = startLanguage;
        }
        
    }
    public partial class App : Application
        {
            protected override void OnStartup(StartupEventArgs e)
            {
                TranslationLoader.Load();
                base.OnStartup(e);
            }
        }
}