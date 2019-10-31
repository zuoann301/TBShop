var util = require('../../utils/util.js');
var api = require('../../config/api.js');


var app = getApp();

Page({
  data: {
    id: 0,
    brand: {},
    goodsList: [],
    page: 1,
    size: 1000,

    FileHost:''
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
    var that = this;
    that.setData({ FileHost: api.FileHost});
    that.setData({id: options.id}); 
    this.getBrand();
  },
  getBrand: function () {
    let that = this;
    util.request(api.BrandDetail, { id: that.data.id },'GET').then(function (res) {
      if (res.Status === 100) 
      {
        //res.Data.ImageUrl = api.FileHost + res.Data.ImageUrl;
        that.setData({brand: res.Data});
        that.getGoodsList();
      }
    });
  },
  getGoodsList() {
    var that = this;
    util.request(api.BrandProduct, { id: that.data.id})
      .then(function (res) {
        if (res.Status ===100) 
        {
          that.setData({goodsList: res.Data});
        }
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

  }
})