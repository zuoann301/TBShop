const util = require('../utils/util.js');
const api = require('../config/api.js');

function getShop(shopid) {
  return new Promise(function (resolve, reject) 
  {
    util.request(api.ShopInfo, { ShopID: shopid }, 'GET').then((res) => {
      if (res.Status == 100) {
        resolve(res.Data);
      }
      else {
        reject(res);
      }
    });
    
  });
}
//设置页面标题
function setPageTitle()
{
   var aps=getApp();
  if (aps.globalData.shopInfo!=null)
  {
    wx.setNavigationBarTitle({ title: aps.globalData.shopInfo.ShopName });
  }
}

 
//---------------------------------------------
//首先定义一下，全局变量
var lastTime = 0; //此变量用来记录上次摇动的时间
var x = 0,
  y = 0,
  z = 0,
  lastX = 0,
  lastY = 0,
  lastZ = 0; //此组变量分别记录对应 x、y、z 三轴的数值和上次的数值
var shakeSpeed = 110; //设置阈值
//编写摇一摇方法
function shake(acceleration, audioCtx) 
{
  var nowTime = new Date().getTime(); //记录当前时间
  //如果这次摇的时间距离上次摇的时间有一定间隔 才执行
  if (nowTime - lastTime > 100) {
    var diffTime = nowTime - lastTime; //记录时间段
    lastTime = nowTime; //记录本次摇动时间，为下次计算摇动时间做准备
    x = acceleration.x; //获取 x 轴数值，x 轴为垂直于北轴，向东为正
    y = acceleration.y; //获取 y 轴数值，y 轴向正北为正
    z = acceleration.z; //获取 z 轴数值，z 轴垂直于地面，向上为正
    //计算 公式的意思是 单位时间内运动的路程，即为我们想要的速度
    var speed = Math.abs(x + y + z - lastX - lastY - lastZ) / diffTime * 10000;
    //console.log(speed)
    if (speed > shakeSpeed) 
    { //如果计算出来的速度超过了阈值，那么就算作用户成功摇一摇
      wx.stopAccelerometer()
       
      audioCtx.setSrc(api.FileHost+'/s.mp3')
      audioCtx.play();
      setTimeout(function () { wx.showLoading({ title: '摇一摇' });},500);
      setTimeout(function () { wx.showLoading({ title: '寻找' }); }, 1000);
      setTimeout(function () { wx.showLoading({ title: '最近便利店' }); }, 1500);
     
      setTimeout(function () {
        audioCtx.setSrc(api.FileHost +'/r.mp3')
        audioCtx.play();
      }, 1500)

      setTimeout(function(){
        wx.reLaunch({ url: '/pages/shoplist/index' });
      },2000);

    }
    lastX = x; //赋值，为下一次计算做准备
    lastY = y; //赋值，为下一次计算做准备
    lastZ = z; //赋值，为下一次计算做准备
  }
}
//--------------------------------------------
module.exports = {
  getShop,
  setPageTitle,
  shake
};

