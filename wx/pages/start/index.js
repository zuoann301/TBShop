// pages/start/index.js
const util = require('../../utils/util.js');
const api = require('../../config/api.js');
const shop = require('../../services/shop.js');
const app=getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {

  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
      if(options.id)
      {
        app.globalData.ShopID =options.id;
        wx.setStorageSync("ShopID", options.id);

        shop.getShop(options.id).then(res => {          
          //wx.setStorageSync('shop', res);
          app.globalData.shopInfo=res;
          setTimeout(function () {
            wx.reLaunch({ url: '/pages/index/index' });
          }, 500);
        }).catch((err) => 
        {
          wx.hideLoading();
          wx.showModal({
            title: '提示', content: '请选择离您最近的便利店',
            success: function (res) {
              wx.reLaunch({ url: '/pages/shoplist/index' });
            }
          });
        });

        
      }
      else
      {
        var shopid=wx.getStorageSync('ShopID');
        if (shopid=="")
        {
            wx.reLaunch({url: '/pages/shoplist/index'});
        }
        else
        {
            app.globalData.ShopID = shopid;

          shop.getShop(shopid).then(res => {
            //wx.setStorageSync('shop', res);
            app.globalData.shopInfo = res;
            setTimeout(function () {
              wx.reLaunch({ url: '/pages/index/index' });
            }, 500);
          }).catch((err) => {
            wx.hideLoading();
            wx.showModal({
              title: '提示', content: '请选择离您最近的便利店',
              success: function (res) {
                wx.reLaunch({ url: '/pages/shoplist/index' });
              }
            });
          }); 
        }
      }
       
      
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})