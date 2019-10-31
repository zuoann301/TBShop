var util = require('../../utils/util.js');
var api = require('../../config/api.js');
const shop = require('../../services/shop.js');
var app=getApp();
Page({
  data: {
    // text:"这是一个页面"
    navList: [],
    goodsList: [],
    id: '',
    pid:'',
    currentCategory: {},
    scrollLeft: 0,
    scrollTop: 0,
    scrollHeight: 0,
    page: 1,
    size:6,
    loadmoreText: '正在加载更多数据',
    nomoreText: '全部加载完成',
    nomore: false,
    totalPages: 1,
    FileHost:''
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
    //shop.setPageTitle();
    var that = this;
    that.setData({FileHost:api.FileHost});
    if (options.id) 
    {
      that.setData({id: options.id});
    }
    if (options.pid) {
      that.setData({ pid: options.pid });
    }

    wx.getSystemInfo({
      success: function (res) {
        that.setData({
          scrollHeight: res.windowHeight
        });
      }
    });

    this.getCurCateInfo();
    this.getCategoryList();

  },
  getCurCateInfo:function(){
    var that = this;
    util.request(api.CatalogCurrent, { id: that.data.id },'GET')
      .then(function (res) 
      {
        that.setData({currentCategory: res.Data});
      });
  },
  getCategoryList: function () {
    let that = this;
    util.request(api.IndexUrlChannel, { Pid: this.data.pid },'GET')
      .then(function (res) 
      {
        if (res.Status == 100) {
          that.setData({navList: res.Data});

          //nav位置
          let currentIndex = 0;
          let navListCount = that.data.navList.length;
          for (let i = 0; i < navListCount; i++) 
          {
            currentIndex += 1;
            if (that.data.navList[i].Id == that.data.id) {
              break;
            }
          }
          if (currentIndex > navListCount / 2 && navListCount > 5) {
            that.setData({
              scrollLeft: currentIndex * 60
            });
          }
          that.getGoodsList();

        } else {
          //显示错误信息
        }
        
      });
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

  /**
     * 页面上拉触底事件的处理函数
     */
  onReachBottom: function () {
    this.getGoodsList()
  },

  getGoodsList: function () {
    var that = this;

    if (that.data.totalPages <= that.data.page-1) {
      that.setData({
        nomore: true
      })
      return;
    }
    var shopid = app.globalData.ShopID;
    var pd = {ShopID:shopid, ProSortID: that.data.id, Page: that.data.page, PageSize: that.data.size, OrderType:0};
    util.request(api.ProductPageList,pd,'GET')
      .then(function (res) {
        that.setData({
          goodsList: that.data.goodsList.concat(res.Data.Models),        
          page: res.Data.CurrentPage+1,
          totalPages: res.Data.TotalPage
        });
      });
  },
  onUnload: function () {
    // 页面关闭
  },
  switchCate: function (event) {
    if (this.data.id == event.currentTarget.dataset.id) {
      return false;
    }
    var that = this;
    var clientX = event.detail.x;
    var currentTarget = event.currentTarget;
    if (clientX < 60) {
      that.setData({
        scrollLeft: currentTarget.offsetLeft - 60
      });
    } else if (clientX > 330) {
      that.setData({
        scrollLeft: currentTarget.offsetLeft
      });
    }
    this.setData({
      id: event.currentTarget.dataset.id,
      page:1,
      totalPages: 1,
      goodsList: [],
      nomore: false
    });
    this.getCurCateInfo();
    this.getGoodsList();
  }
})