namespace IHMServeurEcho
{
    partial class Configuration {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btn_valider = new System.Windows.Forms.Button();
            this.btn_annuler = new System.Windows.Forms.Button();
            this.grp_config = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_port_local = new System.Windows.Forms.TextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.rbn_defaut = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_adresse_catalogue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_port_catalogue = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.grp_config.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_valider
            // 
            this.btn_valider.Location = new System.Drawing.Point(320, 199);
            this.btn_valider.Name = "btn_valider";
            this.btn_valider.Size = new System.Drawing.Size(61, 30);
            this.btn_valider.TabIndex = 0;
            this.btn_valider.Text = "Valider";
            this.btn_valider.UseVisualStyleBackColor = true;
            this.btn_valider.Click += new System.EventHandler(this.btn_valider_Click);
            // 
            // btn_annuler
            // 
            this.btn_annuler.Location = new System.Drawing.Point(253, 199);
            this.btn_annuler.Name = "btn_annuler";
            this.btn_annuler.Size = new System.Drawing.Size(61, 30);
            this.btn_annuler.TabIndex = 1;
            this.btn_annuler.Text = "Annuler";
            this.btn_annuler.UseVisualStyleBackColor = true;
            this.btn_annuler.Click += new System.EventHandler(this.btn_annuler_Click);
            // 
            // grp_config
            // 
            this.grp_config.Controls.Add(this.label2);
            this.grp_config.Controls.Add(this.txt_port_local);
            this.grp_config.Controls.Add(this.radioButton2);
            this.grp_config.Controls.Add(this.rbn_defaut);
            this.grp_config.Location = new System.Drawing.Point(5, 12);
            this.grp_config.Name = "grp_config";
            this.grp_config.Size = new System.Drawing.Size(376, 73);
            this.grp_config.TabIndex = 2;
            this.grp_config.TabStop = false;
            this.grp_config.Text = "Paramètres locaux";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port du serveur";
            // 
            // txt_port_local
            // 
            this.txt_port_local.Enabled = false;
            this.txt_port_local.Location = new System.Drawing.Point(242, 44);
            this.txt_port_local.Name = "txt_port_local";
            this.txt_port_local.Size = new System.Drawing.Size(124, 20);
            this.txt_port_local.TabIndex = 3;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(85, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Personnalisé";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbn_defaut
            // 
            this.rbn_defaut.AutoSize = true;
            this.rbn_defaut.Checked = true;
            this.rbn_defaut.Location = new System.Drawing.Point(7, 19);
            this.rbn_defaut.Name = "rbn_defaut";
            this.rbn_defaut.Size = new System.Drawing.Size(74, 17);
            this.rbn_defaut.TabIndex = 0;
            this.rbn_defaut.TabStop = true;
            this.rbn_defaut.Text = "Par défaut";
            this.rbn_defaut.UseVisualStyleBackColor = true;
            this.rbn_defaut.CheckedChanged += new System.EventHandler(this.rbn_defaut_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txt_adresse_catalogue);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_port_catalogue);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Location = new System.Drawing.Point(5, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 73);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paramètres catalogue";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(130, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Adresse du catalogue";
            // 
            // txt_adresse_catalogue
            // 
            this.txt_adresse_catalogue.Enabled = false;
            this.txt_adresse_catalogue.Location = new System.Drawing.Point(246, 21);
            this.txt_adresse_catalogue.Name = "txt_adresse_catalogue";
            this.txt_adresse_catalogue.Size = new System.Drawing.Size(124, 20);
            this.txt_adresse_catalogue.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Port du catalogue";
            // 
            // txt_port_catalogue
            // 
            this.txt_port_catalogue.Enabled = false;
            this.txt_port_catalogue.Location = new System.Drawing.Point(246, 47);
            this.txt_port_catalogue.Name = "txt_port_catalogue";
            this.txt_port_catalogue.Size = new System.Drawing.Size(124, 20);
            this.txt_port_catalogue.TabIndex = 3;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 42);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "Personnalisé";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 19);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(74, 17);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Par défaut";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 232);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grp_config);
            this.Controls.Add(this.btn_annuler);
            this.Controls.Add(this.btn_valider);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Configuration";
            this.Text = "Configuration";
            this.grp_config.ResumeLayout(false);
            this.grp_config.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_valider;
        private System.Windows.Forms.Button btn_annuler;
        private System.Windows.Forms.GroupBox grp_config;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_port_local;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton rbn_defaut;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_port_catalogue;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_adresse_catalogue;
    }
}