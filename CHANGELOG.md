# 1.0.1 (2021-09-27)
- BUGFIX: An error occurred while trying to create a new log file.
- BUGFIX: ScreenShotBot can properly determine file formats by looking at the provided extension. Supported are png, bmp andjpg. Before it always created a png.
- BUGFIX: Display an error if ffmpeg reported an error.
- BUGFIX: Creating a video could fail, because input.txt was not found. A full file path will now be specified for ffmpeg.
- BUGFIX: Duration Input.txt could sometimes not be parsed depending on the OS culture.
- CHANGE: Input.txt will not be deleted anymore if an error occurs during video creation.
- CHANGE: Changed local appdata folder to "ScreenShotBot".
- CHANGE: Settings are now saved in file "Settings.xml" within the local appdata folder.
- CHANGE: ScreenShotBot will now detect screen resolution changes. Screenshots will be created with the current resolution.
- ADD: Added "Minimize to tray" and "Close to tray" in the options menu.

# 1.0.0 (2021-04-32)
- Hello World!