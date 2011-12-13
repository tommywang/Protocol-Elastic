namespace IHMClient
{
    partial class FormClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_deconnect = new System.Windows.Forms.Button();
            this.btn_send = new System.Windows.Forms.Button();
            this.txt_send = new System.Windows.Forms.TextBox();
            this.gb_msgEcho = new System.Windows.Forms.GroupBox();
            this.txt_echo = new System.Windows.Forms.TextBox();
            this.txt_ip = new System.Windows.Forms.TextBox();
            this.gb_msgEcho.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(25, 246);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(110, 25);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_deconnect
            // 
            this.btn_deconnect.Enabled = false;
            this.btn_deconnect.Location = new System.Drawing.Point(153, 246);
            this.btn_deconnect.Name = "btn_deconnect";
            this.btn_deconnect.Size = new System.Drawing.Size(110, 25);
            this.btn_deconnect.TabIndex = 1;
            this.btn_deconnect.Text = "Deconnect";
            this.btn_deconnect.UseVisualStyleBackColor = true;
            this.btn_deconnect.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // btn_send
            // 
            this.btn_send.Enabled = false;
            this.btn_send.Location = new System.Drawing.Point(187, 207);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 25);
            this.btn_send.TabIndex = 2;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(25, 207);
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(156, 20);
            this.txt_send.TabIndex = 4;
            // 
            // gb_msgEcho
            // 
            this.gb_msgEcho.Controls.Add(this.txt_echo);
            this.gb_msgEcho.Location = new System.Drawing.Point(25, 27);
            this.gb_msgEcho.Name = "gb_msgEcho";
            this.gb_msgEcho.Size = new System.Drawing.Size(237, 157);
            this.gb_msgEcho.TabIndex = 5;
            this.gb_msgEcho.TabStop = false;
            this.gb_msgEcho.Text = "MsgEcho";
            // 
            // txt_echo
            // 
            this.txt_echo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_echo.Enabled = false;
            this.txt_echo.Location = new System.Drawing.Point(3, 16);
            this.txt_echo.Multiline = true;
            this.txt_echo.Name = "txt_echo";
            this.txt_echo.Size = new System.Drawing.Size(231, 138);
            this.txt_echo.TabIndex = 0;
            // 
            // txt_ip
            // 
            this.txt_ip.Location = new System.Drawing.Point(25, 277);
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.Size = new System.Drawing.Size(234, 20);
            this.txt_ip.TabIndex = 6;
            this.txt_ip.Text = "127.0.0.1";
            this.txt_ip.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // FormClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 311);
            this.Controls.Add(this.txt_ip);
            this.Controls.Add(this.gb_msgEcho);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.btn_deconnect);
            this.Controls.Add(this.btn_connect);
            this.Name = "FormClient";
            this.Text = "Client";
            this.gb_msgEcho.ResumeLayout(false);
            this.gb_msgEcho.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_deconnect;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox txt_send;
        private System.Windows.Forms.GroupBox gb_msgEcho;
        private System.Windows.Forms.TextBox txt_echo;
        private System.Windows.Forms.TextBox txt_ip;
    }
}

