var app = getApp();
var util = require('../../utils/util.js');
var api = require('../../config/api.js');
Page({
  data: {
    id:'',
    content: ''
  },
  onLoad: function (options) {

    var that = this;
    that.setData({id: options.id});

  },
  onClose() {
    wx.navigateBack({
      delta: 1
    });
  },
  onPost() {
    let that = this;

    if (!this.data.content) {
      util.showErrorToast('请填写评论')
      return false;
    }

    var u=wx.getStorageSync('u');
    var UserID='';
    if(u!="")
    {
      UserID=u.Id;
    }

    util.request(api.CommentPost, {
      Fid: that.data.id,
      UserID: UserID,
      Summary: that.data.content
    }, 'POST').then(function (res) {
      if (res.Status === 100) {
        wx.showToast({
          title: '评论成功',
          complete: function(){
            wx.navigateBack({
              delta: 1
            });
          }
        })
      }
    });
  },
  bindInpuntValue(event){

    let value = event.detail.value;

    //判断是否超过140个字符
    if (value && value.length > 140) {
      return false;
    }

    this.setData({
      content: event.detail.value,
    })
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

  }
})