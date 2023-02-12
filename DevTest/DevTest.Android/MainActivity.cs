using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using DevTest.Controls;
using DevTest.Droid.NetworkName;
using Android.Net.Wifi;
using Android.Net;
using Android;
using Java.Lang.Reflect;
using Android.Content;
using Java.Interop;

namespace DevTest.Droid
{
    [Activity(Label = "DevTest", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
      
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            DependencyService.Register<IConnectedNetworkName, ConnectedNetworkName>();
            // Request the CHANGE_NETWORK_STATE and WRITE_SETTINGS permissions
            //if (CheckSelfPermission(Manifest.Permission.ChangeNetworkState) != Permission.Granted ||
            //    CheckSelfPermission(Manifest.Permission.WriteSettings) != Permission.Granted)
            //{
            //    RequestPermissions(new string[] { Manifest.Permission.ChangeNetworkState, Manifest.Permission.WriteSettings }, 0);
            //}
           

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnStart()
        {
            base.OnStart();
             
            NetworkRequest request = new NetworkRequest.Builder().AddTransportType(transportType: TransportType.Wifi).Build();
            ConnectivityManager connectivityManager = Android.App.Application.Context.GetSystemService(Android.Content.Context.ConnectivityService) as ConnectivityManager;

            NetworkCallbackFlags flagIncludeLocationInfo = NetworkCallbackFlags.IncludeLocationInfo;
            NetworkCallback networkCallback = new NetworkCallback((int)flagIncludeLocationInfo);
            connectivityManager.RequestNetwork(request, networkCallback);


            ConnectivityManager connectivityManagr = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);

            NetworkInfo networkInfo = connectivityManagr.ActiveNetworkInfo;

            var ssid = networkInfo.TypeName;
            var ssd = networkInfo.Subtype;
            var ss = networkInfo.Type;
            var s = networkInfo.GetJniTypeName();
            




        }
       
        private class NetworkCallback : ConnectivityManager.NetworkCallback
        {
            public NetworkCallback(int flags) : base(flags)
            {
               
            }


            public override void OnCapabilitiesChanged(Network network, NetworkCapabilities networkCapabilities)
            {
                base.OnCapabilitiesChanged(network, networkCapabilities);
                WifiInfo wifiInfo = (WifiInfo)networkCapabilities.TransportInfo;

                if (wifiInfo != null)
                {
                    string ssid = wifiInfo.SSID.Trim(new char[] { '"', '\"' });
                    string bssid = wifiInfo.HiddenSSID.ToString();
                }
            }            //public override void OnCapabilitiesChanged(Network network, NetworkCapabilities networkCapabilities)
            
            
        }
    }
}
 