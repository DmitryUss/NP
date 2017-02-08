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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;

namespace Exam_Sp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<DirectoryInfo>  OCollection = new ObservableCollection<DirectoryInfo>();//создаём коллекцию
        string str;
        public void ShowDisc()
        {
            //чтобы не конфлиутовали потоки
             this.Dispatcher.BeginInvoke(new Action(() =>
            {
                OCollection.Clear();//очищаем коллекцию
            DirectoryInfo dr = new DirectoryInfo(lbx.SelectedItem.ToString());//вычитываем данные из папки

            foreach (var d in dr.GetDirectories())
            {
                OCollection.Add(d);
            }

            listView.ItemsSource = OCollection;
            }));
        }

        public void Search()
        {
            //чтобы не конфлиутовали потоки
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                //string str;
                str = myTextBox.Text;
               
            }));
        }
        public MainWindow()
        {
            InitializeComponent();
        }

      

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
           
            DriveInfo[] di = DriveInfo.GetDrives();//узнаём какие есть 
            foreach (DriveInfo d in di)
                lbx.Items.Add(d.Name);
        }

       

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Thread myThread = new Thread(new ThreadStart(ShowDisc));//запускаем новый поток
            myThread.Start();//стартуем поток
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Search();
            MessageBox.Show(str);
        }
    }
}
