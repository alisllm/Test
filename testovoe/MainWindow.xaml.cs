using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testovoe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            ///Выгрузка в комбобокс 
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
            SqlConnection connection = new SqlConnection(s);
            connection.Open();

            SqlDataAdapter adapter = new SqlDataAdapter("select * from Zakaz", connection);
            System.Data.DataTable dataTable = new System.Data.DataTable("Zakaz");
            adapter.Fill(dataTable);
            datagrid.ItemsSource = dataTable.DefaultView;

            SqlDataAdapter adapter2 = new SqlDataAdapter("select * from Prihod_Deneg", connection);
            System.Data.DataTable dataTable2 = new System.Data.DataTable("Prihod_Deneg");
            adapter2.Fill(dataTable2);
            datagrid2.ItemsSource = dataTable2.DefaultView;

           

        }

        /// <summary>
        /// Метод обработки кнопки создание платежа
        /// </summary>
        private void Platej_Click(object sender, RoutedEventArgs e)
        {
            if (combo.SelectedItem != null && combo_Zakaz.SelectedItem != null)
            {

                ///ПЕременная для хранения поля Остаток из таблицы Приход_Денег
                double ostatok;
                ///ПЕременная для хранения поля СуммаОплаты из таблицы Заказы
                double summaOplat;
                ///ПЕременная для хранения поля Сумма из таблицы Заказы
                double Summa;

                string Sql = "select Ostatok from Prihod_Deneg where Id = '" + combo.Text + "'";
                string Sql2 = "select summaOplat from Zakaz where Id = '" + combo_Zakaz.Text + "'";
                string Sql3 = "select summa from Zakaz where Id = '" + combo_Zakaz.Text + "'";

                SqlConnection connection = new SqlConnection(s);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();

                command = new SqlCommand(Sql, connection);
                ostatok = Convert.ToDouble(command.ExecuteScalar());

                SqlCommand command2 = new SqlCommand();
                command2.Connection = connection;
                command2 = new SqlCommand(Sql2, connection);
                summaOplat = Convert.ToDouble(command2.ExecuteScalar());

                SqlCommand command3 = new SqlCommand();
                command3.Connection = connection;
                command3 = new SqlCommand(Sql3, connection);
                Summa = Convert.ToDouble(command3.ExecuteScalar());
                ///
                ///Обработка ЕСЛИ ОСТАТОК ПОКРЫВАЕТ ПОЛНОСТЬЮ СУММУ
                ///
                if (Summa == (ostatok + summaOplat))
                {
                    SqlCommand cmd = new SqlCommand($"INSERT INTO Platej2 (Zakaz_id, Prihod_id, SummaPlatej) VALUES ('{combo_Zakaz.Text}','{combo.Text}','{ostatok}')", connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Остаток полностью оплатил сумму!");
                    ObnovaDatagrid();
                }
                ///
                ///Обработка ЕСЛИ ОСТАТОК БОЛЬШЕ  СУММЫ
                ///
                else if (ostatok > (Summa - summaOplat))
                {
                    double per = (Summa - summaOplat);


                    SqlCommand cmd = new SqlCommand($"INSERT INTO Platej2 (Zakaz_id, Prihod_id, SummaPlatej) VALUES ('{combo_Zakaz.Text}','{combo.Text}','{per}')", connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Остаток был больше суммы платежа");
                    ObnovaDatagrid();
                }
                ///
                ///Обработка ЕСЛИ ОСТАТОК МЕНЬШЕ  СУММЫ
                ///
                else if ((Summa - summaOplat) > ostatok)
                {
                    SqlCommand cmd = new SqlCommand($"INSERT INTO Platej2 (Zakaz_id, Prihod_id, SummaPlatej) VALUES ('{combo_Zakaz.Text}','{combo.Text}','{ostatok}')", connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Остаток меньше суммы оплаты, неободимо будет дополнить разницу след платежем");
                    ObnovaDatagrid();
                }

                connection.Close();
            }
            else
            {
                MessageBox.Show("Plese, Выберите из выпадающего списка номер необходимого заказа и прихода денег");
            }

        }
    }
}
