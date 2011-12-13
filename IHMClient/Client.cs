using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibraryInterfaceClient;
using LibraryDataStructure;
using System.Threading; //on utilise le thread juste pour l'affichage du chargement, c'est à but ostentatoire

namespace IHMClient {
    public partial class Client : Form {
        public delegate void setCatalogueInfo(String adress, String port);
        private delegate void AddRowToListCallback(object[] items);
        private delegate void ClearListCallback();

        Configuration config; //la fenêtre de configuration du serveur catalogue
        static Requete request; //la fenêtre dynamique pour requêter le serveur

        string catalogue_name = "Elastic.Global.Services.Catalog";

        public static string port_catalogue_default = "50000";
        public static string adresse_catalogue_default = "127.0.0.1";

        string adress_catalogue;
        string port_catalogue;

        int nbLignesCochees;

        static IClientTools mySelf = null;

        static List<ServiceInfos> knownServices = null; //services connus par le client
        static Dictionary<int, ServiceInfos> queries = null; //dictionnaire reliant un stamp à une requête, il y a une astuce:
        /**
         * En réalité, le KnownService relié à l'integer ne contient qu'une seule opération, et non une liste.
         * Il s'agit de l'opération demandée à la requête. Ce n'est donc pas le Service complet, avec toutes les opérations.
         */
        static int queryNumber = 0; //pour numéroter les requêtes
        static List<int> emptyQueryNumbers; //stocke les numéro de requêtes libérés
        
        public Client() {
            InitializeComponent();
            config = new Configuration(new setCatalogueInfo(setCataloguePortAndAdress));
            nbLignesCochees = 0;
            initKnownServices();
            emptyQueryNumbers = new List<int>();
            queries = new Dictionary<int, ServiceInfos>();
            adress_catalogue = adresse_catalogue_default;
            port_catalogue = port_catalogue_default;
        }

        private void initKnownServices() {
            knownServices = new List<ServiceInfos>();
            knownServices.Add(new ServiceInfos(
                new Dictionary<string, List<Type>>() 
                { 
                   {"register", new List<Type> () {
                       typeof(string), 
                       typeof(string), 
                       typeof(string), 
                       typeof(string), 
                       typeof(Dictionary<string, List<Type>>), //liste des opérations reliées à leurs paramètres
                       typeof(void)}}, //retourne void
                   {"unregister", new List<Type> () {
                       typeof(string),
                       typeof(void)}}, //retourne void
                   {"getinfos", new List<Type> () {
                       typeof(string),
                       typeof(List<ServiceInfos>)}} //retourne une liste de KnownService
                }, 
                "Elastic.Global.Services.Catalog"));
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e) {
            disconnect();
            Application.Exit();
        }

        public static void disconnect() {
            if(mySelf != null)
                mySelf.Disconnect();
        }

        private void setQueryNumberToHighest() {
            int max = -1;
            foreach(int i in queries.Keys) {
                if (i > max)
                    max = i;
            }
            queryNumber = max;
        }

        public bool connect(String ip, String port) {
            //HACK
            ip = "127.0.0.1";

            mySelf = LibraryInterfaceClient.Utils.createClient(ip, int.Parse(port));
            if (mySelf.Connect()) {
                mySelf.Subscribe(handleToolsEvents);
                return true;
            }
            else
                MessageBox.Show("Impossible d'atteindre le serveur demandé (adresse = " 
                                + ip + ", port = " 
                                + port+")",
                                "Attention",
                                MessageBoxButtons.OK);
            return false;
        }

        public void handleToolsEvents(object o, ClientToolsEvent e) {
            switch (e.Type) {
                case ClientToolsEvent.typeEvent.ERROR:
                    string errorMessage = e.Message[0];
                    MessageBox.Show("Le serveur sur lequel vous êtes connecté à renvoyé l'erreur suivante : " +errorMessage, 
                                    "Attention", 
                                    MessageBoxButtons.OK);
                    break;
                case ClientToolsEvent.typeEvent.EXTINCTION:
                    MessageBox.Show("Le serveur sur lequel vous étiez connecté a signalé une extinction!", 
                                    "Attention", 
                                    MessageBoxButtons.OK);
                    if (request.Visible) {
                        disconnect();
                        request.Dispose();
                        request.Close();
                        request = null;
                        dgv_infos.Rows.Clear();
                    }
                    break;
                case ClientToolsEvent.typeEvent.MESSAGE:
                    //if (int.Parse(e.Stamp) == queryNumber - 1)
                    //    setQueryNumberToHighest();
                    //else
                        emptyQueryNumbers.Add(int.Parse(e.Stamp));

                    ServiceInfos ks = queries[int.Parse(e.Stamp)];

                    if (ks.Operation.Keys.First() != "getinfos") {
                        //Dictionary<string, List<Type>> operation = ks.Operation; //opération de la requête
                        //List<Type> types = operation.Values.First(); //types pour la première et unique opération de cette structure
                        //Type type = types[types.Count - 1]; //on récupère le dernier type de la liste, correspondant à la valeur de retour de l'opération
                        //object retour = Utils.decode(e.Message[0], type);
                        request.appendTextToLog(e.Message[0] + Environment.NewLine);
                    }
                    else {
                        
                        prepareServerInfos(e.Message, e.Message.Count);
                        mySelf.Disconnect();
                        mySelf = null;
                    }
                    queries.Remove(int.Parse(e.Stamp));
                    break;
            }
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e) {
            config.ShowDialog(this);
        }
        
        private void setCataloguePortAndAdress(string newAdress, string newPort) {
            this.adress_catalogue = newAdress;
            this.port_catalogue = newPort;
        }

        private void dgv_infos_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == 0) {
                if (dgv_infos.CurrentCell is DataGridViewCheckBoxCell) {
                    DataGridViewCheckBoxCell tmp = new DataGridViewCheckBoxCell();
                    tmp = (DataGridViewCheckBoxCell)dgv_infos.CurrentCell;
                    if (tmp.Value == null)
                        tmp.Value = false;
                    switch (tmp.Value.ToString()) {
                        case "True":
                            tmp.Value = true;
                            break;
                        case "False":
                            tmp.Value = false;
                            break;
                    }
                    if((bool)tmp.Value)
                        nbLignesCochees--;
                    else
                        nbLignesCochees++;
                    if (nbLignesCochees == 0) {
                        btn_connect.Enabled = false;
                    }
                    else {
                        if (nbLignesCochees > 1)
                            btn_connect.Enabled = false;
                        else
                            btn_connect.Enabled = true;
                    }

                    switch (tmp.Value.ToString()) {
                        case "True":
                            tmp.Value = false;
                            break;
                        case "False":
                            tmp.Value = true;
                            break;
                    }
                }
            }
        }

        private void btn_connect_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dgv_infos.Rows) {
                if (row.Cells[0].Value == null) {
                    row.Cells[0].Value = false;
                }
                if ((bool)row.Cells[0].Value) {
                    DataGridViewTextBoxCell ip = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell port = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell service = new DataGridViewTextBoxCell();
                    ip = (DataGridViewTextBoxCell) row.Cells[1];
                    port = (DataGridViewTextBoxCell) row.Cells[2];
                    service = (DataGridViewTextBoxCell)row.Cells[3];
                    if (connect((string)ip.Value, (string)port.Value)) {
                        request = new Requete(knownServices, (string)service.Value);
                        request.ShowDialog(this);
                    }
                }
            }
        }

        internal static void send(string service, string operation, List<string> param) {
            int iStamp= queryNumber++;
            string stamp = iStamp.ToString();
            ServiceInfos ks = null;
            foreach (ServiceInfos ks_ in knownServices) {
                if(ks_.Service == service) {
                    ks = new ServiceInfos(new Dictionary<string, List<Type>>() { { operation, ks_.Operation[operation] } }, service);
                    break;
                }
            }
            queries.Add(iStamp, ks);
            string address = DataUtils.GetIPaddresses(System.Net.Dns.GetHostName())[0];
            if (!mySelf.Send(address, service, operation, stamp, param)) {
                mySelf.Disconnect();
                MessageBox.Show("Le serveur sur lequel vous étiez connecté s'est arrêté.",
                                "Attention",
                                MessageBoxButtons.OK);
                if (request != null)
                    if (request.Visible)
                        request.Close();
            }

        }

        private void btn_getInfo_Click(object sender, EventArgs e) {
            btn_connect.Enabled = true;
            nbLignesCochees = 0;
            if (DataUtils.IsIPAddressCorrect(adress_catalogue)) {
                connect(adress_catalogue, port_catalogue);
                send(catalogue_name, "getinfos", new List<string>() { "" });
            }
            else
                MessageBox.Show("L'adresse ip " + adress_catalogue + " du catalogue est mal formée. Veuillez revoir vos paramètres.",
                                "Attention",
                                MessageBoxButtons.OK);
        }
        /**
         * ENCODAGE DES OPERATIONS DANS LE TITLE:
         * operation1(param1,param2)retour_operation2(param1,param2)retour_etc...
         */
        private void prepareServerInfos(List<string> data, int paramCount) {
            List<string> serviceNames = new List<string>();
            List<List<string>> operationNames = new List<List<string>>();
            List<List<List<Type>>> typeForOperations = new List<List<List<Type>>>();
            List<string> ipAdresses = new List<string>();
            List<string> portValues = new List<string>();
            int noService = 0;
            for (int i = 0; i < paramCount; i+=4) {
                string serviceName = data[i];
                serviceNames.Add(serviceName);

                string param = data[i + 1];
                string [] operations = param.Split('_');

                typeForOperations.Add(new List<List<Type>>());
                int noOperation = 0;
                foreach (string operation in operations) {
                    string operationName = "";
                    int charAt = 0;
                    do {
                        operationName += operation[charAt++];
                    }
                    while(operation[charAt] != '(');
                    charAt++;
                    operationNames.Add(new List<string>());
                    operationNames[i/4].Add(operationName);

                    typeForOperations[noService].Add(new List<Type>());
                    do {
                        string typeParamIn = "";
                        do {
                            typeParamIn += operation[charAt];
                            charAt++;
                            if (operation[charAt] == ')')
                                break;
                        }
                        while (operation[charAt] != ',');

                        typeForOperations[noService][noOperation].Add(DataUtils.GetType(typeParamIn));
                    }
                    while (operation[charAt] != ')');
                    charAt++;
                    //on est sur le type de retour
                    string typeRetour = "";
                    do {
                        typeRetour += operation[charAt++];
                    }
                    while (charAt < operation.Length);
                    typeForOperations[noService][noOperation].Add(DataUtils.GetType(typeRetour));
                    noOperation++;
                }
                noService++;
                string ip = data[i + 2];
                ipAdresses.Add(ip);
                string port = data[i + 3];
                portValues.Add(port);
            }

            loadServerList(serviceNames, operationNames, typeForOperations, ipAdresses, portValues);
        }

        public void addRowToList(object [] items) {
            if (dgv_infos.InvokeRequired) {
                AddRowToListCallback d = new AddRowToListCallback(addRowToList);
                this.Invoke(d, new object[] { items });
            }
            else {
                dgv_infos.Rows.Add(items);
            }
        }

        public void clearList() {
            if (dgv_infos.InvokeRequired) {
                ClearListCallback d = new ClearListCallback(clearList);
                this.Invoke(d, new object[] { });
            }
            else {
                dgv_infos.Rows.Clear();
            }
        }

        //Retourner tous les services disponibles dans le catalogue
        private void loadServerList(List<string> serviceNames, 
                                    List<List<string>> operationNames, 
                                    List<List<List<Type>>> typeForOperations, 
                                    List<string> ipAdresses, 
                                    List<string> portValues) {
            clearList();
            initKnownServices();
            for (int i = 0; i < serviceNames.Count; i++) {
                Dictionary<string, List<Type>> operations = new Dictionary<string, List<Type>>();
                int noOperation = 0;
                foreach (string operationName in operationNames[i]) {
                    List<Type> typeForOperation = typeForOperations[i][noOperation];
                    operations.Add(operationName, typeForOperation);
                    noOperation++;
                }
                ServiceInfos si = new ServiceInfos(operations, serviceNames[i]);
                knownServices.Add(si);
                addRowToList(new object[] { false, ipAdresses[i], portValues[i], serviceNames[i] });
            }
        }
    }

    
}