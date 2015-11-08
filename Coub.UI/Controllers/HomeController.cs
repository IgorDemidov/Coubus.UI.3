using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Orders;
using Adam.Core.Records;
using Adam.Core.Search;
using Coub.Domain.ConcreteRepositories;
using Coub.UI.Models;
using Adam.Web.UI;
using System.IO.Compression;



namespace Coub.UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {   
            Application app = new Application();
            LogOnStatus status = app.LogOn("TRAINING", "Demidov", "5090588");
            Console.WriteLine(status);
            if (status != LogOnStatus.LoggedOn)
            {
                throw new UnauthorizedAccessException();
            }
            Session["AdamApp"] = app;
            
            return View();
        }
        //needs refactor 
        public ActionResult GetVideoItemList()
        {
            return GetMediaItemList("AdamCoubusVideoClassification", "video");

            #region lastVersion
            //try
            //{
            //    RecordRepository recordRepository = new RecordRepository((Application)Session["AdamApp"]);
            //    string configClassificationPath = ConfigurationManager.AppSettings["AdamCoubusVideoClassification"];
            //    var recordCollection = recordRepository.GetRecordCollectionByClassificationNamePath(configClassificationPath);
            //    MediaSetViewModel model = new MediaSetViewModel(recordCollection, "video");

            //    return PartialView("_MediaListPartial", model);
            //}
            //catch (Exception)
            //{
            //    ((Application)Session["AdamApp"]).LogOff();
            //    throw;
            //}
            #endregion 
            
        }

        public ActionResult GetAudioItemList()
        {
            return GetMediaItemList("AdamCoubusAudioClassification", "audio");            
        }

        private ActionResult GetMediaItemList(string classificationNamePathAppSettingsKey, string mediaType)
        {
            try
            {
                RecordRepository recordRepository = new RecordRepository((Application)Session["AdamApp"]);
                string configClassificationPath = ConfigurationManager.AppSettings[classificationNamePathAppSettingsKey];
                var recordCollection = recordRepository.GetRecordCollectionByClassificationNamePath(configClassificationPath);
                MediaSetViewModel model = new MediaSetViewModel(recordCollection, mediaType);

                return PartialView("_MediaListPartial", model);
            }
            catch (Exception)
            {
                ((Application)Session["AdamApp"]).LogOff();
                throw;
            }
        }  
      

        public FileContentResult GetRecordMasterPreview(Guid recordId)
        {
            try
            {
                RecordRepository recordRepository = new RecordRepository((Application)Session["AdamApp"]);
                byte[] previewFileBytes = recordRepository.Get(recordId).Files.LatestMaster.GetPreview().GetBytes();
                return new FileContentResult(previewFileBytes, "image/jpeg");
            }
            catch (Exception)
            {
                ((Application)Session["AdamApp"]).LogOff();
                throw;
            }
        }

        public ActionResult GetVideoFileStream(Guid videoRecordId)
        {
            try
            {
                RecordRepository recordRepository = new RecordRepository((Application)Session["AdamApp"]);
                string path = recordRepository.Get(videoRecordId).Files.LatestMaster.Path;

                //74bdf7e4-6fb7-4153-94d9-a54700f71604
                FileWebRequest request = (FileWebRequest)WebRequest.Create(path);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                FileWebResponse response = (FileWebResponse)request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    return File(responseStream, "video/mp4");
                }
            }
            catch (Exception)
            {
                ((Application)Session["AdamApp"]).LogOff();
                throw;
            }
        }

        public ContentResult GenerateVideoTag(Guid videoRecordId, string createdTagId)
        {
            try
            {
                RecordRepository recordRepository = new RecordRepository((Application)Session["AdamApp"]);
                string path = recordRepository.Get(videoRecordId).Files.LatestMaster.Path;

                TagBuilder videoTag = new TagBuilder("video");
                videoTag.Attributes.Add("id", createdTagId);
                videoTag.Attributes.Add("width", "50%");
                videoTag.Attributes.Add("height", "auto");
                videoTag.Attributes.Add("loop", "true");
                videoTag.Attributes.Add("controls", "controls");
                videoTag.Attributes.Add("style", "min-height:50px;");

                TagBuilder sourceTag = new TagBuilder("source");
                UrlHelper urlHelper = new UrlHelper(HttpContext.Request.RequestContext);
                string url = urlHelper.Action("GetVideoFileStream", "Home", new { videoRecordId = videoRecordId });
                sourceTag.Attributes.Add("src", url);
                sourceTag.Attributes.Add("type", @"video/mp4");
                sourceTag.Attributes.Add("codecs", "avc1.42E01E, mp4a.40.2");
                videoTag.InnerHtml += sourceTag;
                return Content(videoTag.ToString());   
            }
            catch (Exception)
            {
                ((Application)Session["AdamApp"]).LogOff();
                throw;
            }         
        }

        public void Logoff()
        {
            ((Application)Session["AdamApp"]).LogOff();
        }


        public ActionResult GetMediaItemInfo(Guid mediaRecId)
        {
            RecordRepository recordRepository = new RecordRepository((Application)Session["AdamApp"]);

            MediaViewModel model = new MediaViewModel(recordRepository.Get(mediaRecId));
            return PartialView("_MediaModelPartial", model);
        }




        private void UnZipFile(Stream compressedStream)
        {
            using (GZipStream zip = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                //ZipFileExtensions
                                   
            }
            

        }


        private Guid StringToGuid(string id)
        {
            Guid guid = Guid.Parse("17203685-4c28-47b9-baa9-a546018ac067");

            return guid;
        }

        




        

        


        #region Temp

        private List<string> GetByte64Previews()
        {
            RecordCollection recordCollection = new RecordCollection((Application)Session["AdamApp"]);
            Adam.Core.Search.SearchExpression se = new Adam.Core.Search.SearchExpression("File.Version.Extension = jpg");

            recordCollection.Load(se);
            List<string> base64Previews = new List<string>();
            byte[] tempBytes;
            foreach (Record rec in recordCollection)
            {
                tempBytes = rec.Files.Master.GetPreview().GetBytes();
                base64Previews.Add(Convert.ToBase64String(tempBytes));
            }

            return base64Previews;
        }

        public FileContentResult GetFile()
        {
            Record rec = new Record((Application) Session["AdamApp"]);
            Guid recId;
            Guid.TryParse("7819340e-b66c-4225-a604-a54500a909b2", out recId);
            rec.Load(recId);

            IReadOnlyImage prev = rec.Files.Master.GetPreview();
            return new FileContentResult(prev.GetBytes(), "image/jpeg");
        }

        private string GetByte64Image()
        {
            Record rec = new Record((Application) Session["AdamApp"]);
            Guid recId;
            Guid.TryParse("7819340e-b66c-4225-a604-a54500a909b2", out recId);
            rec.Load(recId);

            IReadOnlyImage prev = rec.Files.Master.GetPreview();
            byte[] imagData = prev.GetBytes();
            string imageDataString = Convert.ToBase64String(imagData);
            return imageDataString;
        }

        #endregion


    }
}
