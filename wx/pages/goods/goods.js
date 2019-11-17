var app = getApp();
var WxParse = require('../../lib/wxParse/wxParse.js');
var util = require('../../utils/util.js');
var api = require('../../config/api.js');
const shop = require('../../services/shop.js');
Page({
  data: {
    winHeight: "",
    id: 0,
    goods: {},
    gallery: [],
    attribute: [],
    issueList: [],
    comment: [],
    brand: {},
    specificationList: [],
    productList: [],
    relatedGoods: [],
    cartGoodsCount: 0,
    userHasCollect: 0,
    number: 1,
    checkedSpecText: '请选择规格数量',
    openAttr: false,
    noCollectImage: "/static/images/icon_collect.png",
    hasCollectImage: "/static/images/icon_collect_checked.png",
    collectBackImage: "/static/images/icon_collect.png",
    FileHost: '',
    size_price:0,
    sizeid:'',
    userInfo:null
  },
  getGoodsInfo: function () {
    let that = this;
    var uid="";
    if(that.data.userInfo!=null)
    {
      uid=that.data.userInfo.Id;
    }
     
    util.request(api.ProductInfo, { id: that.data.id, hit: 1, userid: uid }).then(function (res) {
      if (res.Status === 100) {
        /*
        that.setData({
          goods: res.data.info,
          gallery: res.data.gallery,
          attribute: res.data.attribute,
          issueList: res.data.issue,
          comment: res.data.comment,
          brand: res.data.brand,
          specificationList: res.data.specificationList,
          productList: res.data.productList,
          userHasCollect: res.data.userHasCollect
        });
        */
        //轮播图
        
        that.setData({ goods: res.Data.product});
        that.setData({ size_price: res.Data.product.Price});
        //res.Data.SizeList[0].checked=true;
        that.setData({ specificationList: res.Data.SizeList });
        that.setData({ brand: res.Data.brand });
        if (res.Data.product.ImageList!='')
        {
          var arr=new Array();
          arr = res.Data.product.ImageList.split(';');
          that.setData({ gallery: arr })
        }
        
          //设置默认值
          that.setDefSpecInfo(that.data.specificationList);
        if (res.Data.HasCollect == 1) 
        {
          that.setData({'collectBackImage': that.data.hasCollectImage});
        } 
        else
        {
          that.setData({'collectBackImage': that.data.noCollectImage});
        }
        if (res.Data.product.Contents)
        {
          WxParse.wxParse('goodsDetail', 'html', res.Data.product.Contents, that);
        }
        

        that.getGoodsRelated(res.Data.product.ProSortID);
        that.getIssueList();
      }
    });

  },
  getGoodsRelated: function (sortid) {
    let that = this;
    var shopid=app.globalData.ShopID;
    util.request(api.ProductList, {ShopID:shopid, Num:4, SortID: sortid },'GET').then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({relatedGoods: res.Data});
      }
    });

  },
  getIssueList: function () {
    let that = this;
    util.request(api.IndexUrlTopic, { Num:4, SortID: 9, IsValid: 1 }, 'GET').then(function (res) {
      if (res.Status === 100) 
      {
        that.setData({ issueList:res.Data}); 
      }
    });

  },
  closeSize:function()
  {
    this.setData({ openAttr: false });
  },
  selectSize: function (event)
  {
    let that = this;
    let _specificationList = that.data.specificationList;
    let sizeid = event.target.dataset.sizeid;
    let price = event.target.dataset.price;
    that.setData({ size_price: price});
    that.setData({ sizeid: sizeid });
    for (let i = 0; i < _specificationList.length;i++)
    {
      if (_specificationList[i].Id == sizeid)
      {
        _specificationList[i].checked=true;
      }
      else
      {
        _specificationList[i].checked = false;
      }
    }
    that.setData({ specificationList: _specificationList});
  },
  clickSkuValue: function (event) {
    let that = this;
    let sizeid = event.currentTarget.dataset.sizeid;
    let specValueId = event.currentTarget.dataset.valueId;

    //判断是否可以点击

    //TODO 性能优化，可在wx:for中添加index，可以直接获取点击的属性名和属性值，不用循环
    let _specificationList = this.data.specificationList;
    for (let i = 0; i < _specificationList.length; i++) {
      if (_specificationList[i].specification_id == specNameId) {
        for (let j = 0; j < _specificationList[i].valueList.length; j++) {
          if (_specificationList[i].valueList[j].id == specValueId) {
            //如果已经选中，则反选
            if (_specificationList[i].valueList[j].checked) {
              _specificationList[i].valueList[j].checked = false;
            } else {
              _specificationList[i].valueList[j].checked = true;
            }
          } else {
            _specificationList[i].valueList[j].checked = false;
          }
        }
      }
    }
    this.setData({
      'specificationList': _specificationList
    });
    //重新计算spec改变后的信息
    this.changeSpecInfo();

    //重新计算哪些值不可以点击
  },

  //获取选中的规格信息
  getCheckedSpecValue: function () {
    let checkedValues = [];
    let _specificationList = this.data.specificationList;
    for (let i = 0; i < _specificationList.length; i++) {
      let _checkedObj = {
        nameId: _specificationList[i].specification_id,
        valueId: 0,
        valueText: ''
      };
      for (let j = 0; j < _specificationList[i].valueList.length; j++) {
        if (_specificationList[i].valueList[j].checked) {
          _checkedObj.valueId = _specificationList[i].valueList[j].id;
          _checkedObj.valueText = _specificationList[i].valueList[j].value;
        }
      }
      checkedValues.push(_checkedObj);
    }

    return checkedValues;

  },
  //根据已选的值，计算其它值的状态
  setSpecValueStatus: function () {

  },
  //判断规格是否选择完整
  isCheckedAllSpec: function () {
    return !this.getCheckedSpecValue().some(function (v) {
      if (v.valueId == 0) {
        return true;
      }
    });
  },
  getCheckedSpecKey: function () {
    let checkedValue = this.getCheckedSpecValue().map(function (v) {
      return v.valueId;
    });

    return checkedValue.join('_');
  },
  changeSpecInfo: function () {
    let checkedNameValue = this.getCheckedSpecValue();

    //设置选择的信息
    let checkedValue = checkedNameValue.filter(function (v) {
      if (v.valueId != 0) {
        return true;
      } else {
        return false;
      }
    }).map(function (v) {
      return v.valueText;
    });
    if (checkedValue.length > 0) {
      this.setData({
        'checkedSpecText': checkedValue.join('　')
      });
    } else {
      this.setData({
        'checkedSpecText': '请选择规格数量'
      });
    }

  },
  getCheckedProductItem: function (key) {
    return this.data.productList.filter(function (v) {
      if (v.goods_specification_ids.indexOf(key) > -1) {
        return true;
      } else {
        return false;
      }
    });
  },
  onLoad: function (options) {
    //shop.setPageTitle();
    this.setData({ FileHost: api.FileHost });
    if(options.shopid)
    {
      wx.setStorageSync('ShopID',options.shopid);
    }
    // 页面初始化 options为页面跳转所带来的参数 1181000
    this.setData({id: options.id});
    //this.setData({ id: 1181000 });
    var that = this;
    

    var userInfo = wx.getStorageSync('userInfo');
    if (userInfo!="")
    {
      that.setData({ userInfo: userInfo });
      var userid = userInfo.Id;
      var shopid=app.globalData.ShopID; 
      util.request(api.CartGoodsCount, { UserID: userid,ShopID:shopid},'GET').then(function (res) {
        if (res.Status == 100) 
        {
          that.setData({cartGoodsCount: res.Data});
        }
      });
    }
    else
    {

    }
    that.getGoodsInfo();
    
    //高度自适应
    wx.getSystemInfo({
      success: function (res) {
        var clientHeight = res.windowHeight,
          clientWidth = res.windowWidth,
          rpxR = 750 / clientWidth;
        var calc = clientHeight * rpxR - 100;
        that.setData({
          winHeight: calc
        });
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
  onUnload: function () {
    // 页面关闭

  },
  switchAttrPop: function () {
    if (this.data.openAttr == false) {
      this.setData({
        openAttr: !this.data.openAttr,
        collectBackImage: "/static/images/detail_back.png"
      });
    }
  },
  closeAttrOrCollect: function () {
    let that = this;
    var userid = '';
    if (that.data.userInfo != null) 
    {
      userid = that.data.userInfo.Id;
    }
    else {
      util.showErrorToast('请登录后再收藏');
      setTimeout(function () {
        wx.navigateTo({ url: '/pages/auth/mobile/mobile' });
      }, 1500);
      return;
    }

      if (that.data.userHasCollect == 1) 
      {
        
        util.request(api.CollectAdd, { UserID: userid, ProductID: that.data.id }, "GET")
          .then(function (res) 
          {
            let _res = res;
            if (_res.Status == 100) 
            {
              that.setData({ 'collectBackImage': that.data.noCollectImage });
              that.setData({ 'userHasCollect': 0 });
            }
            else 
            {
              wx.showToast({ image: '/static/images/icon_error.png', title: '请先登录', mask: true });
            }
          });
      } 
      else 
      {
        
        util.request(api.CollectAdd, { UserID: userid, ProductID: that.data.id }, "GET")
          .then(function (res) 
          {
            let _res = res;
            if (_res.Status == 100) {
              that.setData({ 'collectBackImage': that.data.hasCollectImage });
              that.setData({ 'userHasCollect': 1 });
            }
            else {
              wx.showToast({ image: '/static/images/icon_error.png', title: '请先登录', mask: true });
            }
          });
      }
     

  },
  openCartPage: function () {
    wx.switchTab({
      url: '/pages/cart/cart',
    });
  },

  /**
   * 直接购买
   */
  buyGoods: function () {
    var that = this;
    if (this.data.openAttr == false) 
    {
      //打开规格选择窗口
      this.setData({
        openAttr: !this.data.openAttr,
        collectBackImage: "/static/images/detail_back.png"
      });
    } 
    else 
    {
      //提示选择完整规格
      if (this.data.sizeid == '') {
        util.showErrorToast('请选择完整规格');
        return;
      }
      var userid = '';
      if (that.data.userInfo != null) {
        userid = that.data.userInfo.Id;
      }
      else {
        
        util.showErrorToast('请登录后再购买');
        setTimeout(function () {
          wx.navigateTo({ url: '/pages/auth/mobile/mobile' });
        }, 1500);
        return;
      }

      var shopid=app.globalData.ShopID;

      var pd = {ShopID:shopid, UserID: userid, ProductID: that.data.id, ProSizeID: that.data.sizeid, ItemNum: that.data.number };
      
      // 直接购买商品
      util.request(api.SetCart, pd, "GET")
        .then(function (res) {
          let _res = res;
          if (_res.Status == 100) 
          {
            that.setData({openAttr: !that.data.openAttr});
            wx.navigateTo({
              url: '/pages/shopping/checkout/checkout?isBuy=true',
            })
          } 
          else 
          {
            wx.showToast({
              image: '/static/images/icon_error.png',
              title: _res.Data,
              mask: true
            });
          }

        });

    }
  },

  /**
   * 添加到购物车
   */
  addToCart: function () {
    var that = this;
     
    var userid='';
    if(that.data.userInfo!=null)
    {
      userid = that.data.userInfo.Id;
    }
    else
    {
      util.showErrorToast('请登录后再购买');
      setTimeout(function(){
        wx.navigateTo({url: '/pages/auth/mobile/mobile'});
      },1500);
      return;
    }
    var shopid= app.globalData.ShopID;
    var pd = {ShopID:shopid, UserID: userid, ProductID: that.data.id, ProSizeID:'', ItemNum: that.data.number};
    //添加到购物车
    util.request(api.SetCart,pd, 'GET')
      .then(function (res) {
        let _res = res;
        if (_res.Status == 100) 
        { 
          that.setData({ cartGoodsCount: _res.Data});
          that.setData({openAttr: !that.data.openAttr});
          that.setData({ sizeid:''});
          let sizeList = that.data.specificationList.map(v => {
            v.checked = false;
            return v;
          });
          that.setData({ specificationList: sizeList });
          
          wx.showModal({
            title: '添加成功',
            content: '是否立即进入购物车',
            success(res1) {
              if (res1.confirm) 
              { 
                wx.reLaunch({
                  url: '/pages/cart/cart',
                })
              }
              else if (res1.cancel) 
              {
                //wx.navigateTo({ url: '/pages/index/index' });
              }
            }
          })

        } 
        else 
        {
          wx.showToast({
            image: '/static/images/icon_error.png',
            title: _res.Data,
            mask: true
          });
        }

      });
    

  },
  cutNumber: function () {
    this.setData({
      number: (this.data.number - 1 > 1) ? this.data.number - 1 : 1
    });
  },
  addNumber: function () {
    this.setData({
      number: this.data.number + 1
    });
  },
    setDefSpecInfo: function (specificationList) {
        //未考虑规格联动情况
        let that = this;
        if (!specificationList)return;
        for (let i = 0; i < specificationList.length;i++){
            let specification = specificationList[i];
            let specNameId = specification.specification_id;
            //规格只有一个时自动选择规格
            if (specification.valueList && specification.valueList.length == 1){
                let specValueId = specification.valueList[0].id;
                that.clickSkuValue({ currentTarget: { dataset: { "nameId": specNameId, "valueId": specValueId } } });
            }
        }
        specificationList.map(function(item){

        });

    },
  onShareAppMessage: function () {
    var that=this;
    return {
      title:that.data.goods.ProductName,
      desc:that.data.goods.Summary,
      path: '/pages/goods/goods?id=' + that.data.id + '&shopid=' + app.globalData.ShopID
    }
  },
})