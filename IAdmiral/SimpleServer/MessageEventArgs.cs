using System;

namespace SimpleServer
{
    /// <summary>
    /// Представляет класс, содержащий данные событий, связанных с сообщениями.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Создаёи экземпляр класса данных события.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public MessageEventArgs(string message)
        {
            Message = message;
        }   

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Message { get; }
    }
}
