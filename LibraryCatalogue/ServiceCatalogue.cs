using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LibraryDataStructure;
using LibraryInterfaceServer;
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
        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "register", "IRegister")]
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

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IRegister", null)]
        public static List<string> IRegister(List<string> param) {
            Dictionary<string, List<Type>> dico = new Dictionary<string, List<Type>>();

            string title = param[1];

            List<string> operationNames = new List<string>();
            List<List<Type>> typeForOperations = new List<List<Type>>();

            string[] operations = title.Split('_');

            int noOperation = 0;
            foreach (string operation in operations) {
                string operationName = "";
                int charAt = 0;
                do {
                    operationName += operation[charAt++];
                }
                while (operation[charAt] != '(');
                charAt++;
                operationNames.Add(operationName);

                typeForOperations.Add(new List<Type>());
                do {
                    string typeParamIn = "";
                    do {
                        typeParamIn += operation[charAt];
                        charAt++;
                        if (operation[charAt] == ')')
                            break;
                    }
                    while (operation[charAt] != ',');

                    typeForOperations[noOperation].Add(DataUtils.GetType(typeParamIn));
                }
                while (operation[charAt] != ')');
                charAt++;
                //on est sur le type de retour
                string typeRetour = "";
                do {
                    typeRetour += operation[charAt++];
                }
                while (charAt < operation.Length);
                typeForOperations[noOperation].Add(DataUtils.GetType(typeRetour));
                noOperation++;
            }

            for (int i = 0; i < operationNames.Count; i++) {
                dico.Add(operationNames[i], typeForOperations[i]);
            }
            string service = param[0];
            string address = param[2];
            string port = param[3];
            ServiceInfos si = new ServiceInfos(dico, service);
            
            Register(si, address, int.Parse(port), int.Parse(param[4]));
            return null;
        }

        /// <summary>
        /// Unregister a computer
        /// </summary>
        /// <param name="name">The name of the computer.</param>
        /// <returns>True if everything went OK, False otherwise</returns>
        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "unregister", "IUnregister")]
        public static bool Unregister(string si)
        {
            foreach (Tuple<Tuple<ServiceInfos, IPEndPoint>, int> tuple in subscribers) {
                if (tuple.Item1.Item1.Service == si)
                    return subscribers.Remove(tuple);
            }
            return false;
        }

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IUnregister", null)]
        public static List<string> IUnregister(List<string> param) {
            Unregister(param[0]);
            return null;
        }

        /// <summary>
        /// Return the IP address and the port number of a registered computer by his name
        /// </summary>
        /// <param name="name">The name of the computer.</param>
        /// <returns>The ip address and the port number of the computer</returns>
        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "getinfos", "IGetInfos")]
        public static List<Tuple<Tuple<ServiceInfos, IPEndPoint>, int>> GetInfos(string si)
        {
            if (si == "")
                return GetAllInfos();
            foreach (Tuple<Tuple<ServiceInfos, IPEndPoint>, int> tuple in subscribers) {
                if (tuple.Item1.Item1.Service == si)
                    return new List<Tuple<Tuple<ServiceInfos, IPEndPoint>, int>>() {tuple};
            }
            return new List<Tuple<Tuple<ServiceInfos, IPEndPoint>, int>>() { };
        }

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IGetInfos", null)]
        public static List<string> IGetInfos(List<string> param_) {
            List<Tuple<Tuple<ServiceInfos, IPEndPoint>, int>> infos = ServiceCatalogue.GetInfos(param_[0]);
            
            List<String> param = new List<String>();
            foreach (Tuple<Tuple<ServiceInfos, IPEndPoint>, int> t in infos) {
                param.Add(t.Item1.Item1.Service);

                Dictionary<string, List<Type>> operations = t.Item1.Item1.Operation;
                string title = "";
                foreach (string s in operations.Keys) {
                    title += s + "(";
                    for (int i = 0; i < operations[s].Count - 1; i++) {
                        title += operations[s][i].ToString();
                        title += ",";
                    }
                    title = title.Remove(title.Length - 1);
                    title += ")";
                    title += operations[s].Last();
                    title += "_";
                }
                title = title.Remove(title.Length - 1);
                param.Add(title);
                param.Add(t.Item1.Item2.Address.ToString());
                param.Add(t.Item1.Item2.Port.ToString());
            }
            return param;
        }

        /// <summary>
        /// Return the IP address and the port number of all registered computers
        /// </summary>
        /// <returns>A list with the ip address and port numbers of all computers</returns>
        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "getallinfos", null)]
        public static List<Tuple<Tuple<ServiceInfos, IPEndPoint>,int>> GetAllInfos()
        {
            return subscribers;
        }
    }
}
