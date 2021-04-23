using System;
using System.Diagnostics;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ScreenShotBot.Properties;

namespace ScreenShotBot
{
    public static class Tools
    {
        private static readonly Regex _regexCounter = new(@"\{(0+)\}", RegexOptions.Compiled);

        #region Strings

        public static string Swap(this string s, params object[] replacements)
        {
            if (s == null)
            {
                Debug.Fail("Swap failed. String is null");
                return s;
            }

            for (var i = 0; i < replacements.Length; i++)
            {
                Debug.Assert(s.Contains($"{{{i}}}"), $"Swap failed. {{{i}}} not found.");
                s = s.Replace($"{{{i}}}", replacements[i].ToString());
            }

            Debug.Assert(!Regex.IsMatch(s, @"\{\d*\}"), "Swap failed. Something was not replaced.");

            return s;
        }

        public static string GetEnumString<T>(ResourceManager resourceManager, T value)
        {
            string id = $"{typeof(T).Name}_{value}";
            string str = resourceManager.GetString(id);

            if (str == null)
            {
                str = value.ToString();
                Debug.Fail($"Failed to get enum string for [{id}].");
            }

            return str;
        }

        public static void GetReadableDuration(int totalMsecs, bool showMsecs, out string value)
        {
            value = string.Empty;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(totalMsecs);

            bool force = false;

            if (timeSpan.TotalDays >= 1.0)
            {
                value += (int) timeSpan.TotalDays + " d  ";
                force = true;
            }

            if (force || timeSpan.Hours > 0)
            {
                value += timeSpan.Hours + " h  ";
                force = true;
            }

            if (force || timeSpan.Minutes > 0)
            {
                value += timeSpan.Minutes + " m  ";
            }
            
            value += timeSpan.Seconds + " s";

            if (showMsecs)
            {
                value += "  " + timeSpan.Milliseconds.ToString("000") + " ms";
            }
        }

        public static void AddFormats(ToolStripItemCollection items)
        {
            items.Add("{yyyy} " + Resources.info_DateYear);
            items.Add("{yy} " + Resources.info_DateYear);
            items.Add("{MM} " + Resources.info_DateMonth);
            items.Add("{dd} " + Resources.info_DateDay);
            items.Add("{HH} " + Resources.info_DateHour);
            items.Add("{mm} " + Resources.info_DateMinute);
            items.Add("{ss} " + Resources.info_DateSecond);
            items.Add("{0000000} " + Resources.info_Counter);
        }

        public static bool TryApplyFormat(string format, DateTime dateTime, int counter, out string result, out bool hasCounter)
        {
            result = format
                .Trim()
                .Replace("{yyyy}", dateTime.ToString("yyyy"))
                .Replace("{yy}", dateTime.ToString("yy"))
                .Replace("{MM}", dateTime.ToString("MM"))
                .Replace("{dd}", dateTime.ToString("dd"))
                .Replace("{HH}", dateTime.ToString("HH"))
                .Replace("{mm}", dateTime.ToString("mm"))
                .Replace("{ss}", dateTime.ToString("ss"));

            var matches = _regexCounter.Matches(result);
            hasCounter = matches.Count > 0;

            if (hasCounter)
            {
                foreach (Match match in matches)
                {
                    string variable = match.Value;
                    string zeroes = match.Groups[1].Value;

                    result = result.Replace(variable, counter.ToString(zeroes));

                    hasCounter = true;
                }
            }

            if (result.Contains("{") || result.Contains("}"))
            {
                result = null;
                return false;
            }

            return true;
        }

        #endregion Strings
    }
}
