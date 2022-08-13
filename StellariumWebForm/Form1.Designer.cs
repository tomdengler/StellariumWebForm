namespace StellariumWebForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonCurrentView = new System.Windows.Forms.Button();
            this.TextBoxCurrentView = new System.Windows.Forms.TextBox();
            this.buttonSetCurrentView = new System.Windows.Forms.Button();
            this.textBoxSetCurrentView = new System.Windows.Forms.TextBox();
            this.textBoxResponse = new System.Windows.Forms.TextBox();
            this.buttonSetRotation = new System.Windows.Forms.Button();
            this.textBoxSetRotation = new System.Windows.Forms.TextBox();
            this.textBoxJ2000 = new System.Windows.Forms.TextBox();
            this.textBoxJNow = new System.Windows.Forms.TextBox();
            this.textBoxAltAz = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBoxRADec = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonCurrentView
            // 
            this.ButtonCurrentView.Location = new System.Drawing.Point(12, 25);
            this.ButtonCurrentView.Name = "ButtonCurrentView";
            this.ButtonCurrentView.Size = new System.Drawing.Size(169, 23);
            this.ButtonCurrentView.TabIndex = 0;
            this.ButtonCurrentView.Text = "Get Current View";
            this.ButtonCurrentView.UseVisualStyleBackColor = true;
            this.ButtonCurrentView.Click += new System.EventHandler(this.ButtonCurrentView_Click);
            // 
            // TextBoxCurrentView
            // 
            this.TextBoxCurrentView.Location = new System.Drawing.Point(187, 25);
            this.TextBoxCurrentView.Name = "TextBoxCurrentView";
            this.TextBoxCurrentView.Size = new System.Drawing.Size(601, 23);
            this.TextBoxCurrentView.TabIndex = 1;
            // 
            // buttonSetCurrentView
            // 
            this.buttonSetCurrentView.Location = new System.Drawing.Point(12, 193);
            this.buttonSetCurrentView.Name = "buttonSetCurrentView";
            this.buttonSetCurrentView.Size = new System.Drawing.Size(169, 24);
            this.buttonSetCurrentView.TabIndex = 2;
            this.buttonSetCurrentView.Text = "Set Current View";
            this.buttonSetCurrentView.UseVisualStyleBackColor = true;
            this.buttonSetCurrentView.Click += new System.EventHandler(this.buttonSetCurrentView_Click);
            // 
            // textBoxSetCurrentView
            // 
            this.textBoxSetCurrentView.Location = new System.Drawing.Point(187, 193);
            this.textBoxSetCurrentView.Name = "textBoxSetCurrentView";
            this.textBoxSetCurrentView.Size = new System.Drawing.Size(256, 23);
            this.textBoxSetCurrentView.TabIndex = 3;
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Location = new System.Drawing.Point(187, 273);
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.Size = new System.Drawing.Size(604, 23);
            this.textBoxResponse.TabIndex = 4;
            // 
            // buttonSetRotation
            // 
            this.buttonSetRotation.Location = new System.Drawing.Point(12, 223);
            this.buttonSetRotation.Name = "buttonSetRotation";
            this.buttonSetRotation.Size = new System.Drawing.Size(169, 23);
            this.buttonSetRotation.TabIndex = 5;
            this.buttonSetRotation.Text = "Set Rotation";
            this.buttonSetRotation.UseVisualStyleBackColor = true;
            this.buttonSetRotation.Click += new System.EventHandler(this.buttonSetRotation_Click);
            // 
            // textBoxSetRotation
            // 
            this.textBoxSetRotation.Location = new System.Drawing.Point(187, 224);
            this.textBoxSetRotation.Name = "textBoxSetRotation";
            this.textBoxSetRotation.Size = new System.Drawing.Size(68, 23);
            this.textBoxSetRotation.TabIndex = 6;
            // 
            // textBoxJ2000
            // 
            this.textBoxJ2000.Location = new System.Drawing.Point(187, 54);
            this.textBoxJ2000.Name = "textBoxJ2000";
            this.textBoxJ2000.Size = new System.Drawing.Size(256, 23);
            this.textBoxJ2000.TabIndex = 7;
            // 
            // textBoxJNow
            // 
            this.textBoxJNow.Location = new System.Drawing.Point(187, 83);
            this.textBoxJNow.Name = "textBoxJNow";
            this.textBoxJNow.Size = new System.Drawing.Size(256, 23);
            this.textBoxJNow.TabIndex = 8;
            // 
            // textBoxAltAz
            // 
            this.textBoxAltAz.Location = new System.Drawing.Point(187, 112);
            this.textBoxAltAz.Name = "textBoxAltAz";
            this.textBoxAltAz.Size = new System.Drawing.Size(256, 23);
            this.textBoxAltAz.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "J2000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "JNow";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "Alt/Az";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // textBoxRADec
            // 
            this.textBoxRADec.Location = new System.Drawing.Point(449, 54);
            this.textBoxRADec.Name = "textBoxRADec";
            this.textBoxRADec.Size = new System.Drawing.Size(174, 23);
            this.textBoxRADec.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxRADec);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAltAz);
            this.Controls.Add(this.textBoxJNow);
            this.Controls.Add(this.textBoxJ2000);
            this.Controls.Add(this.textBoxSetRotation);
            this.Controls.Add(this.buttonSetRotation);
            this.Controls.Add(this.textBoxResponse);
            this.Controls.Add(this.textBoxSetCurrentView);
            this.Controls.Add(this.buttonSetCurrentView);
            this.Controls.Add(this.TextBoxCurrentView);
            this.Controls.Add(this.ButtonCurrentView);
            this.Name = "Form1";
            this.Text = "Stellarium Web Interface";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ButtonCurrentView;
        private TextBox TextBoxCurrentView;
        private Button buttonSetCurrentView;
        private TextBox textBoxSetCurrentView;
        private TextBox textBoxResponse;
        private Button buttonSetRotation;
        private TextBox textBoxSetRotation;
        private TextBox textBoxJ2000;
        private TextBox textBoxJNow;
        private TextBox textBoxAltAz;
        private Label label1;
        private Label label2;
        private Label label3;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TextBox textBoxRADec;
    }
}