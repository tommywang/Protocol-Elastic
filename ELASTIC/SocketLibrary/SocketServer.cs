using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace SocketLibrary {

    /// <summary>
    /// La classe Server de la couche Socket.
    /// Elle implémente l'interface IServer qui sera manipulé par les entités externes à la bibliothèque.
    /// </summary>
    /// <example> 
    /// Mise en oeuvre : 
    /// <code> 
    ///     IServer monServeur = SocketLibrary.utils.createServeur(50000);
    ///     if (!myServer.start()) 
    ///         erreur();
    ///     myServer.subscribe(listener);
    ///     myServer.send(id, message);
    ///     myServer.stop();
    ///     myServer.unsubscribe();
    /// </code> 
    /// </example>
    /// <seealso cref="SocketLibrary.IServer"/>
    internal class Server : IServer {

        private delegate void ServerSocketReceiveEventHandle(Object sender, ServerEventArgs e);
        private event ServerSocketReceiveEventHandle serverSocketReceiveEvent;
        private ServerSocketReceiveEventHandle receive;

        private TcpListener listener;
        private Thread listenerThread;
        private Dictionary<MySocket, Thread> clients;
        private int port;

        /// <summary>
        /// Le constructeur de la classe Server.
        /// </summary>
        /// <param name="port">Port sur lequel le serveur va démarrer.</param>
        internal Server(int port) {
            this.port = port;
            this.clients = new Dictionary<MySocket, Thread>();
        }

        public bool Start() {
            try {
                this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                //Processus léger d'écoute de connexions:
                listenerThread = new Thread(listenConnect);
                listenerThread.Start();
                //Tout s'est bien passé:
                return true;
            }
            catch {
                //Il y a eu un problème:
                return false;
            }
        }

        /// <summary>
        /// Méthode du processus léger écoutant les connexions des clients. A chaque nouveau client est associé un processus léger ainsi qu'une socket.
        /// </summary>
        private void listenConnect() {
            Socket clientSocket = null;
            MySocket clientMySocket = null;
            while (true) {
                try {
                    clientSocket = listener.AcceptSocket();
                    clientMySocket = new MySocket(clientSocket);
                    //Un client se connecte, démarrage un processus léger:
                    Thread clientThread = new Thread(receiveClientMsg);
                    clientThread.Start(clientMySocket);
                    
                    //Ajout du client dans la table de hachage:
                    clients.Add(clientMySocket, clientThread);
                    //Création et levée de l'évènement:
                    ServerEventArgs e = new ServerEventArgs(clientMySocket.Id, ServerEventArgs.typeEvent.CONNEXION, null);
                    serverSocketReceiveEvent(this, e);
                }
                catch {
                    break;
                }
            }
        }

        /// <summary>
        /// Méthode du processus léger écoutant les messages des clients. Le processus léger est bloquant sur le receive, et implique que la taille du message soit contenue dans les quatre premiers octets de celui-ci.
        /// Un event est levé à chaque réception de message.
        /// </summary>
        private void receiveClientMsg(Object o) {
            MySocket cs = (MySocket)o;
            while (true) {
                try {
                    //Les quatre premiers octets contiennent la taille du message:
                    byte[] count = new byte[4];
                    cs.Socket.Receive(count, SocketFlags.None);
                    //La taille du message est maintenant connue:
                    byte[] msg = new byte[BitConverter.ToInt32(count, 0)];
                    cs.Socket.Receive(msg, msg.Length, SocketFlags.None);
                    //Création et levée de l'évènement:
                    ServerEventArgs e = new ServerEventArgs(cs.Id, ServerEventArgs.typeEvent.MESSAGE, msg);
                    serverSocketReceiveEvent(this, e);

                }
                catch {
                    if (cs.Socket != null) {
                        //Erreur durant la réception du message, le client est déconnecté:
                        cs.Socket.Close();
                    }
                    if (serverSocketReceiveEvent != null) {
                        ServerEventArgs e = new ServerEventArgs(cs.Id, ServerEventArgs.typeEvent.DECONNEXION, null);
                        serverSocketReceiveEvent(this, e);
                        MySocket.addFreeId(cs.Id, getBiggestId());
                        Thread thisThread = clients[cs];
                        clients.Remove(cs);
                        thisThread.Abort();
                    }
                    break;
                }

            }
        }

        public void Stop() {
            //Arrêt du TCPListener:
            listener.Stop();
            //Arrêt du processus léger d'écoute de connexions:
            if (listenerThread != null)
                listenerThread.Abort();
            //On arrête tous les clients:
            if (clients.Count != 0) {
                foreach (MySocket cs in clients.Keys) {
                    if (cs != null) {
                        cs.Socket.Close();
                        clients[cs].Abort();
                    }
                }
            }
        }

        public bool Send(int id, byte[] msg) {
            try {
                foreach (MySocket ms in clients.Keys) {
                    if (ms.Id == id) {
                        ms.Socket.Send(msg, SocketFlags.None);
                        return true;
                    }
                }
                return false;
            }
            catch {
                ServerEventArgs e = new ServerEventArgs(id, ServerEventArgs.typeEvent.DECONNEXION, null);
                serverSocketReceiveEvent(this, e);
                MySocket.addFreeId(id, getBiggestId());
                return false;
            }
        }

        public void Broadcast(byte[] msg) {
            foreach (MySocket ms in clients.Keys) {
                try {
                    ms.Socket.Send(msg, SocketFlags.None);
                }
                catch {
                }
            }
        }

        public void Subscribe(Action<object, ServerEventArgs> listener) {
            receive = new ServerSocketReceiveEventHandle(listener);
            serverSocketReceiveEvent += receive;
        }

        public void Unsubscribe() {
            serverSocketReceiveEvent -= receive;
        }

        private int getBiggestId() {
            int max = -1; 
            foreach (MySocket ms in clients.Keys) {
                if (ms.Socket != null) {
                    if (max < ms.Id)
                        max = ms.Id;
                }
            }
            return max;
        }
    }

    /// <summary>
    /// La classe ServerEventArgs.
    /// Ce sera le type de l'évènement levé à chaque réception de message.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class ServerEventArgs : EventArgs {
        /// <summary>
        /// L'énumération définissant les différents type d'évènements lancés par le serveur.
        /// </summary>
        public enum typeEvent {
            /// <summary>
            /// Type d'un évènement lancé lors d'une connexion.
            /// </summary>
            CONNEXION,
            /// <summary>
            /// Type d'un évènement lancé lorsque le serveur réalise qu'un client s'est déconnecté.
            /// </summary>
            DECONNEXION,
            /// <summary>
            /// Type d'un évènement lancé lors de la réception d'un message.
            /// </summary>
            MESSAGE
        }

        /// <summary>
        /// Le constructeur de l'évènement.
        /// </summary>
        /// <param name="id">Identifiant du client qui a envoyé le message.</param>
        /// <param name="type">Le type de l'évènement.</param> 
        /// <param name="msg">Le message sous la forme d'un tableau d'octet.</param>
        public ServerEventArgs(int id, typeEvent type, byte[] msg) {
            this.Type = type;
            this.Id = id;
            this.Msg = msg;
        }

        /// <summary>
        /// Propriété d'accès à la Socket liée à l'évènement.
        /// </summary>
        /// <returns>L'id du client relatif à l'évènement.</returns>
        public int Id { get; set; }

        /// <summary>
        /// Propriété d'accès au message lié à l'évènement.
        /// </summary>
        /// <returns>Le message reçu.</returns>
        public byte[] Msg { get; set; }

        /// <summary>
        /// Propriété d'accès au type de l'évènement.
        /// </summary>
        /// <returns>Le type de l'évènement.</returns>
        public typeEvent Type { get; set; }
    }

    internal class MySocket {
        private static int globalId = 0;
        private static Stack<int> freeIds = new Stack<int>();
        
        public MySocket(Socket s) {
            this.Socket = s;
            if (freeIds.Count > 0) {
                this.Id = freeIds.Pop();
            }
            else
                this.Id = globalId++;
        }

        public Socket Socket { get; set; }
        public int Id { get; set; }

        public static void addFreeId(int freeId, int biggestId) {
            freeIds.Push(freeId);
            globalId = biggestId + 1;
        }
    }
}
