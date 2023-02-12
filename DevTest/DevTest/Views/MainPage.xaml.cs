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
            string srvrdbname = "mydb";
            string srvrname = "192.168.1.73";
            string srvrusername = "samir";
            string srvrpassword = "123456";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
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
        private void NextBtn_Clicked(object sender, EventArgs e)
        {
            //try
            //{
            //    sqlConnection.Open();
            //    using (SqlCommand command = new SqlCommand("INSERT INTO dbo.mytable VALUES(@Id, @Title , @Body)", sqlConnection))
            //    {
            //        command.Parameters.Add(new SqlParameter("Id", UserId.Text));
            //        command.Parameters.Add(new SqlParameter("Title", UserTitle.Text));
            //        command.Parameters.Add(new SqlParameter("Body", UserBody.Text));
            //        command.ExecuteNonQuery();
            //    }
            //    sqlConnection.Close();
            //    await App.Current.MainPage.DisplayAlert("Alert", "Congrats you just posted data", "Ok");
            //}
            //catch (Exception ex)
            //{
            //    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
            //    throw;
            //}

        }

    }
}
