using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryDataStructure {
    public class Codes {
        public static string MANIFEST_PRESENCE_CODE = "IS_THERE_SOMEBODY_OUT_THERE";
        public static string ACKNOWLEDGE_PRESENCE_CODE = "I_AM_STILL_HERE";
        public static string EXTINCTION_CODE = "SHUTTING_DOWN";
        public static string BAD_PARAMETERS_CODE = "BAD_PARAMETERS_CODE";
        public static string UNKOWN_OPERATION = "UNKNOWN_OPERATION"; //normalement, n'apparaît jamais (mais c'est pris en charge au cas où)

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
    }
    public class ServiceInfos {
        public ServiceInfos(Dictionary<string, List<Type>> operation, string service) {
            this.Operation = operation;
            this.Service = service;
        }

        /**
         * le dictionnaire fait un mapping entre le nom d'une opération et la liste des types pour cette opération;
         * la dernière entrée de Type étant la valeur de retour
         */

        public Dictionary<string, List<Type>> Operation { get; set; }

        public string Service { get; set; }

        public static bool operator ==(ServiceInfos si1, ServiceInfos si2) {
            return si1.Service == si2.Service;
        }

        public static bool operator !=(ServiceInfos si1, ServiceInfos si2) {
            return si1.Service != si2.Service;
        }

        public override int GetHashCode(){
        return this.GetHashCode();
        }

        public override bool Equals(object o) {
            return this.Equals(o);
        }
    }
}
