using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gnostice.StarDocsSDK;


namespace OrgProject.Controllers
{
    [AllowAnonymous]
    public class JustToSeePDFController : Controller
    {

         
        //public ActionResult SeeThis(string namePdf)
        //{

        //    try
        //    {
        //        string file = Server.MapPath("~/PdfImageVideo/PDFFOLDER/" + namePdf);
        //        ViewerSettings viewerSettings = new ViewerSettings();
        //        viewerSettings.VisibleFileOperationControls.Open = true;
        //        ViewResponse viewResponse = MvcApplication.starDocs.Viewer.CreateView(
        //            new FileObject(file), null, viewerSettings);
        //        return new RedirectResult(viewResponse.Url);
        //    }
        //    catch (Exception)
        //    {

        //        return View("Error");

        //    }

        //}
       

        public ActionResult GetPdf(string fileName)
        {
            if(fileName ==null)
            {
                return View("Error");
            }

            var path = Server.MapPath(@"~/PdfImageVideo/PDFFOLDER/" + fileName);
            if(path ==null)
            {
                return View("Error");
            }
            try
            {
                var fileStream = new FileStream(path,
                                           FileMode.Open,
                                           FileAccess.Read
                                         );
                var fsResult = new FileStreamResult(fileStream, "application/pdf");
                return fsResult;
            }
            catch (Exception)
            {

                return View("Error");
            }
          
        }



    }
}