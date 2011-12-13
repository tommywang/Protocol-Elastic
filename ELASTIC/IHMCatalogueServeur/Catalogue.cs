using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibraryInterfaceServer;
using LibraryDataStructure;
using LibraryCatalogue;
using System.Net;

namespace IHMCatalogueServeur {
    public partial class Catalogue : Form {
        private delegate void AppendTextToLogCallback(string text);
        private IServerTools mySelf;
        public delegate void setCatalogueInfo(String port);
        
        IHMCatalogueServeur.Configuration config;
        public static string port_catalogue_default = "50000";
        string port_catalogue;

        /**
         * contrairement aux autres serveurs, on ne conserve pas les informations du catalogue (via la structure de donnée "ServerInfos").
         * ce choix a été fait car l'hypothèse de départ est que tout le monde connaît le catalogue.
         * ce serait redondant que d'envoyer les informations du catalogue via un getinfos au client qui connaît déjà le catalogue.
         */

        public Catalogue() {
            InitializeComponent();
            config = new Configuration(new setCatalogueInfo(setCataloguePortAndAdress));
            port_catalogue = port_catalogue_default;
        }

        private void setCataloguePortAndAdress(string newPort) {
            this.port_catalogue = newPort;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            btn_stop.Enabled = true;
            mySelf = Utils.createServer(int.Parse(port_catalogue));
            mySelf.Subscribe(handleIntefaceEvents);
            mySelf.Start();
        }

        private void handleIntefaceEvents(object sender, ServerToolsEvent e) {
            switch (e.Type) {
                case ServerToolsEvent.typeEvent.MESSAGE:
                    AppendTextToLog("Message from client number " + e.Id + " received!" + Environment.NewLine);
                    AppendTextToLog("Operation asked : " + e.Operation + Environment.NewLine);
                    //TODO: décoder les paramètres + faire la réponse et l'envoyer
                    break;
                case ServerToolsEvent.typeEvent.INFORMATION:
                    if (e.Id == -1) {
                        AppendTextToLog("Error! The server cannot connect!" + Environment.NewLine);
                        btn_start.Enabled = true;
                        btn_stop.Enabled = false;
                    }
                    else
                        AppendTextToLog("Server successfully started on port " + port_catalogue + " !" + Environment.NewLine);
                    break;
                case ServerToolsEvent.typeEvent.DECONNEXION:
                    AppendTextToLog("Client number " + e.Id + " disconnected!" + Environment.NewLine);
                    UnregisterById(e.Id);
                    break;
                case ServerToolsEvent.typeEvent.CONNEXION:
                    AppendTextToLog("Client number " + e.Id + " connected!" + Environment.NewLine);
                    break;
            }
        }

        private void btn_stop_Click(object sender, EventArgs e) {
            mySelf.Stop();
            mySelf = null;
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
        }

        private void AppendTextToLog(string text) {
            if (!btn_write.Enabled)
                btn_write.Enabled = true;
            if (txt_log.InvokeRequired) {
                AppendTextToLogCallback d = new AppendTextToLogCallback(AppendTextToLog);
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
            if (mySelf != null) 
                mySelf.Stop();
        }

        private void parametresToolStripMenuItem_Click(object sender, EventArgs e) {
            config.ShowDialog(this);
        }

        private void BroadCastExctinction() {
            mySelf.Broadcast(Codes.EXTINCTION_CODE);
        }

        private void CheckIfStillHere(int id) {
            mySelf.Send(Codes.MANIFEST_PRESENCE_CODE, id);
        }

        private void UnregisterByName(string service) {
            foreach (Tuple<Tuple<ServiceInfos, IPEndPoint>, int> tuple in ServiceCatalogue.GetAllInfos()) {
                if (tuple.Item1.Item1.Service == service)
                    ServiceCatalogue.Unregister(service);
            }
        }

        private void UnregisterById(int id) {
            foreach (Tuple<Tuple<ServiceInfos, IPEndPoint>, int> tuple in ServiceCatalogue.GetAllInfos()) {
                if (tuple.Item2 == id)
                    ServiceCatalogue.Unregister(tuple.Item1.Item1.Service);
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            foreach(Tuple<Tuple<ServiceInfos, IPEndPoint>, int> tuple in ServiceCatalogue.GetAllInfos()) {
                CheckIfStillHere(tuple.Item2);
            }
        }
    }
}
