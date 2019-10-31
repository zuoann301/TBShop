var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');

Page({
  data:{
    orderList: [],
    page: 1,
    size: 5,
    loadmoreText: '正在加载更多数据',
    nomoreText: '全部加载完成',
    nomore: false,
    totalPages: 1,
    FileHost:''
  },
  onLoad:function(options)
  {
    // 页面初始化 options为页面跳转所带来的参数
    // 页面显示
    this.setData({FileHost:api.FileHost});
    wx.showLoading({
      title: '加载中...',
      success: function () {

      }
    });
    var userInfo=wx.getStorageSync("userInfo");
    if(userInfo!="")
    {
      this.setData({userInfo:userInfo});
    }
    else
    {
      wx.showModal({
        title: '提示',
        content:'请先登录',
        success(res1) {
          if (res1.confirm) {
            wx.reLaunch({ url: '/pages/auth/mobile/mobile' });
          }
          else if (res1.cancel) {
            wx.reLaunch({ url: '/pages/index/index' });             
          }
        }
      });
      wx.hideLoading();
      return;
    }
    this.getOrderList();
  },

  /**
       * 页面上拉触底事件的处理函数
       */
  onReachBottom: function () {
    this.getOrderList()
  },

  getOrderList(){
    let that = this;

    if (that.data.totalPages <= that.data.page - 1) 
    {
      that.setData({nomore: true});
      return;
    }
    var userid="";
    if(that.data.userInfo!="")
    {
      userid=that.data.userInfo.Id;
    }
    else
    {
      wx.showToast({title: '请先登录'});
      return;
    }
    util.request(api.OrderList, {UserID:userid, Page: that.data.page, PageSize: that.data.size},'GET').then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({
          orderList: that.data.orderList.concat(res.Data.Models),
          page: res.Data.CurrentPage + 1,
          totalPages: res.Data.TotalPage
        });
        wx.hideLoading();
      }
      else
      {
        wx.showToast({title: '没有数据'});
      }
    });
  },
  payOrder(event){
      let that = this; 
      let orderIndex = event.currentTarget.dataset.orderIndex;
      let order = that.data.orderList[orderIndex];
      var orderid = event.currentTarget.dataset.orderId;
    var total = event.currentTarget.dataset.total;
      wx.redirectTo({
        url: '/pages/pay/pay?orderId=' + orderid + '&total=' + total,
      })
  },
  onReady:function(){
    // 页面渲染完成
  },
  onShow:function(){

  },
  onHide:function(){
    // 页面隐藏
  },
  onUnload:function(){
    // 页面关闭
  }
})