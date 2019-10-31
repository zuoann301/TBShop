// pages/auth/shop/index.js
//商家注册
var api = require('../../../config/api.js');
var util = require('../../../utils/util.js');
var app = getApp();

Page({

  /**
   * 页面的初始数据
   */
  data: {
    shop_url:'',
    lic_url:'',
    btn_text:'上 传 营 业 执 照',
    btn_text2:'上 传 商 家 门 面 图'
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {

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
  UPLOAD_FILE: function () {
    var that = this;
    wx.chooseImage({
      success(res) {
        const tempFilePaths = res.tempFilePaths
        wx.uploadFile({
          url:api.Upload, //仅为示例，非真实的接口地址
          filePath: tempFilePaths[0],
          name: 'file',
          formData: { 'dir': 'image','subPath':'shop'},
          success(res1) {
            console.log(res1.data);
            var obj = JSON.parse(res1.data);
            if (obj.Status==100)
            {
              that.setData({ shop_url: obj.Data });
              that.setData({ btn_text:'上 传 营 业 执 照 [OK]'});
            }
            else
            {
              that.setData({ shop_url: "" });
              that.setData({ btn_text: '上 传 营 业 执 照 [错误]' });
            }
          }
        })
      }
    })
  },
  UPLOAD_FILE2: function () {
    var that = this;
    wx.chooseImage({
      success(res) {
        const tempFilePaths = res.tempFilePaths
        wx.uploadFile({
          url: api.Upload, //仅为示例，非真实的接口地址
          filePath: tempFilePaths[0],
          name: 'file',
          formData: { 'dir': 'image', 'subPath': 'lic' },
          success(res1) {
            var obj = JSON.parse(res1.data);
            if (obj.Status == 100) {
              that.setData({ lic_url: obj.Data });
              that.setData({ btn_text2:'上 传 商 家 门 面 图 [OK]'});
            }
            else {
              that.setData({ lic_url: "" });
              that.setData({ btn_text2: '上 传 商 家 门 面 图 [错误]' });
            }
          }
        })
      }
    })
  },

})