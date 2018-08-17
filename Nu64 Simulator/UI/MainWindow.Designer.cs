using Nu64.Display;

namespace Nu64
{
    partial class MainWindow
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ModeText = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastKeyPressed = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cPUToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.memoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeTimer = new System.Windows.Forms.Timer(this.components);
            this.timerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.gpu = new Nu64.Display.Gpu();
            this.performanceTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModeText,
            this.lastKeyPressed,
            this.timerStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 487);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(807, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ModeText
            // 
            this.ModeText.Name = "ModeText";
            this.ModeText.Size = new System.Drawing.Size(64, 17);
            this.ModeText.Text = "Immediate";
            // 
            // lastKeyPressed
            // 
            this.lastKeyPressed.Name = "lastKeyPressed";
            this.lastKeyPressed.Size = new System.Drawing.Size(25, 17);
            this.lastKeyPressed.Text = "$00";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.windowsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(807, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cPUToolStripMenuItem,
            this.memoryToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // cPUToolStripMenuItem
            // 
            this.cPUToolStripMenuItem.Name = "cPUToolStripMenuItem";
            this.cPUToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.cPUToolStripMenuItem.Text = "CPU";
            // 
            // memoryToolStripMenuItem
            // 
            this.memoryToolStripMenuItem.Name = "memoryToolStripMenuItem";
            this.memoryToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.memoryToolStripMenuItem.Text = "Memory";
            // 
            // executeTimer
            // 
            this.executeTimer.Enabled = true;
            this.executeTimer.Interval = 10;
            this.executeTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerStatus
            // 
            this.timerStatus.Name = "timerStatus";
            this.timerStatus.Size = new System.Drawing.Size(34, 17);
            this.timerStatus.Text = "0 cps";
            // 
            // gpu
            // 
            this.gpu.CursorPos = 0;
            this.gpu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpu.Location = new System.Drawing.Point(0, 24);
            this.gpu.Name = "gpu";
            this.gpu.Size = new System.Drawing.Size(807, 463);
            this.gpu.TabIndex = 0;
            this.gpu.X = 0;
            this.gpu.Y = 0;
            // 
            // performanceTimer
            // 
            this.performanceTimer.Enabled = true;
            this.performanceTimer.Interval = 1000;
            this.performanceTimer.Tick += new System.EventHandler(this.performanceTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 509);
            this.Controls.Add(this.gpu);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Nu64 BASIC";
            this.Load += new System.EventHandler(this.BasicWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BasicWindow_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BasicWindow_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BasicWindow_KeyUp);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Gpu gpu;
        private global::System.Windows.Forms.StatusStrip statusStrip1;
        private global::System.Windows.Forms.ToolStripStatusLabel ModeText;
        private global::System.Windows.Forms.ToolStripStatusLabel lastKeyPressed;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cPUToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem memoryToolStripMenuItem;
        private System.Windows.Forms.Timer executeTimer;
        private System.Windows.Forms.ToolStripStatusLabel timerStatus;
        private System.Windows.Forms.Timer performanceTimer;
    }
}

