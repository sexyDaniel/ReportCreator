using ReportCreater.Models;
using ReportCreater.Services;
using ReportCreater.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;


namespace ReportCreater
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationViewModel a;
        public MainWindow()
        {
            InitializeComponent();
            a = new ApplicationViewModel(new DefaultDialogService(), new EFClientRepository());
            DataContext = a;
        }

        private void OneHourePriceTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (OneHourePriceTextBox.Text != null && OneHourePriceTextBox.Text != "")
            {
                a.SelectedClient.OneHourePrice = double.Parse(OneHourePriceTextBox.Text);
                a.SelectedClient.TotalPrice = a.SelectedClient.Client.CalcTotalPrice();
                MinutePriceTextBox.Text = Math.Round((a.SelectedClient.OneHourePrice / 60), 3).ToString();
            }
            else
            {
                a.SelectedClient.TotalPrice = 0;
                MinutePriceTextBox.Text = "0";
            }
        }

        private void HourCount_KeyUp(object sender, KeyEventArgs e)
        {
            var r = ListBox_SelectedClient;
            var currentItem = r.Items.CurrentItem as ClientInfoViewModel;
            //currentItem.HourCount = int.Parse(c);
            //a.SelectedClient.TotalPrice = (decimal)a.SelectedClient.ClientInfoCollection.Sum(i => i.HourCount * a.SelectedClient.OneHourePrice);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            FromDate.Text = "";
            ByDate.Text = "";
        }
    }
}
