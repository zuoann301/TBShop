var util = require('../../utils/util.js');
var api = require('../../config/api.js');
const pay = require('../../services/pay.js');

var app = getApp();
Page({
  data: {
    status: false,
    orderId:''
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
    this.setData({
      orderId: options.orderId,
      status:options.status
    })
    if (options.status =='true')
    {
      this.updateSuccess();
    }
   
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
  
  updateSuccess: function () {
    let that = this
    util.request(api.UpdateOrderStatus, {id: this.data.orderId,ST:1}).then(function (res) {})
  },

  payOrder() 
  {
    var token=wx.getStorageSync('wx');
    var openid = token.openid;
    pay.payOrder(this.data.orderId, openid).then(res => {
      this.setData({
        status: true
      });
      util.showSuccessToast('支付成功');
    }).catch(res => {
      util.showErrorToast('支付失败');
    });
  }
})