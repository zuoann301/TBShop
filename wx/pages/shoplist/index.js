// pages/shoplist/index.js
const util = require('../../utils/util.js');
const api = require('../../config/api.js');
const app=getApp();
Page({

  /**
   * 页面的初始数据
   */
  data: {
    shoplist:[],
    loc_err:true
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) 
  {
    var that=this;
    if(app.globalData.GPS_X>0)
    {
      that.GetShopList();
    }
    else
    {
      wx.getLocation({
        success: function(res) {
          console.log(res);
          app.globalData.GPS_X = res.longitude;
          app.globalData.GPS_Y = res.latitude;
          that.GetShopList();
        },
        fail(r)
        {
          that.setData({ loc_err:false});
        }
      })
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

  },
  GetShopList:function()
  {
    var that=this;
    var x = app.globalData.GPS_X;
    var y = app.globalData.GPS_Y;

    util.request(api.ShopList, { GPS_X: x, GPS_Y:y }, 'GET').then(function (res) {
      if (res.Status === 100) {
        for(var i=0;i<res.Data.length;i++)
        {
          res.Data[i].Distance =util.formatDistance(res.Data[i].Distance);
        }
        that.setData({ shoplist: res.Data});
      }
    });
  },
  chooseLoc:function(){
    var that=this;
    wx.chooseLocation({
      success: function (res) {
        // success
        console.log(res.name)
        console.log(res.latitude)
        console.log(res.longitude)
        app.globalData.GPS_X = res.longitude;
        app.globalData.GPS_Y = res.latitude;
        that.setData({ loc_err: true });
        that.GetShopList();
      },
      fail: function () {
        // fail
      },
      complete: function () {
        // complete
      }
    })
     
  }
})