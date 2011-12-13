using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryInterfaceServer;



namespace LibraryEcho
{
    /// <summary>
    /// Permet d'appeler la méthode d'une manière réflexive 
    /// </summary>
    public class ServiceEcho
    {
        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "echo", "IDoEcho")]
        public static string DoEcho(string a)
        {
            return a;
        }

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IDoEcho", null)]
        public static List<string> IDoEcho(List<string> param) {
            try {
                return new List<string>() { DoEcho(param[0]) };
            }
            catch {
                return null;
            }
        }
    }
}
