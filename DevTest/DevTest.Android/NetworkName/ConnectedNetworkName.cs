using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevTest.Controls;
using System.Threading.Tasks;
using DevTest.Droid.NetworkName;
using Xamarin.Forms;
using Java.Interop;

[assembly: Dependency(typeof(ConnectedNetworkName))]
namespace DevTest.Droid.NetworkName
{
    public class ConnectedNetworkName: IConnectedNetworkName
    {

        [Obsolete]
        public string GetNetworkName()
        {
            try
            {
                var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo networkInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);
                if (networkInfo.IsConnected)
                {

                    WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
                    WifiInfo wifiInfo = wifiManager.ConnectionInfo;

                    string name = networkInfo.ExtraInfo;
                    string ssid =  wifiInfo.SSID;
                    StaticClass.NetworkName = ssid.ToString();
                    
                    if (StaticClass.NetworkName == "<unknown ssid>")
                    {
                        ConnectivityManager connectivityManagr = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
                        NetworkInfo networkInf = connectivityManagr.ActiveNetworkInfo;

                        StaticClass.NetworkName  = networkInfo.TypeName;
                        return StaticClass.NetworkName;


                    }
                    return StaticClass.NetworkName;
                
                }
                else
                {
                    return StaticClass.NetworkName; 
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
    }
}