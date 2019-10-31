const root = '/platform/api/';
//const API_BASE_URL = 'http://localhost:8080' + root;
const API_BASE_URL = 'http://fly2you.cn' + root;
//const API_URL = 'http://localhost:62021';//api.zhaixiangyoukuai.com
const API_URL = 'https://api.zhaixiangyoukuai.com';

module.exports = {
  FileHost: API_URL,
  Upload: API_URL +'/File/UploadFile',
  GetOpenID: API_URL +'/API/JsCode2Json',//Get OpenID

  CheckOpenID: API_URL +'/API/CheckOpenID',//检测是否注册
  ShopList: API_URL +'/API/ShopList',//商家列表
  ShopInfo: API_URL +'/API/ShopInfo',//商家详细信息

  SimpleLogin: API_URL +'/API/SimpleLogin',//简单登录

  IndexUrlBanner: API_URL + '/API/LinkList', //首页banner
  IndexUrlChannel: API_URL + '/API/ProSortList', //banner下的分类

  IndexUrlBrand: API_URL + '/API/BrandList', //品牌制造商
  IndexUrlNewGoods: API_URL + '/API/ProTopList', //新品首发
  IndexUrlHotGoods: API_URL + '/API/ProHotList', //热卖商品

  BrandDetail: API_URL + '/API/BrandDetail',  //品牌详情
  BrandProduct: API_URL +'/API/BrandProduct',//品牌商品

  IndexUrlTopic: API_URL + '/API/NewsList', //专题精选
  NewsDetail: API_URL +'/API/NewsDetail',//专题详细内容
  CommentList: API_URL + '/API/CommentList',//评论列表
  CommentPost: API_URL + '/API/CommentPost',   //发表评论


  NewsPageList: API_URL +'/API/NewsPageList',//专题列表 不分商家  全部显示 分页
  

  IndexUrlCategory: API_BASE_URL + 'index/category', //首页底部的分类及商品列表
  

  CatalogList: API_URL + '/API/CateList',  //分类目录全部分类数据接口 catalog/index
  CatalogCurrent: API_URL + '/API/CateInfo',  //分类目录当前分类数据接口 catalog/current

    AuthLoginByWeixin: API_BASE_URL + 'auth/login_by_weixin', //微信登录

    GoodsCount: API_URL + '/API/ProductCount',  //统计商品总数
    ProductList: API_URL +'/API/ProductList', //商品列表
  ProductPageList: API_URL + '/API/ProductPageList',//商品列表 分页
    ProductTopList: API_URL +'/API/ProductTopList',//新品列表
    ProductHotList: API_URL + '/API/ProductHotList',//人气推荐列表

    AuthLoginByWeixin: API_BASE_URL + 'auth/login_by_weixin', //微信登录

    GoodsCount: API_URL + '/API/ProductCount',  //统计商品总数


  SetCart: API_URL +'/API/SetCart',//添加到购物车
  SmsCode: API_URL +'/API/SmsCode',//短信验证码
  BindMobile: API_URL + '/API/BindMobile', //绑定手机

  CartGoodsCount: API_URL + '/API/CartGoodsCount', // 获取购物车商品件数
    
    GoodsList: API_BASE_URL + 'goods/list',  //获得商品列表
    GoodsCategory: API_BASE_URL + 'goods/category',  //获得分类数据
    GoodsDetail: API_URL + '/API/ProductInfo',  //获得商品的详情 good/detail
    //GoodsDetail: API_BASE_URL + 'good/detail',  //获得商品的详情 good/detail
    GoodsHot: API_BASE_URL + 'goods/hot',  //人气推荐
    GoodsRelated: API_BASE_URL + 'goods/related',  //商品详情页的关联商品（大家都在看）

  BrandPageList: API_URL + '/API/BrandPageList',  //品牌列表
  Users_SecurityList: API_URL +'/API/Users_SecurityList',//登录日志
    

    CartList: API_URL + '/API/CartList', //获取购物车的数据
    CartAdd: API_BASE_URL + 'cart/add', // 添加商品到购物车
    BuyAdd: API_BASE_URL + 'buy/add', // 直接购买    
  CartUpdate: API_URL + '/API/CartUpdate', // 更新购物车的商品
  CartChecked: API_URL +'/API/CartChecked',//确定要付款的项目
  CartDelete: API_URL + '/API/CartDelete', // 删除购物车的商品
    
  CartTempOrder: API_URL + '/API/CartTempOrder', // 下单前信息确认

  OrderSubmit: API_URL + '/API/OrderSubmit', // 提交订单
  PayPrepayId: API_URL + '/API/GetPrepayid', //获取微信统一下单prepay_id
  UpdateOrderStatus: API_URL +'/API/UpdateOrderStatus',//更新订单状态

  CollectList: API_URL + '/API/ProCollectionList',  //收藏列表
  CollectDelete: API_URL + '/API/CollectDelete',  //添加或取消收藏
  CollectAdd: API_URL +'/API/CollectAdd',//添加收藏
    CommentCount: API_BASE_URL + 'comment/count',  //评论总数
    

    TopicList: API_BASE_URL + 'topic/list',  //专题列表
    TopicDetail: API_BASE_URL + 'topic/detail',  //专题详情
    TopicRelated: API_BASE_URL + 'topic/related',  //相关专题

  SearchIndex: API_URL + '/API/SearchIndex',  //搜索页面数据
  SearchHelper: API_URL + '/API/SearchHelper',  //搜索帮助
  SearchClearHistory: API_BASE_URL + 'search/clearhistory',  //搜索帮助

  AddressList: API_URL + '/API/AddressList',  //收货地址列表
  AddressDetail: API_URL + '/API/AddressDetail',  //收货地址详情
  AdressAdd: API_URL +'/API/AdressAdd',
  AdressUpdate: API_URL +'/API/AdressUpdate',
  AddressDelete: API_URL + '/API/AddressDelete',  //删除收货地址
  AddressDefault: API_URL +'/API/AddressDefault',//默认地址
  AddressInfo: API_URL +'/API/AddressInfo',//地址信息

    RegionList: API_BASE_URL + 'region/list',  //获取区域列表

  OrderList: API_URL + '/API/OrderList',  //订单列表
  OrderDetail: API_URL + '/API/OrderDetail',  //订单详情
  OrderCancel: API_URL + '/API/OrderCancel',  //取消订单
    OrderConfirm: API_BASE_URL + 'order/confirmOrder',  //确认收货

  ProductRcdList: API_URL + '/API/ProductRcdList',  //足迹列表
  ProductRcdDelete: API_URL + '/API/ProductRcdDelete',  //删除足迹

  FeedbackAdd: API_URL + '/API/FeedbackAdd', //添加反馈
    
    Login: API_BASE_URL + 'auth/login', //账号登录
    Register: API_BASE_URL + 'auth/register', //注册
    CouponList: API_BASE_URL + 'coupon/list', // 优惠券列表
    GoodsCouponList: API_BASE_URL + 'coupon/listByGoods', // 商品优惠券列表   
    OrderQuery: API_BASE_URL + 'pay/query',//微信查询订单状态

};
