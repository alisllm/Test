using System;
using System.Windows;
using System.Data.SqlClient;
using testovoe.DataSet1TableAdapters;

namespace testovoe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataSet1 tableDataset;
        Platej2TableAdapter platejTableadapter;
        Prihod_DenegTableAdapter prihodTableadapter;
        ZakazTableAdapter zakazTableadapter;
        string s = @"Data Source = DESKTOP-8SU3AO0\SQLEXPRESS;Integrated Security = true;Initial Catalog=Test2";
        public MainWindow()
        {
            InitializeComponent();
            ObnovaDatagrid();
            SqlConnection connection = new SqlConnection(s);
            connection.Open();
            ////
            ///Выгрузка в комбобокс 
            string Sql = "select Id from Prihod_Deneg";
            SqlCommand cmd = new SqlCommand(Sql, connection);
            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read())
            {
                combo.Items.Add(DR[0]);
            }
            cmd.Dispose();
            DR.Close();

            ////
            ///Выгрузка в комбобокс 2
            string Sql2 = "select Id from Zakaz";
            SqlCommand cmd2 = new SqlCommand(Sql2, connection);
            SqlDataReader DR2 = cmd2.ExecuteReader();

            while (DR2.Read())
            {
                combo_Zakaz.Items.Add(DR2[0]);

            }
            cmd2.Dispose();
            DR2.Close();
            connection.Close();
        }
        public void ObnovaDatagrid()
        {
            zakazTableadapter = new ZakazTableAdapter();
            prihodTableadapter = new Prihod_DenegTableAdapter();

            datagrid.ItemsSource = zakazTableadapter.GetData();
            datagrid2.ItemsSource = prihodTableadapter.GetData();
        }

        /// <summary>
        /// Метод обработки кнопки создание платежа
        /// </summary>
        private void Platej_Click(object sender, RoutedEventArgs e)
        {
            tableDataset = new DataSet1();
            if (combo.SelectedItem != null && combo_Zakaz.SelectedItem != null)
            {
                platejTableadapter = new Platej2TableAdapter();
                platejTableadapter.InsertQuery(Convert.ToInt32(combo_Zakaz.Text), Convert.ToInt32(combo.Text));
                platejTableadapter.Fill(tableDataset.Platej2);

                ObnovaDatagrid();
            }
            else
            {
                MessageBox.Show("Plese, Выберите из выпадающего списка номер необходимого заказа и прихода денег");
            }

        }
    }
}
