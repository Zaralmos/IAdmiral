namespace SimpleServer
{
    /// <summary>
    /// Представляет класс, содержащий данные события, связанного с сообщениями клиента.
    /// </summary>
    public class ClientMessageEventArgs
    {
        /// <summary>
        /// Создаёт экземпляр класса, содержащего данные о событии с сообщениями клиента.
        /// </summary>
        /// <param name="client">Клиент, связанный с сообщением.</param>
        /// <param name="message">Текст сообщения.</param>
        public ClientMessageEventArgs(Client client, string message)
        {
            Client = client;
            Message = message;
        }

        /// <summary>
        /// Клиент, связанный с сообщением.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Message { get; set; }        
    }
}