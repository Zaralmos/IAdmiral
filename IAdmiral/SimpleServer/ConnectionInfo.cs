using System.Net;

namespace SimpleServer
{
    /// <summary>
    /// Представляет класс, содержащий информацию об удалённом узле.
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>
        /// Создаёт экземпляр класса, содержащего информацию об удалённом узле.
        /// </summary>
        /// <param name="endPoint">Сетевой адрес удалённого узла.</param>
        public ConnectionInfo(EndPoint endPoint)
        {
            RemoteEndPoint = endPoint;
        }

        /// <summary>
        /// Сетевой адрес удалённого узла.
        /// </summary>
        public EndPoint RemoteEndPoint { get; }
    }
}