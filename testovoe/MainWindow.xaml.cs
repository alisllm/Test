using System;
using System.Windows;
using System.Data.SqlClient;
using System.Linq;
using testovoe.DataSet1TableAdapters;
using System.Collections.Generic;
using System.Data.Linq;
using testovoe.Properties;

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
      
        public MainWindow()
        {
            InitializeComponent();
            ObnovaDatagrid();
        }
        public void ObnovaDatagrid()
        {
            ////
            ///ОБНОВЛЕНИЕ ДАТАГРИДОВ
            ///
            zakazTableadapter = new ZakazTableAdapter();
            prihodTableadapter = new Prihod_DenegTableAdapter();

            datagrid.ItemsSource = zakazTableadapter.GetData();
            datagrid2.ItemsSource = prihodTableadapter.GetData();

            DataContext db = new DataContext(Settings.Default.DefaultConnection);
            Table<Zakaz> zakaz = db.GetTable<Zakaz>();
            Table<Prihod_Deneg> Prihod_Deneg = db.GetTable<Prihod_Deneg>();
            ////
            ///СОРТИРОВКА ОПЛАЧЕННЫЗ ЗАКАЗОВ
            ///
            var id_Zakaza = from u in zakaz
                            where u.Summa != u.SummaOplat
                            select u.Id;

            combo_Zakaz.ItemsSource = id_Zakaza;

            ////
            ///СОРТИРОВКА ИЗРАСХОДОВАННЫХ ПРИХОДОВ
            ///
            var id_Prihoda = from u in Prihod_Deneg
                             where u.Ostatok != 0
                             select u.Id;

            combo.ItemsSource = id_Prihoda;

        }

        /// <summary>
        /// Метод обработки кнопки создание платежа
        /// </summary>
        private void Platej_Click(object sender, RoutedEventArgs e)
        {

            tableDataset = new DataSet1();
            if (combo.SelectedItem != null && combo_Zakaz.SelectedItem != null)
            {
                zakazTableadapter = new ZakazTableAdapter();

                var returnValue = zakazTableadapter.ScalarQuery(Convert.ToInt32(combo_Zakaz.Text));

                if (Convert.ToDouble(returnValue)>0)
                {
                    platejTableadapter = new Platej2TableAdapter();
                    platejTableadapter.InsertQuery(Convert.ToInt32(combo_Zakaz.Text), Convert.ToInt32(combo.Text));
                    platejTableadapter.Fill(tableDataset.Platej2);

                    ObnovaDatagrid();
                }
                else
                {
                    MessageBox.Show("Заказ уже был оплачен ");
                    ObnovaDatagrid();
                }                
            }
            else
            {
                MessageBox.Show("Plese, Выберите из выпадающего списка номер необходимого заказа и прихода денег");
            }

        }

    }
   
}
