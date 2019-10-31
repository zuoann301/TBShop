var util = require('../../utils/util.js');
var api = require('../../config/api.js');

var app = getApp()
Page({
  data: {
    keywrod: '',
    searchStatus: false,
    goodsList: [],
    helpKeyword: [],
    historyKeyword: [],
    categoryFilter: false,
    currentSortType: 'default',
    currentSortOrder: '',
    filterCategory: [],
    defaultKeyword: {},
    hotKeyword: [],
    page: 1,
    size: 20,
    currentSortType: 'id',
    currentSortOrder: 'desc',
    categoryId: 0,
    OrderType: 0,
    id:'',

    loadmoreText: '正在加载更多数据',
    nomoreText: '全部加载完成',
    nomore: false,
  },
  //事件处理函数
  closeSearch: function () {
    wx.navigateBack()
  },
  clearKeyword: function () {
    this.setData({
      keyword: '',
      searchStatus: false
    });
  },
  onLoad: function (options) {
    this.setData({FileHost:api.FileHost});
    if (options.id)
    {
      this.setData({id:options.id});
    }
    this.getSearchKeyword();
  },

  getSearchKeyword() {
    let that = this;
    util.request(api.SearchIndex).then(function (res) {
      if (res.Status === 100) {
        that.setData({
          historyKeyword: res.Data.historyKeywordList.split(';'),
          defaultKeyword: res.Data.defaultKeyword,
          hotKeyword: res.Data.hotKeywordList.split(';')
        });
      }
    });
  },

  inputChange: function (e) {

    this.setData({
      keyword: e.detail.value,
      searchStatus: false
    });
    this.getHelpKeyword();
  },
  getHelpKeyword: function () {
    let that = this;
    var shopid = app.globalData.ShopID;
    util.request(api.SearchHelper, { keyword: that.data.keyword,ShopID:shopid },'GET').then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({helpKeyword: res.Data});
      }
      else
      {
        that.setData({ helpKeyword:null});
      }
    });
  },
  inputFocus: function () {
    this.setData({
      searchStatus: false,
      goodsList: []
    });

    if (this.data.keyword) {
      this.getHelpKeyword();
    }
  },
  clearHistory: function () {
    this.setData({
      historyKeyword: []
    })

    util.request(api.SearchClearHistory, {})
      .then(function (res) {
      });
  },
  getGoodsList: function () {
    let that = this;
    //int ShopID, string ProSortID, string keyword, int OrderType = 0

    if (that.data.totalPages <= that.data.page - 1) {
      that.setData({
        nomore: true
      })
      return;
    }

    if(that.data.page==1)
    {
      that.setData({ goodsList: [], totalPages:0});
    }

    var shopid = app.globalData.ShopID;
    var pd = { keyword: that.data.keyword, ShopID: shopid, ProSortID: '', Page: that.data.page, PageSize: that.data.size, OrderType: that.data.OrderType};

    util.request(api.ProductPageList, pd,'GET').then(function (res) {
      if (res.Status === 100) {
        that.setData({
          searchStatus: true,
          categoryFilter: false,
          goodsList: that.data.goodsList.concat(res.Data.Models),
          page: res.Data.CurrentPage + 1,
          totalPages: res.Data.TotalPage
        });
      }

      //重新获取关键词
      that.getSearchKeyword();
    });
  },
  onKeywordTap: function (event) {
    
    this.getSearchResult(event.target.dataset.keyword);

  },
  getSearchResult:function (s) {
    this.setData({
      keyword: s,
      page: 1,
      goodsList: []
    });

    this.getGoodsList();
  },
  openSortFilter: function (event) {
    let currentId = event.currentTarget.id;
    //console.log(currentId);
    //return;
    switch (currentId) {
      case 'priceSort':
        let tmpSortOrder = 'asc';
        if (this.data.currentSortOrder == 'asc') 
        {
          tmpSortOrder = 'desc';
          this.setData({ OrderType: 1 });
        }
        else 
        {
          this.setData({ OrderType: 2 });
        }
        this.setData({
          'currentSortType': 'price',
          'currentSortOrder': tmpSortOrder,
          'categoryFilter': false
        });
        break;
      case 'hit':
        this.setData({
          'categoryFilter': !this.data.categoryFilter,
          'currentSortType': 'hit',
          'currentSortOrder': 'asc'
        });
        this.setData({ OrderType: 3 });
        break;
      default:
        //综合排序
        this.setData({
          'currentSortType': 'default',
          'currentSortOrder': 'desc',
          'categoryFilter': false
        });
        this.setData({ OrderType: 0 });
    }
    this.setData({ page:1 });
    this.getGoodsList();
  },
  selectCategory: function (event) {
    let currentIndex = event.target.dataset.categoryIndex;
    let filterCategory = this.data.filterCategory;
    let currentCategory = null;
    for (let key in filterCategory) {
      if (key == currentIndex) {
        filterCategory[key].selected = true;
        currentCategory = filterCategory[key];
      } else {
        filterCategory[key].selected = false;
      }
    }
    this.setData({
      'filterCategory': filterCategory,
      'categoryFilter': false,
      categoryId: currentCategory.id,
      page: 1,
      goodsList: []
    });
    this.getGoodsList();
  },
  onKeywordConfirm(event) {
    this.getSearchResult(event.detail.value);
  }
})