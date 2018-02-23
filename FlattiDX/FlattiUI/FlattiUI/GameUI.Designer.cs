namespace FlattiUI
{
    partial class GameUI
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameUI));
            this.SuspendLayout();
            // 
            // GameUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1493, 726);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameUI";
            this.Text = "FlattiDX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameUI_FormClosing);
            this.Load += new System.EventHandler(this.GameUI_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameUI_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameUI_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameUI_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GameUI_MouseUp);
            this.Resize += new System.EventHandler(this.GameUI_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

