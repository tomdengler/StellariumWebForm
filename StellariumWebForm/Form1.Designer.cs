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
            this.buttonSetCurrentView.Location = new System.Drawing.Point(12, 54);
            this.buttonSetCurrentView.Name = "buttonSetCurrentView";
            this.buttonSetCurrentView.Size = new System.Drawing.Size(169, 24);
            this.buttonSetCurrentView.TabIndex = 2;
            this.buttonSetCurrentView.Text = "Set Current View";
            this.buttonSetCurrentView.UseVisualStyleBackColor = true;
            this.buttonSetCurrentView.Click += new System.EventHandler(this.buttonSetCurrentView_Click);
            // 
            // textBoxSetCurrentView
            // 
            this.textBoxSetCurrentView.Location = new System.Drawing.Point(187, 54);
            this.textBoxSetCurrentView.Name = "textBoxSetCurrentView";
            this.textBoxSetCurrentView.Size = new System.Drawing.Size(601, 23);
            this.textBoxSetCurrentView.TabIndex = 3;
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Location = new System.Drawing.Point(187, 134);
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.Size = new System.Drawing.Size(604, 23);
            this.textBoxResponse.TabIndex = 4;
            // 
            // buttonSetRotation
            // 
            this.buttonSetRotation.Location = new System.Drawing.Point(21, 84);
            this.buttonSetRotation.Name = "buttonSetRotation";
            this.buttonSetRotation.Size = new System.Drawing.Size(160, 23);
            this.buttonSetRotation.TabIndex = 5;
            this.buttonSetRotation.Text = "Set Rotation";
            this.buttonSetRotation.UseVisualStyleBackColor = true;
            this.buttonSetRotation.Click += new System.EventHandler(this.buttonSetRotation_Click);
            // 
            // textBoxSetRotation
            // 
            this.textBoxSetRotation.Location = new System.Drawing.Point(187, 85);
            this.textBoxSetRotation.Name = "textBoxSetRotation";
            this.textBoxSetRotation.Size = new System.Drawing.Size(188, 23);
            this.textBoxSetRotation.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxSetRotation);
            this.Controls.Add(this.buttonSetRotation);
            this.Controls.Add(this.textBoxResponse);
            this.Controls.Add(this.textBoxSetCurrentView);
            this.Controls.Add(this.buttonSetCurrentView);
            this.Controls.Add(this.TextBoxCurrentView);
            this.Controls.Add(this.ButtonCurrentView);
            this.Name = "Form1";
            this.Text = "Form1";
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
    }
}