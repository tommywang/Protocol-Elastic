using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace SocketLibrary {
    /// <summary>
    /// La classe utils.
    /// Permet l'interfaçage de la couche Socket avec les assembly externes à la bibliothèque.
    /// </summary>
    public class Utils {

        /// <summary>
        /// Crée un client.
        /// </summary>
        /// <param name="ip">Adresse IP sur laquelle le client doit se connecter.</param>
        /// <param name="port">Port sur lequel le client doit se connecter.</param>
        /// <returns>Le client qui pourra être manipulé.</returns>
        public static IClient CreateClient(string ip, int port) {
            return new Client(ip, port);
        }

        /// <summary>
        /// Crée un serveur.
        /// </summary>
        /// <param name="port">Port sur lequel le serveur va opérer.</param>
        /// <returns>Le serveur qui pourra être manipulé.</returns>
        public static IServer CreateServer(int port) {
            return new Server(port);
        }
    }

    /// <summary>
    /// L'interface IClient.
    /// Tout client de la couche Socket doit implémenter cette interface.
    /// </summary>
    public interface IClient {

        /// <summary>
        /// Connecte le client au serveur.
        /// </summary>
        bool Connect();

        /// <summary>
        /// Envoie un message au serveur sur lequel le client est connecté.
        /// </summary>
        /// <param name="msg">Tableau d'octet qui va être envoyé.</param>
        void Send(byte[] msg);

        /// <summary>
        /// Déconnecte le client du serveur.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Méthode d'abonnement évènementielle.
        /// </summary>
        /// <param name="listener">Procédure qui sera appelée lors de la levée de l'évènement.</param>
        void Subscribe(Action<object, ClientEventArgs> listener);

        /// <summary>
        /// Méthode de désabonnement évènementielle.
        /// </summary>
        void Unsubscribe();
    }

    /// <summary>
    /// L'interface IServer.
    /// Tout serveur de la couche Socket doit implémenter cette interface.
    /// </summary>
    public interface IServer {

        /// <summary>
        /// Démarre le serveur et le met également sur écoute TCP.
        /// </summary>
        /// <returns>True si tout s'est bien passé, False sinon.</returns>
        bool Start();

        /// <summary>
        /// Arrête le serveur. Tous les processus légers sont interrompus, et la liste des clients est vidée.
        /// </summary>
        void Stop();

        /// <summary>
        /// Envoie un message à un client donné.
        /// </summary>
        /// <param name="id">Id du client auquel il faut envoyer le message</param>
        /// <param name="msg">Tableau d'octet qui va être envoyé.</param>
        /// <return>True si le message a été envoyé, false si le client est déconnecté ou n'existe pas.</return>
        bool Send(int id, byte[] msg);

        /// <summary>
        /// Envoie un message à tous les clients.
        /// </summary>
        /// <param name="msg">Tableau d'octet qui va être envoyé.</param>
        void Broadcast(byte[] msg);

        /// <summary>
        /// Méthode d'abonnement évènementielle.
        /// </summary>
        /// <param name="listener">Procédure qui sera appelée lors de la levée de l'évènement.</param>
        void Subscribe(Action<object, ServerEventArgs> listener);

        /// <summary>
        /// Méthode de désabonnement évènementielle.
        /// </summary>
        void Unsubscribe();
    }
}


