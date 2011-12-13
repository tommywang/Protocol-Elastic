using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryMathematique
{
    public class ServiceMathematique //service général
    {
        public static int DoAddition(int a, int b)
        {
            return a + b;
        }

        public static int DoSoustraction(int a, int b)
        {
            return a - b;
        }

        public static int DoMultiplication(int a, int b)
        {
            return a * b;
        }

        public static int DoDivision(int a, int b) {
            return a / b;
        }
    }
}
