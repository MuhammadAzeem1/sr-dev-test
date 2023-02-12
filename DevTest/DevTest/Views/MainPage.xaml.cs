using Android.App;
using Android.Net.Wifi.Aware;
using Android.Telephony;
using DevTest.Controls;
using Java.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace DevTest.Views
{
    public partial class MainPage : ContentPage
    {
        SqlConnection sqlConnection;
        public MainPage()
        {
            InitializeComponent();
            string srvrdbname = "DevTest";
            string srvrname = "192.168.10.5";
            string pass = "123";
            string  name = "ghulam";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={name};Password={pass}";       
            sqlConnection = new SqlConnection(sqlconn);
        }

        private void userCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (userCode.Text.Length == 4)
                {
                    NetworkName.Text = DependencyService.Get<IConnectedNetworkName>().GetNetworkName();
                }
                if (string.IsNullOrEmpty(userCode.Text))
                {
                    NetworkName.Text = "";
                }
            }
            catch (Exception ex) 
            {

                throw ex;
            }
           
        }
        private async void NextBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
               
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO dbo.InfoTable VALUES(@Code , @NetworkName)", sqlConnection))
                {
                    //command.Parameters.Add(new SqlParameter("ID", id));
                    command.Parameters.Add(new SqlParameter("Code", userCode.Text));
                    command.Parameters.Add(new SqlParameter("NetworkName", NetworkName.Text));
                    command.ExecuteNonQuery();
                }
                sqlConnection.Close();
                await DisplayAlert("Alert", "Congrats you just posted data", "Ok");
                await Navigation.PushAsync(new ConfirmationPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "Ok");              
            }

        }

    }
}
