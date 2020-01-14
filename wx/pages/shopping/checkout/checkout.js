var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');
const pay = require('../../../services/pay.js');

var app = getApp();

Page({
  data: {
    checkedGoodsList: [],
    checkedAddress:null,
    checkedCoupon: [],
    couponList: [],
    goodsTotalPrice: 0.00, //商品总价
    freightPrice: 0.00,    //快递费
    couponPrice: 0.00,     //优惠券的价格
    orderTotalPrice: 0.00,  //订单总价
    actualPrice: 0.00,     //实际需要支付的总价
    addressId:'',
    couponId: 0,
    isBuy: false,
    couponDesc: '',
    couponCode: '',
    buyType: '',
    userInfo:null,
    FileHost: ''
  },
  onLoad: function (options) {
    this.setData({ FileHost: api.FileHost });
    // 页面初始化 options为页面跳转所带来的参数
    if (options.isBuy!=null) {
      this.data.isBuy = options.isBuy
    }
     
    this.data.buyType = this.data.isBuy?'buy':'cart'
    //每次重新加载界面，清空数据
    //app.globalData.userCoupon = 'NO_USE_COUPON'
    //app.globalData.courseCouponCode = {};
    var userInfo=wx.getStorageSync('userInfo');
    if(userInfo!="")
    {
      this.setData({ userInfo: userInfo});
    }
  },
  onReady: function () {
    // 页面渲染完成

  },
  onShow: function () {
    //this.getCouponData()
    // 页面显示
    wx.showLoading({
      title: '加载中...',
    })
    //this.getCheckoutInfo();
    var addressId = wx.getStorageSync('addressId');
    if (addressId != "") {
      this.setData({ addressId: addressId });
    }
    
    this.getTempOrder();
    this.getAddress();
  },
  getTempOrder:function(){
    var that=this;
    var userid = that.data.userInfo.Id;
    if(userid!="")
    {
      util.request(api.CartTempOrder, { UserID: userid}, 'GET').then(function (res) {
        if (res.Status === 100) 
        { 
          that.setData({ checkedGoodsList: res.Data});
          let goodsTotalPrice = 0;
          res.Data.forEach(function (v) 
          {
            goodsTotalPrice += v.ItemNum * v.Price;
          });
          that.setData({ goodsTotalPrice: goodsTotalPrice });
          var total = goodsTotalPrice + that.data.freightPrice;
          that.setData({ actualPrice: total});
        }
      });
    }
  },
  getAddress: function () {
    var that = this;
    var userid = that.data.userInfo.Id;
    var addressId = that.data.addressId;
    
    if (addressId!='')
    {
      util.request(api.AddressDetail, { Id: addressId }, 'GET').then(function (res) {
        if (res.Status === 100) {
          that.setData({ checkedAddress: res.Data });
        }
      });
    }
    else
    {
      if (userid != "") 
      {
        util.request(api.AddressDefault, { UserID: userid }, 'GET').then(function (res) {
          if (res.Status === 100) {
            that.setData({ checkedAddress: res.Data });
            that.setData({ addressId:res.Data.Id});
            wx.setStorageSync("addressId", res.Data.Id);
          }
        });
      }
    }
    
  },
  getCheckedGoodsAmount: function () {
    let checkedGoodsAmount = 0;
    this.data.cartGoods.forEach(function (v) {
      if (v.checked === true) {
        checkedGoodsAmount += v.ItemNum * v.Price;
      }
    });
    return checkedGoodsAmount;
  },
  getCheckoutInfo: function () {
    let that = this;
    var url = api.CartCheckout
    let buyType = this.data.isBuy ? 'buy' : 'cart'
    util.request(url, { addressId: that.data.addressId, couponId: that.data.couponId, type: buyType }).then(function (res) {
      if (res.errno === 0) {
        that.setData({
          checkedGoodsList: res.data.checkedGoodsList,
          checkedAddress: res.data.checkedAddress,
          actualPrice: res.data.actualPrice,
          checkedCoupon: res.data.checkedCoupon ? res.data.checkedCoupon : "",
          couponList: res.data.couponList ? res.data.couponList : "",
          couponPrice: res.data.couponPrice,
          freightPrice: res.data.freightPrice,
          goodsTotalPrice: res.data.goodsTotalPrice,
          orderTotalPrice: res.data.orderTotalPrice
        });
        //设置默认收获地址
        if (that.data.checkedAddress.id){
            let addressId = that.data.checkedAddress.id;
            if (addressId) {
                that.setData({ addressId: addressId });
            }
        }else{
            wx.showModal({
                title: '',
                content: '请添加默认收货地址!',
                success: function (res) {
                    if (res.confirm) {
                        that.selectAddress();
                    }
                }
            })
        }
      }
      wx.hideLoading();
    });
  },
  selectAddress() {
    wx.navigateTo({
      url: '/pages/shopping/address/address',
    })
  },
  addAddress() {
    wx.navigateTo({
      url: '/pages/shopping/addressAdd/addressAdd',
    })
  },
  

  /**
   * 获取优惠券
   */
  getCouponData: function () {
    if (app.globalData.userCoupon == 'USE_COUPON') {
      this.setData({
        couponDesc: app.globalData.courseCouponCode.name,
        couponId: app.globalData.courseCouponCode.user_coupon_id,
      })
    } else if (app.globalData.userCoupon == 'NO_USE_COUPON') {
      this.setData({
        couponDesc: "不使用优惠券",
        couponId: '',
      })
    }
  },

  onHide: function () {
    // 页面隐藏

  },
  onUnload: function () {
    // 页面关闭

  },

  /**
   * 选择可用优惠券
   */
  tapCoupon: function () {
    let that = this
  
      wx.navigateTo({
        url: '../selCoupon/selCoupon?buyType=' + that.data.buyType,
      })
  },

  submitOrder: function () {
    if (this.data.addressId <= 0) {
      util.showErrorToast('请选择收货地址');
      return false;
    }
    var token=wx.getStorageSync('wx');
    var openid=token.openid;
    var userid=this.data.userInfo.Id;
    var shopid=app.globalData.ShopID;
    var pd = { UserID: userid,AddressID: this.data.addressId,ShopID:shopid};
    util.request(api.OrderSubmit, pd).then(res => {
      if (res.Status === 100) 
      {
        const orderId = res.Data;
        pay.payOrder(orderId, openid).then(res => {
          wx.redirectTo({
            url: '/pages/payResult/payResult?status=true&orderId=' + orderId
          });
        }).catch(res => {
          wx.redirectTo({
            url: '/pages/payResult/payResult?status=false&orderId=' + orderId
          });
        });
      } else {
        util.showErrorToast('下单失败');
      }
    });
  }
})