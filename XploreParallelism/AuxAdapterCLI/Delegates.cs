using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static AuxParallel.FaceRecognition;

namespace AuxParallel {
    public class Delegates {
        public delegate string Transfer(string code);

        public void Main(string[] args) {
            SomeMethod("US");
            Transfer transfer = new Transfer(SomeMethod);
            string callback = transfer.Invoke("IN");
            Console.WriteLine($"Boom!: {callback} in action.");
            FaceRecognition fr = new FaceRecognition();
            fr.Search(Printing);
        }
        public static void Printing(int i) {

            Console.WriteLine($"Currently got...{i}");
            if (i == 5) {
                Console.Write($"Got the culprit. Filing a case against {i}.");
            }
        }

        public static string SomeMethod(string code) {
            Console.WriteLine("something");
            if (code == "US")
                return "CIA";
            if (code == "IN")
                return "RAW";
            else {
                return "Alliens";
            }
        }
    }

    public class FaceRecognition {
        public delegate void Intimate(int i);
        public void Search(Intimate intimate) {
            for (int i = 1; i <= 10; i++) {
                intimate(i);
                Console.WriteLine($"Searched...{i}");
            }
        }
    }
}
