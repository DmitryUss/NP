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
using System.Windows.Threading;
using System.ComponentModel;

namespace Exam_Sp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<DirectoryInfo> OCollection = new ObservableCollection<DirectoryInfo>();//создаём коллекцию
        string str;// для поиска

        public void ShowDisc()
        {
            //чтобы не конфлиутовали потоки
            this.Dispatcher.BeginInvoke(new Action(() =>
           {
               //смотрим в каком потоке выполняется
               MessageBox.Show("показываем диски: " + Thread.CurrentThread.ManagedThreadId.ToString());

               OCollection.Clear();//очищаем коллекцию
               DirectoryInfo dr = new DirectoryInfo(lbx.SelectedItem.ToString());//вычитываем данные из папки

               foreach (var d in dr.GetDirectories())
               {
                   OCollection.Add(d);
               }

               listView.ItemsSource = OCollection;
           }));
        }

        private BackgroundWorker backgroundWorker;//создаём 

        public MainWindow()
        {
            InitializeComponent();
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));// привязываем к форме xaml
        }

        // после загрузки узнаём какие диски есть
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            DriveInfo[] di = DriveInfo.GetDrives();//узнаём какие есть 
            foreach (DriveInfo d in di)
                lbx.Items.Add(d.Name);
        }

        // выбираем нужный диск
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Thread myThread = new Thread(new ThreadStart(ShowDisc));//запускаем новый поток
            myThread.Start();//стартуем поток
        }

        // выполняем поиск
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //смотрим в каком потоке выполняется
            MessageBox.Show("кнопка поиск нажата в: " + Dispatcher.CurrentDispatcher.Thread.ManagedThreadId.ToString());

            backgroundWorker.DoWork += BackgroundWorker_DoWork; //добовляем событие

            backgroundWorker.RunWorkerAsync(); //запускаем асинхронно
        }

        // событие поиска
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show("асинхронный поиск: " + Thread.CurrentThread.ManagedThreadId.ToString()); //в каком потоке выполняется
            Thread.Sleep(5000);

            MessageBox.Show("до бегин: " + Thread.CurrentThread.ManagedThreadId.ToString()); //в каком потоке выполняется

            MessageBox.Show("сам метод поиска: " + Thread.CurrentThread.ManagedThreadId.ToString()); //смотрим в каком потоке выполняется
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                //string str;
                str = myTextBox.Text;

                foreach (var item in OCollection)
                {
                    if (item.Name == str)
                    {
                        MessageBox.Show(item.Name);
                    }
                }
            }
            ));
        }

        public async void Test()
        {
            Thread.Sleep(3000);
            ObservableCollection<String> OCollection2 = new ObservableCollection<String>();
            await this.Dispatcher.BeginInvoke((Action)(() =>
             {
                //string str;
                str = myTextBox.Text;
                // OCollection.Clear();
                foreach (var item in OCollection)
                 {
                     if (item.Name == str)
                     {
                         OCollection2.Add(item.Name);
                     }
                 }
                 listView.ItemsSource = OCollection2;
             }
            ));
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Task task = new Task(Test);
            task.Start();
        }
    }
}
