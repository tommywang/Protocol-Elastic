using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonServer{
    public partial class Configuration : Form {
        Server.setCatalogueInfo setter;
        bool default_local = true;
        bool default_catalogue = true;
        public Configuration(Server.setCatalogueInfo setter) {
            InitializeComponent();
            this.setter = setter;
        }

        private void btn_annuler_Click(object sender, EventArgs e) {
            if (default_catalogue)
                rbn_defaut.Checked = true;
            if (default_local)
                radioButton3.Checked = true;
            this.Close();
        }

        private void btn_valider_Click(object sender, EventArgs e) {
            setter(txt_port_local.Text, txt_adresse_catalogue.Text, txt_port_catalogue.Text);
            default_local = rbn_defaut.Checked;
            default_catalogue = radioButton3.Checked;
            this.Close();
        }

        private void rbn_defaut_CheckedChanged(object sender, EventArgs e) {
            txt_port_local.Text = "50002";
            txt_port_local.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            txt_port_local.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            txt_port_catalogue.Text = "50000";
            txt_adresse_catalogue.Text = "127.0.0.1";
            txt_port_catalogue.Enabled = false;
            txt_adresse_catalogue.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            txt_port_catalogue.Enabled = true;
            txt_adresse_catalogue.Enabled = true;
        }
    }
}
