var api = require('../../../config/api.js');
var util = require('../../../utils/util.js');
var app = getApp()

Page({
  data: {
    mobile: '',
    code: '',
    userInfo: {
      avatarUrl: '',
      nickName: ''
    },
    disableGetMobileCode: false,
    disableSubmitMobileCode: true,
    getCodeButtonText: '获取验证码',

    userInfo:null,
    extInfo: { mobile:'',openid:''}
  },

  onShow: function () {},

  onLoad: function () 
  {
    var that = this;
 
    var userInfo=wx.getStorageSync('userInfo');
    if(userInfo!="")
    {
      this.setData({ userInfo: userInfo});
    }
    var extInfo=wx.getStorageSync('extInfo');
    if(extInfo!="")
    {
      this.setData({extInfo:extInfo});
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
    that.setData({
      disableGetMobileCode: true
    })
  },

  bindInputMobile: function (e) {
    this.setData({
      mobile: e.detail.value
    });
  },
  bindInputCode: function (e) {
    this.setData({
      code: e.detail.value
    });
  },
  countDownPassCode: function () {
    if (!this.bindCheckMobile(this.data.mobile)) {
      return
    }
    //-------------------------------------------
    wx.request({
      url: api.SmsCode,
      data: {
        phone: this.data.mobile
      },
      header: {
        'content-type': 'application/json'
      },
      success(res) {
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
    
  },
  bindGetUserInfo: function (e) {
    var that = this;
    var userInfo = e.detail.userInfo;
    var wxo = wx.getStorageSync('wx');
    var fromid = wx.getStorageSync("fromid");
    if (userIcon != "" && wxo != "") {
      var openid = wxo.openid;
      var userIcon = e.detail.userInfo.avatarUrl;
      var userName = e.detail.userInfo.nickName;
      var mobile = that.data.mobile;
      var code = that.data.code;
      if (!this.bindCheckMobile(mobile)) {
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
      var pd = {
        Code: code,
        OpenID: openid,
        Mobile: mobile,
        UserName: userName,
        UserIcon: userIcon,
        FromID: fromid
      };
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

          } else {
            wx.showModal({
              title: '提示',
              content: res.Msg,
              showCancel: false
            })
          }
        }
      })


    } else {
      util.showErrorToast("无法绑定手机号码，请重试");
    }


  },
 
  getPhoneNumber(e) 
  {
    var that=this;
    var iv = e.detail.iv;
    var encryptedData = e.detail.encryptedData;
    wx.login({
      success(res) 
      {
        if (res.code) 
        {
          util.request(api.GetPhoneNumber, { jscode: res.code, encryptedData: encryptedData, IV: iv}, 'POST').then(function (res) {
            if (res.Status === 100) 
            {
              that.setData({ extInfo: res.Data });
              wx.setStorageSync('extInfo', res.Data);
            }
          });
        }
        else 
        {
          console.log('登录失败！' + res.errMsg)
        }
      }
    })
    
  },
  getUserInfo2: function (e) 
  {
    var that=this;
    var tempInfo = e.detail.userInfo;
    that.setData({ tempInfo: e.detail.userInfo});
    var userInfo = wx.getStorageSync('userInfo');
    var shopid = wx.getStorageSync('ShopID')||1;
    var uid = wx.getStorageSync('uid')||'';
    if (userInfo=='')
    {
      userInfo = e.detail.userInfo;
      var pd = { OpenID:that.data.extInfo.openid,
        Mobile: that.data.extInfo.mobile,
        UserName: tempInfo.nickName,
        UserIcon: tempInfo.avatarUrl,
        FromID:uid,
        ShopID: shopid
        };
      //string OpenID,string Mobile,string UserName,string UserIcon,string FromID="",int ShopID=1
      util.request(api.WeChatLogin, pd, 'POST').then(function (res) 
      {
        if (res.Status === 100) 
        {
          that.setData({ userInfo: res.Data });
          wx.setStorageSync('userInfo', res.Data);
          wx.switchTab({url: '/pages/index/index'});
        }
        else
        {
          wx.showModal({title: 'ERROR',content: '操作发生错误'});
        }
      });
    }
  },
})