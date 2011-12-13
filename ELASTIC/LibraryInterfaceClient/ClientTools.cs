using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketLibrary;
using LibraryDataStructure;

namespace LibraryInterfaceClient {
    public class Utils {
        public static IClientTools createClient(string ip, int port) {
            return new ClientTools(ip, port);
        }
        public static bool IsIPAddressCorrect(string ip) {
            return Codes.IsIPAddressCorrect(ip);
        }
        public static string decodeString(byte[] message) {
            return System.Text.Encoding.UTF8.GetString(message);
        }

        public static int decodeInt(byte[] message) {
            return System.BitConverter.ToInt32(message, 0);
        }

        public static object decode(byte[] message, Type type) {
            if (type == typeof(string))
                return decodeString(message);
            if (type == typeof(int))
                return decodeInt(message);
            return null;
        }
    }

    public interface IClientTools {
        bool Connect();
        void Subscribe(Action<object, ClientToolsEvent> listener);
        void Unsubscribe();
        void Send(string service, string operation, string stamp, byte[][] param);
        void Disconnect();
    }

    public class ClientTools : IClientTools {
        private delegate void ClientToolsReceiveEventHandle(Object sender, ClientToolsEvent e);
        private event ClientToolsReceiveEventHandle clientToolsReceiveEvent;
        private ClientToolsReceiveEventHandle receive;
        private IClient mySelf;
        private int port;
        private string ip;

        internal ClientTools(string ip, int port) {
            this.ip = ip;
            this.port = port;
        }

        

        public bool Connect() {
            this.mySelf = SocketLibrary.Utils.CreateClient(ip, port);
            bool b = this.mySelf.Connect();
            if (!b)
                return false;

            this.mySelf.Subscribe(HandleClientEvents);
            return true;
        }

        public void Subscribe(Action<object, ClientToolsEvent> listener) {
            receive = new ClientToolsReceiveEventHandle(listener);
            clientToolsReceiveEvent += receive;
        }

        public void Unsubscribe() {
            clientToolsReceiveEvent -= receive;
        }

        public void Send(string service, string operation, string stamp, byte[][] param) {
            //encoder message
            //la méthode encode prend 4 string et une liste de paramètre
            byte[] msg = Encoding.Default.GetBytes("lol");
            byte[] count = new byte[4];
            count = BitConverter.GetBytes(msg.Length);
            byte[] msgsend = new byte[4 + msg.Length];
            count.CopyTo(msgsend, 0);
            msg.CopyTo(msgsend, 4);

            this.mySelf.Send(msgsend);
        }

        public void Disconnect() {
            this.mySelf.Unsubscribe();
            this.mySelf.Disconnect();
        }

        private void HandleClientEvents(object sender, ClientEventArgs e) {
            ClientToolsEvent sta = null;
            //décoder message
            //déterminer type de message:
            ClientToolsEvent.typeEvent type = ClientToolsEvent.typeEvent.MESSAGE;
            sta = new ClientToolsEvent(type, new List<byte[]>() { new byte[] { } }, "0");
            clientToolsReceiveEvent(this, sta);
        }
    }

    public class ClientToolsEvent : EventArgs {

        public enum typeEvent {
            EXTINCTION,
            ERROR,
            MESSAGE
        }

        public ClientToolsEvent(typeEvent type, List<byte[]> message, string stamp) {
            this.Type = type;
            this.Message = message;
            this.Stamp = stamp;
        }

        public string Stamp { get; set; }

        public List<byte[]> Message { get; set; }

        public typeEvent Type { get; set; }
    }
}
