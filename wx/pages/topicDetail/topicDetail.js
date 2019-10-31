var app = getApp();
var WxParse = require('../../lib/wxParse/wxParse.js');
var util = require('../../utils/util.js');
var api = require('../../config/api.js');
const shop = require('../../services/shop.js');
Page({
  data: {
    id: 0,
    topic: {},
    topicList: [],
    commentCount: 0,
    commentList: [],
    FileHost: ''
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
    var that = this;
    //shop.setPageTitle();
    if(options.shopid)
    {
      wx.setStorageSync('ShopID',options.shopid);
    }
    that.setData({id: options.id});
    that.setData({ FileHost: api.FileHost });
    util.request(api.NewsDetail, { id: that.data.id},'GET').then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({topic: res.Data});
        WxParse.wxParse('topicDetail', 'html', res.Data.Contents, that);
      }
    });
    
    util.request(api.IndexUrlTopic, { SortID: 2, IsValid: 1 }, 'GET').then(function (res) {
      if (res.Status === 100) {
        that.setData({ topicList: res.Data});
      }
    });
    
  },
  getCommentList(){
    let that = this;
    util.request(api.CommentList, { Fid: that.data.id, Num:10 },'GET').then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({commentList: res.Data});
      }
    });
  },
  postComment (){
    wx.navigateTo({
      url: '/pages/commentPost/commentPost?id='+this.data.id,
    })
  },
  onReady: function () {

  },
  onShow: function () {
    // 页面显示
    this.getCommentList();
  },
  onHide: function () {
    // 页面隐藏

  },
  onUnload: function () {
    // 页面关闭

  },
  onShareAppMessage: function () 
  {
    var that=this;
    return {
      title: '物华天宝商城',
      desc: '物华天宝',
      path: '/pages/topicDetail/topicDetail?id='+that.data.id+'&shopid='+app.globalData.ShopID
    }
  },
})