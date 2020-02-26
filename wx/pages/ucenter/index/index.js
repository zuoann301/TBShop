var util = require('../../../utils/util.js');
var api = require('../../../config/api.js');
var user = require('../../../services/user.js');
const shop = require('../../../services/shop.js');
var app = getApp();

Page({
    data: {
        userInfo: {},
        temp_data:{},
        hasMobile: ''
    },
    onLoad: function (options) {
        // 页面初始化 options为页面跳转所带来的参数
      //shop.setPageTitle();
        var that=this;
        console.log('--------------------------------------');
      // 获取用户信息
      wx.getSetting({
        success: res => 
        {
          if (res.authSetting['scope.userLocation']) 
          {
            wx.getLocation({
              type: 'wgs84',
              success(res) 
              {
                var loc = { latitude: res.latitude, longitude: res.longitude};
                wx.setStorageSync('loc',loc);
              }
            })
          }
          else 
          {
            wx.chooseLocation({
              success: function (res) {
                console.log(res, "location")
                console.log(res.name)
                console.log(res.latitude)
                console.log(res.longitude)
                //
                var loc={latitude:res.latitude,longitude:res.longitude};
                wx.setStorageSync('loc',loc);
                
              },
              fail: function () {
                // fail
              },
              complete: function () {
                // complete
              }
            })
             
          }
        }
      })
      console.log('--------------------------------------');
    },
    onReady: function () {
      
    },
    onShow: function () {
      var that=this;
         
      let token = wx.getStorageSync('wx');
      // 页面显示
      var userInfo = wx.getStorageSync('userInfo');
      if (userInfo) 
      {
        that.setData({ userInfo: userInfo});
        app.globalData.userInfo = res.Data;
      }
      else 
      {
        var openid="";
        if(token!="")
        {
          openid = token.openid;
          util.request(api.CheckOpenID, { OpenID: openid }, 'GET').then(function (res) {
            if (res.Status === 100) 
            {
              wx.showModal({
                title: '提示',
                content:'检测到您已经注册,是否自动登录',
                success(res1) 
                {
                  if (res1.confirm) 
                  {
                    wx.setStorageSync('userInfo', res.Data);
                    that.setData({ userInfo: res.Data });
                    app.globalData.userInfo = res.Data;
                  }
                  else if (res1.cancel) 
                  {
                    wx.switchTab({ url: '/pages/index/index' });
                  }
                }
              })

            }
            else
            {
              wx.navigateTo({ url: '/pages/auth/mobile/mobile' });
            }
          });
        }
        else
        {
          wx.reLaunch({ url: '/pages/start/index' });           
        }
        

      }

        
 

    },
    onHide: function () {
        // 页面隐藏

    },
    onUnload: function () {
        // 页面关闭
    },
    bindGetUserInfo:function() {
      let that = this;
      let userInfo = wx.getStorageSync('userInfo');
      let token = wx.getStorageSync('wx');
      if (userInfo) 
      {
        that.setData({ userInfo: userInfo});
        return;
      }
      else
      {
        if(token)
        {
          var openid = token.openid;
          var loc = wx.getStorageSync('loc');
          var x=0;
          var y=0;
          if(loc!="")
          {
            x = loc.longitude; y = loc.latitude;
          }
          var pd = { OpenID: openid, GPS_X: x, GPS_Y: y};
          util.request(api.SimpleLogin,pd, 'GET').then(function (res) {
            if (res.Status === 100) 
            {
              wx.setStorageSync('userInfo', res.Data);
              wx.reLaunch({
                url: '/pages/ucenter/index/index',
              })
            }
            else
            {
                wx.showModal({
                  title: '提示',
                  content: res.Msg,
                  success(res1) {
                    if (res1.confirm) 
                    {
                      wx.navigateTo({ url: '/pages/auth/mobile/mobile' });
                    } 
                    else if (res1.cancel) 
                    {
                      wx.navigateTo({ url: '/pages/index/index' });
                    }
                  }
                })

            }
          });
        }
        else
        {
          wx.showToast({title: '无法获取openid'});
        }
      }
      
    },
    exitLogin: function () {
        wx.showModal({
            title: '',
            confirmColor: '#b4282d',
            content: '退出登录？',
            success: function (res) 
            {
                if (res.confirm) 
                {
                  app.globalData.userInfo={};
                    //wx.removeStorageSync('wx');
                    wx.removeStorageSync('userInfo');
                    //wx.clearStorage();
                    wx.reLaunch({url: '/pages/index/index'});
                }
            }
        })

    },
  callMaker:function(){
    wx.navigateTo({
      url: '/pages/ucenter/wxinfo/index',
    })
  }
})