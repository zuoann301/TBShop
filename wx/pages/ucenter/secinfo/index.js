Page({

  /**
   * 页面的初始数据
   */
  data: {
    latitude: 22.72060,
    longitude: 114.24936,
    markers: [{
      iconPath: "/static/images/loc.png",
      id: 0,
      latitude:0,
      longitude:0,
      width: 40,
      height: 40,
      callout: {
        content: '本次登录地点',
        color: '#000000',
        padding: 10,
        display: 'ALWAYS',
        textAlign: 'left',
        borderRadius: 10,
        borderWidth: 2,
        borderColor: '#ff8106'
      }
    },
    {
      iconPath: "/static/images/loc.png",
      id: 1,
      latitude:0,
      longitude:0,
      width: 40,
      height: 40,
      callout: {
        content: '现在的位置',
        color: '#000000',
        padding: 10,
        display: 'ALWAYS',
        textAlign: 'left',
        borderRadius: 10,
        borderWidth: 2,
        borderColor: '#ff8106'
      }
    }

    ],
    polyline: [
      {
        points: [],
        color: "#000000",
        width: 2,
        dottedLine: true
      }
    ],
    controls: [{
      id: 1,
      iconPath: '/static/images/loc.png',
      position: {
        left: 0,
        top: 300 - 50,
        width: 50,
        height: 50
      },
      clickable: true
    }],
    cur_point1: [],
    cur_point2: []
  },
  regionchange(e) {
    console.log(e.type)
  },
  markertap(e) {
    console.log(e.markerId)
  },
  controltap(e) {
    console.log(e.controlId)
  },
  translateMarker: function () {
    var that = this;
    this.mapCtx.translateMarker({
      markerId: 0,
      autoRotate: true,
      duration: 1000,
      destination: {
        latitude: that.data.latitude,
        longitude: that.data.longitude,
      },
      animationEnd() {
        console.log('animation end')
      }
    })
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    var cur_markers = this.data.markers;
    var cur_polyline = this.data.polyline;

    if (options.latitude != '' && options.longitude != '') 
    {
      this.setData({ latitude: options.latitude });
      this.setData({ longitude: options.longitude });
      this.mapCtx = wx.createMapContext('myMap');



      cur_markers[0].latitude = options.latitude;
      cur_markers[0].longitude = options.longitude;
      
      that.setData({ cur_point1: { longitude: options.longitude, latitude: options.latitude } });

      var loc = wx.getStorageSync('loc');
      cur_markers[1].latitude = loc.latitude;
      cur_markers[1].longitude = loc.longitude;

      that.mapCtx.translateMarker({
        markerId: 0,
        autoRotate: true,
        duration: 1000,
        destination: {
          latitude: options.latitude,
          longitude: options.longitude,
        },
        animationEnd() {
          console.log('animation end')
        }
      })

      that.setData({ cur_point2: { longitude: loc.longitude, latitude: loc.latitude } });

      that.setData({ markers: cur_markers });
      that.setData({ polyline: cur_polyline });


    }



  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

    var arr = new Array();
    arr[0] = this.data.cur_point1;
    arr[1] = this.data.cur_point2;

    var obj = [{
      points: arr,
      color: "#000000",
      width: 2,
      dottedLine: true
    }
    ];

    this.setData({ polyline: obj });
    console.log(this.data.cur_point1);
    console.log(this.data.cur_point2);
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})