namespace Nu64.UI
{
    partial class DebugWindow
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
            this.components = new System.ComponentModel.Container();
            this.messageText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stepsLabel = new System.Windows.Forms.Label();
            this.stepsInput = new System.Windows.Forms.TextBox();
            this.StepButton = new System.Windows.Forms.Button();
            this.locationLabel = new System.Windows.Forms.Label();
            this.locationInput = new System.Windows.Forms.TextBox();
            this.JumpButton = new System.Windows.Forms.Button();
            this.MemoryButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.lastLine = new System.Windows.Forms.TextBox();
            this.stackText = new System.Windows.Forms.TextBox();
            this.HeaderTextbox = new System.Windows.Forms.TextBox();
            this.registerDisplay1 = new Nu64.RegisterDisplay();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageText
            // 
            this.messageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageText.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageText.Location = new System.Drawing.Point(0, 97);
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.messageText.Size = new System.Drawing.Size(851, 433);
            this.messageText.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stepsLabel);
            this.panel1.Controls.Add(this.stepsInput);
            this.panel1.Controls.Add(this.StepButton);
            this.panel1.Controls.Add(this.locationLabel);
            this.panel1.Controls.Add(this.locationInput);
            this.panel1.Controls.Add(this.JumpButton);
            this.panel1.Controls.Add(this.MemoryButton);
            this.panel1.Controls.Add(this.RunButton);
            this.panel1.Controls.Add(this.PauseButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(851, 24);
            this.panel1.TabIndex = 2;
            // 
            // stepsLabel
            // 
            this.stepsLabel.AutoSize = true;
            this.stepsLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsLabel.Location = new System.Drawing.Point(241, 0);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.stepsLabel.Size = new System.Drawing.Size(61, 17);
            this.stepsLabel.TabIndex = 8;
            this.stepsLabel.Text = "Steps (dec)";
            // 
            // stepsInput
            // 
            this.stepsInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.stepsInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsInput.Location = new System.Drawing.Point(186, 0);
            this.stepsInput.Name = "stepsInput";
            this.stepsInput.Size = new System.Drawing.Size(55, 23);
            this.stepsInput.TabIndex = 7;
            this.stepsInput.Text = "1";
            this.stepsInput.Enter += new System.EventHandler(this.stepsInput_Enter);
            // 
            // StepButton
            // 
            this.StepButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.StepButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StepButton.Location = new System.Drawing.Point(124, 0);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(62, 24);
            this.StepButton.TabIndex = 1;
            this.StepButton.Text = "Step";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // locationLabel
            // 
            this.locationLabel.AutoSize = true;
            this.locationLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.locationLabel.Location = new System.Drawing.Point(615, 0);
            this.locationLabel.Name = "locationLabel";
            this.locationLabel.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.locationLabel.Size = new System.Drawing.Size(57, 17);
            this.locationLabel.TabIndex = 6;
            this.locationLabel.Text = "Location $";
            // 
            // locationInput
            // 
            this.locationInput.Dock = System.Windows.Forms.DockStyle.Right;
            this.locationInput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locationInput.Location = new System.Drawing.Point(672, 0);
            this.locationInput.Name = "locationInput";
            this.locationInput.Size = new System.Drawing.Size(55, 23);
            this.locationInput.TabIndex = 5;
            this.locationInput.Text = "000000";
            this.locationInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.locationInput.TextChanged += new System.EventHandler(this.locationInput_TextChanged);
            // 
            // JumpButton
            // 
            this.JumpButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.JumpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.JumpButton.Location = new System.Drawing.Point(727, 0);
            this.JumpButton.Name = "JumpButton";
            this.JumpButton.Size = new System.Drawing.Size(62, 24);
            this.JumpButton.TabIndex = 3;
            this.JumpButton.Text = "Jump";
            this.JumpButton.UseVisualStyleBackColor = true;
            // 
            // MemoryButton
            // 
            this.MemoryButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.MemoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MemoryButton.Location = new System.Drawing.Point(789, 0);
            this.MemoryButton.Name = "MemoryButton";
            this.MemoryButton.Size = new System.Drawing.Size(62, 24);
            this.MemoryButton.TabIndex = 4;
            this.MemoryButton.Text = "Memory";
            this.MemoryButton.UseVisualStyleBackColor = true;
            // 
            // RunButton
            // 
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.RunButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RunButton.Location = new System.Drawing.Point(62, 0);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(62, 24);
            this.RunButton.TabIndex = 2;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PauseButton.Location = new System.Drawing.Point(0, 0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(62, 24);
            this.PauseButton.TabIndex = 0;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // lastLine
            // 
            this.lastLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lastLine.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastLine.Location = new System.Drawing.Point(0, 530);
            this.lastLine.Name = "lastLine";
            this.lastLine.Size = new System.Drawing.Size(851, 23);
            this.lastLine.TabIndex = 3;
            this.lastLine.Text = "Click [Step] to execute an instruction";
            // 
            // stackText
            // 
            this.stackText.Dock = System.Windows.Forms.DockStyle.Right;
            this.stackText.Font = new System.Drawing.Font("Consolas", 10F);
            this.stackText.Location = new System.Drawing.Point(851, 0);
            this.stackText.Multiline = true;
            this.stackText.Name = "stackText";
            this.stackText.Size = new System.Drawing.Size(150, 553);
            this.stackText.TabIndex = 4;
            // 
            // HeaderTextbox
            // 
            this.HeaderTextbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderTextbox.Font = new System.Drawing.Font("Consolas", 10F);
            this.HeaderTextbox.Location = new System.Drawing.Point(0, 77);
            this.HeaderTextbox.Multiline = true;
            this.HeaderTextbox.Name = "HeaderTextbox";
            this.HeaderTextbox.Size = new System.Drawing.Size(851, 20);
            this.HeaderTextbox.TabIndex = 5;
            // 
            // registerDisplay1
            // 
            this.registerDisplay1.CPU = null;
            this.registerDisplay1.Dock = System.Windows.Forms.DockStyle.Top;
            this.registerDisplay1.Location = new System.Drawing.Point(0, 24);
            this.registerDisplay1.Name = "registerDisplay1";
            this.registerDisplay1.Size = new System.Drawing.Size(851, 53);
            this.registerDisplay1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 30;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 553);
            this.ControlBox = false;
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.HeaderTextbox);
            this.Controls.Add(this.registerDisplay1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lastLine);
            this.Controls.Add(this.stackText);
            this.Location = new System.Drawing.Point(1280, 0);
            this.Name = "DebugWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RegisterWindow";
            this.Load += new System.EventHandler(this.DebugWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public RegisterDisplay registerDisplay1;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox locationInput;
        private System.Windows.Forms.Button MemoryButton;
        private System.Windows.Forms.Button JumpButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button StepButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.TextBox lastLine;
        private System.Windows.Forms.TextBox stackText;
        private System.Windows.Forms.Label locationLabel;
        private System.Windows.Forms.Label stepsLabel;
        private System.Windows.Forms.TextBox stepsInput;
        private System.Windows.Forms.TextBox HeaderTextbox;
        private System.Windows.Forms.Timer timer1;
    }
}