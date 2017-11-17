using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleServer
{
    public class Server
    {
        /// <summary>
        /// Событие присоединения к серверу нового клиента.
        /// </summary>
        public event EventHandler<ClientEventArgs> ClientConnected;

        /// <summary>
        /// Событие отсоединения от сервера одного из клиентов.
        /// </summary>
        public event EventHandler<ClientEventArgs> ClientDisconnected;

        /// <summary>
        /// Событие получения сообщения от одного из клиентов.
        /// </summary>
        public event EventHandler<ClientMessageEventArgs> MessageGot;

        /// <summary>
        /// Слушатель порта.
        /// </summary>
        private TcpListener Listener { get; set; }

        /// <summary>
        /// Список присоединённых к серверу клиентов.
        /// </summary>
        private List<Client> ClientsList { get; } = new List<Client>();

        /// <summary>
        /// Неизменяемый список клиентов, присоединённых к серверу.
        /// </summary>
        public ReadOnlyCollection<Client> Clients => ClientsList.AsReadOnly();

        /// <summary>
        /// Количество клиентов, присоединённых к серверу.
        /// </summary>
        public int ClientsCount => ClientsList.Count();

        /// <summary>
        /// Предельное количество клиентов, которые одновременно могут быть присоединены к серверу.
        /// </summary>
        public int MaxClients { get; set; } = 10;

        /// <summary>
        /// Запускает сервер, который начинает слушать порт и принимать клиентов.
        /// </summary>
        /// <param name="port">Порт, на котором должен быть запущен сервер.</param>
        public bool Start(int port)
        {
            bool result = true;
            try
            {

                Listener = new TcpListener(IPAddress.Any, port);
                IsListening = true;
                new Thread(() => Listen()).Start();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Пока 
        /// </summary>
        private void Listen()
        {
            Listener.Start();
            try
            {
                while (IsListening)

                {
                    // Программа приостанавливается, ожидая входящее соединение
                    TcpClient acceptedClient = Listener.AcceptTcpClient();
                    if (ClientsCount < MaxClients)
                        AddClient(acceptedClient);


                }
            }
            catch
            {
            }
        }

        private void AddClient(TcpClient acceptedClient)
        {
            Client client = new Client(acceptedClient);
            client.MessageGot += Client_MessageGot;
            ClientsList.Add(client);
            client.ConnectionLost += Client_ConnectionLost;
            ClientConnected?.Invoke(this, new ClientEventArgs(client));
        }

        private void Client_MessageGot(object sender, MessageEventArgs e)
        {
            MessageGot(this, new ClientMessageEventArgs(sender as Client, e.Message));
        }

        private void Client_ConnectionLost(object sender, EventArgs e)
        {
            ClientsList.Remove(sender as Client);
            ClientDisconnected?.Invoke(this, new ClientEventArgs(sender as Client));
        }

        public void Stop()
        {
            IsListening = false; // это приводит к остановке цикла в Listen в другом потоке
            while (ClientsCount > 0)
            {
                ClientsList[0].BreakConnection(); // а тут мы отключаем всех ещё подключённых клиентов
            }
            Listener?.Stop();
        }

        public bool IsListening { get; private set; }

        public int Send(string message)
        {
            int count = 0;
            foreach (Client client in ClientsList)
                if (client.Send(message))
                    count++;
            return count;
        }
    }
}
