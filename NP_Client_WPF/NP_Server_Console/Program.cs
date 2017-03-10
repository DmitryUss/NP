using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using street;
using Newtonsoft.Json;
using System.Threading;

namespace NP_Server_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);//создаём новый сокет
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 1024);

            s.Bind(ep);//связываем сокет с ендпоинтом
            s.Listen(10);//слушаем

            try
            {
                while (true)
                {
                    Socket ns = s.Accept();//блокируем поток, ждём сообщения

                    byte[] mes = new byte[1024];

                    Streets streets = new Streets();
                    streets.AddStreet();
                    //streets.ShowStreets();
                    

                    Console.WriteLine(ns.RemoteEndPoint.ToString());
                    Console.WriteLine(DateTime.Now.ToString());
                    ns.Receive(mes);//метод получения сообщения
                   
                    string strMes = Encoding.UTF8.GetString(mes);//декодируем из массива байтов в cтроку серриализованную в джейсон
                    Streets ccc = JsonConvert.DeserializeObject<Streets>(strMes);//дессириализуем из джейсон в наш класс
                    Console.WriteLine(ccc.name);

                    //streets.Search(int.Parse(ccc.name));
                    string result = JsonConvert.SerializeObject(streets.Search(ccc.index));//выполняем поиск и сериализуем в джейсон
                    //Thread.Sleep(2000);
                    ns.Send(Encoding.ASCII.GetBytes(result));//отправляем результат
                    ns.Shutdown(SocketShutdown.Both);//блокируем передачу данных
                    ns.Close();//освобождаем ресурсы

                }
            }
            catch(SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
