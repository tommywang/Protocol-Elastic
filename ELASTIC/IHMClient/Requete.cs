using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibraryDataStructure;

namespace IHMClient {
    public partial class Requete : Form {
        List<RadioButton> rbtns = new List<RadioButton>();
        List<List<TextBox>> txtbx = new List<List<TextBox>>();
        List<Panel> panels = new List<Panel>();
        private delegate void AppendTextToLogCallback(string text);
        string service;
        string operation;
        internal Requete(List<ServiceInfos> knownServices, string service) {
            /**
             * on construit dynamiquement les contrôles:
             * - un radio button par opération pour le service
             * - une textbox par paramètre pour l'opération sélectionnée
             */
            this.service = service;
            InitializeComponent();
            ServiceInfos ks = null;
            foreach (ServiceInfos ks_ in knownServices) {
                if (ks_.Service == service) {
                    ks = ks_;
                    break;
                }
            }
            int rbtnX = 0;
            int rbtnY = 0;
            int i = 0;
            rbtns = new List<RadioButton>();
            txtbx = new List<List<TextBox>>();
            foreach (String s in ks.Operation.Keys) {
                rbtns.Add(new RadioButton());
                rbtns[i].Text = s;
                rbtns[i].Name = i.ToString();
                rbtns[i].Location = new Point(rbtnX, rbtnY);
                txtbx.Add(new List<TextBox>());
                panels.Add(new Panel());
                panels[i].Name = "panel" + i;
                panels[i].Height = 1000;
                for (int param = 0; param < ks.Operation[s].Count-1; param++) {
                    txtbx[i].Add(new TextBox());
                    txtbx[i][param].Width = 200;
                    txtbx[i][param].Location = new Point(rbtnX, rbtnY + 30*param + 30);
                    txtbx[i][param].Name = i.ToString() + " _ " + param.ToString();
                    txtbx[i][param].Text = "Enter a " + ks.Operation[s][param].ToString() + " here.";
                    panels[i].Controls.Add(txtbx[i][param]);
                }
                panels[i].Location = new Point(rbtnX + 100, rbtnY - 100*i);
                if (i != 0)
                    panels[i].Visible = false;
                else {
                    rbtns[i].Checked = true;
                    operation = rbtns[i].Text;
                }
                rbtns[i].Click += new System.EventHandler(dynamicRbtnSelect);
                this.Controls.Add(rbtns[i]);
                this.Controls.Add(panels[i]);
                i++;
                rbtnY += 100;
            }
        }

        private void dynamicRbtnSelect(Object sender, System.EventArgs e) {
            RadioButton rb = (RadioButton)sender;
            operation = rb.Text;
            bool visible;
            for (int i = 0; i < panels.Count; i++) {
                if (i == int.Parse(rb.Name))
                    visible = true;
                else
                    visible = false;
                panels[i].Visible = visible;
            }
        }


        private void Requete_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            Client.disconnect();
            this.Dispose();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            List<string> param = new List<string>();
            foreach(Panel p in panels) {
                if(p.Visible) {
                    foreach(TextBox tb in p.Controls) {
                        param.Add(tb.Text);
                    }
                }
            }
            Client.send(service, operation, param);
        }

        public void appendTextToLog(string text) {
            if (txt_request.InvokeRequired) {
                AppendTextToLogCallback d = new AppendTextToLogCallback(appendTextToLog);
                this.Invoke(d, new object[] { text });
            }
            else {
                txt_request.AppendText(text);
            }
        }
    }
}
