using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace FFmpegTool
{
    public abstract class FFmpegTool
    {
        protected string ffmpegPath;
        protected string ffprobePath;
        protected string tempPath = Path.GetTempPath();

        protected FFmpegTool(string ffmpegExecutableFolder)
        {
            if (!Directory.Exists(ffmpegExecutableFolder))
                throw new ArgumentException("Provided folder doesn't exists or doesn't have required permission",
                    "ffmegExecutableFolder");
            ffmpegPath = Path.Combine(ffmpegExecutableFolder, "ffmpeg.exe");
            if (!File.Exists(ffmpegPath))
                throw new ArgumentException("Provided folder doesn't contain  file ffmpeg.exe or doesn't have required permission.",
                    "ffmegExecutableFolder");
            ffprobePath = Path.Combine(ffmpegExecutableFolder, "ffprobe.exe");
            if (!File.Exists(ffprobePath))
                throw new ArgumentException("Provided folder doesn't contain  file ffprobe.exe or doesn't have required permission.", "ffmegExecutableFolder");
        }

        protected StreamReader Execute(string programPath, string param)
        {
            var inf = new ProcessStartInfo(programPath, param)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var prc = new Process();
            prc.StartInfo = inf;
            prc.Start();
            prc.WaitForExit();
            return prc.StandardOutput;
        }
    }
}
