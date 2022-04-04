namespace MonitorAdjuster
{
    partial class Adjuster
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
            Adjuster.Reset(forceResetUsed);
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Adjuster));
            this.resetButton = new System.Windows.Forms.Button();
            this.forceReset = new System.Windows.Forms.Button();
            this.load1button = new System.Windows.Forms.Button();
            this.save1button = new System.Windows.Forms.Button();
            this.load2button = new System.Windows.Forms.Button();
            this.save2button = new System.Windows.Forms.Button();
            this.load3button = new System.Windows.Forms.Button();
            this.save3button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // resetButton
            // 
            this.resetButton.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.resetButton.Location = new System.Drawing.Point(10, 10);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(130, 50);
            this.resetButton.TabIndex = 0;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // forceReset
            // 
            this.forceReset.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.forceReset.Location = new System.Drawing.Point(146, 10);
            this.forceReset.Name = "forceReset";
            this.forceReset.Size = new System.Drawing.Size(203, 50);
            this.forceReset.TabIndex = 1;
            this.forceReset.Text = "Force Reset";
            this.forceReset.UseVisualStyleBackColor = true;
            this.forceReset.Click += new System.EventHandler(this.forceReset_Click);
            // 
            // load1button
            // 
            this.load1button.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.load1button.Location = new System.Drawing.Point(10, 70);
            this.load1button.Name = "load1button";
            this.load1button.Size = new System.Drawing.Size(130, 50);
            this.load1button.TabIndex = 2;
            this.load1button.Text = "Load 1";
            this.load1button.UseVisualStyleBackColor = true;
            this.load1button.Click += (sender, EventsArgs) => { loadButton_Click(sender, EventsArgs, 1); };
            // 
            // save1button
            // 
            this.save1button.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.save1button.Location = new System.Drawing.Point(145, 70);
            this.save1button.Name = "save1button";
            this.save1button.Size = new System.Drawing.Size(130, 50);
            this.save1button.TabIndex = 3;
            this.save1button.Text = "Save 1";
            this.save1button.UseVisualStyleBackColor = true;
            this.save1button.Click += (sender, EventsArgs) => { saveButton_Click(sender, EventsArgs, 1); };
            // 
            // load2button
            // 
            this.load2button.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.load2button.Location = new System.Drawing.Point(280, 70);
            this.load2button.Name = "load2button";
            this.load2button.Size = new System.Drawing.Size(130, 50);
            this.load2button.TabIndex = 2;
            this.load2button.Text = "Load 2";
            this.load2button.UseVisualStyleBackColor = true;
            this.load2button.Click += (sender, EventsArgs) => { loadButton_Click(sender, EventsArgs, 2); };
            // 
            // save2button
            // 
            this.save2button.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.save2button.Location = new System.Drawing.Point(415, 70);
            this.save2button.Name = "save2button";
            this.save2button.Size = new System.Drawing.Size(130, 50);
            this.save2button.TabIndex = 3;
            this.save2button.Text = "Save 2";
            this.save2button.UseVisualStyleBackColor = true;
            this.save2button.Click += (sender, EventsArgs) => { saveButton_Click(sender, EventsArgs, 2); };
            // 
            // load3button
            // 
            this.load3button.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.load3button.Location = new System.Drawing.Point(550, 70);
            this.load3button.Name = "load3button";
            this.load3button.Size = new System.Drawing.Size(130, 50);
            this.load3button.TabIndex = 2;
            this.load3button.Text = "Load 3";
            this.load3button.UseVisualStyleBackColor = true;
            this.load3button.Click += (sender, EventsArgs) => { loadButton_Click(sender, EventsArgs, 3); };
            // 
            // save3button
            // 
            this.save3button.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.save3button.Location = new System.Drawing.Point(685, 70);
            this.save3button.Name = "save3button";
            this.save3button.Size = new System.Drawing.Size(130, 50);
            this.save3button.TabIndex = 3;
            this.save3button.Text = "Save 3";
            this.save3button.UseVisualStyleBackColor = true;
            this.save3button.Click += (sender, EventsArgs) => { saveButton_Click(sender, EventsArgs, 3); };
            // 
            // Adjuster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 790);
            this.Controls.Add(this.save1button);
            this.Controls.Add(this.load1button);
            this.Controls.Add(this.save2button);
            this.Controls.Add(this.load2button);
            this.Controls.Add(this.save3button);
            this.Controls.Add(this.load3button);
            this.Controls.Add(this.forceReset);
            this.Controls.Add(this.resetButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Adjuster";
            this.Text = "Monitor Adjuster";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button forceReset;
        private System.Windows.Forms.Button load1button;
        private System.Windows.Forms.Button save1button;
        private System.Windows.Forms.Button load2button;
        private System.Windows.Forms.Button save2button;
        private System.Windows.Forms.Button load3button;
        private System.Windows.Forms.Button save3button;
    }
}

