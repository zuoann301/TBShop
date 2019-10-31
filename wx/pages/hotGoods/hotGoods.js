var util = require('../../utils/util.js');
var api = require('../../config/api.js');
var app = getApp();

Page({
  data: {
    bannerInfo: {
      'img_url': 'https://x3.1766179.com/upload/image/pro_bj.png',
      'name': '人气推荐'
    },
    categoryFilter: false,
    filterCategory: [],
    goodsList: [],
    categoryId: '',
    currentSortType: 'default',
    currentSortOrder: 'desc',
    OrderType: 0,
    page: 1,
    size: 1000
  },
  getData: function () {
    let that = this;
    util.request(api.IndexUrlChannel, { Pid: 0 }, 'GET').then(function (res) {
      if (res.Status === 100) {
        that.setData({ filterCategory: res.Data });
        that.getGoodsList();
      }
    });
  },
  getGoodsList() {
    var that = this;

    util.request(api.ProductHotList, { Page: that.data.page, PageSize: that.data.size, OrderType: that.data.OrderType, ProSortID: that.data.categoryId }, 'GET')
      .then(function (res) {
        if (res.Status === 100) {
          that.setData({ goodsList: res.Data.Models });
        }
      });
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
    this.setData({ FileHost: api.FileHost });
    this.getGoodsList();
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
  openSortFilter: function (event) {
    let currentId = event.currentTarget.id;
    //console.log(currentId);
    //return;
    switch (currentId) {
       
      case 'hit':
        this.setData({
          'categoryFilter': !this.data.categoryFilter,
          'currentSortType': 'hit',
          'currentSortOrder': 'desc'
        });
        this.setData({ OrderType: 3 });
        this.getGoodsList();
        break;
      case 'priceSort':
        let tmpSortOrder = 'asc';
        if (this.data.currentSortOrder == 'asc') {
          tmpSortOrder = 'desc';
          this.setData({ OrderType: 1 });
        }
        else {
          this.setData({ OrderType: 2 });
        }
        this.setData({
          'currentSortType': 'price',
          'currentSortOrder': tmpSortOrder,
          'categoryFilter': false
        });

        this.getGoodsList();
        break;
      default:
        //综合排序
        this.setData({
          'currentSortType': 'default',
          'currentSortOrder': 'desc',
          'categoryFilter': false
        });
        this.setData({ OrderType: 0 });
        this.getGoodsList();
    }
  },
  selectCategory: function (event) {
    let currentIndex = event.target.dataset.categoryIndex;
    this.setData({
      'categoryFilter': false,
      'categoryId': this.data.filterCategory[currentIndex].Id
    });
    this.getData();

  }
})