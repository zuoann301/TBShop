/**
 * 支付相关服务
 */

const util = require('../utils/util.js');
const api = require('../config/api.js');
const app=getApp();
/**
 * 判断用户是否登录
 */
function payOrder(orderId,openId) {
  return new Promise(function (resolve, reject) {
    util.request(api.PayPrepayId, {
      OrderID: orderId,OpenId: openId
    },'GET').then((res) => 
    {
      if (res.success==true) 
      {
        const payParam = res;
        app.globalData.prepay_id = res.prepay_id;
        wx.requestPayment({
          'timeStamp': payParam.timeStamp,
          'nonceStr': payParam.nonceStr,
          'package': payParam.package,
          'signType': payParam.signType,
          'paySign': payParam.paySign,
          'success': function (res1) 
          {
            resolve(res1);
            console.log(res1);
          },
          'fail': function (res2) {
            reject(res2);
          },
          'complete': function (res3) {
            reject(res3);
          }
        });
      } else {
        reject(res);
      }
    });
  });
}


module.exports = {
  payOrder,
};











