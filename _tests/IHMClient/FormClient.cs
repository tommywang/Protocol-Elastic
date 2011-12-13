using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SocketLibrary;

namespace IHMClient
{
    public partial class FormClient : Form
    {
        private SocketLibrary.IClient myClient;

        public FormClient()
        {
            InitializeComponent();
            
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (!utils.isIPAddressCorrect(txt_ip.Text)) {
                MessageBox.Show("L'adresse IP spécifiée n'est pas au bon format.", "Attention" ,MessageBoxButtons.OK);
                return;
            }
            myClient = utils.createClient("127.0.0.1", 50000);
            myClient.subscribe(clientEventFunc);
            if (!myClient.connect()) {
                MessageBox.Show("Problème de connexion au serveur demandé.", "Attention", MessageBoxButtons.OK);
                return;
            }
            
            

            btn_connect.Enabled = false;
            btn_deconnect.Enabled = true;
            btn_send.Enabled = true;
            txt_ip.Enabled = false;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (myClient != null)
            {
                myClient.disconnect();
                myClient.unsubscribe();
            }
            btn_connect.Enabled = true;
            btn_deconnect.Enabled = false;
            btn_send.Enabled = false;
            txt_ip.Enabled = true;
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            byte[] msg = Encoding.Default.GetBytes(txt_send.Text);
            byte[] count = new byte[4];
            count = BitConverter.GetBytes(msg.Length);
            byte[] msgsend = new byte[4 + msg.Length];
            count.CopyTo(msgsend, 0);
            msg.CopyTo(msgsend, 4);
            myClient.send(msgsend);
        }

        public void clientEventFunc(object sender, SocketLibrary.ClientEventArgs e)
        {
            SetMsgecho(Encoding.Default.GetString(e.Msg));
        }

        private delegate void SetMsgechoDelegate(string str);

        private void SetMsgecho(string str)
        {
            if (txt_echo.InvokeRequired)
            {
                SetMsgechoDelegate d = SetMsgecho;
                txt_echo.Invoke(d, str);
            }
            else
            {
                txt_echo.Text += str + "\r\n";
            }
        }
    }
}
