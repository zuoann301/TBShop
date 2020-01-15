const util = require('../../utils/util.js');
const api = require('../../config/api.js');
const shop = require('../../services/shop.js');

//获取应用实例
const app = getApp();

Page({
  data: {
    newGoods: [],
    hotGoods: [],
    topics: [],
    brand: [],
    floorGoods: [],
    banner: [],
    channel: [],
    FileHost:''
  },
  onShareAppMessage: function () {
    return {
      title:app.globalData.shopInfo.ShopName,
      desc: app.globalData.shopInfo.Summary,
      path: '/pages/start/index?id='+app.globalData.ShopID
    }
  },
  onPullDownRefresh(){
	  	// 增加下拉刷新数据的功能
	    var self = this;
	    this.getIndexData();
 },
  getIndexData: function () {
    let that = this;
    var data = new Object();
    var shopid = app.globalData.ShopID;
    util.request(api.IndexUrlNewGoods, { Num: 4, IsTop: 1, ShopID:0},'GET').then(function (res) 
    {
      if (res.Status === 100) 
      {
        data.newGoods= res.Data;
        that.setData(data);
      }
    });
    util.request(api.IndexUrlHotGoods, { Num: 4, IsHot: 1, ShopID: 0},'GET').then(function (res) {
      if (res.Status === 100) 
      {
        data.hotGoods = res.Data;
        that.setData(data);
      }
    });
    util.request(api.IndexUrlTopic, { ShopID:0, Num:4, SortID: 2, IsValid:1},'GET').then(function (res) {
      if (res.Status === 100) 
      {
        data.topics = res.Data
        that.setData(data);
      }
    });
    util.request(api.IndexUrlBrand, {ShopID:0, IsTop:1},'GET').then(function (res) {
      if (res.Status === 100) 
      {
        data.brand = res.Data;
        that.setData(data);
      }
    });
    /*
    util.request(api.IndexUrlCategory).then(function (res) {
      if (res.errno === 0) {
        data.floorGoods = res.data.categoryList
        that.setData(data);
      }
    });
    */
    
    util.request(api.IndexUrlBanner, {ShopID:0, SortID:4},"GET").then(function (res) 
    {
      console.log(res);
      if (res.Status == 100) 
      { 
        that.setData({ banner: res.Data});
      }
    });
    util.request(api.ProSortList,{Pid:'0'},"GET").then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({ channel: res.Data });
      }
    });

  },
  onLoad: function (options) {
    this.setData({ FileHost: api.FileHost });
    this.getIndexData();
    
  },
  onReady: function () {
    // 页面渲染完成
    
  },
  onShow: function () {
    // 页面显示
    //shop.setPageTitle();//显示商家名称
     
  },
  onHide: function () {
    // 页面隐藏
    this.isShow = false;
  },
  onUnload: function () {
    // 页面关闭
  },
  //------------------------------------------------
  
  
//--------------------------------------------------
})
