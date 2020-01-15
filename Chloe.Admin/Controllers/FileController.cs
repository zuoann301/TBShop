using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;
using Ace;

namespace Chloe.File.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return Content("ooo");
        }

        [HttpPost]
        public IActionResult FileSave(string type="file")
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.ContentType = "text/plain;charset=utf-8";
            Dictionary<string, object> result = new Dictionary<string, object>();
            
            if (Request.Form.Files.Count > 0)
            {
                string urlPath = string.Empty;
                if (Request.Form.Files[0].Length > 0)
                {
                    string fileExt = Path.GetExtension(Request.Form.Files[0].FileName); //文件扩展名，不含“.”
                    string orgFileName = Path.GetFileName(Request.Form.Files[0].FileName);
                    const string fileFilt = ".png|.jpg|";
                    //判断后缀是否是图片
                    if (fileExt == null)
                    {
                        result.Add("ST", 0);
                        result.Add("Msg", "上传的文件格式错误");
                        result.Add("Url","");
                        //return Ok(new { st = 0, msg = "上传的文件格式错误" });
                    }
                    if (fileFilt.IndexOf(fileExt.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        result.Add("ST", 0);
                        result.Add("Msg", "上传的文件不是图片");
                        result.Add("Url", "");
                        //return Ok(new { st = 0, msg = "上传的文件不是图片" });
                    }

                    string physicalFilePath = Directory.GetCurrentDirectory() + "\\" + Globals.Configuration["AppSettings:FileRootDir"];// hostingEnv.WebRootPath;

                    long fileSize = Request.Form.Files[0].Length; //获得文件大小，以字节为单位                     
                    String newFileName = Math.Abs(Guid.NewGuid().GetHashCode()).ToString() + fileExt;

                    string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);

                    string dirPath = physicalFilePath + "\\" + type + "\\" + ymd;
                    string dicPath = "/" + type + "/" + ymd;// _hostingEnvironment.ContentRootPath + imgPath;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    urlPath = Globals.Configuration["AppSettings:FileDomain"] +"/" + type + "/" + ymd + "/" + newFileName;
                    string fileName = $@"{dirPath}//{newFileName}";
                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                    result.Add("ST", 100);
                    result.Add("Msg", "");
                    result.Add("Url", urlPath);
                }
                else
                {
                    result.Add("ST", 0);
                    result.Add("Msg", "");
                    result.Add("Url", "");
                }
                //return Ok(new { st = 1, msg = urlPath });
            }
            else
            {
                result.Add("ST", 0);
                result.Add("Msg", "");
                result.Add("Url", "");
                //return Ok(new { st = 0, msg = "请选择文件" });
            }
            return Json(result);
        }



        [HttpPost]
        public async Task<ActionResult> UploadFile()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            string FileDir = Directory.GetCurrentDirectory()+ "\\"+ Globals.Configuration["AppSettings:FileRootDir"];
            string FileHost = Globals.Configuration["AppSettings:FileHost"];

            var data = Request.Form.Files["data"];
            string lastModified = Request.Form["lastModified"].ToString();
            var total = Request.Form["total"];
            var fileName = Request.Form["fileName"];
            var index = Request.Form["index"];
            var type= Request.Form["type"];//上传类型

            //string s= Directory.GetCurrentDirectory();

            string temporary = Path.Combine(FileDir, lastModified);//临时保存分块的目录
            //try
            //{
            if (!Directory.Exists(temporary))
                Directory.CreateDirectory(temporary);
            string filePath = Path.Combine(temporary, index.ToString());
            if (!Convert.IsDBNull(data))
            {
                await Task.Run(() =>
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        data.CopyTo(fs);
                    }
                });
            }
            string webPath = string.Empty;
            string fileExt = Path.GetExtension(fileName);
            string finalPath = string.Empty;// Path.Combine(FileDir, DateTime.Now.ToString("yyMMddHHmmss") + fileExt);
            string dirPath = Path.Combine(FileDir, type, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string rdm = Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
            finalPath = Path.Combine(dirPath, DateTime.Now.ToString("yyMMddHHmmss")+ rdm + fileExt);
            webPath = FileHost + "/"+ type+"/" + DateTime.Now.ToString("yyyyMMdd")+"/"+ DateTime.Now.ToString("yyMMddHHmmss")+ rdm + fileExt;

            bool mergeOk = false;
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (total == index)
            {
                mergeOk = await FileMerge(lastModified, finalPath);

                result.Add("number", index);
                result.Add("mergeOk", mergeOk);
                result.Add("webPath", webPath);
            }
            else
            {
                result.Add("number", index);
                result.Add("mergeOk", mergeOk);
                result.Add("webPath","");
            }
            
            
            return Json(result);

            //}
            //catch (Exception ex)
            //{
            //    Directory.Delete(temporary);//删除文件夹
            //    throw ex;
            //}
        }

        public async Task<bool> FileMerge(string lastModified, string finalPath)
        {
            bool ok = false;
            string FileDir = Directory.GetCurrentDirectory() + "\\" + Globals.Configuration["AppSettings:FileRootDir"];
            //try
            //{
            var temporary = Path.Combine(FileDir, lastModified);//临时文件夹
            //fileName = Request.Form["fileName"];//文件名
            //string fileExt = Path.GetExtension(fileName);//获取文件后缀
            var files = Directory.GetFiles(temporary);//获得下面的所有文件
            //var finalPath = Path.Combine(FileDir, DateTime.Now.ToString("yyMMddHHmmss") + fileExt);//最终的文件名（demo中保存的是它上传时候的文件名，实际操作肯定不能这样）

            using (var fs = new FileStream(finalPath, FileMode.Create))
            {
                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                {
                    var bytes = System.IO.File.ReadAllBytes(part);
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                    bytes = null;
                    System.IO.File.Delete(part);//删除分块
                }
                fs.Close();
                Directory.Delete(temporary);//删除文件夹
                ok = true;
            }


            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return ok;
        }


        public IActionResult DownLoad(string file)
        {
            string FileDir = Globals.Configuration["AppSettings:FileRootDir"];
            string FileHost = Globals.Configuration["AppSettings:FileHost"];

            var addrUrl = file;
            var stream = System.IO.File.OpenRead(Path.Combine(FileDir, file));
            string fileExt = Path.GetExtension(Path.Combine(FileDir, file));
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi, Path.GetFileName(addrUrl));
        }




    }
}
