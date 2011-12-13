using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LibraryDataStructure;
//TODO:revoir la documentation pour ce fichier
namespace LibraryCatalogue
{
    public class ServiceCatalogue
    {
        /**
         * Il s'agit d'une liste de paire composées de: 
         *  - une paire qui relie une ServiceInfos à un IPEndPoint
         *  - un id pour retrouver le client correspondant
         */
        private static List<Tuple<Tuple<ServiceInfos, IPEndPoint>, int>> subscribers =
            new List<Tuple<Tuple<ServiceInfos, IPEndPoint>, int>>();
        
        /// <summary>
        /// Register a computer
        /// </summary>
        /// <param name="serverInfos">The information of the computer.</param>
        /// <param name="ip">The IP address of the computer.</param>
        /// <param name="port">The port number.</param>
        /// <returns>True if success or False if fail</returns>
        public static bool Register(ServiceInfos serverInfos, string ip, int port, int id)
        {
            try 
            {
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip),port);
                subscribers.Add(new Tuple<Tuple<ServiceInfos, IPEndPoint>, int> (new Tuple<ServiceInfos, IPEndPoint>(serverInfos, ipEnd), id));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Unregister a computer
        /// </summary>
        /// <param name="name">The name of the computer.</param>
        /// <returns>True if everything went OK, False otherwise</returns>
        public static bool Unregister(string si)
        {
            foreach (Tuple<Tuple<ServiceInfos, IPEndPoint>, int> tuple in subscribers) {
                if (tuple.Item1.Item1.Service == si)
                    return subscribers.Remove(tuple);
            }
            return false;
        }

        /// <summary>
        /// Return the IP address and the port number of a registered computer by his name
        /// </summary>
        /// <param name="name">The name of the computer.</param>
        /// <returns>The ip address and the port number of the computer</returns>
        public static Tuple<Tuple<ServiceInfos, IPEndPoint>, int> GetInfos(string si)
        {
            foreach (Tuple<Tuple<ServiceInfos, IPEndPoint>, int> tuple in subscribers) {
                if (tuple.Item1.Item1.Service == si)
                    return tuple;
            }
            return null;
        }

        /// <summary>
        /// Return the IP address and the port number of all registered computers
        /// </summary>
        /// <returns>A list with the ip address and port numbers of all computers</returns>
        public static List<Tuple<Tuple<ServiceInfos, IPEndPoint>,int>> GetAllInfos()
        {
            return subscribers;
        }
    }
}
