var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');



var app = getApp();

Page({
  data: {
    footprintList: [],
    userInfo:null,

    page: 1,
    size: 5,
    loadmoreText: '正在加载更多数据',
    nomoreText: '全部加载完成',
    nomore: false,
    totalPages: 1,

    FileHost:''
  },
  onPullDownRefresh(){
    // 增加下拉刷新数据的功能
    var self = this;
    this.getFootprintList();
  },
  getFootprintList() {
    let that = this;
    var tmpFootPrint;
    var userid=that.data.userInfo.Id;
    util.request(api.ProductRcdList, { UserID:userid},'GET').then(function (res) {
      if (res.Status === 100) 
      {
        for (var i = 0; i < res.Data.Models.length;i++)
        {
          res.Data.Models[i].UpdateTime = util.formatTime(res.Data.Models[i].UpdateTime);
          res.Data.Models[i].CreateDate = util.formatTime(res.Data.Models[i].CreateDate);
        }
        that.setData({
          footprintList: that.data.footprintList.concat(res.Data.Models),
          page: res.Data.CurrentPage + 1,
          totalPages: res.Data.TotalPage
        }); 
      }
    });
  },
  deleteItem (event){
    let that = this;
    var cid = event.currentTarget.dataset.id; 
    var touchTime = that.data.touch_end - that.data.touch_start;
    //如果按下时间大于350为长按
    if (touchTime > 350) {
      wx.showModal({
        title: '',
        content: '要删除所选足迹？',
        success: function (res) {
          if (res.confirm) {
            util.request(api.ProductRcdDelete, { id: cid },'GET').then(function (res) {
              if (res.Status === 100) 
              {
                wx.showToast({
                  title:'删除成功',
                  icon: 'success',
                  duration: 2000,
                  complete:function(){
                    that.getFootprintList();
                  }
                });                
              } else{
                util.showErrorToast('删除失败');
              }
            });
          }
        }
      });
    } else {
      wx.navigateTo({
        url: '/pages/goods/goods?id=' + footprint.goodsId,
      });
    }
    
  },
  onLoad: function (options) 
  {
    this.setData({FileHost:api.FileHost});
    var userInfo=wx.getStorageSync('userInfo');
    if (userInfo!="")
    {
      this.setData({ userInfo: userInfo });
      this.getFootprintList();
    }
    
    
  },
  onReady: function () {

  },
  onShow: function () {

  },
  onHide: function () {
    // 页面隐藏

  },
  onUnload: function () {
    // 页面关闭
  },
  //按下事件开始  
  touchStart: function (e) {
    let that = this;
    that.setData({
      touch_start: e.timeStamp
    })
  },
  //按下事件结束  
  touchEnd: function (e) {
    let that = this;
    that.setData({
      touch_end: e.timeStamp
    })
  },
  onReachBottom: function () {
    this.getFootprintList();
  },
})