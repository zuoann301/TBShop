var util = require('../../utils/util.js');
var api = require('../../config/api.js');
const shop = require('../../services/shop.js');
var app = getApp();

Page({
  data: {
    cartGoods: [],
    cartTotal: {
      "goodsCount": 0,
      "goodsAmount": 0.00,
      "checkedGoodsCount": 0,
      "checkedGoodsAmount": 0.00
    },
    isEditCart: false,
    checkedAllStatus: true,
    editCartList: [],
    userInfo:null,
    FileHost: ''
  },
  onLoad: function (options) {
    //shop.setPageTitle();
    this.setData({ FileHost: api.FileHost });
    // 页面初始化 options为页面跳转所带来的参数
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
    // 页面显示
    this.getCartList();
  },
  onHide: function () {
    // 页面隐藏

  },
  onUnload: function () {
    // 页面关闭

  },
  getCartList: function () 
  {
    let that = this;
    var userid='';
    if(that.data.userInfo!=null)
    {
      userid=that.data.userInfo.Id;
    }
    else
    {
      return;
    }
    var shopid = app.globalData.ShopID;
    util.request(api.CartList,{UserID:userid,ShopID:shopid},'GET').then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({cartGoods: res.Data});
      }

      that.setData({checkedAllStatus: that.isCheckedAll()});
    });
  },
  isCheckedAll: function () {
    //判断购物车商品已全选
    return this.data.cartGoods.every(function (element, index, array) {
      if (element.checked == true) {
        return true;
      } else {
        return false;
      }
    });
  },
  checkedItem: function (event) {
    let itemIndex = event.target.dataset.itemIndex;
    let that = this;

    if (!this.data.isEditCart) 
    { 
      let tmpCartData = this.data.cartGoods.map(function (element, index, array) {
        if (index == itemIndex) {
          element.checked = !element.checked;
        }
        return element;
      });
        
     
      that.setData({
        cartGoods: tmpCartData,
        checkedAllStatus: that.isCheckedAll(),
        'cartTotal.checkedGoodsCount': that.getCheckedGoodsCount(),
        'cartTotal.checkedGoodsAmount': that.getCheckedGoodsAmount()
      });
    } 
    else 
    {
      //编辑状态
      let tmpCartData = this.data.cartGoods.map(function (element, index, array) {
        if (index == itemIndex){
          element.checked = !element.checked;
        }
        return element;
      });

      that.setData({
        cartGoods: tmpCartData,
        checkedAllStatus: that.isCheckedAll(),
        'cartTotal.checkedGoodsCount': that.getCheckedGoodsCount(),
        'cartTotal.checkedGoodsAmount': that.getCheckedGoodsAmount()
      });
    }
  },
  getCheckedGoodsCount: function(){
    let checkedGoodsCount = 0;
    this.data.cartGoods.forEach(function (v) {
      if (v.checked === true) {
        checkedGoodsCount += v.ItemNum;
      }
    });
    return checkedGoodsCount;
  },
  getCheckedGoodsAmount: function () {
    let checkedGoodsAmount = 0;
    this.data.cartGoods.forEach(function (v) {
      if (v.checked === true) 
      {
        checkedGoodsAmount += v.ItemNum*v.Price;
      }
    });
    return checkedGoodsAmount;
  },
  checkedAll: function () {
    let that = this;

    if (!this.data.isEditCart) {
      var productIds = this.data.cartGoods.map(function (v) {
        return v.product_id;
      });
       

      let checkedAllStatus = that.isCheckedAll();
      let tmpCartData = this.data.cartGoods.map(function (v) {
        v.checked = !checkedAllStatus;
        return v;
      });

      that.setData({
        cartGoods: tmpCartData,
        checkedAllStatus: that.isCheckedAll(),
        'cartTotal.checkedGoodsCount': that.getCheckedGoodsCount(),
        'cartTotal.checkedGoodsAmount': that.getCheckedGoodsAmount()
      });
      
    } else {
      //编辑状态
      let checkedAllStatus = that.isCheckedAll();
      let tmpCartData = this.data.cartGoods.map(function (v) {
        v.checked = !checkedAllStatus;
        return v;
      });

      that.setData({
        cartGoods: tmpCartData,
        checkedAllStatus: that.isCheckedAll(),
        'cartTotal.checkedGoodsCount': that.getCheckedGoodsCount(),
        'cartTotal.checkedGoodsAmount': that.getCheckedGoodsAmount()
      });
    }

  },
  editCart: function () {
    var that = this;
    if (this.data.isEditCart) {
      this.getCartList();
      this.setData({
        isEditCart: !this.data.isEditCart
      });
    } else {
      //编辑状态
      let tmpCartList = this.data.cartGoods.map(function (v) {
        v.checked = false;
        return v;
      });
      this.setData({
        editCartList: this.data.cartGoods,
        cartGoods: tmpCartList,
        isEditCart: !this.data.isEditCart,
        checkedAllStatus: that.isCheckedAll(),
        'cartTotal.checkedGoodsCount': that.getCheckedGoodsCount(),
        'cartTotal.checkedGoodsAmount': that.getCheckedGoodsAmount()
      });
    }

  },
  toIndexPage: function () {
    wx.switchTab({
      url: "/pages/index/index"
    });
  },
  updateCart: function (id,num) {
    let that = this;

    util.request(api.CartUpdate, {id: id,num: num}).then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({
          'cartTotal.checkedGoodsCount': that.getCheckedGoodsCount(),
          'cartTotal.checkedGoodsAmount': that.getCheckedGoodsAmount()
        });
      }

      that.setData({
        checkedAllStatus: that.isCheckedAll()
      });
    });

  },
  cutNumber: function (event) {

    let itemIndex = event.target.dataset.itemIndex;
    let cartItem = this.data.cartGoods[itemIndex];
    let number = (cartItem.ItemNum - 1 > 1) ? cartItem.ItemNum - 1 : 1;
    cartItem.ItemNum = number;
    this.setData({cartGoods: this.data.cartGoods});
    this.updateCart(cartItem.Id, number);
  },
  addNumber: function (event) {
    let itemIndex = event.target.dataset.itemIndex;
    let cartItem = this.data.cartGoods[itemIndex];
    let number = cartItem.ItemNum + 1;
    cartItem.ItemNum = number;
    this.setData({cartGoods: this.data.cartGoods});
    this.updateCart(cartItem.Id, number);
  },
  checkoutOrder: function () {
    //获取已选择的商品
    let that = this;

    var checkedGoods = this.data.cartGoods.filter(function (element, index, array) {
      if (element.checked == true) {
        return true;
      } else {
        return false;
      }
    });

    if (checkedGoods.length <= 0) 
    {
      util.showErrorToast('请选择购物车中要下单的商品')
      return false;
    }

    var productIds = checkedGoods.map(function (element, index, array) {
      if (element.checked == true) {
        return element.Id;
      }
    });

    var userid=that.data.userInfo.Id;

    util.request(api.CartChecked, { Ids: productIds.join(','),UserID:userid }).then(function (res) {
      if (res.Status === 100) 
      {
        wx.navigateTo({url: '../shopping/checkout/checkout'});
      }
      else {
        util.showErrorToast(res.Data);
      }
    });

    
  },
  deleteCart: function () {
    //获取已选择的商品
    let that = this;

    let productIds = this.data.cartGoods.filter(function (element, index, array) {
      if (element.checked == true) {
        return true;
      } else {
        return false;
      }
    });

    if (productIds.length <= 0) {
      return false;
    }

    productIds = productIds.map(function (element, index, array) {
      if (element.checked == true) {
        return element.Id;
      }
    });
    util.request(api.CartDelete, {Ids: productIds.join(',')}).then(function (res)   {
      if (res.Status === 100) 
      {
        var cartList = new Array();
        that.data.cartGoods.map(v => {
          if (v.checked == false) {
            cartList.push(v);
          }
        });
        that.setData({cartGoods: cartList});
      }
      else
      {
        util.showErrorToast(res.Data);
      }
      that.setData({checkedAllStatus: that.isCheckedAll()});
    });
  }
})