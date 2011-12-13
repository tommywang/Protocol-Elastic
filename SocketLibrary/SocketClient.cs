using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace SocketLibrary {

    /// <summary>
    /// La classe Client de la couche Socket.
    /// Elle implémente l'interface IClient qui sera manipulé par les entités externes à la bibliothèque.
    /// </summary>
    /// <example> 
    /// Mise en oeuvre : 
    /// <code> 
    ///     IClient monServeur = SocketLibrary.utils.createClient(127.0.0.1, 50000);
    ///     myClient.subscribe(clientEventFunc);
    ///     if (!myClient.connect()) 
    ///         erreur();
    ///     myClient.send(message);
    ///     myClient.disconnect();
    ///     myClient.unsubscribe();
    /// </code> 
    /// </example>
    /// <seealso cref="SocketLibrary.IClient"/>
    internal class Client : IClient {
        private delegate void ClientSocketReceiveEventHandle(Object sender, ClientEventArgs e);
        private event ClientSocketReceiveEventHandle clientSocketReceiveEvent;
        private ClientSocketReceiveEventHandle receive;

        private IPEndPoint ipEnd;
        private Socket socket;
        private Thread receiveThread;
        

        /// <summary>
        /// Le constructeur de la classe Client.
        /// </summary>
        /// <param name="ip">Adresse du serveur.</param>
        /// <param name="port">Port du serveur sur lequel le client va se connecter.</param>
        internal Client(string ip, int port) {
            this.ipEnd = new IPEndPoint(IPAddress.Parse(ip), port);
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool Connect() {
            try {
                this.socket.Connect(ipEnd);
                //Processus léger de réception de message:
                this.receiveThread = new Thread(receiveMessage);
                this.receiveThread.Start();
                //Tout s'est bien passé:
                return true;
            }
            catch {
                //Il y a eu un problème:
                return false;
            }
        }

        public bool Send(byte[] msg) {
            try {
                 socket.Send(msg, SocketFlags.None);
                 return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Méthode du processus léger écoutant les messages du serveur. Le processus léger est bloquant sur le receive, et implique que la taille du message soit contenue dans les quatre premiers octets de celui-ci.
        /// Un event est levé à chaque réception de message.
        /// </summary>
        private void receiveMessage() {
            while (true) {
                try {
                    //Les quatre premiers octets contiennent la taille du message:
                    byte[] count = new byte[4];
                    socket.Receive(count, count.Length, SocketFlags.None);
                    //La taille du message est maintenant connue:
                    byte[] msg = new byte[BitConverter.ToInt32(count, 0)];
                    socket.Receive(msg, msg.Length, SocketFlags.None);
                    //Création et levée de l'évènement:
                    ClientEventArgs e = new ClientEventArgs(msg);
                    clientSocketReceiveEvent(this, e);
                }
                catch {
                    //Erreur durant l'envoi du message
                    break;
                }
            }
        }

        public void Disconnect() {
            //Fermeture de la socket:
            if (socket != null)
                socket.Close();
            //Arrêt du processus léger d'écoute de message:
            if (receiveThread != null)
                receiveThread.Abort();
        }

        public void Subscribe(Action<object, ClientEventArgs> listener) {
            receive = new ClientSocketReceiveEventHandle(listener);
            clientSocketReceiveEvent += receive;
        }

        public void Unsubscribe() {
            clientSocketReceiveEvent -= receive;
        }
    }

    /// <summary>
    /// La classe ClientEventArgs.
    /// Ce sera le type de l'évènement levé à chaque réception de message.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class ClientEventArgs : EventArgs {

        /// <summary>
        /// Le constructeur de l'évènement.
        /// </summary>
        /// <param name="msg">Le message sous la forme d'un tableau d'octet.</param>
        public ClientEventArgs(byte[] msg) {
            this.Msg = msg;
        }

        /// <summary>
        /// Propriété d'accès au message lié à l'évènement.
        /// </summary>
        /// <returns>Le message reçu.</returns>
        public byte[] Msg { get; set; }
    }

}
