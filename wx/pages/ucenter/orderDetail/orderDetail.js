var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');
var app=getApp();
Page({
  data: {
    orderId:'',
    orderInfo: {},
    FileHost:'',
    token:null
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
    var token=wx.getStorageSync('wx');
    if(token)
    {
      this.setData({token:token});
    }
    this.setData({FileHost:api.FileHost});
    this.setData({
      orderId: options.id
    });
    this.getOrderDetail();
  },
  getOrderDetail() {
    let that = this;
    util.request(api.OrderDetail, {
      id: that.data.orderId
    },'GET').then(function (res) {
      if (res.Status === 100) 
      {
        res.Data.CreateDate = util.formatTime(res.Data.CreateDate);
        that.setData({
          orderInfo: res.Data
        });
        //that.payTimer();
      }
    });
  },
  payTimer() {
    let that = this;
    let orderInfo = that.data.orderInfo;

    setInterval(() => {
      orderInfo.add_time -= 1;
      that.setData({
        orderInfo: orderInfo,
      });
    }, 1000);
  },
  cancelOrder(){
    let that = this;
    let orderInfo = that.data.orderInfo;

    var order_status = orderInfo.ST;

    var errorMessage = '';
    switch (order_status){
      case 2: {
        errorMessage = '订单已发货';
        break;
      }
      case 3:{
        errorMessage = '订单已收货';
        break;
      }
      case 10:{
        errorMessage = '订单已取消';
        break;
      }
       
    }
      
    if (errorMessage != '') {
      util.showErrorToast(errorMessage);
      return false;
    }
    
    wx.showModal({
      title: '',
      content: '确定要取消此订单？',
      success: function (res) {
        if (res.confirm) 
        {
          util.request(api.OrderCancel,{id: orderInfo.Id},'GET').then(function (res) {
            if (res.Status === 100) {
              wx.showModal({
                title:'提示',
                content: res.data,
                showCancel:false,
                confirmText:'继续',
                success: function (res) {
                  wx.navigateBack({
                    url: 'pages/ucenter/order/order',
                  });
                }
              });
            }
          });

        }
      }
    });
  },
  payOrder() {
    let that = this;
    
    if(that.data.token==null)
    {
      wx.showToast({
        title: 'openid不存在，请重新打开小程序'
      });
      return;
    }
    var openid = that.data.token.openid;

    util.request(api.PayPrepayId, {
      OrderID: that.data.orderId, OpenId:openid
    }).then(function (res) {
      if (res.success ===true) 
      {
        const payParam = res;
        wx.requestPayment({
          'timeStamp': payParam.timeStamp,
          'nonceStr': payParam.nonceStr,
          'package': payParam.package,
          'signType': payParam.signType,
          'paySign': payParam.paySign,
          'success': function (res1) {
            console.log(res1);
            wx.redirectTo({url: '/pages/ucenter/order/order'});
          },
          'fail': function (res2) {
            wx.showToast({title: '付款失败'});
          }
        });
      }
      else
      {
        wx.showToast({ title: '付款失败' });
      }
    });

  },
  confirmOrder() {
//确认收货
      let that = this;
      let orderInfo = that.data.orderInfo;

      var order_status = orderInfo.order_status;

      var errorMessage = '';
      switch (order_status) {
          // case 300: {
          //   errorMessage = '订单已发货';
          //   break;
          // }
          case 301: {
              errorMessage = '订单已收货';
              break;
          }
          case 101: {
              errorMessage = '订单已取消';
              break;
          }
          case 102: {
              errorMessage = '订单已删除';
              break;
          }
          case 401: {
              errorMessage = '订单已退款';
              break;
          }
          case 402: {
              errorMessage = '订单已退货';
              break;
          }
      }

      if (errorMessage != '') {
          util.showErrorToast(errorMessage);
          return false;
      }

      wx.showModal({
          title: '',
          content: '确定已经收到商品？',
          success: function (res) {
              if (res.confirm) {

                  util.request(api.OrderConfirm, {
                      orderId: orderInfo.id
                  }).then(function (res) {
                      if (res.errno === 0) {
                          wx.showModal({
                              title: '提示',
                              content: res.data,
                              showCancel: false,
                              confirmText: '继续',
                              success: function (res) {
                                  //  util.redirect('/pages/ucenter/order/order');
                                  wx.navigateBack({
                                      url: 'pages/ucenter/order/order',
                                  });
                              }
                          });
                      }
                  });

              }
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