using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFmpegTool
{
    public class CoubMaker : FFmpegTool
    {
        public CoubMaker(string ffmpegExecutableFolder):
            base(ffmpegExecutableFolder)
        {
        }

        public void MakeCoub(string videoPath, string audioPath, string coubPath)
        {
            if ((Path.GetExtension(videoPath) == ".webm") || (Path.GetExtension(videoPath) == ".flv"))
            {
                string tempVideoPath = Path.ChangeExtension(Path.GetTempFileName(), ".mp4");
                ConvertToMp4(videoPath, tempVideoPath);
                videoPath = tempVideoPath;
            }
            MakeCoub(videoPath, GetDuration(videoPath), audioPath, coubPath);
        }

        public void MakeCoub(string videoPath, double videoStart, double videoDuration, string audioPath, string coubPath)
        {
            string tempVideoPath = Path.ChangeExtension(Path.GetTempFileName(), ".mp4");
            if ((Path.GetExtension(videoPath) == ".webm") || (Path.GetExtension(videoPath) == ".flv"))
            {
                ConvertToMp4AndCut(videoPath, videoStart, videoDuration, tempVideoPath);
            }
            else
            {
                Cut(videoPath, videoStart, videoDuration, tempVideoPath);
            }
            MakeCoub(tempVideoPath, videoDuration, audioPath, coubPath);
        }

        private void MakeCoub(string videoPath, double videoDuration, string audioPath, string coubPath)
        {
            double audioDuration = GetDuration(audioPath);
            string listPath = Path.GetTempFileName();
            if (audioDuration > videoDuration)
            {
                int loops = (int)(audioDuration / videoDuration);
                GenerateConcatinaionFile(videoPath, listPath, loops);
                ConcatVideoAndReduce(listPath, audioPath, coubPath);
            }
            else
            {
                int loops = (int)(videoDuration / audioDuration);
                GenerateConcatinaionFile(audioPath, listPath, loops);
                ConcatAudioAndReduce(listPath, videoPath, coubPath);
            }
        }

        private void ConvertToMp4(string videoPath, string outputPath)
        {
            string param = String.Format("-i \"{0}\" -c:v libx264 -c:a aac -strict experimental -y \"{1}\"", videoPath, outputPath);
            Execute(ffmpegPath, param);
        }
        private void Cut(string videoPath, double start, double duration, string outputPath)
        {
            string param = String.Format("-ss {1} -i \"{0}\" -t {2} -c:v copy -c:a copy -y \"{3}\"",
                videoPath, start.ToString(CultureInfo.InvariantCulture), duration.ToString(CultureInfo.InvariantCulture), outputPath);
            Execute(ffmpegPath, param);
        }

        private void ConvertToMp4AndCut(string videoPath, double start, double duration, string outputPath)
        {
            string param = String.Format("-ss {1} -i \"{0}\" -t {2} -c:v libx264 -c:a aac -strict experimental -vcodec libx264 -y \"{3}\"",
                videoPath, start.ToString(CultureInfo.InvariantCulture), duration.ToString(CultureInfo.InvariantCulture), outputPath);
            Execute(ffmpegPath, param);
        }

        private static void GenerateConcatinaionFile(string mediaPath, string listPath, int loops)
        {
            using (StreamWriter file = new StreamWriter(listPath))
            {
                for (int i = 0; i <= loops; i++)
                {
                    file.WriteLine("file '{0}'\n", mediaPath);
                }
            }
        }

        private double GetDuration(string mediaPath)
        {
            string param = String.Format("-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{0}\"", mediaPath);
            StreamReader output = Execute(ffprobePath, param);
            double duration = 0.0;
            if (!output.EndOfStream)
            {
                duration = double.Parse(output.ReadLine(), CultureInfo.InvariantCulture);
            }
            return duration;
        }

        private void ConcatVideoAndReduce(string txtPath, string audioPath, string outputPath)
        {
            string param = String.Format("-i \"{1}\" -f concat -i \"{0}\" -c:v copy -c:a copy -y -shortest \"{2}\"",
                txtPath, audioPath, outputPath);
            Execute(ffmpegPath, param);
        }

        private void ConcatAudioAndReduce(string txtPath, string videoPath, string outputPath)
        {
            string param = String.Format(" -f concat -i \"{0}\" -i \"{1}\" -c:v copy -c:a copy -y -shortest \"{2}\"",
                txtPath, videoPath, outputPath);
            Execute(ffmpegPath, param);
        }

    }
}
