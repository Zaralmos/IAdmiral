using System;

namespace SimpleServer
{
    /// <summary>
    /// Представляет класс, содержащий данные события, связанного с клиентом.
    /// </summary>
    public class ClientEventArgs : EventArgs
    {
        /// <summary>
        /// Создаёт экземпляр класса, содержащего данные события, связанного с клиентом.
        /// </summary>
        /// <param name="client">Клиент.</param>
        public ClientEventArgs(Client client)
        {
            Client = client;
        }

        /// <summary>
        /// Клиент.
        /// </summary>
        public Client Client { get; }
    }
}