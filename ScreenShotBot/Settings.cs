using System;

namespace ScreenShotBot
{
    [Serializable]
    public class Settings
    {
        private static string _path;

        public static Settings Instance { get; private set; }

        public static void Load(ILog log, string path)
        {
            _path = path;
            Instance = Tools.Deserialize<Settings>(log, path);
        }

        public void Save(ILog log)
        {
            Tools.Serialize(log, this, _path);
        }


        public int Version { get; set; } = 1;

        public string ScreenShotDir { get; set; }

        public int ScreenIndex { get; set; } = -1;

        public float ScreenShotInterval { get; set; } = -1;

        public IntervalUnit ScreenShotIntervalUnits { get; set; } = IntervalUnit.Secs;

        public bool ResetStatusAfterStart { get; set; } = true;

        public string ScreenShotSubDir { get; set; } = "{yy}-{MM}-{dd}";

        public string ScreenShotFilename { get; set; } = "Screenshot_{0000000}.jpg";

        public bool LogTracing { get; set; } = false;

        public string VideoConverterPath { get; set; } = string.Empty;

        public string VideoFilePattern { get; set; } = "*.jpg";

        public float VideoImagesPerSecond { get; set; } = 10;

        public int VideoScaleType { get; set; } = 0;

        public int VideoScale { get; set; } = 720;

        public string VideoOutputDir { get; set; } = "..";

        public string VideoConverterCustomCommand { get; set; }

        public bool UseVideoConverterCustomCommand { get; set; }

        public VideoSubDirsType VideoSubDirsType { get; set; } = VideoSubDirsType.IncludeSubDirs;

        public string VideoOutputFilename { get; set; } = "Timelapse-{yy}-{MM}-{dd}.mp4";

        public bool MinimizeToTray { get; set; }
        public bool CloseToTray { get; set; }
    }
}
