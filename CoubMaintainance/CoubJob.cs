using System;
using Adam.Core.Maintenance;
using Adam.Core;
using Adam.Core.Records;
using Adam.Core.Orders;
using FFmpegTool;
using Adam.Core.Fields;

namespace CoubMaintenance
{
    public class CoubJob : ManualMaintenanceJob 
    {
        public CoubJob(Application app)
            : base(app)
        {
        }

        protected override void OnExecute()
        {
            string ffmpegPath = App.GetSetting(".ffmpegInstallationPath").ToString();
            CoubMaker maker = new CoubMaker(ffmpegPath);
            string tempFile = App.GetTemporaryFile("mp4");            
            Record record = new Record(App);
            foreach (CoubTarget target in Targets)
            {
                maker.MakeCoub(target.VideoPath, target.AudioPath, tempFile);                        
                AddRecord(record, target, tempFile);
            }
        }

        private static void AddRecord(Record record, CoubTarget target, string tempFile)
        {
            record.AddNew();
            record.Classifications.Add(new Adam.Core.Classifications.ClassificationPath("/Cubus/Coub"));
            record.Fields.GetField<TextField>("Name").SetValue(target.Name);
            record.Fields.GetField<TextField>("Author").SetValue(target.Author);
            record.Fields.GetField<TextField>("Description").SetValue(target.Description);
            record.Files.AddFile(tempFile);
            record.Save();
        }

        public override bool IsRetryingSupported
        {
            get { return false; }
        }

        protected override void OnDeserialize(System.Xml.XmlReader reader)
        {
        }

        protected override MaintenanceTarget OnDeserializeTarget(System.Xml.XmlReader reader)
        {
            while (reader.Name != "target")
                reader.Read();
            var target = new CoubTarget(reader["name"], reader["author"], reader["audioPath"], reader["videoPath"]);             
            target.Description = reader["description"];
            return target;
        }

        protected override void OnSerialize(System.Xml.XmlWriter writer)
        {
        }

        protected override void OnSerializeTarget(MaintenanceTarget target, System.Xml.XmlWriter writer)
        {
            CoubTarget t = (CoubTarget)target;
            writer.WriteStartElement("target");
            writer.WriteAttributeString("author", t.Author);
            writer.WriteAttributeString("name", t.Name);
            writer.WriteAttributeString("description", t.Description);
            writer.WriteAttributeString("audioPath", t.AudioPath);
            writer.WriteAttributeString("videoPath", t.VideoPath);
            writer.WriteEndElement();
        }

        protected override Type TargetType
        {
            get { return typeof(CoubTarget); }
        }
    }
}
