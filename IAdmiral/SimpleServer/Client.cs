using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleServer
{
    public class Client
    {
        public event EventHandler<MessageEventArgs> MessageGot;

        public event EventHandler ConnectionLost;

        public Client(TcpClient client)
        {
            ClientObject = client;
            Info = new ConnectionInfo(ClientObject.Client.RemoteEndPoint);
            new Thread(() => Listen()).Start();
        }

        private TcpClient ClientObject { get; }

        private NetworkStream Stream => ClientObject.GetStream();

        public ConnectionInfo Info { get; private set; }

        private bool isConnected = false;
        public bool IsConnected
        {
            get => isConnected;
            private set
            {
                if (isConnected == true && value == false)
                    ConnectionLost?.Invoke(this, EventArgs.Empty);            
                isConnected = value;
            }
        }


        public bool Send(string message)
        {
            bool result = message != null;
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            if (result)
                try
                {
                    Stream.Write(bytes, 0, bytes.Length);
                }
                catch
                {
                    result = false;
                }
            return result;
        }        

        private void Listen()
        {
            try
            {
                IsConnected = true;
                while (IsConnected)
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    do
                    {
                        int bytes = 0;
                        bytes = Stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (IsConnected && Stream.DataAvailable);
                    if (builder.Length == 0)
                        IsConnected = false;
                    else
                        MessageGot(this, new MessageEventArgs(builder.ToString()));
                }
            }
            catch
            {
                IsConnected = false;
            }
        }

        public void BreakConnection()
        {
            ClientObject.Close();
            IsConnected = false;
        }

        static public Client Connect(string host, int port)
        {
            Client clientModel = null;
            try
            {
                TcpClient ServerObject = new TcpClient(host, port);
                clientModel = new Client(ServerObject);
            }
            catch
            {
            }
            return clientModel;
        }
    }
}
