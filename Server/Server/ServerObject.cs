using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ServerObject
    {
        static public TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>();
        ClientObject clientObject = null;
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void Listen()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8888);
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                clientObject = new ClientObject(tcpClient, this);
                Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                clientThread.Start();
            }
        }

        protected internal void Disconnect()
        {
            tcpListener.Stop();
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }

        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.ID == id);
            if (client != null)
            {
                clients.Remove(client);
            }
        }
    }
}
