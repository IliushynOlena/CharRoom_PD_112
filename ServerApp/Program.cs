using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    class ChatServer
    {
        const short port = 4041;
        const string JOIN_CMD = "$<join>";
        HashSet<IPEndPoint> members = new HashSet<IPEndPoint>();
        UdpClient server = new UdpClient(port);
        IPEndPoint clientIpEndPoint = null;
        private void AddMember(IPEndPoint member)
        {
            members.Add(member);
            Console.WriteLine("Member was added!!!");
        }
        private void SendToAll(byte[]data)
        {
            foreach (IPEndPoint member in members)
            {
                server.SendAsync(data, data.Length, member);
            }
        }
        public void Start()
        {
            while (true)
            {
                byte[] data = server.Receive(ref clientIpEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($" {message} at {DateTime.Now.ToShortTimeString()} " +
                    $"from {clientIpEndPoint}");
                switch (message)
                {
                    case JOIN_CMD:
                       AddMember(clientIpEndPoint); 
                        break;
                    default:
                        SendToAll(data);
                        break;
                }
            }
        }
    }
    internal class Program
    {
       
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();
            server.Start();

           
           
        }

    }
}
