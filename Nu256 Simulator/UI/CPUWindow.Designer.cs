﻿namespace Nu256.UI
{
    partial class CPUWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new global::System.ComponentModel.Container();
            this.messageText = new global::System.Windows.Forms.TextBox();
            this.panel1 = new global::System.Windows.Forms.Panel();
            this.stepsInput = new global::System.Windows.Forms.TextBox();
            this.BPLabel = new global::System.Windows.Forms.Label();
            this.BPCombo = new global::System.Windows.Forms.ComboBox();
            this.AddBPButton = new global::System.Windows.Forms.Button();
            this.DeleteBPButton = new global::System.Windows.Forms.Button();
            this.stepsLabel = new global::System.Windows.Forms.Label();
            this.StepButton = new global::System.Windows.Forms.Button();
            this.RunButton = new global::System.Windows.Forms.Button();
            this.PauseButton = new global::System.Windows.Forms.Button();
            this.locationLabel = new global::System.Windows.Forms.Label();
            this.locationInput = new global::System.Windows.Forms.TextBox();
            this.JumpButton = new global::System.Windows.Forms.Button();
            this.lastLine = new global::System.Windows.Forms.TextBox();
            this.stackText = new global::System.Windows.Forms.TextBox();
            this.HeaderTextbox = new global::System.Windows.Forms.TextBox();
            this.timer1 = new global::System.Windows.Forms.Timer(this.components);
            this.panel2 = new global::System.Windows.Forms.Panel();
            this.ClearTraceButton = new global::System.Windows.Forms.Button();
            this.registerDisplay1 = new Nu256.RegisterDisplay();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageText
            // 
            this.messageText.Dock = global::System.Windows.Forms.DockStyle.Fill;
            this.messageText.Font = new global::System.Drawing.Font("Consolas", 10F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageText.Location = new global::System.Drawing.Point(0, 121);
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
            this.messageText.Size = new global::System.Drawing.Size(621, 409);
            this.messageText.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stepsInput);
            this.panel1.Controls.Add(this.BPLabel);
            this.panel1.Controls.Add(this.BPCombo);
            this.panel1.Controls.Add(this.AddBPButton);
            this.panel1.Controls.Add(this.DeleteBPButton);
            this.panel1.Controls.Add(this.stepsLabel);
            this.panel1.Controls.Add(this.StepButton);
            this.panel1.Controls.Add(this.RunButton);
            this.panel1.Controls.Add(this.PauseButton);
            this.panel1.Dock = global::System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new global::System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new global::System.Drawing.Size(621, 24);
            this.panel1.TabIndex = 2;
            // 
            // stepsInput
            // 
            this.stepsInput.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.stepsInput.Font = new global::System.Drawing.Font("Consolas", 10F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsInput.Location = new global::System.Drawing.Point(256, 0);
            this.stepsInput.Name = "stepsInput";
            this.stepsInput.Size = new global::System.Drawing.Size(64, 23);
            this.stepsInput.TabIndex = 3;
            this.stepsInput.Text = "1";
            this.stepsInput.Enter += new global::System.EventHandler(this.stepsInput_Enter);
            // 
            // BPLabel
            // 
            this.BPLabel.Dock = global::System.Windows.Forms.DockStyle.Right;
            this.BPLabel.Location = new global::System.Drawing.Point(356, 0);
            this.BPLabel.Name = "BPLabel";
            this.BPLabel.Padding = new global::System.Windows.Forms.Padding(0, 4, 0, 0);
            this.BPLabel.Size = new global::System.Drawing.Size(96, 24);
            this.BPLabel.TabIndex = 5;
            this.BPLabel.Text = "Breakpoint";
            this.BPLabel.TextAlign = global::System.Drawing.ContentAlignment.TopRight;
            // 
            // BPCombo
            // 
            this.BPCombo.Dock = global::System.Windows.Forms.DockStyle.Right;
            this.BPCombo.FormattingEnabled = true;
            this.BPCombo.Location = new global::System.Drawing.Point(452, 0);
            this.BPCombo.Name = "BPCombo";
            this.BPCombo.Size = new global::System.Drawing.Size(121, 21);
            this.BPCombo.TabIndex = 6;
            // 
            // AddBPButton
            // 
            this.AddBPButton.Dock = global::System.Windows.Forms.DockStyle.Right;
            this.AddBPButton.Location = new global::System.Drawing.Point(573, 0);
            this.AddBPButton.Name = "AddBPButton";
            this.AddBPButton.Size = new global::System.Drawing.Size(24, 24);
            this.AddBPButton.TabIndex = 7;
            this.AddBPButton.Text = "+";
            this.AddBPButton.UseVisualStyleBackColor = true;
            this.AddBPButton.Click += new global::System.EventHandler(this.AddBPButton_Click);
            // 
            // DeleteBPButton
            // 
            this.DeleteBPButton.Dock = global::System.Windows.Forms.DockStyle.Right;
            this.DeleteBPButton.Location = new global::System.Drawing.Point(597, 0);
            this.DeleteBPButton.Name = "DeleteBPButton";
            this.DeleteBPButton.Size = new global::System.Drawing.Size(24, 24);
            this.DeleteBPButton.TabIndex = 8;
            this.DeleteBPButton.Text = "-";
            this.DeleteBPButton.UseVisualStyleBackColor = true;
            this.DeleteBPButton.Click += new global::System.EventHandler(this.DeleteBPButton_Click);
            // 
            // stepsLabel
            // 
            this.stepsLabel.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.stepsLabel.Location = new global::System.Drawing.Point(192, 0);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Padding = new global::System.Windows.Forms.Padding(0, 4, 0, 0);
            this.stepsLabel.Size = new global::System.Drawing.Size(64, 24);
            this.stepsLabel.TabIndex = 4;
            this.stepsLabel.Text = "Steps (dec)";
            // 
            // StepButton
            // 
            this.StepButton.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.StepButton.Location = new global::System.Drawing.Point(128, 0);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new global::System.Drawing.Size(64, 24);
            this.StepButton.TabIndex = 2;
            this.StepButton.Text = "Step";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new global::System.EventHandler(this.StepButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.RunButton.Location = new global::System.Drawing.Point(64, 0);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new global::System.Drawing.Size(64, 24);
            this.RunButton.TabIndex = 1;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new global::System.EventHandler(this.RunButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.PauseButton.Location = new global::System.Drawing.Point(0, 0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new global::System.Drawing.Size(64, 24);
            this.PauseButton.TabIndex = 0;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new global::System.EventHandler(this.PauseButton_Click);
            // 
            // locationLabel
            // 
            this.locationLabel.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.locationLabel.Location = new global::System.Drawing.Point(160, 0);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Padding = new global::System.Windows.Forms.Padding(0, 4, 0, 0);
            this.locationLabel.Size = new global::System.Drawing.Size(64, 24);
            this.locationLabel.TabIndex = 9;
            this.locationLabel.Text = "Location $";
            // 
            // locationInput
            // 
            this.locationInput.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.locationInput.Font = new global::System.Drawing.Font("Consolas", 10F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationInput.Location = new global::System.Drawing.Point(224, 0);
            this.locationInput.Name = "locationInput";
            this.locationInput.Size = new global::System.Drawing.Size(64, 23);
            this.locationInput.TabIndex = 10;
            this.locationInput.Text = "000000";
            this.locationInput.TextChanged += new global::System.EventHandler(this.locationInput_TextChanged);
            // 
            // JumpButton
            // 
            this.JumpButton.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.JumpButton.Location = new global::System.Drawing.Point(96, 0);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new global::System.Drawing.Size(64, 24);
            this.JumpButton.TabIndex = 11;
            this.JumpButton.Text = "Jump";
            this.JumpButton.UseVisualStyleBackColor = true;
            this.JumpButton.Click += new global::System.EventHandler(this.JumpButton_Click);
            // 
            // lastLine
            // 
            this.lastLine.Dock = global::System.Windows.Forms.DockStyle.Bottom;
            this.lastLine.Font = new global::System.Drawing.Font("Consolas", 10F, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastLine.Location = new global::System.Drawing.Point(0, 530);
            this.lastLine.Name = "lastLine";
            this.lastLine.Size = new global::System.Drawing.Size(621, 23);
            this.lastLine.TabIndex = 4;
            this.lastLine.Text = "Click [Step] to execute an instruction";
            // 
            // stackText
            // 
            this.stackText.Dock = global::System.Windows.Forms.DockStyle.Right;
            this.stackText.Font = new global::System.Drawing.Font("Consolas", 10F);
            this.stackText.Location = new global::System.Drawing.Point(621, 0);
            this.stackText.Multiline = true;
            this.stackText.Name = "stackText";
            this.stackText.Size = new global::System.Drawing.Size(150, 553);
            this.stackText.TabIndex = 3;
            // 
            // HeaderTextbox
            // 
            this.HeaderTextbox.Dock = global::System.Windows.Forms.DockStyle.Top;
            this.HeaderTextbox.Font = new global::System.Drawing.Font("Consolas", 10F);
            this.HeaderTextbox.Location = new global::System.Drawing.Point(0, 101);
            this.HeaderTextbox.Multiline = true;
            this.HeaderTextbox.Name = "HeaderTextbox";
            this.HeaderTextbox.Size = new global::System.Drawing.Size(621, 20);
            this.HeaderTextbox.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.locationInput);
            this.panel2.Controls.Add(this.locationLabel);
            this.panel2.Controls.Add(this.JumpButton);
            this.panel2.Controls.Add(this.ClearTraceButton);
            this.panel2.Dock = global::System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new global::System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new global::System.Drawing.Size(621, 24);
            this.panel2.TabIndex = 5;
            // 
            // ClearTraceButton
            // 
            this.ClearTraceButton.Dock = global::System.Windows.Forms.DockStyle.Left;
            this.ClearTraceButton.Location = new global::System.Drawing.Point(0, 0);
            this.ClearTraceButton.Name = "ClearTraceButton";
            this.ClearTraceButton.Size = new global::System.Drawing.Size(96, 24);
            this.ClearTraceButton.TabIndex = 12;
            this.ClearTraceButton.Text = "Clear Trace";
            this.ClearTraceButton.UseVisualStyleBackColor = true;
            this.ClearTraceButton.Click += new global::System.EventHandler(this.ClearTraceButton_Click);
            // 
            // registerDisplay1
            // 
            this.registerDisplay1.CPU = null;
            this.registerDisplay1.Dock = global::System.Windows.Forms.DockStyle.Top;
            this.registerDisplay1.Location = new global::System.Drawing.Point(0, 48);
            this.registerDisplay1.Name = "registerDisplay1";
            this.registerDisplay1.Size = new global::System.Drawing.Size(621, 53);
            this.registerDisplay1.TabIndex = 0;
            // 
            // CPUWindow
            // 
            this.AutoScaleDimensions = new global::System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new global::System.Drawing.Size(771, 553);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.HeaderTextbox);
            this.Controls.Add(this.registerDisplay1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lastLine);
            this.Controls.Add(this.stackText);
            this.Location = new global::System.Drawing.Point(1280, 0);
            this.Name = "CPUWindow";
            this.StartPosition = global::System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RegisterWindow";
            this.Load += new global::System.EventHandler(this.CPUWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public RegisterDisplay registerDisplay1;
        private global::System.Windows.Forms.TextBox messageText;
        private global::System.Windows.Forms.Panel panel1;
        private global::System.Windows.Forms.TextBox locationInput;
        private global::System.Windows.Forms.Button JumpButton;
        private global::System.Windows.Forms.Button RunButton;
        private global::System.Windows.Forms.Button StepButton;
        private global::System.Windows.Forms.Button PauseButton;
        private global::System.Windows.Forms.TextBox lastLine;
        private global::System.Windows.Forms.TextBox stackText;
        private global::System.Windows.Forms.Label locationLabel;
        private global::System.Windows.Forms.Label stepsLabel;
        private global::System.Windows.Forms.TextBox stepsInput;
        private global::System.Windows.Forms.TextBox HeaderTextbox;
        private global::System.Windows.Forms.Timer timer1;
        private global::System.Windows.Forms.Label BPLabel;
        private global::System.Windows.Forms.ComboBox BPCombo;
        private global::System.Windows.Forms.Button AddBPButton;
        private global::System.Windows.Forms.Button DeleteBPButton;
        private global::System.Windows.Forms.Panel panel2;
        private global::System.Windows.Forms.Button ClearTraceButton;
    }
}