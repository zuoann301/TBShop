var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');



var app = getApp();

Page({
  data: {
    array: ['请选择反馈类型', '商品相关', '物流状况', '客户服务', '优惠活动', '功能异常', '其他'],
    sortlist:[
      { id: 0, title:"请选择反馈类型"},
      { id: 12, title: "商品相关" },
      { id: 13, title: "物流状况" },
      { id: 14, title: "客户服务" },
      { id: 15, title: "优惠活动" },
      { id: 16, title: "功能异常" },
      { id: 17, title: "其他" }
    ],
    index: 0,
    content:'',
    contentLength:0,
    mobile:'',
    userInfo:null
  },
  bindPickerChange: function (e) {
    this.setData({
      index: e.detail.value
    });
  },
  mobileInput: function (e) {
    let that = this;
    this.setData({
      mobile: e.detail.value,
    });
  },
  contentInput: function (e) {
   
    let that = this;
    this.setData({
      contentLength: e.detail.cursor,
      content: e.detail.value,
    });
  },
  cleanMobile:function(){
    let that = this;

  },
  sbmitFeedback : function(e){
    let that = this;
    if (that.data.index == 0){
      util.showErrorToast('请选择反馈类型');
      return false;
    }
    var sort_id = that.data.sortlist[that.data.index].id;
    var sort_name = that.data.sortlist[that.data.index].title;
    if (that.data.content == '') {
      util.showErrorToast('请输入反馈内容');
      return false;
    }

    if (that.data.mobile == '') {
      util.showErrorToast('请输入手机号码');
      return false;
    }

    if (that.data.userInfo ==null) {
      util.showErrorToast('请先登录');
      return false;
    }
    var userid=that.data.userInfo.Id;

    wx.showLoading({
      title: '提交中...',
      mask:true,
      success: function () {

      }
    });
    //string Summary,int SortID,string Mobile,string UserID,string SortName
    var pd = { Summary: that.data.content, SortID: sort_id, Mobile: that.data.mobile, UserID: userid, SortName:sort_name};
    util.request(api.FeedbackAdd,pd).then(function (res) {
      if (res.Status === 100) 
      {
        wx.hideLoading();
        wx.showToast({
          title: '提交成功',
          icon: 'success',
          duration: 2000,
          complete: function () {
            that.setData({
              index: 0,
              content: '',
              contentLength: 0,
              mobile: ''
            });
          }
        });
      } else {
        util.showErrorToast('提交失败');
      }
      
    });
  },
  onLoad: function (options) 
  {
     var userInfo=wx.getStorageSync('userInfo');
    if (userInfo!="")
    {
      this.setData({ userInfo: userInfo});
    }
  },
  onReady: function () {

  },
  onShow: function () {

  },
  onHide: function () {
    // 页面隐藏

  },
  onUnload: function () {
    // 页面关闭
  }
})