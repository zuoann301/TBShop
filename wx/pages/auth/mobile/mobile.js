var api = require('../../../config/api.js');
var util = require('../../../utils/util.js');
var app = getApp()

Page({
    data: 
    {
        mobile: '',
        code:'',
        userInfo: {
            avatarUrl: '',
            nickName: ''
        },
        disableGetMobileCode: false,
        disableSubmitMobileCode: true,
        getCodeButtonText: '获取验证码'
    },

    onShow: function () {
    },

    onLoad: function () {
        var that = this;
      
        that.setData({userInfo: app.globalData.userInfo})

        if (app.globalData.token) 
        {
        } else 
        {
            var token = wx.getStorageSync('userToken')
            if (token) 
            {
                app.globalData.token = token
            }
        }

    },

  quickLogin: function () {
    let that = this;
    let userInfo = wx.getStorageSync('userInfo');
    let token = wx.getStorageSync('wx');
    if (userInfo) {
      that.setData({ userInfo: userInfo });
      return;
    }
    else 
    {
      if (token) 
      {
        var openid = token.openid;
        var loc = wx.getStorageSync('loc');
        var x = 0;
        var y = 0;
        if (loc != "") {
          x = loc.longitude; y = loc.latitude;
        }
        var pd = { OpenID: openid, GPS_X: x, GPS_Y: y };

        util.request(api.SimpleLogin, pd, 'GET').then(function (res) {
          if (res.Status === 100) {
            wx.setStorageSync('userInfo', res.Data);
            wx.reLaunch({
              url: '/pages/ucenter/index/index',
            })
          }
          else {
            wx.showModal({
              title: '提示',
              content: res.Msg,
              success(res1) {
                if (res1.confirm) {
                  wx.navigateTo({ url: '/pages/auth/mobile/mobile' });
                }
                else if (res1.cancel) {
                  wx.navigateTo({ url: '/pages/index/index' });
                }
              }
            })

          }
        });
      }
      else {
        wx.showToast({ title: '无法获取openid' });
      }
    }

  },

    bindCheckMobile: function (mobile) {
        if (!mobile) {
            wx.showModal({
                title: '错误',
                content: '请输入手机号码'
            });
            return false
        }
        if (!mobile.match(/^1[3-9][0-9]\d{8}$/)) {
            wx.showModal({
                title: '错误',
                content: '手机号格式不正确，仅支持国内手机号码'
            });
            return false
        }
        return true
    },

    bindGetPassCode: function (e) {
        var that = this
        that.setData({disableGetMobileCode: true})
    },

    bindInputMobile: function (e) {
        this.setData({mobile: e.detail.value});
    },
  bindInputCode: function (e) {
    this.setData({ code: e.detail.value });
  },
  countDownPassCode: function () 
  {
        if (!this.bindCheckMobile(this.data.mobile)) 
        {
            return
        }
        //-------------------------------------------
    wx.request({
      url: api.SmsCode, 
      data: {phone: this.data.mobile},
      header: {'content-type': 'application/json'},
      success(res) 
      {
        console.log(res);
        console.log(res.header);
        wx.removeStorageSync('sessionid');
        wx.setStorageSync("sessionid", res.header["Set-Cookie"]);

        if (res.data.Status == 100) {
          wx.showToast({
            title: res.data.Data,
            icon: 'success',
            duration: 1000
          })
          var pages = getCurrentPages()
          var i = 60;
          var intervalId = setInterval(function () {
            i--
            if (i <= 0) {
              pages[pages.length - 1].setData({
                disableGetMobileCode: false,
                disableSubmitMobileCode: false,
                getCodeButtonText: '获取验证码'
              })
              clearInterval(intervalId)
            } else {
              pages[pages.length - 1].setData({
                getCodeButtonText: i,
                disableGetMobileCode: true,
                disableSubmitMobileCode: false
              })
            }
          }, 1000);
        } else {
          wx.showToast({
            title: '发送失败',
            icon: 'none',
            duration: 1000
          })
        }
        //---
      }
    })

        //----------------------------------------
      /*
        util.request(api.SmsCode, {phone: this.data.mobile}, 'GET').then(function (res) 
        {
              if (res.Status == 100) 
                {
                    wx.showToast({
                        title:res.Data,
                        icon: 'success',
                        duration: 1000
                    })
                    var pages = getCurrentPages()
                    var i = 60;
                    var intervalId = setInterval(function () {
                        i--
                        if (i <= 0) {
                            pages[pages.length - 1].setData({
                                disableGetMobileCode: false,
                                disableSubmitMobileCode: false,
                                getCodeButtonText: '获取验证码'
                            })
                            clearInterval(intervalId)
                        } else {
                            pages[pages.length - 1].setData({
                                getCodeButtonText: i,
                                disableGetMobileCode: true,
                                disableSubmitMobileCode: false
                            })
                        }
                    }, 1000);
                } else {
                    wx.showToast({
                        title: '发送失败',
                        icon: 'none',
                        duration: 1000
                    })
                }
            });
      */
    },
  bindGetUserInfo:function(e)
  {
    var that=this;
    var userInfo = e.detail.userInfo;
    var wxo=wx.getStorageSync('wx');
    var fromid = wx.getStorageSync("fromid");
    if (userIcon != "" && wxo!="")
    {
      var openid = wxo.openid;
      var userIcon = e.detail.userInfo.avatarUrl;
      var userName = e.detail.userInfo.nickName;
      var mobile = that.data.mobile;
      var code = that.data.code;
      if (!this.bindCheckMobile(mobile)) 
      {
        util.showErrorToast("您输入的手机号码格式错误");
        return;
      }
      if (!(code && code.length === 4)) {
        util.showErrorToast("请输入验证码");
        return;
      }

      wx.showToast({
        title: '操作中...',
        icon: 'loading',
        duration: 5000
      });
      //string Code, string OpenID,string Mobile,string UserName,string UserIcon, string FromID=""
      var pd = { Code: code, OpenID: openid, Mobile: mobile, UserName: userName, UserIcon: userIcon, FromID:fromid};
      wx.request({
        url: api.BindMobile, // 仅为示例，并非真实的接口地址
        data: pd,
        header: {
          'content-type': 'application/json', // 默认值
          'cookie': wx.getStorageSync("sessionid")
        },
        success(res) {
          if (res.data.Status == 100) {
            wx.setStorageSync('userInfo', res.data.Data);
            wx.showModal({
              title: '提示',
              content: '操作成功',
              showCancel: false
            })
            wx.switchTab({
              url: '/pages/ucenter/index/index'
            });

          }
          else {
            wx.showModal({
              title: '提示',
              content: res.Msg,
              showCancel: false
            })
          }
        }
      })
    /*
      util.request(api.BindMobile,pd,'GET')
        .then(function (res) {
          if (res.Status == 100) 
          {
            wx.setStorageSync('u', res.Data);
            wx.showModal({
              title: '提示',
              content: '操作成功',
              showCancel: false
            })
            wx.switchTab({
              url: '/pages/ucenter/index/index'
            });
            
          }
          else {
            wx.showModal({
              title: '提示',
              content: res.Msg,
              showCancel: false
            })
          }
        })
      */

    }
    else
    {
      util.showErrorToast("无法绑定手机号码，请重试");
    }
    
    
  },

    bindLoginMobilecode: function (e) {
        var mobile = this.data.mobile;
        if (!this.bindCheckMobile(mobile)) {
            return
        }
        if (!(e.detail.value.code && e.detail.value.code.length === 4)) {
            return
        }
        wx.showToast({
            title: '操作中...',
            icon: 'loading',
            duration: 5000
        })
        util.request(api.BindMobile, {mobile_code: e.detail.value.code, mobile: mobile})
            .then(function (res) {
                if (res.data.code == 200) 
                {
                    wx.showModal({
                        title: '提示',
                        content: '操作成功',
                        showCancel: false
                    })
                    wx.switchTab({
                        url: '/pages/ucenter/index/index'
                    });
                } 
                else 
                {
                    wx.showModal({
                        title: '提示',
                        content: '验证码错误',
                        showCancel: false
                    })
                }
            })
    }
})