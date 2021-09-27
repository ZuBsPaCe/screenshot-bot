# 1.0.1 (2021-??-??)
- BUGFIX: An error occurred while trying to create a new log file.
- BUGFIX: ScreenShotBot can properly determine file formats by looking at the provided extension. Supported are png, bmp andjpg. Before it always created a png.
- BUGFIX: Display an error if ffmpeg reported an error.
- BUGFIX: Creating a video could fail, because input.txt was not found. A full file path will now be specified for ffmpeg.
- BUGFIX: Duration Input.txt could sometimes not be parsed depending on the OS culture.
- DETAIL: Input.txt will not be deleted anymore if an error occurs during video creation.