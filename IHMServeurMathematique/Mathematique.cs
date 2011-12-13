﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibraryInterfaceServer;
using LibraryInterfaceClient;
using LibraryDataStructure;
using System.Reflection;
using LibraryMathematique;

namespace IHMServeurMathematique {
    public partial class Mathematique : Form {
        private static ServiceInfos myInfos;

        public delegate void setCatalogueInfo(string port_local, string adresse_catalogue, string port_catalogue);
        private delegate void AppendTextToLogCallback(string text);

        private IServerTools mySelfServer;
        private IClientTools mySelfClient;

        public static string port_local_default = "50002";
        public static string port_catalogue_default = "50000";
        public static string adresse_catalogue_default = "127.0.0.1";
        public static string catalogue_name = "Elastic.Global.Services.Catalog";
        public static string service = "Elastic.Global.Services.Mathematique";

        string port_local;
        string port_catalogue;
        string adresse_catalogue;

        Configuration config;

        private void setCataloguePortAndAdress(string newPort, string adresse_catalogue, string port_catalogue)
        {
            this.port_local = newPort;
            this.adresse_catalogue = adresse_catalogue;
            this.port_catalogue = port_catalogue;
        }
        public Mathematique() {
            InitializeComponent();
            config = new Configuration(new setCatalogueInfo(setCataloguePortAndAdress));
            
            Dictionary<string, List<Type>> param = new Dictionary<string, List<Type>>() {
                {"echo", 
                    new List<Type>() { 
                        typeof(string), //prend une chaîne en entrée
                        typeof(string) }} //retourne une chaîne
            };
            myInfos = new ServiceInfos(param, service);

            port_local = port_local_default;
            port_catalogue = port_catalogue_default;
            adresse_catalogue = adresse_catalogue_default;
        }

        public void disconnect() {
            mySelfClient.Disconnect();
        }

        public bool connectToCatalogue() {
            mySelfClient = LibraryInterfaceClient.Utils.createClient(adresse_catalogue, int.Parse(port_catalogue));
            if (mySelfClient.Connect()) {
                mySelfClient.Subscribe(handleToolsEvents);
                appendTextToLog("Connected to catalog server!" + Environment.NewLine);
                return true;
            }
            else
                MessageBox.Show("Impossible d'atteindre le serveur catalogue demandé (adresse = " + adresse_catalogue + ", port = " + port_catalogue + ")");
            return false;
        }


        public void handleToolsEvents(object o, ClientToolsEvent e) {
            switch (e.Type) {
                case ClientToolsEvent.typeEvent.MESSAGE:
                    appendTextToLog("Message from catalog server :" + e.Message[0] + Environment.NewLine);
                    if (e.Message[0] == DataUtils.MANIFEST_PRESENCE_CODE) {
                        string address = DataUtils.GetIPaddresses(System.Net.Dns.GetHostName())[0];
                        mySelfClient.Send(address, catalogue_name, "", "0", new List<string>() { DataUtils.ACKNOWLEDGE_PRESENCE_CODE });
                        appendTextToLog("Acknowledging presence to catalog server." + Environment.NewLine);
                    }
                    break;
                case ClientToolsEvent.typeEvent.ERROR:
                    MessageBox.Show("Le serveur catalogue a signalé l'erreur suivante :"+e.Message[0]);
                    break;
                case ClientToolsEvent.typeEvent.EXTINCTION:
                    MessageBox.Show("Le serveur catalogue a signalé son extinction. Vous allez maintenant être déconnecté.");
                    break;
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            btn_stop.Enabled = true;

            mySelfServer = LibraryInterfaceServer.Utils.createServer(int.Parse(port_local));
            mySelfServer.Subscribe(handleInterfaceEvents);

            if (DataUtils.IsIPAddressCorrect(adresse_catalogue)) {
                if (mySelfServer.Start()) {
                    if (connectToCatalogue()) {
                        registerService();
                        appendTextToLog("Server registered!" + Environment.NewLine);
                    }
                }
                else
                    MessageBox.Show("Le serveur ne peut pas démarrer. Vérifier votre configuration.",
                                "Attention",
                                MessageBoxButtons.OK);
            }
            else
                MessageBox.Show("L'adresse ip " + adresse_catalogue + " du catalogue est mal formée. Veuillez revoir vos paramètres.",
                                "Attention",
                                MessageBoxButtons.OK);
        }

        private void handleInterfaceEvents(object sender, ServerToolsEvent e) {
            switch (e.Type) {
                case ServerToolsEvent.typeEvent.MESSAGE:
                    appendTextToLog("Message from client number " + e.Id + " received!" + Environment.NewLine);
                    appendTextToLog("Operation asked : " + e.Operation + Environment.NewLine);
                    //TODO: décoder les paramètres + faire la réponse et l'envoyer
                    mySelfServer.handleRequest(e, typeof(ServiceMathematique), service);
                    break;
                case ServerToolsEvent.typeEvent.INFORMATION:
                    if (e.Id == -1) {
                        appendTextToLog("Error! The server cannot start!" + Environment.NewLine);
                        btn_start.Enabled = true;
                        btn_stop.Enabled = false;
                    }
                    else
                        appendTextToLog("Server successfully started on port " + port_local + " !" + Environment.NewLine);
                    break;
                case ServerToolsEvent.typeEvent.DECONNEXION:
                    appendTextToLog("Client number " + e.Id + " disconnected!" + Environment.NewLine);
                    break;
                case ServerToolsEvent.typeEvent.CONNEXION:
                    appendTextToLog("Client number " + e.Id + " connected!" + Environment.NewLine);
                    break;
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            mySelfServer.Stop();
            mySelfServer = null;
            unregister();
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
        }

        private void unregister() {
            //envoyer au catalogue que l'on veut se unregister
            string address = DataUtils.GetIPaddresses(System.Net.Dns.GetHostName())[0];
            mySelfClient.Send(address, catalogue_name, "unregister", "1", new List<string>() { service });

            mySelfClient.Disconnect();
        }
        private void registerService()
        {
            //envoyer au catalogue que l'on veut se register
            string title = LibraryInterfaceServer.Utils.getTitle(typeof(ServiceMathematique));
            string address = DataUtils.GetIPaddresses(System.Net.Dns.GetHostName())[0];
            mySelfClient.Send(address, catalogue_name, "register", "0", new List<string>() { service, title, address, port_local });
        }

        private void paramètresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config.ShowDialog(this);
        }

        private void appendTextToLog(string text) {
            if (!btn_write.Enabled)
                btn_write.Enabled = true;
            if (txt_log.InvokeRequired) {
                AppendTextToLogCallback d = new AppendTextToLogCallback(appendTextToLog);
                this.Invoke(d, new object[] { text });
            }
            else {
                txt_log.AppendText(text);
            }
        }

        private void btn_clearLog_Click(object sender, EventArgs e) {
            if (btn_write.Enabled)
                btn_write.Enabled = false;
            txt_log.Clear();
        }

        private void btn_write_Click(object sender, EventArgs e) {
            sfd_log.ShowDialog();
            if (@sfd_log.FileName == "")
                return;
            System.IO.File.WriteAllText(@sfd_log.FileName, txt_log.Text);
            MessageBox.Show("Fichier crée avec succès!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Catalogue_FormClosed(object sender, FormClosedEventArgs e) {
            if (mySelfServer != null)
                mySelfServer.Stop();
            if (mySelfClient != null)
                mySelfClient.Disconnect();
        }
    }
}
