﻿namespace Nu64.UI
{
    partial class RegisterControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new global::System.Windows.Forms.Label();
            this.textBox1 = new global::System.Windows.Forms.TextBox();
            this.panel1 = new global::System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = global::System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new global::System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new global::System.Drawing.Size(150, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Register";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = global::System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new global::System.Drawing.Font("Consolas", 8.25F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new global::System.Drawing.Point(0, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new global::System.Drawing.Size(150, 13);
            this.textBox1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new global::System.Drawing.Point(0, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new global::System.Drawing.Size(150, 4);
            this.panel1.TabIndex = 2;
            // 
            // RegisterControl
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "RegisterControl";
            this.Size = new global::System.Drawing.Size(150, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::System.Windows.Forms.Label label1;
        private global::System.Windows.Forms.TextBox textBox1;
        private global::System.Windows.Forms.Panel panel1;
    }
}
