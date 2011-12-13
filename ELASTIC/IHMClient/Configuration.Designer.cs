﻿namespace IHMClient {
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
            this.rbn_defaut = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.txt_adress = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grp_config.SuspendLayout();
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
            this.grp_config.Controls.Add(this.label1);
            this.grp_config.Controls.Add(this.txt_port);
            this.grp_config.Controls.Add(this.txt_adress);
            this.grp_config.Controls.Add(this.radioButton2);
            this.grp_config.Controls.Add(this.rbn_defaut);
            this.grp_config.Location = new System.Drawing.Point(5, 12);
            this.grp_config.Name = "grp_config";
            this.grp_config.Size = new System.Drawing.Size(376, 181);
            this.grp_config.TabIndex = 2;
            this.grp_config.TabStop = false;
            this.grp_config.Text = "Choix du catalogue";
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
            // txt_adress
            // 
            this.txt_adress.Enabled = false;
            this.txt_adress.Location = new System.Drawing.Point(99, 84);
            this.txt_adress.Name = "txt_adress";
            this.txt_adress.Size = new System.Drawing.Size(124, 20);
            this.txt_adress.TabIndex = 2;
            this.txt_adress.Text = "127.0.0.1";
            // 
            // txt_port
            // 
            this.txt_port.Enabled = false;
            this.txt_port.Location = new System.Drawing.Point(99, 110);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(124, 20);
            this.txt_port.TabIndex = 3;
            this.txt_port.Text = "50000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Adresse";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port";
            // 
            // Configuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 232);
            this.ControlBox = false;
            this.Controls.Add(this.grp_config);
            this.Controls.Add(this.btn_annuler);
            this.Controls.Add(this.btn_valider);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Configuration";
            this.Text = "Configuration";
            this.grp_config.ResumeLayout(false);
            this.grp_config.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_valider;
        private System.Windows.Forms.Button btn_annuler;
        private System.Windows.Forms.GroupBox grp_config;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.TextBox txt_adress;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton rbn_defaut;
    }
}