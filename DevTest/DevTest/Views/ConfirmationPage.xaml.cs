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
            string srvrdbname = "DevTest";
            string srvrname = "192.168.10.5";
            string pass = "123";
            string name = "ghulam";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={name};Password={pass}";         
            sqlConnection = new SqlConnection(sqlconn);
        }


        private async void Back_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

        }

        private void userCode_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private async void Next_Clicked(object sender, EventArgs e)
        {
            string srvrdbname = "DevTest";
            string srvrname = "192.168.10.5";
            string pass = "123";
            string name = "ghulam";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={name};Password={pass}";
            // sqlConnection = new SqlConnection(sqlconn);
           var NetworkName = DependencyService.Get<IConnectedNetworkName>().GetNetworkName();

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
            if( myTableLists.Count > 0)
            {
                bool exist = myTableLists.Any(a => a.Code == userCode.Text && a.NetworkName == NetworkName);
                if( exist ) {
                     var UserName = myTableLists.Where(b => b.Code == userCode.Text).FirstOrDefault();
                   if( UserName != null )
                    {
                      StaticClass.UserName = UserName.NetworkName;
                      await Navigation.PushAsync(new ResultPage()); }    
                    }
            }    
            reader.Close();
            sqlConnection.Close();

           

            //using (SqlConnection connection = new SqlConnection(sqlconn))
            //{
            //    connection.Open();

            //    string query = $"SELECT COUNT(*) FROM dbo.InfoTable WHERE Code = {userCode.Text} AND NetworkName = {NetworkName}";
            //    SqlCommand command = new SqlCommand(query, connection);
            //    command.Parameters.AddWithValue("@Code", userCode.Text);
            //    command.Parameters.AddWithValue("@NetworkName", NetworkName);

            //    int result = (int)command.ExecuteScalar();

            //    if (result > 0)
            //    {
            //        // User exists in database
            //    }
            //    else
            //    {
            //        // User does not exist in database
            //    }
            //}
        }
    }
}