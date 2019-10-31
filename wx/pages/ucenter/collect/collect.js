var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');

var app = getApp();

Page({
  data: {
    typeId: 0,
    collectList: [],
    token:null
  },
  getCollectList() {
    let that = this;
    
    if (that.data.userInfo==null)
    {
      util.showErrorToast('请先登录');
      return;
    }
    var uerid = that.data.userInfo.Id;
    util.request(api.CollectList, { UserID: uerid},'GET').then(function (res) {
      if (res.Status === 100) {
        that.setData({collectList: res.Data});
      }
    });
  },
  onLoad: function (options) {
    this.setData({FileHost:api.FileHost});
    var userInfo=wx.getStorageSync('userInfo');
    if (userInfo)
     {
      this.setData({ userInfo: userInfo});
      this.getCollectList();
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
  },
  openGoods(event) { 
    let that = this;
    var cid = event.currentTarget.dataset.id;
    let goodsId = event.currentTarget.dataset.proid;
    var uerid = that.data.userInfo.Id;
    //触摸时间距离页面打开的毫秒数  
    var touchTime = that.data.touch_end - that.data.touch_start;
    //如果按下时间大于350为长按
    if (touchTime > 350) {
      wx.showModal({
        title: '',
        content: '确定删除收藏吗？',
        success: function (res) {
          if (res.confirm) {
            util.request(api.CollectDelete, { id:cid},'GET').then(function (res) {
              if (res.Status === 100) 
              {
                wx.showToast({
                  title: '删除成功',
                  icon: 'success',
                  duration: 2000
                });
                that.getCollectList();
              }
            });
          }
        }
      })
    } else {
      
      wx.navigateTo({
        url: '/pages/goods/goods?id=' + goodsId,
      });
    }  
  },
  //按下事件开始  
  touchStart: function (e) {
    let that = this;
    that.setData({
      touch_start: e.timeStamp
    })
  },
  //按下事件结束  
  touchEnd: function (e) {
    let that = this;
    that.setData({
      touch_end: e.timeStamp
    })
  },
})