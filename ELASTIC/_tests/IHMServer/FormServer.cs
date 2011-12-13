using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SocketLibrary;

namespace IHMServer
{
    public partial class FormServer : Form
    {
        private SocketLibrary.IServer myServer;
        public FormServer()
        {
            InitializeComponent();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            myServer = utils.createServer(50000);
            myServer.subscribe(serverEventFunc);
            if (!myServer.start()) {
                MessageBox.Show("Problème de démarrage du serveur au port spécifié.", "Attention", MessageBoxButtons.OK);
                return;
            }
            btn_start.Enabled = false;
            btn_stop.Enabled = true;
        }

        public void serverEventFunc(object sender, SocketLibrary.ServerEventArgs e)
        {
            switch (e.Type) {
                case ServerEventArgs.typeEvent.MESSAGE://dans cet exemple on fait un écho dès la réception du message
                    byte[] count = new byte[4];
                    count = BitConverter.GetBytes(e.Msg.Length);
                    byte[] msgecho = new byte[4 + e.Msg.Length];
                    count.CopyTo(msgecho, 0);
                    e.Msg.CopyTo(msgecho, 4);
                    //test the msg at server
                    string ss = Encoding.Default.GetString(e.Msg);
                    setMsgecho(ss);
                    if (!myServer.send(e.Id, msgecho)) Console.WriteLine("client déconnecté");
                        //client déconnecté
                    break;
                case ServerEventArgs.typeEvent.CONNEXION:
                    //Un client s'est connecté. (e.Msg = null)
                    MessageBox.Show("Connexion du client " + e.Id, "co", MessageBoxButtons.OK);
                    break;
                case ServerEventArgs.typeEvent.DECONNEXION:
                    //Un client s'est déconnecté. (e.Msg = null)
                    MessageBox.Show("Déconnexion du client " + e.Id, "déco", MessageBoxButtons.OK);
                    break;
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (myServer != null) {
                myServer.stop();
                myServer.unsubscribe();
            }
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
        }

        private delegate void SetMsgechoDelegate(string str);

        private void setMsgecho(string str)
        {
            if (txt_echo.InvokeRequired)
            {
                SetMsgechoDelegate d = setMsgecho;
                txt_echo.Invoke(d, str);
            }
            else
            {
                txt_echo.Text += str + "\r\n";
            }
        }
    }
}
