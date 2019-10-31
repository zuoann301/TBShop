var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');
Page({

  /**
   * 页面的初始数据
   */
  data: {
    orderList: [],
    page: 1,
    size: 5,
    loadmoreText: '正在加载更多数据',
    nomoreText: '全部加载完成',
    nomore: false,
    totalPages: 1,
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var userInfo=wx.getStorageSync('userInfo');
    if(userInfo!="")
    {
      this.setData({ userInfo: userInfo});
      this.loglist();
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
    this.loglist();
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  },
  loglist:function()
  {
    let that = this;

    if (that.data.totalPages <= that.data.page - 1) {
      that.setData({ nomore: true });
      return;
    }

    if (that.data.userInfo==null)
    {
      return;
    }
    var userid = that.data.userInfo.Id;

    util.request(api.Users_SecurityList, { UserID: userid, Page: that.data.page, PageSize: that.data.size }, 'GET').then(function (res) 
    {
      if (res.Status === 100) 
      {
        that.setData({
          orderList: that.data.orderList.concat(res.Data.Models),
          page: res.Data.CurrentPage + 1,
          totalPages: res.Data.TotalPage
        });
        wx.hideLoading();
      }
      else {
        wx.showToast({ title: '没有数据' });
      }
    });
  }

})