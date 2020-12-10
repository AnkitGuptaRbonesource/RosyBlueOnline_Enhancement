using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Rosyblueonline.Framework
{
    public class ErrorLog
    {
        public static void Log(string ClassName, string MethodName, Exception ex)
        {
            try
            {
                if (!(ex.GetType() == typeof(UserDefinedException)))
                {
                    string ErrorPath = ConfigurationManager.AppSettings["ErrorPath"];
                    if (!string.IsNullOrEmpty(ErrorPath))
                    {
                        var path = HttpContext.Current.Server.MapPath(ErrorPath);
                        var FileName = DateTime.Now.ToString("yyyy-MM-dd");
                        var FilePath = path + FileName + ".txt";
                        if (!File.Exists(FilePath))
                        {
                            File.Create(FilePath);
                        }
                        //File.ReadAllLines(FilePath)
                        using (FileStream fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(fs))
                            //using (StreamWriter writer = File.CreateText(FilePath))
                            {
                                
                                writer.WriteLine("-----------------------------------------------------------------------------");
                                writer.WriteLine("ClassName : " + ClassName + " Method: " + MethodName);
                                while (ex != null)
                                {
                                    writer.WriteLine(ex.GetType().FullName);
                                    writer.WriteLine("Message : " + ex.Message);
                                    writer.WriteLine("StackTrace : " + ex.StackTrace);
                                    if (ex.InnerException != null)
                                    {
                                        writer.WriteLine("Inner-Exception->");
                                    }
                                    ex = ex.InnerException;
                                }
                                
                                writer.Flush();
                                writer.Close();
                                writer.Dispose();
                            }
                            //fs.Flush();
                            fs.Close();
                            fs.Dispose();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }


        public static void TestLog(string ClassName, string MethodName)
        {
            try
            { 
                    string ErrorPath = ConfigurationManager.AppSettings["ErrorPath"];
                    if (!string.IsNullOrEmpty(ErrorPath))
                    {
                        var path = HttpContext.Current.Server.MapPath(ErrorPath);
                        var FileName = DateTime.Now.ToString("yyyy-MM-dd");
                        var FilePath = path + FileName + "_test.txt";
                        if (!File.Exists(FilePath))
                        {
                            File.Create(FilePath);
                        }
                        //File.ReadAllLines(FilePath)
                        using (FileStream fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(fs))
                            //using (StreamWriter writer = File.CreateText(FilePath))
                            {

                                writer.WriteLine("-----------------------------------------------------------------------------");
                                writer.WriteLine("ClassName : " + ClassName + " Method: " + MethodName);
                                 
                                writer.Flush();
                                writer.Close();
                                writer.Dispose();
                            }
                            //fs.Flush();
                            fs.Close();
                            fs.Dispose();
                        }
                    }
                 
            }
            catch (Exception e)
            {

            }
        }
    }
}
