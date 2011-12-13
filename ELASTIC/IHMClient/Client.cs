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

        Configuration config; //la fenêtre de configuration du serveur catalogue
        Requete request; //la fenêtre dynamique pour requêter le serveur

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
            mySelf = LibraryInterfaceClient.Utils.createClient(ip, int.Parse(port));
            if (mySelf.Connect()) {
                mySelf.Subscribe(handleToolsEvents);
                return true;
            }
            else
                MessageBox.Show("Impossible d'atteindre le serveur demandé (adresse = " + ip + ", port = " + port+")",
                                "Attention",
                                MessageBoxButtons.OK);
            return false;
        }

        public void handleToolsEvents(object o, ClientToolsEvent e) {
            switch (e.Type) {
                case ClientToolsEvent.typeEvent.ERROR:
                    string errorMessage = Utils.decodeString(e.Message[0]);
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
                    }
                    break;
                case ClientToolsEvent.typeEvent.MESSAGE:
                    if (int.Parse(e.Stamp) == queryNumber - 1)
                        setQueryNumberToHighest();
                    else
                        emptyQueryNumbers.Add(int.Parse(e.Stamp));

                    ServiceInfos ks = queries[int.Parse(e.Stamp)];

                    if (ks.Operation.Keys.First() != catalogue_name) {

                        Dictionary<string, List<Type>> operation = ks.Operation; //opération de la requête
                        List<Type> types = operation.Values.First(); //types pour la première et unique opération de cette structure
                        Type type = types[types.Count - 1]; //on récupère le dernier type de la liste, correspondant à la valeur de retour de l'opération

                        object retour = Utils.decode(e.Message[0], type);

                        request.appendTextToLog(retour.ToString() + Environment.NewLine);
                    }
                    else {
                        //traiter le getinfos ici
                        mySelf.Disconnect();
                        mySelf = null;
                    }
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

                        btn_connect.Enabled = false;
                        btn_getInfo.Enabled = false;
                        btn_disconnect.Enabled = true;
                        dgv_infos.Enabled = false;
                    }
                }
            }
        }

        private void btn_disconnect_Click(object sender, EventArgs e) {
            btn_connect.Enabled = true;
            btn_getInfo.Enabled = true;
            dgv_infos.Enabled = true;
            disconnect();
        }

        internal static void send(string service, string operation, List<string> param) {
            //convertir param en byte[][]
            int iStamp;
            if (emptyQueryNumbers.Count > 0)
                iStamp = emptyQueryNumbers[0];
            else
                iStamp = queryNumber++;
            string stamp = iStamp.ToString();
            ServiceInfos ks = null;
            foreach (ServiceInfos ks_ in knownServices) {
                if(ks_.Service == service) {
                    ks = new ServiceInfos(new Dictionary<string, List<Type>>() { { operation, ks_.Operation[operation] } }, service);
                    break;
                }
            }
            queries.Add(iStamp, ks);
            mySelf.Send(service, operation, stamp, new byte[][] {});
        }

        private void btn_getInfo_Click(object sender, EventArgs e) {
            if (Utils.IsIPAddressCorrect(adress_catalogue)) {
                connect(adress_catalogue, port_catalogue);
                send(catalogue_name, "getinfos", new List<string>() { "" });
            }
            else
                MessageBox.Show("L'adresse ip " + adress_catalogue + " du catalogue est mal formée. Veuillez revoir vos paramètres.",
                                "Attention",
                                MessageBoxButtons.OK);
        }

        private void button1_Click(object sender, EventArgs e) {
            if (connect(adress_catalogue, port_catalogue)) {
                request = new Requete(knownServices, "Elastic.Global.Services.Catalog");
                request.ShowDialog(this);
            }
        }

        private void loadListTest() {
            //pour les tests on rentre ça en dur:
            dgv_infos.Rows.Clear();
            dgv_infos.Rows.Add(new object[] { false, "127.0.0.1", "50001", "Elastic.Global.Services.Echo" });
            dgv_infos.Rows.Add(new object[] { false, "127.0.0.1", "50002", "Elastic.Global.Services.Mathematique" });
            knownServices.Add(new ServiceInfos(
                new Dictionary<string, List<Type>>() 
                { 
                   {"echo", new List<Type> () {
                       typeof(string),
                       typeof(string)}}
                },
                "Elastic.Global.Services.Echo"));
            knownServices.Add(new ServiceInfos(
                new Dictionary<string, List<Type>>() 
                { 
                   {"addition", new List<Type> () {
                       typeof(int), 
                       typeof(int), 
                       typeof(int)}},
                   {"soustraction", new List<Type> () {
                       typeof(int), 
                       typeof(int), 
                       typeof(int)}},
                   {"division", new List<Type> () {
                       typeof(int), 
                       typeof(int), 
                       typeof(int)}},
                   {"multiplication", new List<Type> () {
                       typeof(int), 
                       typeof(int), 
                       typeof(int)}},
                },
                "Elastic.Global.Services.Mathematique"));
                
        }

        private void button2_Click(object sender, EventArgs e) {
            //loadListTest();
            string name1 = "Elastic.Global.Services.Echo";
            string name2 = "Elastic.Global.Services.Mathematique";

            string param1 = "echo(string)string";
            string param2 = "addition(int,int)int_multiplication(int,int)int_division(int,int)int_soustraction(int,int)int";

            string ip1 = "127.0.0.1";
            string ip2 = "127.0.0.1";

            string port1 = "50001";
            string port2 = "50002";

            byte[] n1 = System.Text.Encoding.UTF8.GetBytes(name1);
            byte[] n2 = System.Text.Encoding.UTF8.GetBytes(name2);

            byte[] p1 = System.Text.Encoding.UTF8.GetBytes(param1);
            byte[] p2 = System.Text.Encoding.UTF8.GetBytes(param2);

            byte[] i1 = System.Text.Encoding.UTF8.GetBytes(ip1);
            byte[] i2 = System.Text.Encoding.UTF8.GetBytes(ip2);

            byte[] po1 = System.Text.Encoding.UTF8.GetBytes(port1);
            byte[] po2 = System.Text.Encoding.UTF8.GetBytes(port2);

            byte[][] foo = new byte[][] { n1, p1, i1, po1, n2, p2, i2, po2};
            prepareServerInfos(foo, 8);

        }


        /**
         * ENCODAGE DES OPERATIONS DANS LE TITLE:
         * operation1(param1,param2)retour_operation2(param1,param2)retour_etc...
         */
        private void prepareServerInfos(byte[][] rawData, int paramCount) {
            List<string> serviceNames = new List<string>();
            List<List<string>> operationNames = new List<List<string>>();
            List<List<Type>> typeForOperations = new List<List<Type>>();
            List<string> ipAdresses = new List<string>();
            List<string> portValues = new List<string>();
            for (int i = 0; i < paramCount; i+=4) {
                string serviceName = Utils.decodeString(rawData[i]);
                serviceNames.Add(serviceName);

                string param = Utils.decodeString(rawData[i + 1]);
                string [] operations = param.Split('_');

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

                    typeForOperations.Add(new List<Type>());
                    do {
                        string typeParamIn = "";
                        do {
                            typeParamIn += operation[charAt];
                            charAt++;
                            if (operation[charAt] == ')')
                                break;
                        }
                        while (operation[charAt] != ',');

                        typeForOperations[noOperation].Add(getType(typeParamIn));
                    }
                    while (operation[charAt] != ')');
                    charAt++;
                    //on est sur le type de retour
                    string typeRetour = "";
                    do {
                        typeRetour += operation[charAt++];
                    }
                    while (charAt < operation.Length);
                    typeForOperations[noOperation].Add(getType(typeRetour));
                    noOperation++;
                }
                string ip = Utils.decodeString(rawData[i + 2]);
                ipAdresses.Add(ip);
                string port = Utils.decodeString(rawData[i + 3]);
                portValues.Add(port);
            }

            loadServerList(serviceNames, operationNames, typeForOperations, ipAdresses, portValues);
        }

        private Type getType(string type) {
            if (type.ToLower().Contains("void"))
                return typeof(void);
            else if (type.ToLower().Contains("string"))
                return typeof(string);
            else if (type.ToLower().Contains("float"))
                return typeof(float);
            else if (type.ToLower().Contains("bool"))
                return typeof(bool);
            else if (type.ToLower().Contains("double"))
                return typeof(double);
            else if (type.ToLower().Contains("long"))
                return typeof(long);
            else if (type.ToLower().Contains("char"))
                return typeof(char);
            else
                return typeof(int);
        }

        private void loadServerList(List<string> serviceNames, 
                                    List<List<string>> operationNames, 
                                    List<List<Type>> typeForOperations, 
                                    List<string> ipAdresses, 
                                    List<string> portValues) {
            for (int i = 0; i < serviceNames.Count; i++) {
                Dictionary<string, List<Type>> operations = new Dictionary<string, List<Type>>();
                foreach (string operationName in operationNames[i]) {
                    List<Type> typeForOperation = typeForOperations[i];
                    operations.Add(operationName, typeForOperation);
                }
                ServiceInfos si = new ServiceInfos(operations, serviceNames[i]);
                knownServices.Add(si);
                dgv_infos.Rows.Add(new object[] { false, ipAdresses[i], portValues[i], serviceNames[i] });
            }
        }
    }

    
}