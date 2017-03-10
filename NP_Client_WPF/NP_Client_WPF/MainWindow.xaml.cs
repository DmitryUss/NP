using System.Text;
using System.Windows;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using street;
using System.Collections.Generic;

namespace NP_Client_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 1024);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                s.Connect(ep);

                if (s.Connected)
                {
                    //string str = textBox.Text;
                    Streets street = new Streets();
                    street.index = int.Parse(textBox.Text);//вводим индекс для поиска
                    var JsonStreet = JsonConvert.SerializeObject(street);//серриолизуем в джейсон
                    s.Send(Encoding.ASCII.GetBytes(JsonStreet));//кодируем сообщение в байтовый массив
                    //s.Send(Encoding.UTF8.GetBytes(str));
                    byte[] buffer = new byte[1024];
                    int i;

                    do
                    {
                        i = s.Receive(buffer);//принимаем сообщение
                        string jasonRes = Encoding.ASCII.GetString(buffer);
                        List<Streets> res = JsonConvert.DeserializeObject<List<Streets>>(jasonRes);

                        foreach (var item in res)
                        {
                            listView.Items.Add(item.name);
                        }
                        

                        i = 0;// иначе выводит результат 2 раза
                    } while (i > 0);
                }
                else
                    MessageBox.Show("Error");
            }
            catch(SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
            }

            
            }

    }
}
