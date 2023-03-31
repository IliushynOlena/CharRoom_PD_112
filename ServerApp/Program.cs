using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    class ChatServer
    {
        const short port = 4040;
        const string address = "127.0.0.1";
       
        TcpListener listener = null;
        public ChatServer()
        {
            listener = new TcpListener(IPAddress.Parse(address), port);
        }
        public void Start()
        { 
            listener.Start();
            Console.WriteLine("Waiting for connection ........");
            TcpClient client =  listener.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream ns = client.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            while (true)
            {

                string message = sr.ReadLine();
                if (message == "exit") { listener.Stop();break; }
                Console.WriteLine($" {message} at {DateTime.Now.ToShortTimeString()} " +
                    $"from {client.Client.LocalEndPoint}");
                sw.WriteLine("Thanks!!!!");
                sw.Flush(); 
             
            }
           
        }
    }
    internal class Program
    {
       
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();
            try
            {
                server.Start();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
           

           
           
        }

    }
}
