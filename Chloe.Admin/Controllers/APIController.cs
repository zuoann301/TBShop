using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ace;
using Ace.Application.Common;
using Ace.Application.Wiki;
using Ace.Entity.Wiki;
using Ace.IdStrategy;
using Ace.Web.Mvc;
using Ace.Web.Mvc.Authorization;
using Ace.Web.Mvc.Models;
using Chloe.Admin.Common;
using Chloe.Admin.Common.Tree;
using Microsoft.AspNetCore.Mvc;

using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Ace.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Senparc.Weixin;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.TenPay.V3;
using NLog;
using Senparc.CO2NET.Utilities;
using Chloe.Admin.Models;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;

namespace Chloe.Admin.Controllers
{
    public class APIController : WebController
    { 
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 获取OpenID
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="jscode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult JsCode2Json(string jscode)
        {
            string appid = Globals.Configuration["AppSettings:WxOpenAppId"].ToString();
            string secret = Globals.Configuration["AppSettings:WxOpenAppSecret"].ToString();

            var data = Senparc.Weixin.WxOpen.AdvancedAPIs.Sns.SnsApi.JsCode2Json(appid, secret, jscode);
            //string IsDebug = Globals.Configuration["AppSettings:IsDebug"].ToString();
            //data.P2PData = IsDebug;
            return this.SuccessData(data);
        }

        /// <summary>
        /// 商家列表 距离计算
        /// </summary>
        /// <param name="GPS_X"></param>
        /// <param name="GPS_Y"></param>
        /// <returns></returns>
        public ActionResult ShopList(decimal GPS_X=0,decimal GPS_Y=0)
        {
            List<ShopSample> list = this.CreateService<IShopService>().GetShopList(GPS_X, GPS_Y);
            return this.SuccessData(list);
        }

        public ActionResult ShopInfo(int ShopID)
        {
            Shop shop = this.CreateService<IShopService>().GetModel(ShopID);
            return this.SuccessData(shop);
        }


        public ActionResult LinkList(int ShopID, int SortID, string keyword)
        {
            List<Link> list = this.CreateService<ILinkService>().GetShopLinkList(ShopID,SortID, keyword);
            return this.SuccessData(list);
        }


        public ActionResult ProSortList(string Pid="", string keyword="")
        {
            List<Pro_Sort> list = this.CreateService<IPro_SortService>().GetList(Pid, keyword);
            return this.SuccessData(list);
        }

        public ActionResult ProTopList(int Num=4,int IsTop=0,int ShopID=0)
        {
            List<Product> list = this.CreateService<IProductService>().GetTopList(Num, IsTop, ShopID);
            return this.SuccessData(list);
        }

        public ActionResult ProHotList(int Num = 4, int IsHot = 0,int ShopID=0)
        {
            List<Product> list = this.CreateService<IProductService>().GetHotList(Num, IsHot, ShopID);
            return this.SuccessData(list);
        }

        public ActionResult BrandList(int ShopID, int IsTop = 0, string keyword = "")
        {
            List<Brand> list = this.CreateService<IBrandService>().GetShopBrandList(ShopID,IsTop, keyword);
            return this.SuccessData(list);
        }

        public ActionResult BrandPageList(Pagination pagination, int IsTop = 0, string keyword = "")
        {
            PagedData<Brand> pagedData = this.CreateService<IBrandService>().GetPageData(pagination, IsTop,keyword);
            return this.SuccessData(pagedData); ;
        }

        public ActionResult BrandDetail(string id)
        {
            Brand info = this.CreateService<IBrandService>().GetModel(id);
            return this.SuccessData(info);
        }

        public ActionResult BrandProduct(string id)
        {
            List<Product> list = this.CreateService<IProductService>().GetProductListByBrandID(id);
            return this.SuccessData(list);
        }

        public ActionResult NewsList(int ShopID, int Num=4, int SortID=0,int IsValid=1)
        {
            List<News> list = this.CreateService<INewsService>().GetShopNewsList(Num,SortID, ShopID, IsValid).ToList();
            return this.SuccessData(list);
        }

        public ActionResult NewsPageList(Pagination pagination, int SortID, int IsValid=-1, string keyword="")
        {
            PagedData<News> pagedData = this.CreateService<INewsService>().GetPageData(pagination, SortID, IsValid, keyword);
            return this.SuccessData(pagedData);
        }
         


        public ActionResult NewsDetail(string id)
        {
            News news = this.CreateService<INewsService>().GetModel(id);
            return this.SuccessData(news);
        }

        public ActionResult CommentList(string Fid,int Num=10)
        {
            List<Comment> list = this.CreateService<ICommentService>().GetList(Fid, Num).ToList();
            return this.SuccessData(list);
        }

        [HttpPost]
        public ActionResult CommentPost(string Fid,string Summary, string UserID)
        {
            AddCommentInput addCommentInput = new AddCommentInput();
            addCommentInput.CreateDate = DateTime.Now;
            addCommentInput.Fid = Fid;
            addCommentInput.Summary = Summary;
            addCommentInput.UserID = UserID;

            this.CreateService<ICommentService>().Add(addCommentInput);

            return this.SuccessMsg();
        }

        public ActionResult CateList(string Pid)
        {
            List<Pro_Sort> pro_Sorts = this.CreateService<IPro_SortService>().GetList( Pid, "");
            return this.SuccessData(pro_Sorts);
        }

        public ActionResult CateInfo(string id)
        {
            ProSortInfo curCateInfo = new ProSortInfo();            
            Pro_Sort pro_Sort = this.CreateService<IPro_SortService>().GetModle(id);

            curCateInfo.Icon = pro_Sort.Icon;
            curCateInfo.Id = pro_Sort.Id;
            curCateInfo.Pid = pro_Sort.Pid;
            curCateInfo.ProCount = pro_Sort.ProCount;
            curCateInfo.SortCode = pro_Sort.SortCode;
            curCateInfo.SubSortList = this.CreateService<IPro_SortService>().GetList(id, "");
            curCateInfo.Title = pro_Sort.Title;
            curCateInfo.ImageUrl = pro_Sort.ImageUrl;
            curCateInfo.Summary = pro_Sort.Summary;


            return this.SuccessData(curCateInfo);
        }

        public ActionResult ProductCount(string SortID="")
        {
            int count = this.CreateService<IProductService>().GetProductCount(SortID);
            return this.SuccessData(count);
        }

        public ActionResult ProductList(int ShopID, int Num, string SortID)
        {
            List<Product> list = this.CreateService<IProductService>().GetShopProductList(ShopID,Num, SortID);
            return this.SuccessData(list);
        }

        /// <summary>
        /// 置顶推荐
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ProSortID"></param>
        /// <param name="keyword"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        public ActionResult ProductTopList(Pagination pagination, string ProSortID, string keyword, int OrderType = 0)
        {
            PagedData<Product> pagedData = this.CreateService<IProductService>().GetTopPageData(pagination, 1, ProSortID, keyword, OrderType);
            return this.SuccessData(pagedData);
        }

        /// <summary>
        /// 人气推荐
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="ProSortID"></param>
        /// <param name="keyword"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        public ActionResult ProductHotList(Pagination pagination, string ProSortID, string keyword, int OrderType = 0)
        {
            PagedData<Product> pagedData = this.CreateService<IProductService>().GetHotPageData(pagination, 1, ProSortID, keyword, OrderType);
            return this.SuccessData(pagedData);
        }

        public ActionResult ProductPageList(Pagination pagination,int ShopID, string ProSortID, string keyword, int OrderType = 0)
        {
            PagedData<Product> pagedData = this.CreateService<IProductService>().GetShopProductPageList(pagination,ShopID, ProSortID, keyword, OrderType);
            return this.SuccessData(pagedData);
        }

        public ActionResult ProductInfo(string id,int hit=0,string userid="")
        {
            IProductService productService = this.CreateService<IProductService>();
            Product product = productService.GetModle(id);

            //更新点击量
            if(hit==1)
            {
                productService.UpDateHit(id,product.Hit+1);
            }

            //访问记录
            if(!string.IsNullOrEmpty(userid))
            {
                IProduct_rcdService rcdService = this.CreateService<IProduct_rcdService>();
                Product_rcd rcd = this.CreateService<IProduct_rcdService>().GetModel(userid, id);
                if(rcd!=null)
                {
                    rcdService.UpdateHit(rcd.Id, rcd.Hit + 1);
                }
                else
                {
                    AddProduct_rcdInput input = new AddProduct_rcdInput();
                    input.CreateDate = DateTime.Now;
                    input.Hit = 1;
                    input.ProductID = id;
                    input.UpdateTime = DateTime.Now;
                    input.UserID = userid;
                    rcdService.Add(input);
                }

            }

            ProductInfo modle = new ProductInfo();
            modle.product = product;

            AdminSession adminSession = this.CurrentSession;
            if(adminSession!=null)
            {
                string UserID = adminSession.UserId;
                int Count = this.CreateService<IPro_CollectionService>().Count(UserID, id);
                if(Count>0)
                {
                    modle.HasCollect = 1;
                }
                else
                {
                    modle.HasCollect = 0;
                }
            }
            else
            {
                modle.HasCollect = 0;
            }

            List<Product_Size> sizelist = this.CreateService<IProduct_SizeService>().GetList(product.ProductCode);
            modle.SizeList = sizelist;


            if(product.BrandID.IsNotNullOrEmpty())
            {
                Brand brand = this.CreateService<IBrandService>().GetModel(product.BrandID);
                modle.brand = brand;
            }
            else
            {
                modle.brand = null;
            }

            return this.SuccessData(modle);
        }

        public ActionResult Pro_CollectionAdd(string ProductID,string UserID)
        {
            AddPro_CollectionInput addPro_CollectionInput = new AddPro_CollectionInput();
            addPro_CollectionInput.CreateDate = DateTime.Now;
            addPro_CollectionInput.ProductID = ProductID;
            addPro_CollectionInput.UserID = UserID;
            this.CreateService<IPro_CollectionService>().Add(addPro_CollectionInput);
            return this.SuccessMsg();
        }


        public ActionResult Pro_CollectionPageList(Pagination pagination, string UserID)
        {
            PagedData<Pro_CollectionInfo> pagedData = this.CreateService<IPro_CollectionService>().GetPageData(pagination, UserID);
            return this.SuccessData(pagedData);
        }

        public ActionResult SetCart(int ShopID, string UserID, string ProductID, string ProSizeID, int ItemNum )
        {
            int n = 0;            
            if(!string.IsNullOrEmpty(UserID))
            { 
                n = this.CreateService<ICartService>().SetCart(ShopID,ProductID, ProSizeID, ItemNum, UserID);

                int count = this.CreateService<ICartService>().CountByUserID(UserID, ShopID);
                return this.SuccessData(count.ToString());
            }
            else
            {
                return this.FailedMsg("请登录后再添加到购物车");
            }
        }

        /// <summary>
        /// 购物车商品数量
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult CartGoodsCount(string UserID,int ShopID)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(UserID))
            {
               count = this.CreateService<ICartService>().CountByUserID(UserID, ShopID);
            }            
            return this.SuccessData(count.ToString());
        }

        public ActionResult CartList(string UserID,int ShopID)
        {
            List<CartInfo> list= this.CreateService<ICartService>().GetCartListItem(UserID, ShopID);
            return this.SuccessData(list);
        }

        [HttpPost]
        public ActionResult CartDelete(string Ids)
        {
            if(!string.IsNullOrEmpty(Ids))
            {
                string[] arr = Ids.Split(',');
                foreach(string s in arr)
                {
                    this.CreateService<ICartService>().Delete(s);
                }
                return this.SuccessData("删除成功");
            }
            else
            {
                return this.FailedMsg("请选择删除项");
            }
        }

        [HttpPost]
        public ActionResult CartUpdate(string id,int num)
        {
            if(!string.IsNullOrEmpty(id)&&num>0)
            {
                this.CreateService<ICartService>().UpdateCart(id, num);
                return this.SuccessData("修改成功");
            }
            else
            {
                return this.FailedMsg("数量修改失败");
            }
        }

        [HttpPost]
        public ActionResult CartChecked(string Ids,string UserID)
        {
            if (!string.IsNullOrEmpty(Ids))
            {
                ICartService cartService=  this.CreateService<ICartService>();
                cartService.ClearTempOrder(UserID);

                string[] arr = Ids.Split(',');
                foreach (string s in arr)
                {
                    cartService.CartChecked(s);
                }
                return this.SuccessData("OK");
            }
            else
            {
                return this.FailedMsg("请选择要下单的项目");
            }
        }

        public ActionResult CartTempOrder(string UserID)
        {
            List<CartInfo> list = this.CreateService<ICartService>().GetTempOrderItem(UserID);
            return this.SuccessData(list);
        }

        public ActionResult SmsCode(string phone)
        {
            string SMS_RegionId= Globals.Configuration["AppSettings:SMS_RegionId"];
            string SMS_AccessKeyId = Globals.Configuration["AppSettings:SMS_AccessKeyId"];
            string SMS_Secret = Globals.Configuration["AppSettings:SMS_Secret"];

            IClientProfile profile = DefaultProfile.GetProfile(SMS_RegionId, SMS_AccessKeyId,SMS_Secret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            //try
            //{
            string code = AceUtils.GenerateRandomNumber(4);
            // 构造请求
            CommonRequest request = new CommonRequest();

            request.Method = Aliyun.Acs.Core.Http.MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";

            request.RegionId = SMS_RegionId;
            request.QueryParameters.Add("PhoneNumbers", phone);
            request.QueryParameters.Add("SignName", "添道营销");
            request.QueryParameters.Add("TemplateCode", "SMS_86660148");
            request.QueryParameters.Add("TemplateParam", "{\"number\":\"" + code + "\"}");

            CommonResponse response = client.GetCommonResponse(request);
            //SmsResult smsResult= Ace.JsonHelper.Deserialize<SmsResult>(response.Data);
            
            if (response.Data.Contains("OK"))
            {
                HttpContext.Session.SetString(Ace.Globals.SmsCode, code);
                return this.SuccessData("验证码发送成功" + code);
            }
            else
            {
                return this.FailedMsg("验证码发送失败" + code);
            }

            //}
            //catch (ServerException ex)
            //{
            //    return this.FailedMsg(ex.ToString());
            //}
            //catch (ClientException ex)
            //{
            //    return this.FailedMsg(ex.ToString());
            //}

        }

        public ActionResult SimpleLogin(string OpenID,decimal GPS_X=0,decimal GPS_Y=0)
        {
            if(!string.IsNullOrEmpty(OpenID))
            {
                Users users = this.CreateService<IUsersService>().GetModelByOpenID(OpenID);
                if (users != null)
                {
                    if(GPS_X>0&&GPS_Y>0)
                    {
                        AddUsers_SecurityInput m = new AddUsers_SecurityInput();
                        m.GPS_X = GPS_X;
                        m.GPS_Y = GPS_Y;
                        m.CreateDate = DateTime.Now;
                        m.UserID = users.Id;
                        this.CreateService<IUsers_SecurityService>().Add(m);
                    }

                    return this.SuccessData(users);
                }
                else
                {
                    return this.FailedMsg("请先注册");
                }
            }
            else
            {
                return this.FailedMsg("无法获取OpenID");
            }
            
        }
        
        public ActionResult CheckOpenID(string OpenID)
        {
            if(!string.IsNullOrEmpty(OpenID))
            {
                var US = this.CreateService<IUsersService>();
                Users users = US.GetModelByOpenID(OpenID);
                if (users != null)
                {
                    return this.SuccessData(users);
                }
                else
                {
                    return this.FailedMsg("没有注册");
                }
            }
            else
            {
                return this.FailedMsg("无法获取OpenID");
            }
        }

        public ActionResult BindMobile(int ShopID, string Code, string OpenID,string Mobile,string UserName,string UserIcon, string FromID="",decimal GPS_X=0,decimal GPS_Y=0)
        {
            string SmsCode = HttpContext.Session.GetString(Ace.Globals.SmsCode);

            //return this.SuccessData(SmsCode);
            AddUsersInput addUsersInput = new AddUsersInput();

            var US = this.CreateService<IUsersService>();
            if (!string.IsNullOrEmpty(Code))
            {
                if (!string.IsNullOrEmpty(SmsCode))
                {
                    Users users = US.GetModelByOpenID(OpenID);
                    if (users != null)
                    {
                        US.UpdateUserInfo(OpenID, Mobile, UserIcon, UserName);
                    }
                    else
                    {
                        addUsersInput.CreateDate = DateTime.Now;
                        addUsersInput.Email = "";
                        addUsersInput.FromID = FromID;
                        addUsersInput.LastLoginDate = DateTime.Now;
                        addUsersInput.Mobile = Mobile;
                        addUsersInput.RoleID = 0;
                        addUsersInput.Sex = 0;
                        addUsersInput.ShopID = 0;
                        addUsersInput.ST = 0;
                        addUsersInput.UserName = UserName;
                        addUsersInput.UserPass = "";
                        addUsersInput.UserSecretkey = "";
                        addUsersInput.OpenID = OpenID;
                        addUsersInput.UserIcon = UserIcon;
                        addUsersInput.ShopID = ShopID;
                        US.Add(addUsersInput);
                        users = US.GetModelByOpenID(OpenID);
                    }
                    //记录登录地点
                    if (GPS_X > 0 && GPS_Y > 0)
                    {
                        AddUsers_SecurityInput m = new AddUsers_SecurityInput();
                        m.GPS_X = GPS_X;
                        m.GPS_Y = GPS_Y;
                        m.CreateDate = DateTime.Now;
                        m.UserID = users.Id;
                        this.CreateService<IUsers_SecurityService>().Add(m);
                    }

                    //AdminSession session = new AdminSession();
                    //session.UserId = users.Id;
                    //session.AccountName = users.OpenID;
                    //session.Name = users.UserName;
                    //session.LoginIP = HttpContext.GetClientIP();
                    //session.IsAdmin = false;
                    //this.CurrentSession = session;

                    return this.SuccessData(users);

                }
                else
                {
                    return this.FailedMsg("验证码已经过期");
                }
            }
            else
            {
                return this.FailedMsg("验证码不能为空");
            }


        }

        public ActionResult AddressDetail(string Id)
        {
            if(!string.IsNullOrEmpty(Id))
            {
                Users_Address users_Address = this.CreateService<IUsers_AddressService>().GetModel(Id);
                return this.SuccessData(users_Address);
            }
            else
            {
                return this.FailedMsg("参数缺失");
            }
        }
        [HttpGet]
        public ActionResult AddressList(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                List<Users_Address> list = this.CreateService<IUsers_AddressService>().GetList(UserID);
                return this.SuccessData(list);
            }
            else
            {
                return this.FailedMsg("参数缺失");
            }
        }

        [HttpPost]
        public ActionResult AdressAdd(AddUsers_AddressInput input)
        {
            this.CreateService<IUsers_AddressService>().Add(input);
            return this.SuccessData("OK");
        }

        [HttpPost]
        public ActionResult AdressUpdate(UpdateUsers_AddressInput input)
        {
            this.CreateService<IUsers_AddressService>().Update(input);
            return this.SuccessData("OK");
        }

        public ActionResult AddressDelete(string id)
        {
            this.CreateService<IUsers_AddressService>().Delete(id);
            return this.SuccessData("OK");
        }

        public ActionResult AddressDefault(string UserID)
        {
            Users_Address modle = this.CreateService<IUsers_AddressService>().GetDefaultAddress(UserID);
            return this.SuccessData(modle);
        }

        [HttpPost]
        public ActionResult OrderSubmit(string UserID, string AddressID,int ShopID)
        {
            string n = this.CreateService<IShopOrderService>().SubmitOrder(UserID, AddressID, ShopID);
            return this.SuccessData(n);
        }
        
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="CreateID"></param>
        /// <returns></returns>
        public ActionResult OrderList(Pagination pagination, string UserID)
        {
            PagedData<ShopOrderInfo2> pagedData = this.CreateService<IShopOrderService>().GetPageData(pagination, UserID);
            return this.SuccessData(pagedData);
        }

        public ActionResult OrderDetail(string id)
        {
            ShopOrderInfo2 info = new ShopOrderInfo2();
            ShopOrder modle = this.CreateService<IShopOrderService>().GetModel(id);
            info.AddressID = modle.AddressID;
            info.AddressInfo = this.CreateService<IUsers_AddressService>().GetModel(modle.AddressID);
            info.AuthID = "";
            info.CreateDate = modle.CreateDate;
            info.CreateID = "";
            info.Freight = modle.Freight;
            info.Id = modle.Id;
            info.OrderCode = modle.OrderCode;
            info.OrderItem = this.CreateService<IShopOrderItemService>().GetOrderItemList(modle.Id);
            info.ProTotal = modle.ProTotal;
            info.ST = modle.ST;
            info.STName = this.CreateService<IShopOrderService>().GetST_Name(modle.ST);
            info.Total = modle.Total;
            info.UpdateTime = modle.UpdateTime;
            return this.SuccessData(info);
        }

        public ActionResult OrderCancel(string id,int st)
        {
            if(!string.IsNullOrEmpty(id))
            {
                int n = this.CreateService<IShopOrderService>().UpdateOrderStatus(id, st);
                if(n>0)
                {
                    return this.SuccessData("OK");
                }
                else
                {
                    return this.FailedMsg("订单不存在");
                }
            }
            else
            {
                return this.FailedMsg("参数缺失");
            }
        }

        public ActionResult ProCollectionList(string UserID)
        {
            List<Pro_CollectionInfo> list = this.CreateService<IPro_CollectionService>().GetList(UserID);
            return this.SuccessData(list);
        }

        public ActionResult CollectDelete(string id)
        {
            this.CreateService<IPro_CollectionService>().Delete(id);
            return this.SuccessData("OK");
        }

        public ActionResult CollectAdd(string UserID,string ProductID)
        {
            IPro_CollectionService pro_CollectionService = this.CreateService<IPro_CollectionService>();
            int count = pro_CollectionService.Count(UserID, ProductID);
            if(count==0)
            {
                AddPro_CollectionInput input = new AddPro_CollectionInput();
                input.CreateDate = DateTime.Now;
                input.ProductID = ProductID;
                input.UserID = UserID;
                pro_CollectionService.Add(input);
            }
            else
            {
                pro_CollectionService.Delete(UserID, ProductID);
            }
            return this.SuccessData("OK");
        }

        /// <summary>
        /// 浏览记录
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult ProductRcdList(Pagination pagination,string UserID)
        {
            PagedData<Product_rcdInfo> list = this.CreateService<IProduct_rcdService>().GetPageList(pagination, UserID);
            return this.SuccessData(list);
        }


        public ActionResult ProductRcdAdd(string UserID, string ProductID)
        {
            IProduct_rcdService product_RcdService = this.CreateService<IProduct_rcdService>();
            Product_rcd modle = product_RcdService.GetModel(UserID, ProductID);
            if(modle==null)
            {
                AddProduct_rcdInput input = new AddProduct_rcdInput();
                input.CreateDate = DateTime.Now;
                input.Hit = 1;
                input.ProductID = ProductID;
                input.UpdateTime = DateTime.Now;
                input.UserID = UserID;
                product_RcdService.Add(input);
            }
            else
            {
                product_RcdService.UpdateHit(modle.Id, modle.Hit + 1);
            }
            return this.SuccessData("OK");
        }

        public ActionResult ProductRcdDelete(string id)
        {
            this.CreateService<IProduct_rcdService>().Delete(id);
            return this.SuccessData("OK");
        }

        public ActionResult Users_SecurityList(Pagination pagination, string UserID)
        {
            PagedData<Users_SecurityInfo> list = this.CreateService<IUsers_SecurityService>().GetPageData(pagination, UserID);
            return this.SuccessData(list);
        }

        [HttpPost]
        public ActionResult FeedbackAdd(string Summary,int SortID,string Mobile,string UserID,string SortName)
        {
            AddNewsInput input = new AddNewsInput();
            input.Contents = "";
            input.CreateDate = DateTime.Now;
            input.CreateID = UserID;
            input.ImageUrl = "";
            input.IsValid = 0;
            input.LinkUrl = Mobile;
            input.SortID = SortID;
            input.Summary = Summary;
            input.Title = SortName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            this.CreateService<INewsService>().Add(input);
            return this.SuccessData("OK");
        }


        public ActionResult SearchIndex()
        {
            return this.SuccessData(new { historyKeywordList="啤酒;酸奶;辣条;薯片;面包;王老吉", defaultKeyword ="酸奶", hotKeywordList = "啤酒;酸奶;辣条" });
        }

        public ActionResult SearchHelper(string keyword, int ShopID)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                List<ProductSearchKey> list = this.CreateService<IProductService>().GetSearchKeys(keyword, ShopID);
                return this.SuccessData(list);
            }
            else
            {
                return this.FailedMsg();
            }
        }

        public ActionResult UpdateOrderStatus(string id,int ST=0,string prepay_id="",string openId="")
        {
            this.CreateService<IShopOrderService>().UpdateOrderStatus(id, 1);
            if(!string.IsNullOrEmpty(prepay_id) &&!string.IsNullOrEmpty(openId) )
            {
                ShopOrder order = this.CreateService<IShopOrderService>().GetModel(id);

                string pro_cont = string.Empty;
                List<ShopOrderItemInfo> list= this.CreateService<IShopOrderItemService>().GetOrderItemList(id);
                foreach(var item in list)
                {
                    pro_cont += item.ProductName + "(数量:" + item.ItemNum + ")";
                }

                Shop shop = this.CreateService<IShopService>().GetModel(order.ShopID);

                

                string WxOpenAppId = Globals.Configuration["AppSettings:WxOpenAppId"].ToString();
                var data = new
                {
                    keyword1 = new TemplateDataItem(order.Total.ToString()),//订单金额
                    keyword2 = new TemplateDataItem("已付款"),
                    keyword3 = new TemplateDataItem(order.OrderCode),
                    keyword4 = new TemplateDataItem(pro_cont),
                    keyword5 = new TemplateDataItem(order.CreateDate.ToString("yyyy-MM-dd HH:mm")),
                    keyword6 = new TemplateDataItem(shop.ShopName),
                    keyword7 = new TemplateDataItem(shop.ShopAddress),
                    keyword8 = new TemplateDataItem(shop.ShopTel)
                };

                var tmResult = Senparc.Weixin.WxOpen.AdvancedAPIs.Template.TemplateApi.SendTemplateMessage(WxOpenAppId, openId, "83MxvljD9qeLsjG133X9FpTix08jWPHCq29242h8loM", data, prepay_id, "pages/websocket/websocket", "websocket",
                         null);
            }
            return this.SuccessData("OK");
        }

        //---------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="OpenId"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult GetPrepayid(string OpenId, string OrderID)
        {
            //try
            //{
                ShopOrder shopOrder = this.CreateService<IShopOrderService>().GetModel(OrderID);


                string WxOpenAppId = Globals.Configuration["AppSettings:WxOpenAppId"].ToString();
                string WxOpenAppSecret = Globals.Configuration["AppSettings:WxOpenAppSecret"].ToString();
                string TenPayV3_MchId = Globals.Configuration["AppSettings:TenPayV3_MchId"].ToString();
                string TenPayV3_Key = Globals.Configuration["AppSettings:TenPayV3_Key"].ToString();
                string TenpayNotify = Globals.Configuration["AppSettings:TenpayNotify"].ToString();


                //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                //var sp_billno = DateTime.Now.ToString("yyyyMMddHHmm") + TenPayV3Util.BuildRandomStr(6);
                var sp_billno = shopOrder.OrderCode;
                string attach = shopOrder.CreateID + "|" + OpenId + "|" + OrderID + "|" + sp_billno;

                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();
                var ip = HttpContext.GetClientIP();//


                var body = "total：" + shopOrder.Total;
                var price = 1;//单位：分
                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WxOpenAppId, TenPayV3_MchId, body, sp_billno,
                    price, ip, TenpayNotify, Senparc.Weixin.TenPay.TenPayV3Type.JSAPI, OpenId, TenPayV3_Key, nonceStr);
                
                var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口

                //WeixinTrace.SendCustomLog("统一订单接口调用结束", "请求：\r\n\r\n" + xmlDataInfo.ToJson() + "\r\n\r\n返回结果：\r\n\r\n" + result.ToJson());
                //Logger logger = LogManager.GetLogger("SimpleDemo");
                //logger.Info("\r\n\r\n*********************************************************************\r\n\r\n");
                //logger.Info("\r\n\r\n" + xmlDataInfo.ToJson()+"\r\n\r\n" );
                //logger.Info("\r\n\r\n*********************************************************************\r\n\r\n");
                //logger.Info("\r\n\r\n" + result.ToJson()+ "\r\n\r\n");

            var packageStr = "prepay_id=" + result.prepay_id;

                
                return Json(new
                {
                    success = true,
                    prepay_id = result.prepay_id,
                    appId = WxOpenAppId,
                    timeStamp,
                    nonceStr,
                    package = packageStr,
                    signType = "MD5",
                    paySign = TenPayV3.GetJsPaySign(WxOpenAppId, timeStamp, nonceStr, packageStr, TenPayV3_Key)
                });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        msg = ex.Message
            //    });
            //}

        }


        public ActionResult PayNotifyUrl()
        {
            //try
            //{

            Logger logger = LogManager.GetLogger("SimpleDemo");
            logger.Info("\r\n\r\n*********************************************************************\r\n\r\n");

            

            string TenPayV3_Key = Globals.Configuration["AppSettings:TenPayV3_Key"].ToString();
            ResponseHandler resHandler = new ResponseHandler(HttpContext);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            logger.Info("\r\n\r\n return_code:" + return_code + "\r\n\r\n");
            logger.Info("\r\n\r\n return_code:" + return_msg + "\r\n\r\n");


            string res = null;

            resHandler.SetKey(TenPayV3_Key);
            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
            {
                res = "success";//正确的订单处理
                //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
            }
            else
            {
                res = "wrong";//错误的订单处理
            }

            /* 这里可以进行订单处理的逻辑 */
            string UserID = "";
            string OpenId = "";
            string OrderID = "";
            string sp_billno = "";
            if (res == "success")
            {
                //string attach = shopOrder.CreateID + "|" + OpenId + "|" + OrderID + "|" + sp_billno;
                string attach = resHandler.GetParameter("attach");
                logger.Info("\r\n\r\n return_code:" + attach + "\r\n\r\n");
                if (!string.IsNullOrEmpty(attach))
                {
                    string[] arr = attach.Split('|');
                    UserID = arr[0];
                    OpenId = arr[1];
                    OrderID = arr[2];
                    sp_billno = arr[3];
                    this.CreateService<IShopOrderService>().UpdateOrderStatus(OrderID, 1);
                }
            }

            //发送支付成功的模板消息
            try
                {
                    string appId = Config.SenparcWeixinSetting.TenPayV3_AppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
                    string WxOpenAppId = Globals.Configuration["AppSettings:WxOpenAppId"].ToString();
                    string openId = resHandler.GetParameter("openid");
                    var templateData = new WeixinTemplate_PaySuccess("https://weixin.senparc.com", "购买商品", "状态：" + return_code);

                    Senparc.Weixin.WeixinTrace.SendCustomLog("支付成功模板消息参数", appId + " , " + openId);

                    var result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(appId, openId, templateData);

                   logger.Info("\r\n\r\n 支付成功模板消息 suc:" + "", result.ToJson() + "\r\n\r\n");
            }
                catch (Exception ex)
                {
                    //Senparc.Weixin.WeixinTrace.SendCustomLog("支付成功模板消息", ex.ToString());
                    logger.Info("\r\n\r\n 支付成功模板消息 err:" + "", ex.ToString() + "\r\n\r\n");
                }

            var notifyXml = resHandler.ParseXML();

            logger.Info("\r\n\r\n notifyXml:" + "", notifyXml + "\r\n\r\n");
            //var logDir = ServerUtility.ContentRootMapPath(string.Format("~/App_Data/TenPayNotify/{0}", SystemTime.Now.ToString("yyyyMMdd")));
            //if (!Directory.Exists(logDir))
            //{
            //    Directory.CreateDirectory(logDir);
            //}
            //var logPath = Path.Combine(logDir, string.Format("{0}-{1}-{2}.txt", SystemTime.Now.ToString("yyyyMMdd"), SystemTime.Now.ToString("HHmmss"), Guid.NewGuid().ToString("n").Substring(0, 8)));
            //using (var fileStream = System.IO.File.OpenWrite(logPath))
            //{
            //    var notifyXml = resHandler.ParseXML();
            //    //fileStream.Write(Encoding.Default.GetBytes(res), 0, Encoding.Default.GetByteCount(res));

            //    fileStream.Write(Encoding.Default.GetBytes(notifyXml), 0, Encoding.Default.GetByteCount(notifyXml));
            //    fileStream.Close();
            //}

            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", return_code, return_msg);
                return Content(xml, "text/xml");
            //}
            //catch (Exception ex)
            //{
            //    WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));
            //    throw;
            //}
        }




        /*
        public ActionResult GetPrepayid(string OpenId,string OrderID)
        {
            try
            {

                ShopOrder shopOrder = this.CreateService<IShopOrderService>().GetModel(OrderID);


                string WxOpenAppId = Globals.Configuration["AppSettings:WxOpenAppId"].ToString();
                string WxOpenAppSecret = Globals.Configuration["AppSettings:WxOpenAppSecret"].ToString();
                string TenPayV3_MchId = Globals.Configuration["AppSettings:TenPayV3_MchId"].ToString();
                string TenPayV3_Key = Globals.Configuration["AppSettings:TenPayV3_Key"].ToString();
                string TenpayNotify = Globals.Configuration["AppSettings:TenpayNotify"].ToString();

                //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一

                var sp_billno = DateTime.Now.ToString("yyyyMMddHHmmss") + TenPayV3Util.BuildRandomStr(6);
                string attach = shopOrder.CreateID + "|" + OpenId + "|" + OrderID + "|" + sp_billno;

                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();

                var ip = HttpContext.GetClientIP();

                var body ="测试订单总额："+ shopOrder.Total;
                var price = 1;//单位：分
                var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WxOpenAppId, TenPayV3_MchId, body, sp_billno,
                    price, ip, TenpayNotify, TenPayV3Type.JSAPI, OpenId, TenPayV3_Key, nonceStr, null, null, null, null, attach, "CNY", null, OrderID);

                var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口

                WeixinTrace.SendCustomLog("统一订单接口调用结束", "请求：" + xmlDataInfo.ToJson() + "\r\n\r\n返回结果：" + result.ToJson());

                var packageStr = "prepay_id=" + result.prepay_id;

                //记录到缓存

                var cacheStrategy = CacheStrategyFactory.GetObjectCacheStrategyInstance();
                cacheStrategy.Set($"WxOpenUnifiedorderRequestData-{openId}", xmlDataInfo, TimeSpan.FromDays(4));//3天内可以发送模板消息
                cacheStrategy.Set($"WxOpenUnifiedorderResultData-{openId}", result, TimeSpan.FromDays(4));//3天内可以发送模板消息

                return Json(new
                {
                    success = true,
                    prepay_id = result.prepay_id,
                    appId = WxOpenAppId,
                    timeStamp,
                    nonceStr,
                    package = packageStr,
                    signType = "MD5",
                    paySign = TenPayV3.GetJsPaySign(WxOpenAppId, timeStamp, nonceStr, packageStr, TenPayV3_Key)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = ex.Message
                });
            }

        }
        



         
            public ActionResult PayNotifyUrl()
        {
            try
            {
                string TenPayV3_Key = Globals.Configuration["AppSettings:TenPayV3_Key"].ToString();

                ResponseHandler resHandler = new ResponseHandler(HttpContext);

                string return_code = resHandler.GetParameter("return_code");
                string return_msg = resHandler.GetParameter("return_msg");

                string res = null;

                resHandler.SetKey(TenPayV3_Key);
                //验证请求是否从微信发过来（安全）
                if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
                {
                    res = "success";//正确的订单处理
                    //直到这里，才能认为交易真正成功了，可以进行数据库操作，但是别忘了返回规定格式的消息！
                }
                else
                {
                    res = "wrong";//错误的订单处理
                }

                 
                string UserID = "";
                string OpenId = "";
                string OrderID = "";
                string sp_billno = "";
                if (res== "success")
                {
                    //string attach = shopOrder.CreateID + "|" + OpenId + "|" + OrderID + "|" + sp_billno;
                    string attach = resHandler.GetParameter("attach");
                    if (!string.IsNullOrEmpty(attach))
                    {
                        string[] arr = attach.Split('|');
                        UserID = arr[0];
                        OpenId = arr[1];
                        OrderID = arr[2];
                        sp_billno = arr[3];
                    }
                }
                //发送支付成功的模板消息
                try
                {
                    string appId = Config.SenparcWeixinSetting.TenPayV3_AppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
                    string openId = resHandler.GetParameter("openid");
                    var templateData = new WeixinTemplate_PaySuccess("https://weixin.senparc.com", "购买商品", "状态：" + return_code);

                    //Senparc.Weixin.WeixinTrace.SendCustomLog("支付成功模板消息参数", appId + " , " + openId);

                    var result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(appId, openId, templateData);
                }
                catch (Exception ex)
                {
                    //Senparc.Weixin.WeixinTrace.SendCustomLog("支付成功模板消息", ex.ToString());
                }
                 
                string xml = string.Format(@"<xml>
<return_code><![CDATA[{0}]]></return_code>
<return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);
                return Content(xml, "text/xml");
            }
            catch (Exception ex)
            {
                //WeixinTrace.WeixinExceptionLog(new WeixinException(ex.Message, ex));
                throw;
            }
        }
    */

     

    }
}