using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryDataStructure {
    public class DataUtils {
        //message broadcasté aux machines enregistrées sur le catalogue pour vérifier leur présence (si pas de réponse, elles sont effacées du catalogue):
        public static string MANIFEST_PRESENCE_CODE = "IS_THERE_SOMEBODY_OUT_THERE";
        //message envoyé par les machines enregistrées sur le catalogue pour confirmer leur présence:
        public static string ACKNOWLEDGE_PRESENCE_CODE = "I_AM_STILL_HERE";
        //quand le serveur va s'éteindre:
        public static string EXTINCTION_CODE = "SHUTTING_DOWN";
        //quand les paramètres sont du mauvais type/indécodable:
        public static string BAD_PARAMETERS_CODE = "BAD_PARAMETERS_CODE";
        //normalement, n'apparaît jamais (mais c'est pris en charge au cas où), c'est quand le client demande une opération qui n'existe pas:
        public static string UNKOWN_OPERATION = "UNKNOWN_OPERATION"; 
        //quand un serveur s'est déjà enregistré et qu'il essaye de se ré-enregistrer
        public static string SERVER_ALREADY_REGISTERED = "SERVER_ALREADY_REGISTERED_ON_CATALOG"; 
        //quand on essaye d'accéder à un serveur qui n'est pas enregistré(en théorie n'est jamais utilisé)
        public static string SERVER_NOT_REGISTERED = "SERVER_NOT_REGISTERED";

        public static bool isErrorMessage(string msg) {
            if ((msg == BAD_PARAMETERS_CODE) ||
                (msg == UNKOWN_OPERATION) ||
                (msg == SERVER_ALREADY_REGISTERED) ||
                (msg == SERVER_NOT_REGISTERED))
                return true;
            return false;
        }

        /// <summary>
        /// Vérifie le format des adresses IP.
        /// </summary>
        /// <param name="adress">Adresse IP qui doit être vérifiée.</param>
        /// <returns>True si l'adresse est correctement formatée, False sinon.</returns>
        public static bool IsIPAddressCorrect(String adress) {
            String pattern = @"^(25[0-4]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-4]|2[0-4]\d|[0-1]?\d?\d)){3}$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            return regex.IsMatch(adress);
        }
            
        /// <summary>
        /// Parser les types en chaine de caractères
        /// </summary>
        /// <param name="adress"> le type en chaine de caractères</param>
        /// <returns> les déclarations de types </returns>
        public static Type GetType(string type) {
            if (type.ToLower().Contains("void"))
                return typeof(void);
            else if (type.ToLower().Contains("string"))
                return typeof(string);
            else if (type.ToLower().Contains("float"))
                return typeof(float);
            else if (type.ToLower().Contains("bool"))
                return typeof(bool);
            else if (type.ToLower().Contains("double"))
                return typeof(double);
            else if (type.ToLower().Contains("long"))
                return typeof(long);
            else if (type.ToLower().Contains("char"))
                return typeof(char);
            else
                return typeof(int);
        }

        public static string[] GetIPaddresses(string computername) {
            string[] saddr = null;
            System.Net.IPAddress[] addr = System.Net.Dns.GetHostEntry(computername).AddressList;
            if (addr.Length > 0) {
                saddr = new String[addr.Length];
                for (int i = 0; i < addr.Length; i++)
                    saddr[i] = addr[i].ToString();
            }
            return saddr;
        } 
    }


    public class ServiceInfos {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adress"> </param>
        /// <returns> </returns>
        public ServiceInfos(Dictionary<string, List<Type>> operation, string service) {
            this.Operation = operation;
            this.Service = service;
        }

        /**
         * le dictionnaire fait un mapping entre le nom d'une opération et la liste des types pour cette opération;
         * la dernière entrée de la liste de Type étant la valeur de retour
         */

        public Dictionary<string, List<Type>> Operation { get; set; }

        public string Service { get; set; }
    }
}
