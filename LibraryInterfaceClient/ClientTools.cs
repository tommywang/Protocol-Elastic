using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketLibrary;
using LibraryDataStructure;
using MessageLibrary;

namespace LibraryInterfaceClient {
    public class Utils {
        public static IClientTools createClient(string ip, int port) {
            return new ClientTools(ip, port);
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
        bool Send(string address, string service, string operation, string stamp, List<string> param);
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

        public bool Send(string address, string service, string operation, string stamp, List<string> param) {
            Message msg = new Message(address, service, operation, stamp, param);
            return mySelf.Send(MessageUtil.encoder(msg));
        }

        public void Disconnect() {
            this.mySelf.Unsubscribe();
            this.mySelf.Disconnect();
        }

        /// <summary>
        /// Intercepter un évenement, puis créer un ServerToolsEvent correspondant à son type. 
        /// </summary>
        private void HandleClientEvents(object sender, ClientEventArgs e) {
            ClientToolsEvent sta = null;
            Message msg = MessageUtil.decoder(e.Msg);
            ClientToolsEvent.typeEvent type = ClientToolsEvent.typeEvent.MESSAGE;
            if (msg.ListParams.Count > 0) {
                if (DataUtils.isErrorMessage(msg.ListParams[0]))
                    type = ClientToolsEvent.typeEvent.ERROR;
                else if (msg.ListParams[0] == DataUtils.EXTINCTION_CODE)
                    type = ClientToolsEvent.typeEvent.EXTINCTION;
            }
            sta = new ClientToolsEvent(type, msg.ListParams, msg.Stamp);
            clientToolsReceiveEvent(this, sta);
        }
    }

    /// <summary>
    /// Permet de stocker les évenements interceptés pour les utiliser facilement.
    /// </summary>
    public class ClientToolsEvent : EventArgs {

        public enum typeEvent {
            EXTINCTION,
            ERROR,
            MESSAGE
        }

        public ClientToolsEvent(typeEvent type, List<string> message, string stamp) {
            this.Type = type;
            this.Message = message;
            this.Stamp = stamp;
        }

        public string Stamp { get; set; }

        public List<string> Message { get; set; }

        public typeEvent Type { get; set; }
    }
}
