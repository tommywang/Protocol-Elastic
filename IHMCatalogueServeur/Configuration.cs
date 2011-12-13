using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IHMCatalogueServeur {
    public partial class Configuration : Form {
        Catalogue.setCatalogueInfo setter;
        public Configuration(Catalogue.setCatalogueInfo setter) {
            InitializeComponent();
            this.setter = setter;
            txt_port.Text = Catalogue.port_catalogue_default;
        }

        private void btn_annuler_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btn_valider_Click(object sender, EventArgs e) {
            setter(txt_port.Text);
            this.Close();
        }

        private void rbn_defaut_CheckedChanged(object sender, EventArgs e) {
            txt_port.Text = Catalogue.port_catalogue_default;
            txt_port.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            txt_port.Enabled = true;
        }

        private void txt_port_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != ',') {
                e.Handled = true;
            }
        }
    }
}
