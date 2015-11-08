using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFmpegTool
{
    public class PreviewMaker : FFmpegTool
    {
        public PreviewMaker(string ffmpegExecutableFolder):
            base(ffmpegExecutableFolder)
        {
        }
        public void GetPreview(string videoPath, string outputPath)
        {
            string param = String.Format(" -i \"{0}\" -ss 00:00:1.0 -vframes 1 \"{1}\"", videoPath, outputPath);
            Execute(ffmpegPath, param);
        }

        public void GetAlbumCover(string audioPath, string outputPath)
        {
            string param = String.Format(" -i \"{0}\" -an -vcodec copy \"{1}\"", audioPath, outputPath);
            Execute(ffmpegPath, param);
        }
    }
}
