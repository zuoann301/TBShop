var app = getApp();
var util = require('../../utils/util.js');
var api = require('../../config/api.js');

Page({
  data: {
    orderId: 0,
    total: 0.00,
    token:null
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
    this.setData({
      orderId: options.orderId,
      total: options.total
    });
    var token=wx.getStorageSync('wx');
    this.setData({token:token});
  },
  onReady: function () {

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
  //向服务请求支付参数
  requestPayParam() 
  {
    let that = this;
    if(that.data.token==null)
    {
      wx.showToast({title: '无法获取openid，请重新打开小程序'});
      return;
    }
    var openid=that.data.token.openid;
    util.request(api.PayPrepayId, { OrderID: that.data.orderId, OpenId: openid }).then(function (res) {
      if (res.success ===true) {
        let payParam = res;
        wx.requestPayment({
          'timeStamp': payParam.timeStamp,
          'nonceStr': payParam.timeStamp,
          'package': payParam.nonceStr,
          'signType': payParam.signType,
          'paySign': payParam.paySign,
          'success': function (res) {
            wx.redirectTo({
              url: '/pages/payResult/payResult?status=true&orderId=' + that.data.orderId,
            })
          },
          'fail': function (res) {
            wx.redirectTo({
              url: '/pages/payResult/payResult?status=false&orderId=' + that.data.orderId,
            })
          }
        })
      }
    });
  },
  startPay() {
    this.requestPayParam();
  }
})