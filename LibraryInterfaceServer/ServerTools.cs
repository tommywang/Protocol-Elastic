using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketLibrary;
using LibraryDataStructure;
using System.Reflection;
using MessageLibrary;


namespace LibraryInterfaceServer {

    /**/
    [AttributeUsage(AttributeTargets.Method)]
    public class OperationInfo : System.Attribute {
        public enum VisibilityType {
            GLOBAL,
            LOCAL,
            PRIVATE
        }
        public VisibilityType Visibility { get; set; }
        public string Title { get; set; }
        public string Func;

        public OperationInfo(VisibilityType visibility, string title, string function) {
            Visibility = visibility;
            Title = title;
            Func = function;
        }
    }

    public class Utils {
        public static IServerTools createServer(int port) {
            return new ServerTools(port);
        }

        /// <summary>
        /// Mettre la signature d'une fonction sous forme d'un string
        /// </summary>
        /// <param name="adress"> La signature d'une fonction sous forme de System.type</param>
        /// <returns> la siganture en string</returns>
        public static string getTitle(Type type) {
            string title = "";
            foreach (MethodInfo mi in type.GetMethods()) {
                if (mi.GetCustomAttributes(typeof(OperationInfo), false).Count() != 0) {
                    if (mi.GetCustomAttributes(typeof(OperationInfo), false)[0].GetType() == typeof(OperationInfo)) {
                        OperationInfo oi = (OperationInfo)mi.GetCustomAttributes(typeof(OperationInfo), false)[0];
                        if (oi.Visibility == OperationInfo.VisibilityType.GLOBAL) {
                            title += oi.Title + "(";
                            foreach (ParameterInfo pi in mi.GetParameters()) {
                                title += pi.ParameterType + ",";
                            }
                            title = title.Remove(title.Length - 1);
                            title += ')';
                            title += mi.ReturnType;
                            title = title += '_';
                        }
                    }
                }
            }
            title = title.Remove(title.Length - 1);
            return title;
        }

        /// <summary>
        /// Retourner un objet de type MethodInfo que l'on peut invoquer avec ses parametres
        /// </summary>
        public static MethodInfo getMethod(string name, Type type, OperationInfo.VisibilityType visibility) {
            foreach (MethodInfo mi in type.GetMethods()) {
                if (mi.GetCustomAttributes(typeof(OperationInfo), false).Count() != 0) {
                    if (mi.GetCustomAttributes(typeof(OperationInfo), false)[0].GetType() == typeof(OperationInfo)) {
                        OperationInfo oi = (OperationInfo)mi.GetCustomAttributes(typeof(OperationInfo), false)[0];
                        if (oi.Visibility == visibility) {
                            if (oi.Title == name)
                                return mi;
                        }
                    }
                }
            }
            throw new Exception();
        }
    }

    public interface IServerTools {
        bool Start();
        void Stop();
        void Subscribe(Action<object, ServerToolsEvent> listener);
        void Unsubscribe();
        void Send(string service, string address, string operation, string stamp, List<string> param, int id);
        void Broadcast(string msg);
        void handleRequest(ServerToolsEvent e, Type t, string name);
    }

    public class ServerTools : IServerTools {
        private delegate void ServerToolsReceiveEventHandle(Object sender, ServerToolsEvent e);
        private event ServerToolsReceiveEventHandle serverToolsReceiveEvent;
        private ServerToolsReceiveEventHandle receive;
        private IServer mySelf;
        private int port;

        /// <summary>
        /// Exécuter l'opération demandée, puis envoyer le résultat au client
        /// </summary>
        public void handleRequest(ServerToolsEvent e, Type type, string name) {
            try {
                MethodInfo mi = LibraryInterfaceServer.Utils.getMethod(e.Operation, type, OperationInfo.VisibilityType.GLOBAL);
                if (mi.GetCustomAttributes(typeof(OperationInfo), false).Count() != 0) {
                    if (mi.GetCustomAttributes(typeof(OperationInfo), false)[0].GetType() == typeof(OperationInfo)) {
                        OperationInfo oi = (OperationInfo)mi.GetCustomAttributes(typeof(OperationInfo), false)[0];
                        if (oi.Visibility == OperationInfo.VisibilityType.GLOBAL) {
                            try {
                                e.Param.Add(e.Id.ToString());
                                MethodInfo mib = LibraryInterfaceServer.Utils.getMethod(oi.Func, type, OperationInfo.VisibilityType.PRIVATE);
                                List<string> result = (List<string>)mib.Invoke(null, new object[] { e.Param });
                                if (result != null)
                                    Send(name, e.Source, e.Operation, e.Stamp.ToString(), result, e.Id);
                                return;
                            }
                            catch {
                                Send(name, e.Source, e.Operation, e.Stamp.ToString(), new List<string>() { DataUtils.BAD_PARAMETERS_CODE }, e.Id);
                                return;
                            }

                        }
                    }
                }

            }
            catch {
                Send(name, e.Source, e.Operation, e.Stamp.ToString(), new List<string>() { DataUtils.UNKOWN_OPERATION }, e.Id);
            }
        }

        public ServerTools(int port) {
            this.port = port;
        }

        /// <summary>
        /// Démarrer le serveur
        /// </summary>
        public bool Start() {
            this.mySelf = SocketLibrary.Utils.CreateServer(port);
            ServerToolsEvent sta = null;
            bool ret;
            if (!this.mySelf.Start()) {
                sta = new ServerToolsEvent(-1, ServerToolsEvent.typeEvent.INFORMATION, null, null, null, -1);
                ret = false;
            }
            else {
                sta = new ServerToolsEvent(0, ServerToolsEvent.typeEvent.INFORMATION, null, null, null, -1);
                ret = true;
            }
            serverToolsReceiveEvent(this, sta);
            this.mySelf.Subscribe(HandleServerEvent);
            return ret;
        }

        /// <summary>
        /// Stopper le serveur
        /// </summary>
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

        public void Send(string service, string address, string operation, string stamp, List<string> param, int id) {
            Message msg = new Message(service, address, operation, stamp, param);
            mySelf.Send(id, MessageUtil.encoder(msg));
        }

        public void Broadcast(string msg) {
            //encoder message
            byte[] message = null;
            mySelf.Broadcast(message);
        }

        /// <summary>
        /// Intercepter un évenement, puis créer un ServerToolsEvent correspondant à son type. 
        /// </summary>
        private void HandleServerEvent(object sender, ServerEventArgs e) {
            ServerToolsEvent sta = null;
            switch (e.Type) {
                case ServerEventArgs.typeEvent.CONNEXION:
                    sta = new ServerToolsEvent(e.Id, ServerToolsEvent.typeEvent.CONNEXION, null, null, null, -1);
                    break;
                case ServerEventArgs.typeEvent.DECONNEXION:
                    sta = new ServerToolsEvent(e.Id, ServerToolsEvent.typeEvent.DECONNEXION, null, null, null, -1);
                    break;
                case ServerEventArgs.typeEvent.MESSAGE:
                    Message msg = MessageUtil.decoder(e.Msg);
                    sta = new ServerToolsEvent(e.Id, ServerToolsEvent.typeEvent.MESSAGE, msg.Operation, msg.ListParams, msg.Source, int.Parse(msg.Stamp));
                    break;
            }
            serverToolsReceiveEvent(this, sta);
        }
    }


    /// <summary>
    /// Permet de stocker les évenements interceptés pour les utiliser facilement.
    /// </summary>
    public class ServerToolsEvent : EventArgs {

        public enum typeEvent {
            CONNEXION,
            DECONNEXION,
            MESSAGE,
            INFORMATION
        }

        public ServerToolsEvent(int id, typeEvent type, string operation, List<string> param, string source, int stamp) {
            this.Type = type;
            this.Id = id;
            this.Operation = operation;
            this.Param = param;
            this.Source = source;
            this.Stamp = stamp;
        }

        public int Id { get; set; }

        public int Stamp { get; set; }

        public List<string> Param { get; set; }

        public string Operation { get; set; }

        public typeEvent Type { get; set; }

        public string Source { get; set; }
    }
}
