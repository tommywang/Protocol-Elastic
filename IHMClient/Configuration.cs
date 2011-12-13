using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IHMClient {
    public partial class Configuration : Form {
        Client.setCatalogueInfo setter;
        public Configuration(Client.setCatalogueInfo setter) {
            InitializeComponent();
            this.setter = setter;
        }

        private void btn_annuler_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btn_valider_Click(object sender, EventArgs e) {
            setter(txt_adress.Text, txt_port.Text);
            this.Close();
        }

        private void rbn_defaut_CheckedChanged(object sender, EventArgs e) {
            txt_adress.Text = Client.adresse_catalogue_default;
            txt_port.Text = Client.port_catalogue_default;
            txt_adress.Enabled = false;
            txt_port.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            txt_adress.Enabled = true;
            txt_port.Enabled = true;
        }
    }
}
