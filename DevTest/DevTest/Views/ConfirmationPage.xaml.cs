using DevTest.Controls;
using DevTest.Model;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DevTest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmationPage : ContentPage
    {
        SqlConnection sqlConnection;
        public ConfirmationPage()
        {
            InitializeComponent();
            string DbName = "DevTest";
            string srvrName = "192.168.10.5";
            string userName = "ghulam";
            string password = "123";
            string sqlconn = $"Data Source={srvrName};Initial Catalog={DbName};User ID={userName};Password={password}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private async void Back_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            try
            {
                string DbName = "DevTest";
                string srvrName = "192.168.10.5";
                string userName = "ghulam";
                string password = "123";
                string sqlconn = $"Data Source={srvrName};Initial Catalog={DbName};User ID={userName};Password={password}";
                sqlConnection = new SqlConnection(sqlconn);
                var NetworkName = DependencyService.Get<IConnectedNetworkName>().GetNetworkName();

                //getting list of data from DB to compare the code and network name 
                if(!string.IsNullOrEmpty(userCode.Text) )
                {
                    if(userCode.Text == StaticClass.UserCode)
                    {
                        List<NetworkInfo_M> myTableLists = new List<NetworkInfo_M>();
                        sqlConnection.Open();
                        string queryString = "Select * from dbo.InfoTable";
                        SqlCommand command = new SqlCommand(queryString, sqlConnection);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            myTableLists.Add(new NetworkInfo_M
                            {
                                Code = reader["Code"].ToString(),
                                NetworkName = reader["NetworkName"].ToString(),
                            }
                            );
                        }
                        if (myTableLists.Count > 0)
                        {
                            bool exist = myTableLists.Any(a => a.Code == userCode.Text && a.NetworkName == NetworkName);
                            if (exist)
                            {
                                var UserName = myTableLists.Where(b => b.Code == userCode.Text).FirstOrDefault();

                                if (UserName != null)
                                {
                                    StaticClass.UserName = UserName.NetworkName;
                                    await Navigation.PushAsync(new ResultPage());
                                }
                            }
                            else
                            {
                                await DisplayAlert("Alert", "Please Insert a valid Code", "Ok");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Alert", "You have no data", "Ok");
                        }
                        reader.Close();
                        sqlConnection.Close();
                    }
                    else
                    {
                        await DisplayAlert("Alert", "Please enter the correct code", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Alert", "you must enter the code", "Ok");
                }

            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}