using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketLibrary;
using LibraryDataStructure;

namespace LibraryInterfaceServer {
    public class Utils {
        public static IServerTools createServer(int port) {
            return new ServerTools(port);
        }

        public static bool IsIPAddressCorrect(string ip) {
            return Codes.IsIPAddressCorrect(ip);
        }
    }

    public interface IServerTools {
        void Start();
        void Stop();
        void Subscribe(Action<object, ServerToolsEvent> listener);
        void Unsubscribe();
        void Send(string msg, int id);
        void Broadcast(string msg);
    }

    public class ServerTools : IServerTools {
        private delegate void ServerToolsReceiveEventHandle(Object sender, ServerToolsEvent e);
        private event ServerToolsReceiveEventHandle serverToolsReceiveEvent;
        private ServerToolsReceiveEventHandle receive;
        private IServer mySelf;
        private int port;

        public ServerTools(int port) {
            this.port = port;
        }

        public void Start() {
            this.mySelf = SocketLibrary.Utils.CreateServer(port);
            ServerToolsEvent sta = null;
            if (!this.mySelf.Start())
                sta = new ServerToolsEvent(-1, ServerToolsEvent.typeEvent.INFORMATION, null, null);
            else
                sta = new ServerToolsEvent(0, ServerToolsEvent.typeEvent.INFORMATION, null, null);
            serverToolsReceiveEvent(this, sta);
            this.mySelf.Subscribe(HandleServerEvent);
        }

        public void Stop() {
            this.mySelf.Unsubscribe();
            this.mySelf.Stop();
            this.mySelf = null;
        }

        public void Subscribe(Action<object, ServerToolsEvent> listener) {
            receive = new ServerToolsReceiveEventHandle(listener);
            serverToolsReceiveEvent += receive;
        }

        public void Unsubscribe() {
            serverToolsReceiveEvent -= receive;
        }

        public void Send(string msg, int id) {
            //encoder message
            byte[] message = null;
            mySelf.Send(id, message);
        }

        public void Broadcast(string msg) {
            //encoder message
            byte[] message = null;
            mySelf.Broadcast(message);
        }

        private void HandleServerEvent(object sender, ServerEventArgs e) {
            ServerToolsEvent sta = null;
            switch (e.Type) {
                case ServerEventArgs.typeEvent.CONNEXION:
                    sta = new ServerToolsEvent(e.Id, ServerToolsEvent.typeEvent.CONNEXION, null, null);
                    break;
                case ServerEventArgs.typeEvent.DECONNEXION:
                    sta = new ServerToolsEvent(e.Id, ServerToolsEvent.typeEvent.DECONNEXION, null, null);
                    break;
                case ServerEventArgs.typeEvent.MESSAGE:
                    //Décoder opération demandée
                    sta = new ServerToolsEvent(e.Id, ServerToolsEvent.typeEvent.MESSAGE, "operation", new List<byte[]>() { new byte[] { } });
                    break;
            }
            serverToolsReceiveEvent(this, sta);
        }
    }

    public class ServerToolsEvent : EventArgs {

        public enum typeEvent {
            CONNEXION,
            DECONNEXION,
            MESSAGE,
            INFORMATION
        }

        public ServerToolsEvent(int id, typeEvent type, string operation, List<byte[]> param) {
            this.Type = type;
            this.Id = id;
            this.Operation = operation;
            this.Param = param;
        }

        public int Id { get; set; }

        public List<byte[]> Param { get; set; }

        public string Operation { get; set; }

        public typeEvent Type { get; set; }
    }
}
