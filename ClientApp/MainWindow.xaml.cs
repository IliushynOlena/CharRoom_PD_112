using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();

        IPEndPoint serverEndPoint;
        UdpClient client;
        //const string serverAddress = "127.0.0.1";
        //const short serverPort = 4040;
        public MainWindow()
        {
            InitializeComponent();
            client = new UdpClient();
            string serverAddress = ConfigurationManager.AppSettings["ServerAddress"]!;
            short serverPort = short.Parse( ConfigurationManager.AppSettings["ServerPort"]!);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);  
            this.DataContext = messages;
        }
        async void Listen()
        {
            while (true)
            {
                var result = await client.ReceiveAsync();
                string message = Encoding.UTF8.GetString(result.Buffer);
                messages.Add(new MessageInfo(message));
            }           

        }

        private void SendBtnClick(object sender, RoutedEventArgs e)
        {           
            string message = msgTextBox.Text;
            SendMessage(message);
        }

        private void JoinBtnClick(object sender, RoutedEventArgs e)
        {
            string message = "$<join>";
            SendMessage(message);
            Listen();
        }
        private async void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }

    }
    class MessageInfo
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public MessageInfo(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }
        public override string ToString()
        {
            return $" {Message} : {Time}";
        }
    }
}
