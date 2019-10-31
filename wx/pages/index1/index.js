const util = require('../../utils/util.js');
const api = require('../../config/api.js');
const user = require('../../services/user.js');

//获取应用实例
const app = getApp()
Page({
  data: {
    newGoods: [],
    hotGoods: [],
    topics: [],
    brands: [],
    floorGoods: [],
    banner: [
      'http://x2.1766179.com/png/10_8.jpg',
      'http://x2.1766179.com/png/10_10.jpg',
      'http://x2.1766179.com/png/10_102.jpg',
      'http://x2.1766179.com/png/10_103.jpg',
      'http://x2.1766179.com/png/10_16.jpg',
      'http://x2.1766179.com/png/10_162.jpg',
      'http://x2.1766179.com/png/10_18.jpg',
      'http://x2.1766179.com/png/10_20.jpg',
      'http://x2.1766179.com/png/10_22.jpg',
      'http://x2.1766179.com/png/10_26.jpg',     
      'http://x2.1766179.com/png/10_222.jpg',
      'http://x2.1766179.com/png/10_223.jpg',
      'http://x2.1766179.com/png/10_224.jpg',
      'http://x2.1766179.com/png/10_30.jpg'
    ],
    channel: []
  },
  onShareAppMessage: function () {
    return {
      title: '睡冬宝项目合作信用实情披露',
      desc: '广东省著名家居品牌睡冬宝项目合作信用实情披露',
      path: '/pages/index1/index'
    }
  },onPullDownRefresh(){
	  	// 增加下拉刷新数据的功能
	    var self = this;
	    //this.getIndexData();
 },
  getIndexData: function () {
    let that = this;
    var data = new Object();
    util.request(api.IndexUrlNewGoods).then(function (res) {
      if (res.errno === 0) {
        data.newGoods= res.data.newGoodsList
      that.setData(data);
      }
    });
    util.request(api.IndexUrlHotGoods).then(function (res) {
      if (res.errno === 0) {
        data.hotGoods = res.data.hotGoodsList
      that.setData(data);
      }
    });
    util.request(api.IndexUrlTopic).then(function (res) {
      if (res.errno === 0) {
        data.topics = res.data.topicList
      that.setData(data);
      }
    });
    util.request(api.IndexUrlBrand).then(function (res) {
      if (res.errno === 0) {
        data.brand = res.data.brandList
      that.setData(data);
      }
    });
    util.request(api.IndexUrlCategory).then(function (res) {
      if (res.errno === 0) {
        data.floorGoods = res.data.categoryList
      that.setData(data);
      }
    });
     
    util.request(api.IndexUrlChannel).then(function (res) {
      if (res.errno === 0) {
        data.channel = res.data.channel
      that.setData(data);
      }
    });

  },
  onLoad: function (options) {
    this.getIndexData();
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
  },
})
