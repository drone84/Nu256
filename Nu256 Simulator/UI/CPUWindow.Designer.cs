namespace Nu256.Simulator.UI
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stepsInput = new System.Windows.Forms.TextBox();
            this.BPLabel = new System.Windows.Forms.Label();
            this.BPCombo = new System.Windows.Forms.ComboBox();
            this.AddBPButton = new System.Windows.Forms.Button();
            this.DeleteBPButton = new System.Windows.Forms.Button();
            this.stepsLabel = new System.Windows.Forms.Label();
            this.StepButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationInput = new System.Windows.Forms.TextBox();
            this.JumpButton = new System.Windows.Forms.Button();
            this.stackText = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.CPUTraceCheckBox = new System.Windows.Forms.CheckBox();
            this.ClearTraceButton = new System.Windows.Forms.Button();
            this.traceViewer1 = new Nu256.Simulator.UI.TraceViewer();
            this.registerDisplay1 = new Nu256.Simulator.RegisterDisplay();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 24);
            this.panel1.TabIndex = 2;
            // 
            // stepsInput
            // 
            this.stepsInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsInput.Location = new System.Drawing.Point(256, 0);
            this.stepsInput.Name = "stepsInput";
            this.stepsInput.Size = new System.Drawing.Size(64, 23);
            this.stepsInput.TabIndex = 3;
            this.stepsInput.Text = "1";
            this.stepsInput.Enter += new System.EventHandler(this.stepsInput_Enter);
            // 
            // BPLabel
            // 
            this.BPLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.BPLabel.Location = new System.Drawing.Point(401, 0);
            this.BPLabel.Name = "BPLabel";
            this.BPLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.BPLabel.Size = new System.Drawing.Size(96, 24);
            this.BPLabel.TabIndex = 5;
            this.BPLabel.Text = "Breakpoint";
            this.BPLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BPCombo
            // 
            this.BPCombo.Dock = System.Windows.Forms.DockStyle.Right;
            this.BPCombo.FormattingEnabled = true;
            this.BPCombo.Location = new System.Drawing.Point(497, 0);
            this.BPCombo.Name = "BPCombo";
            this.BPCombo.Size = new System.Drawing.Size(121, 21);
            this.BPCombo.TabIndex = 6;
            // 
            // AddBPButton
            // 
            this.AddBPButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.AddBPButton.Location = new System.Drawing.Point(618, 0);
            this.AddBPButton.Name = "AddBPButton";
            this.AddBPButton.Size = new System.Drawing.Size(24, 24);
            this.AddBPButton.TabIndex = 7;
            this.AddBPButton.Text = "+";
            this.AddBPButton.UseVisualStyleBackColor = true;
            this.AddBPButton.Click += new System.EventHandler(this.AddBPButton_Click);
            // 
            // DeleteBPButton
            // 
            this.DeleteBPButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.DeleteBPButton.Location = new System.Drawing.Point(642, 0);
            this.DeleteBPButton.Name = "DeleteBPButton";
            this.DeleteBPButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteBPButton.TabIndex = 8;
            this.DeleteBPButton.Text = "-";
            this.DeleteBPButton.UseVisualStyleBackColor = true;
            this.DeleteBPButton.Click += new System.EventHandler(this.DeleteBPButton_Click);
            // 
            // stepsLabel
            // 
            this.stepsLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsLabel.Location = new System.Drawing.Point(192, 0);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.stepsLabel.Size = new System.Drawing.Size(64, 24);
            this.stepsLabel.TabIndex = 4;
            this.stepsLabel.Text = "Steps (dec)";
            // 
            // StepButton
            // 
            this.StepButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepButton.Location = new System.Drawing.Point(128, 0);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(64, 24);
            this.StepButton.TabIndex = 2;
            this.StepButton.Text = "Step";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunButton.Location = new System.Drawing.Point(64, 0);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(64, 24);
            this.RunButton.TabIndex = 1;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PauseButton.Location = new System.Drawing.Point(0, 0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(64, 24);
            this.PauseButton.TabIndex = 0;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // locationLabel
            // 
            this.locationLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationLabel.Location = new System.Drawing.Point(160, 0);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.locationLabel.Size = new System.Drawing.Size(64, 24);
            this.locationLabel.TabIndex = 9;
            this.locationLabel.Text = "Location $";
            // 
            // locationInput
            // 
            this.locationInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationInput.Location = new System.Drawing.Point(224, 0);
            this.locationInput.Name = "locationInput";
            this.locationInput.Size = new System.Drawing.Size(64, 23);
            this.locationInput.TabIndex = 10;
            this.locationInput.Text = "000000";
            this.locationInput.TextChanged += new System.EventHandler(this.locationInput_TextChanged);
            // 
            // JumpButton
            // 
            this.JumpButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.JumpButton.Location = new System.Drawing.Point(96, 0);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(64, 24);
            this.JumpButton.TabIndex = 11;
            this.JumpButton.Text = "Jump";
            this.JumpButton.UseVisualStyleBackColor = true;
            this.JumpButton.Click += new System.EventHandler(this.JumpButton_Click);
            // 
            // stackText
            // 
            this.stackText.Dock = System.Windows.Forms.DockStyle.Right;
            this.stackText.Font = new System.Drawing.Font("Consolas", 10F);
            this.stackText.Location = new System.Drawing.Point(666, 0);
            this.stackText.Multiline = true;
            this.stackText.Name = "stackText";
            this.stackText.Size = new System.Drawing.Size(150, 553);
            this.stackText.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CPUTraceCheckBox);
            this.panel2.Controls.Add(this.locationInput);
            this.panel2.Controls.Add(this.locationLabel);
            this.panel2.Controls.Add(this.JumpButton);
            this.panel2.Controls.Add(this.ClearTraceButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 24);
            this.panel2.TabIndex = 5;
            // 
            // CPUTraceCheckBox
            // 
            this.CPUTraceCheckBox.AutoSize = true;
            this.CPUTraceCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CPUTraceCheckBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.CPUTraceCheckBox.Location = new System.Drawing.Point(288, 0);
            this.CPUTraceCheckBox.Name = "CPUTraceCheckBox";
            this.CPUTraceCheckBox.Size = new System.Drawing.Size(79, 24);
            this.CPUTraceCheckBox.TabIndex = 13;
            this.CPUTraceCheckBox.Text = "CPU Trace";
            this.CPUTraceCheckBox.UseVisualStyleBackColor = true;
            this.CPUTraceCheckBox.CheckedChanged += new System.EventHandler(this.CPUTraceCheckBox_CheckedChanged);
            // 
            // ClearTraceButton
            // 
            this.ClearTraceButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ClearTraceButton.Location = new System.Drawing.Point(0, 0);
            this.ClearTraceButton.Name = "ClearTraceButton";
            this.ClearTraceButton.Size = new System.Drawing.Size(96, 24);
            this.ClearTraceButton.TabIndex = 12;
            this.ClearTraceButton.Text = "Clear Trace";
            this.ClearTraceButton.UseVisualStyleBackColor = true;
            this.ClearTraceButton.Click += new System.EventHandler(this.ClearTraceButton_Click);
            // 
            // traceViewer1
            // 
            this.traceViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.traceViewer1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.traceViewer1.Location = new System.Drawing.Point(0, 101);
            this.traceViewer1.Name = "traceViewer1";
            this.traceViewer1.Size = new System.Drawing.Size(666, 452);
            this.traceViewer1.TabIndex = 6;
            // 
            // registerDisplay1
            // 
            this.registerDisplay1.CPU = null;
            this.registerDisplay1.Dock = System.Windows.Forms.DockStyle.Top;
            this.registerDisplay1.Location = new System.Drawing.Point(0, 48);
            this.registerDisplay1.Name = "registerDisplay1";
            this.registerDisplay1.Size = new System.Drawing.Size(666, 53);
            this.registerDisplay1.TabIndex = 0;
            // 
            // CPUWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 553);
            this.Controls.Add(this.traceViewer1);
            this.Controls.Add(this.registerDisplay1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stackText);
            this.Location = new System.Drawing.Point(1280, 0);
            this.Name = "CPUWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RegisterWindow";
            this.Load += new System.EventHandler(this.CPUWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public RegisterDisplay registerDisplay1;
        private global::System.Windows.Forms.Panel panel1;
        private global::System.Windows.Forms.TextBox locationInput;
        private global::System.Windows.Forms.Button JumpButton;
        private global::System.Windows.Forms.Button RunButton;
        private global::System.Windows.Forms.Button StepButton;
        private global::System.Windows.Forms.Button PauseButton;
        private global::System.Windows.Forms.TextBox stackText;
        private global::System.Windows.Forms.Label locationLabel;
        private global::System.Windows.Forms.Label stepsLabel;
        private global::System.Windows.Forms.TextBox stepsInput;
        private global::System.Windows.Forms.Timer timer1;
        private global::System.Windows.Forms.Label BPLabel;
        private global::System.Windows.Forms.ComboBox BPCombo;
        private global::System.Windows.Forms.Button AddBPButton;
        private global::System.Windows.Forms.Button DeleteBPButton;
        private global::System.Windows.Forms.Panel panel2;
        private global::System.Windows.Forms.Button ClearTraceButton;
        private TraceViewer traceViewer1;
        private System.Windows.Forms.CheckBox CPUTraceCheckBox;
    }
}