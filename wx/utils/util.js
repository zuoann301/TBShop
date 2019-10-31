var api = require('../config/api.js');

function formatTime(date) {
  var date = new Date(date); //返回当前时间对象
  var year = date.getFullYear();
  var month = date.getMonth() + 1;
  var day = date.getDate();

  var hour = date.getHours();
  var min = date.getMinutes();

  return [year, month, day].join('-') + ' ' + [hour, min].join(':');
}
 

function formatNumber(n) {
    n = n.toString()
    return n[1] ? n : '0' + n
}

function formatDistance(Distance) {
  var s = '';
  if (Distance >= 1000) {
    s = (Distance / 1000).toFixed(2) + '公里';
  }
  else {
    s = Distance + '米';
  }
  return s;
}

/**
 * 封封微信的的request header = "application/x-www-form-urlencoded"
 */
function request(url, data = {}, method = "POST", header = "application/x-www-form-urlencoded") {
    wx.showLoading({
        title: '加载中...',
    });
    return new Promise(function (resolve, reject) {
        
        wx.request({
            url: url,
            data: data,
            method: method,
            header: 
            {
              'Content-Type': header
            },
            success: function (res) {
                wx.hideLoading();
                if (res.statusCode == 200) 
                {

                    if (res.data.errno == 401) 
                    {
                        //wx.navigateTo({url: '/pages/auth/btnAuth/btnAuth',})
                    } else 
                    {
                        resolve(res.data);
                    }
                } else {
                    reject(res.errMsg);
                }

            },
            fail: function (err) {
              console.log('error:' + url);
                reject(err);
            }
        })
    });
}

/**
 * 检查微信会话是否过期
 */
function checkSession() {
    return new Promise(function (resolve, reject) {
        wx.checkSession({
            success: function () {
                resolve(true);
            },
            fail: function () {
                reject(false);
            }
        })
    });
}

/**
 * 调用微信登录
 */
function login() {
    return new Promise(function (resolve, reject) {
        wx.login({
            success: function (res) {
                if (res.code) {
                    resolve(res);
                } else {
                    reject(res);
                }
            },
            fail: function (err) {
                reject(err);
            }
        });
    });
}

function redirect(url) {

    //判断页面是否需要登录
    if (false) {
        wx.redirectTo({
            url: '/pages/auth/login/login'
        });
        return false;
    } else {
        wx.redirectTo({
            url: url
        });
    }
}

function showErrorToast(msg) {
    wx.showToast({
        title: msg,
        image: '/static/images/icon_error.png'
    })
}

function showSuccessToast(msg) {
    wx.showToast({
        title: msg,
    })
}

module.exports = {
    formatTime,
    request,
    redirect,
    showErrorToast,
    showSuccessToast,
    checkSession,
    login,
  formatDistance
}


