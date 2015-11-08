using Adam.Core.Maintenance;
using System;

namespace CoubMaintenance
{
    public class CoubTarget : MaintenanceTarget
    {

        public CoubTarget(string name, string author, string audio, string video)
        {
            Name = name;
            Author = author;
            VideoPath = video;
            AudioPath = audio;
        }

        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string AudioPath { get; private set; }
        public string VideoPath { get; private set; }

        public override long Impact
        {
            get { return 50; }
        }
    }
}
