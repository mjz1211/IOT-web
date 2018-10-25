using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//空白頁項目範本收錄在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MQClient
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Data Field
        //GPIO Controller物件
        private GpioController controller=GpioController.GetDefault();
        //LED PIN常數
        private const Int32 LEDPIN = 5;
        //房間燈腳位
        private const Int32 BEDPIN = 20;
        //控制led PIN物件
        GpioPin ledPIN;
        GpioPin bedPIN;

        private MqttClient client; //客廳
        //bed燈光控制訂閱
        private MqttClient clientBed;
        public MainPage()
        {
            this.InitializeComponent();
            //初始化LED PIN物件
            ledPIN=controller.OpenPin(LEDPIN);
            //ledPIN初始狀態 設定輸出OUTPUT(通電)
            ledPIN.SetDriveMode(GpioPinDriveMode.Output);
            //初始狀態關燈狀態
            ledPIN.Write(GpioPinValue.Low);
            //房間燈Gpio Pin物件初始化
            bedPIN = controller.OpenPin(BEDPIN);
            bedPIN.SetDriveMode(GpioPinDriveMode.Output);
            bedPIN.Write(GpioPinValue.Low);
            //初始化圖型
            ImageSource imgSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/photos/dark.png")); ;
            imgLight.Source = imgSource;
            imgBed.Source = imgSource;



        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //1連接到Broker service 進行註冊連接
                 client= new MqttClient("192.168.43.123");
                clientBed = new MqttClient("192.168.43.123");
                //2.連接一個端點(發佈端點）
                byte id = client.Connect(Guid.NewGuid().ToString());
                byte bedid = clientBed.Connect(Guid.NewGuid().ToString());

                //呈現訊息
                txtMessage.Text = String.Format("連接Server端點 ID:{0}", id);
            }catch(Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
        }

        //啟動訂閱
        private void btnSub_Click(object sender, RoutedEventArgs e)
        {
            Byte[] level = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };
            //訂閱主題 可以多個主題 等級可以是多次
            UInt16 subId=client.Subscribe(new String[]{"MQ"},level);
            UInt16 bedid = clientBed.Subscribe(new String[] { "BED"},level);

            txtMessage.Text = "訂閱ID:" + subId;
            //開始委派接受訊息的事件程序處理
            client.MqttMsgPublishReceived += 
                new MqttClient.MqttMsgPublishEventHandler(recevicedProcess);
            //被推播資訊下來變更房間燈裝狀態
            clientBed.MqttMsgPublishReceived +=
               new MqttClient.MqttMsgPublishEventHandler(recevicedBedProcess);
        }

        //接受到通知訊息處理程序
        private void recevicedProcess(Object sender, MqttMsgPublishEventArgs args)
        {
            //採用非同步處理
            Task.Run(async () =>
            {
                //執行在背後的執行緒
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //取出訂閱的主題內容
                        String content=System.Text.Encoding.UTF8.GetString(args.Message);
                        txReveive.Text = String.Format("主題:{0} 內容:{1}",
                            args.Topic,content
                            );
                        //切割訊息
                        String[] items=content.Split(new Char[] { ':' });
                        //判斷主題是客廳還是房間

                        //取出最後一個燈光狀態
                        Int32 pos=items.Length - 1;
                        String status=items[pos];
                        if (status.Equals("開燈"))
                        {
                            //換圖片
                            ImageSource imgSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/photos/light.jpg")); ;
                            imgLight.Source = imgSource;
                            ledPIN.Write(GpioPinValue.High);
                        }else
                        {
                            ImageSource imgSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/photos/dark.png")); ;
                            imgLight.Source = imgSource;
                            ledPIN.Write(GpioPinValue.Low);
                        }
                    });
            });


        }


        private void recevicedBedProcess(Object sender, MqttMsgPublishEventArgs args)
        {
            //採用非同步處理
            Task.Run(async () =>
            {
                //執行在背後的執行緒
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        //取出訂閱的主題內容
                        String content = System.Text.Encoding.UTF8.GetString(args.Message);
                        txtBedReceive.Text = String.Format("主題:{0} 內容:{1}",
                            args.Topic, content
                            );
                        //切割訊息
                        String[] items = content.Split(new Char[] { ':' });
                        //判斷主題是客廳還是房間

                        //取出最後一個燈光狀態
                        Int32 pos = items.Length - 1;
                        String status = items[pos];
                        if (status.Equals("開燈"))
                        {
                            //換圖片
                            ImageSource imgSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/photos/light.jpg")); ;
                            imgBed.Source = imgSource;
                            bedPIN.Write(GpioPinValue.High);
                        }
                        else
                        {
                            ImageSource imgSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/photos/dark.png")); ;
                            imgBed.Source = imgSource;
                            bedPIN.Write(GpioPinValue.Low);
                        }
                    });
            });


        }
    }
}
