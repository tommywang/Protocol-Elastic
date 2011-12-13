namespace IHMClient {
    partial class Requete {
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txt_request = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(481, 405);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(239, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Quitter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(481, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(239, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Envoyer";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txt_request
            // 
            this.txt_request.Enabled = false;
            this.txt_request.Location = new System.Drawing.Point(481, 12);
            this.txt_request.Multiline = true;
            this.txt_request.Name = "txt_request";
            this.txt_request.Size = new System.Drawing.Size(239, 358);
            this.txt_request.TabIndex = 2;
            // 
            // Requete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 440);
            this.Controls.Add(this.txt_request);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Requete";
            this.Text = "Requete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txt_request;
    }
}