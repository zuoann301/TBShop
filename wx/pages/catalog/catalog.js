var util = require('../../utils/util.js');
var api = require('../../config/api.js');
const shop = require('../../services/shop.js');

Page({
  data: {
    navList: [],
    categoryList: [],
    currentCategory: {},
    scrollLeft: 0,
    scrollTop: 0,
    goodsCount: 0,
    scrollHeight: 0,
    cur_id:'',
    FileHost: ''
  },
  onLoad: function (options) 
  {
    //shop.setPageTitle();
    this.setData({ FileHost: api.FileHost });
    this.getCatalog();
    
  },
  getCatalog: function () {
    //CatalogList
    let that = this;
    wx.showLoading({
      title: '加载中...',
    });
    util.request(api.CatalogList,{Pid:'0'}).then(function (res) {
        that.setData({navList: res.Data});
        if(that.data.cur_id=='')
        {
          var def_id=res.Data[0].Id;
          that.setData({cur_id:def_id});
          that.getCurrentCategory(def_id);
        }
        wx.hideLoading();
      });
    util.request(api.GoodsCount,{},'GET').then(function (res) {
      that.setData({goodsCount: res.Data});
    });

  },
  getCurrentCategory: function (id) {
    let that = this;
    util.request(api.CatalogCurrent, { id: id },'GET')
      .then(function (res) 
      {
        if (res.Data.ImageUrl!='')
        {
          res.Data.ImageUrl =that.data.FileHost + res.Data.ImageUrl;
        }
        that.setData({currentCategory: res.Data});
      });
  },
  onReady: function () {
    // 页面渲染完成
  },
  onShow: function () {
    // 页面显示
  },
  onHide: function () {
    // 页面隐藏
  },
  onUnload: function () {
    // 页面关闭
  },
  getList: function () {
    var that = this;
    util.request(api.ApiRootUrl + 'api/catalog/' + that.data.currentCategory.cat_id)
      .then(function (res) {
        that.setData({
          categoryList: res.data,
        });
      });
  },
  switchCate: function (event) {
    var that = this;
    var currentTarget = event.currentTarget;
    if (this.data.cur_id == event.currentTarget.dataset.id) {
      return false;
    }
    else{
      that.setData({ cur_id: event.currentTarget.dataset.id});
    }
    this.getCurrentCategory(event.currentTarget.dataset.id);
  }
})