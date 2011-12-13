using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IHMServeurEcho{
    public partial class Configuration : Form {
        Echo.setCatalogueInfo setter;
        bool default_local = true;
        bool default_catalogue = true;
        public Configuration(Echo.setCatalogueInfo setter) {
            InitializeComponent();
            this.setter = setter;
            txt_port_catalogue.Text = Echo.port_catalogue_default;
            txt_adresse_catalogue.Text = Echo.adresse_catalogue_default;
            txt_port_local.Text = Echo.port_local_default;
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
            txt_port_local.Text = Echo.port_local_default;
            txt_port_local.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            txt_port_local.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            txt_port_catalogue.Text = Echo.port_catalogue_default;
            txt_adresse_catalogue.Text = Echo.adresse_catalogue_default;
            txt_port_catalogue.Enabled = false;
            txt_adresse_catalogue.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            txt_port_catalogue.Enabled = true;
            txt_adresse_catalogue.Enabled = true;
        }
    }
}
