using Android.App;
using Android.Net.Wifi.Aware;
using Android.Telephony;
using DevTest.Controls;
using DevTest.Model;
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
            string DbName = "DevTest";
            string srvrName = "192.168.10.5";
            string userName = "ghulam";
            string password = "123";
            string sqlconn = $"Data Source={srvrName};Initial Catalog={DbName};User ID={userName};Password={password}";       
            sqlConnection = new SqlConnection(sqlconn);
        }

        private void userCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (userCode.Text.Length == 4)
                {
                    //Getting the connected network name
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
                //inserting code and network name into sql server
               if(!string.IsNullOrEmpty(userCode.Text) && !string.IsNullOrEmpty(NetworkName.Text))
                {
                    List<NetworkInfo_M> myTableLists = new List<NetworkInfo_M>();
                    sqlConnection.Open();
                    string queryString = "Select * from dbo.InfoTable";
                    SqlCommand commnd = new SqlCommand(queryString, sqlConnection);
                    SqlDataReader reader = commnd.ExecuteReader();
                    while (reader.Read())
                    {
                        myTableLists.Add(new NetworkInfo_M
                        {

                            Code = reader["Code"].ToString(),
                            NetworkName = reader["NetworkName"].ToString(),
                        }
                        );
                    }
                    sqlConnection.Close();
                    if(myTableLists.Count > 0 ) 
                    {
                        bool matched = myTableLists.Where(a => a.Code == userCode.Text).Any(); 
                        if( matched ) 
                        {
                          await DisplayAlert("Alert", "Code exist already !", "Ok");
                           
                        }
                        else
                        {
                            StaticClass.UserCode = userCode.Text;
                            sqlConnection.Open();
                            using (SqlCommand command = new SqlCommand("INSERT INTO dbo.InfoTable VALUES(@Code , @NetworkName)", sqlConnection))
                            {

                                command.Parameters.Add(new SqlParameter("Code", userCode.Text));
                                command.Parameters.Add(new SqlParameter("NetworkName", NetworkName.Text));
                                command.ExecuteNonQuery();
                            }
                            sqlConnection.Close();
                            await DisplayAlert("Alert", "Congrats you just posted data", "Ok");
                            await Navigation.PushAsync(new ConfirmationPage());
                        }

                    }
                    else
                    {
                        StaticClass.UserCode = userCode.Text;
                        sqlConnection.Open();
                        using (SqlCommand command = new SqlCommand("INSERT INTO dbo.InfoTable VALUES(@Code , @NetworkName)", sqlConnection))
                        {

                            command.Parameters.Add(new SqlParameter("Code", userCode.Text));
                            command.Parameters.Add(new SqlParameter("NetworkName", NetworkName.Text));
                            command.ExecuteNonQuery();
                        }
                        sqlConnection.Close();
                        await DisplayAlert("Alert", "Congrats you just posted data", "Ok");
                        await Navigation.PushAsync(new ConfirmationPage());
                    }                                  
                }
                else
                {
                    await DisplayAlert("Alert", "Value can not be null", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "Ok");              
            }

        }

    }
}
