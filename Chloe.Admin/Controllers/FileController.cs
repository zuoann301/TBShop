using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ace;
using Chloe.Admin.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Chloe.Admin.Controllers
{
    public class FileController : WebController
    {

        private IHostingEnvironment hostingEnv;
        readonly string uploadFilePath = "upload";//保存上传文件的根目录

        public FileController(IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> UploadFile(string dir, string subPath)
        {
            if (Request.Form.Files.Count() == 0)
            {
                return this.FailedMsg("请选择上传的文件");
            }

            var file = Request.Form.Files[0];//kindeditor的上传文件控件，一次只传一个文件

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            if (String.IsNullOrEmpty(dir))
            {
                dir = "image";
            }

            String fileExt = Path.GetExtension(file.FileName).ToLower();

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dir]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return FailedMsg("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dir]) + "格式。");
            }

            string physicalFilePath = hostingEnv.WebRootPath;

            //创建文件夹
            string dirPath = physicalFilePath + "//" + uploadFilePath + "//" + subPath;
            string webPath = "/" + uploadFilePath + "/" + subPath + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += "//" + ymd;
            webPath += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            string suijishu = Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
            String newFileName = suijishu + fileExt;

            string fileName = $@"{dirPath}//{newFileName}";
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }

            string UploadDir = Globals.Configuration["AppSettings:FileDomain"].ToString();

            string fName = webPath + newFileName;
            //Hashtable hash = new Hashtable();
            //hash["error"] = 0;
            //hash["url"] = UploadDir + fName;
            string url= UploadDir + fName;

            return SuccessData(url);
        }

    }
}