using KeePass.Plugins;
using KeePass.Util;
using KeePass.Util.Spr;
using KeePassLib;
using System;
using System.Diagnostics;

namespace AutoTypeParseFields
{
    public sealed class AutoTypeParseFieldsExt : Plugin
    {
        private IPluginHost _host = null;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false;
            _host = host;

            // Подсказка для справки (не обязательно)
            SprEngine.FilterPlaceholderHints.Add("{PARSE:<Field>:WORD:<N>} {PARSE:<Field>:BEFORE:<String>}");

            AutoType.FilterCompilePre += AutoType_FilterCompilePre;
            return true;
        }

        public override void Terminate()
        {
            AutoType.FilterCompilePre -= AutoType_FilterCompilePre;
            _host = null;
        }

        private void AutoType_FilterCompilePre(object sender, AutoTypeEventArgs e)
        {
            try
            {
                if (e == null || string.IsNullOrEmpty(e.Sequence)) return;
                e.Sequence = ReplaceParseTags(e.Sequence, e.Entry);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ParseFields plugin error: " + ex);
            }
        }

        private string ReplaceParseTags(string text, PwEntry entry)
        {
            if (string.IsNullOrEmpty(text)) return text;

            int start = 0;
            while (true)
            {
                int open = text.IndexOf("{PARSE:", start, StringComparison.OrdinalIgnoreCase);
                if (open < 0) break;

                int close = text.IndexOf("}", open);
                if (close < 0) break;

                string tag = text.Substring(open + 7, close - (open + 7));
                string replacement = ParseTag(tag, entry);

                text = text.Substring(0, open) + replacement + text.Substring(close + 1);
                start = open + replacement.Length;
            }

            return text;
        }

        private string ParseTag(string tag, PwEntry entry)
        {
            try
            {
                string[] parts = tag.Split(':');
                if (parts.Length < 2) return string.Empty;

                string field = parts[0].ToUpperInvariant();
                string operation = parts[1].ToUpperInvariant();
                string param = (parts.Length > 2 ? parts[2] : null);

                string value = GetFieldValue(entry, field);
                if (string.IsNullOrEmpty(value)) return string.Empty;

                switch (operation)
                {
                    case "WORD":
                        int n;
                        if (int.TryParse(param, out n))
                        {
                            string[] words = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (n > 0 && n <= words.Length)
                                return words[n - 1];
                        }
                        return string.Empty;

                    case "BEFORE":
                        if (!string.IsNullOrEmpty(param))
                        {
                            int idx = value.IndexOf(param, StringComparison.Ordinal);
                            if (idx >= 0) return value.Substring(0, idx);
                        }
                        return string.Empty;

                    default:
                        return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetFieldValue(PwEntry entry, string field)
        {
            switch (field)
            {
                case "TITLE": return entry.Strings.ReadSafe(PwDefs.TitleField);
                case "USERNAME": return entry.Strings.ReadSafe(PwDefs.UserNameField);
                case "PASSWORD": return entry.Strings.ReadSafe(PwDefs.PasswordField);
                case "URL": return entry.Strings.ReadSafe(PwDefs.UrlField);
                case "NOTES": return entry.Strings.ReadSafe(PwDefs.NotesField);
                default:
                    // Кастомное поле
                    return entry.Strings.ReadSafe(field);
            }
        }
    }
}
