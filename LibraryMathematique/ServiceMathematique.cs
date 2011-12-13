using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryInterfaceServer;

namespace LibraryMathematique {
    public class ServiceMathematique //service général
    {
        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "addition", "IDoAddition")]
        public static int DoAddition(int a, int b) {
            return a + b;
        }

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IDoAddition", null)]
        public static List<string> IDoAddition(List<string> param) {
            try {
                return new List<string>() { DoAddition(int.Parse(param[0]), int.Parse(param[1])).ToString() };
            }
            catch {
                return null;
            }
        }

        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "soustraction", "IDoSoustraction")]
        public static int DoSoustraction(int a, int b) {
            return a - b;
        }

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IDoSoustraction", null)]
        public static List<string> IDoSoustraction(List<string> param) {
            try {
                return new List<string>() { DoSoustraction(int.Parse(param[0]), int.Parse(param[1])).ToString() };
            }
            catch {
                return null;
            }
        }

        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "multiplication", "IDoMultiplication")]
        public static int DoMultiplication(int a, int b) {
            return a * b;
        }

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IDoMultiplication", null)]
        public static List<string> IDoMultiplication(List<string> param) {
            try {
                return new List<string>() { DoMultiplication(int.Parse(param[0]), int.Parse(param[1])).ToString() };
            }
            catch {
                return null;
            }
        }

        [OperationInfo(OperationInfo.VisibilityType.GLOBAL, "division", "IDoDivision")]
        public static int DoDivision(int a, int b) {
            return a / b;
        }

        [OperationInfo(OperationInfo.VisibilityType.PRIVATE, "IDoDivision", null)]
        public static List<string> IDoDivision(List<string> param) {
            try {
                return new List<string>() { DoDivision(int.Parse(param[0]), int.Parse(param[1])).ToString() };
            }
            catch {
                return null;
            }
        }
    }
}
