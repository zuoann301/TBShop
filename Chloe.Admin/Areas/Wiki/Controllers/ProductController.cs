using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Ace.Web.Mvc.Models;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Chloe.Admin.Areas.Wiki.Controllers
{
    [Area(AreaNames.Wiki)]
    [Permission("wiki.product")]
    public class ProductController : WebController<IProductService>
    {
        private IHostingEnvironment hostingEnv;
        public ProductController(IHostingEnvironment hostingEnv)
        {
            this.hostingEnv = hostingEnv;
        }

        public ActionResult Index(string Pid="0")
        {
            //List<SysOrg> orgs = this.CreateService<IOrgService>().GetList();
            //this.ViewBag.Orgs = orgs;

            int ShopID = 0;
            if (this.CurrentSession.IsAdmin)
            {

            }
            else
            {
                ShopID = this.CurrentSession.ShopID;
            }

            IPro_SortService SortService = this.CreateService<IPro_SortService>();
            List<Pro_Sort> ListDistrict = SortService.GetList("", "", ShopID);
            ViewBag.SortList = ListDistrict;
            ViewBag.Pid = Pid;

            IBrandService brandService = this.CreateService<IBrandService>();
            List<Brand> ListBrand = brandService.GetList(0, "");
            ViewBag.BrandList = ListBrand;

             

            string FileDomain = Globals.Configuration["AppSettings:FileDomain"].ToString();
            ViewBag.FileDomain = FileDomain;
            return View();

        }

        public ActionResult Size(string ProductCode)
        {
            string FileDomain = Globals.Configuration["AppSettings:FileDomain"].ToString();
            ViewBag.FileDomain = FileDomain;

            ViewBag.ProductCode = ProductCode;
            return View();
        }


        [HttpGet]
        public ActionResult Models(Pagination pagination, string SortID="", string keyword="")
        {
            int ShopID = 0;
            if (this.CurrentSession.IsAdmin)
            {

            }
            else
            {
                ShopID = this.CurrentSession.ShopID;
            }

            PagedData<Product> pagedData = this.Service.GetPageData(pagination, SortID, keyword, ShopID);
            return this.SuccessData(pagedData);
        }

        [HttpGet]
        public ActionResult GetSizeList(string ProductCode = "")
        {
            List<Product_Size> pagedData = this.CreateService<IProduct_SizeService>().GetList(ProductCode);
            return this.SuccessData(pagedData);
        }

        [Permission("wiki.product.add")]
        [HttpPost]
        public ActionResult Add(AddProductInput input)
        {
            if (string.IsNullOrEmpty(input.ProSortID))
            {
                input.ProSortID = "0";
            }
            input.CreateID=CurrentSession.UserId;
            input.CreateDate = DateTime.Now;
            input.ShopID = CurrentSession.ShopID;
            this.Service.Add(input);
            return this.AddSuccessMsg();
        }


        [Permission("wiki.product.add")]
        [HttpPost]
        public ActionResult AddSize(AddProduct_SizeInput input)
        {             
            input.CreateID = CurrentSession.UserId;
            input.CreateDate = DateTime.Now;
            this.CreateService<IProduct_SizeService>().Add(input);
            return this.AddSuccessMsg();
        }


        [Permission("wiki.product.update")]
        [HttpPost]
        public ActionResult Update(UpdateProductInput input)
        {
            if (string.IsNullOrEmpty(input.ProSortID))
            {
                input.ProSortID = "0";
            }
            input.CreateID = CurrentSession.UserId;
            input.CreateDate = DateTime.Now;
            this.Service.Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("wiki.product.update")]
        [HttpPost]
        public ActionResult UpdateSize(UpdateProduct_SizeInput input)
        {
            input.CreateID = CurrentSession.UserId;
            input.CreateDate = DateTime.Now;
            this.CreateService<IProduct_SizeService>().Update(input);
            return this.UpdateSuccessMsg();
        }

        [Permission("wiki.product.delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            this.Service.Delete(id);
            return this.DeleteSuccessMsg();
        }

        [Permission("wiki.product.delete")]
        [HttpPost]
        public ActionResult DeleteSize(string id)
        {
            this.CreateService<IProduct_SizeService>().Delete(id);
            return this.DeleteSuccessMsg();
        }

        public IActionResult FileSave()
        {
            string webRootPath = Ace.Globals.AppRootPath;
            string contentRootPath = Ace.Globals.AppRootPath;

            string UploadDir = Globals.Configuration["AppSettings:UploadDir"].ToString(); 
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
                        return Ok(new { st = 0, msg = "上传的文件格式错误" });
                    }
                    if (fileFilt.IndexOf(fileExt.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        return Ok(new { st = 0, msg = "上传的文件不是图片" });
                    }

                    string physicalFilePath = hostingEnv.WebRootPath;

                    long fileSize = Request.Form.Files[0].Length; //获得文件大小，以字节为单位
                    string suijishu = Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
                    String newFileName = suijishu + fileExt;

                    string ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);

                    string dirPath = physicalFilePath + "//" + UploadDir + "//" + ymd;
                    string dicPath = "/" + UploadDir+"/"+ ymd;// _hostingEnvironment.ContentRootPath + imgPath;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    
                    urlPath= dicPath+"/"+ newFileName;

                    string fileName = $@"{dirPath}//{newFileName}";
                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        Request.Form.Files[0].CopyTo(stream);
                    }
                }                
                return Ok(new { st = 1, msg = urlPath });
            }
            else
            {
                return Ok(new { st = 0, msg = "请选择文件" });
            }

        }


        /// <summary>
        /// 基本信息导入
        /// </summary>
        /// <returns></returns>
        public IActionResult xlsFileSave(int ShopID=0)
        {
            string webRootPath = Ace.Globals.AppRootPath;
            string contentRootPath = Ace.Globals.AppRootPath;

            string UploadXls = Globals.Configuration["AppSettings:UploadXls"].ToString();

            if (Request.Form.Files[0].Length > 0)
            {
                string fileExt = Path.GetExtension(Request.Form.Files[0].FileName); //文件扩展名，不含“.”
                const string fileFilt = ".xls|.xlsx|";
                //判断后缀是否是图片
                if (fileExt == null)
                {
                    return Ok(new { st = 0, msg = "上传的文件格式错误" });
                }
                if (fileFilt.IndexOf(fileExt.ToLower(), StringComparison.Ordinal) <= -1)
                {
                    return Ok(new { st = 0, msg = "上传的文件不是数据表格" });
                }

                long fileSize = Request.Form.Files[0].Length; //获得文件大小，以字节为单位
                string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名 

                string imgPath = "\\"+ UploadXls + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day + "\\";
                string dicPath = Ace.Globals.AppRootPath + imgPath;// _hostingEnvironment.ContentRootPath + imgPath;
                if (!Directory.Exists(dicPath))
                {
                    Directory.CreateDirectory(dicPath);
                }
                string filePath = Path.Combine(dicPath, newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                List<Product> list = GetSheetValues(filePath,ShopID);
                this.Service.BatchInsert(list);
                return Ok(new { st = 1, msg = JsonHelper.Serialize(list) });
            }
            else
            {
                return Ok(new { st = 0, msg = "请选择表格文件" });
            }
        }


        public IActionResult xlsFileSave2(int ShopID=0)
        {
            string webRootPath = Ace.Globals.AppRootPath;
            string contentRootPath = Ace.Globals.AppRootPath;

            string UploadXls = Globals.Configuration["AppSettings:UploadXls"].ToString();

            if (Request.Form.Files[0].Length > 0)
            {
                string fileExt = Path.GetExtension(Request.Form.Files[0].FileName); //文件扩展名，不含“.”
                const string fileFilt = ".xls|.xlsx|";
                //判断后缀是否是图片
                if (fileExt == null)
                {
                    return Ok(new { st = 0, msg = "上传的文件格式错误" });
                }
                if (fileFilt.IndexOf(fileExt.ToLower(), StringComparison.Ordinal) <= -1)
                {
                    return Ok(new { st = 0, msg = "上传的文件不是数据表格" });
                }


                long fileSize = Request.Form.Files[0].Length; //获得文件大小，以字节为单位
                string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名 

                string imgPath = "\\" + UploadXls + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day + "\\";
                string dicPath = Ace.Globals.AppRootPath + imgPath;// _hostingEnvironment.ContentRootPath + imgPath;
                if (!Directory.Exists(dicPath))
                {
                    Directory.CreateDirectory(dicPath);
                }
                string filePath = Path.Combine(dicPath, newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Request.Form.Files[0].CopyTo(stream);
                }
                List<Product_Size> list = GetSheetValues2(filePath,ShopID);
                if (list.Count > 0)
                {
                    IProduct_SizeService SortService = this.CreateService<IProduct_SizeService>();
                    SortService.BatchInsert(list);
                }
                return Ok(new { st = 1, msg = JsonHelper.Serialize(list) });
            }
            else
            {
                return Ok(new { st = 0, msg = "请选择表格文件" });
            }
        }


        public List<Product> GetSheetValues(string filepath,int ShopID=0)
        {
            FileInfo file = new FileInfo(filepath);
            if (file != null)
            {
                List<Product> list = new List<Product>();
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    IPro_SortService SortService= this.CreateService<IPro_SortService>();
                    IBrandService brandService = this.CreateService<IBrandService>();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    //获取表格的列数和行数
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        Product modle = new Product();
                        modle.Id = worksheet.Cells[row, 1].Value.ToString();
                        modle.ProSortID = SortService.GetProSortID(worksheet.Cells[row, 2].Value.ToString());
                        modle.ProductName = worksheet.Cells[row, 3].Value.ToString();
                        modle.ProductCode = worksheet.Cells[row, 4].Value.ToString();
                        modle.ProSize = worksheet.Cells[row, 5].Value.ToString();
                        modle.Price = worksheet.Cells[row, 6].Value != DBNull.Value ? Convert.ToDecimal(worksheet.Cells[row, 6].Value) : 0;
                        modle.BasePrice = worksheet.Cells[row, 7].Value != DBNull.Value ? Convert.ToDecimal(worksheet.Cells[row, 7].Value) : 0;
                        modle.BatchPrice = worksheet.Cells[row, 8].Value != DBNull.Value ? Convert.ToDecimal(worksheet.Cells[row, 8].Value) : 0;
                        
                        modle.SharePercent = worksheet.Cells[row, 9].Value != DBNull.Value ? Convert.ToDecimal(worksheet.Cells[row, 10].Value) : 0;
                        modle.ImageUrl= worksheet.Cells[row, 10].Value.ToString();
                        modle.ImageList = worksheet.Cells[row, 11].Value.ToString();
                        modle.Summary= worksheet.Cells[row, 12].Value.ToString();
                        modle.Contents = worksheet.Cells[row, 13].Value.ToString();
                        if(!string.IsNullOrEmpty(worksheet.Cells[row, 14].Value.ToString()))
                        {
                            if(worksheet.Cells[row, 14].Value.ToString() != "暂无")
                            {
                                modle.BrandID = brandService.GetBrandID(worksheet.Cells[row, 14].Value.ToString());
                            }
                            else
                            {
                                modle.BrandID ="";
                            }
                        }
                        else
                        {
                            modle.BrandID = "";
                        }
                        
                        modle.ShopID = ShopID;

                        modle.CreateDate = DateTime.Now;
                        modle.CreateID = this.CurrentSession.UserId;                        
                        list.Add(modle);
                    }
                    return list;
                }
            }
            return null;
        }


        public List<Product_Size> GetSheetValues2(string filepath,int ShopID=0)
        {
            FileInfo file = new FileInfo(filepath);
            if (file != null)
            {
                List<Product_Size> list = new List<Product_Size>();
                int DefaultCityID = Globals.Configuration["AppSettings:DefaultCityID"].ToInt().Value;
                int DefaultProvinceID = Globals.Configuration["AppSettings:DefaultProvinceID"].ToInt().Value;
                using (ExcelPackage package = new ExcelPackage(file))
                {                     
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    //获取表格的列数和行数
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        Product_Size modle = new Product_Size();
                        modle.Id = worksheet.Cells[row, 1].Value.ToString();
                         
                        modle.ProductCode = worksheet.Cells[row, 2].Value.ToString();                        
                        modle.ProSize = worksheet.Cells[row, 3].Value.ToString();
                        modle.Price = worksheet.Cells[row, 4].Value != DBNull.Value ? Convert.ToDecimal(worksheet.Cells[row, 4].Value) : 0;

                        modle.BasePrice = worksheet.Cells[row, 5].Value != DBNull.Value ? Convert.ToDecimal(worksheet.Cells[row, 5].Value) : 0;
                        modle.BatchPrice = worksheet.Cells[row, 6].Value != DBNull.Value ? Convert.ToDecimal(worksheet.Cells[row, 6].Value) : 0;

                        //modle.ImageUrl = worksheet.Cells[row, 8].Value.ToString();
                        //modle.ImageList = worksheet.Cells[row, 9].Value.ToString();
                        modle.ShopID = ShopID;
                        modle.CreateDate = DateTime.Now;
                        modle.CreateID = this.CurrentSession.UserId;
                        list.Add(modle);
                    }
                    return list;
                }
            }
            return null;
        }


        public ActionResult BatchAdd(int ShopID=0)
        {
            IShopService shopService = this.CreateService<IShopService>();
            List<SimpleShop2> ShopList = shopService.GetCacheList2();
            ViewBag.ShopList = ShopList;
            ViewBag.ShopID = ShopID;
            return View();
        }

        public ActionResult BatchAdd2(int ShopID = 0)
        {
            IShopService shopService = this.CreateService<IShopService>();
            List<SimpleShop2> ShopList = shopService.GetCacheList2();
            ViewBag.ShopList = ShopList;
            ViewBag.ShopID = ShopID;
            return View();
        }


        [HttpGet]
        public ActionResult SetPrice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetPrice(decimal PerBatchPrice,decimal PerPrice,decimal PerSharePercent)
        {
            IProductService productService = this.CreateService<IProductService>();
            productService.SetPrice(PerBatchPrice, PerPrice, PerSharePercent);
            return this.SuccessMsg();
        }

    }
}