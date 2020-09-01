using System.Web;
using System.Web.Mvc;

namespace Rosyblueonline.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public class DeleteFileAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                filterContext.HttpContext.Response.Flush();

                //convert the current filter context to file and get the file path
                string filePath = (filterContext.Result as FilePathResult).FileName;

                //delete the file after download
                System.IO.File.Delete(filePath);
            }
        }
    }
}
