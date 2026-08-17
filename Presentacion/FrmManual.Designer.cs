namespace Presentacion
{
    partial class FrmManual
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManual));
            this.treeSecciones = new System.Windows.Forms.TreeView();
            this.rtbContenido = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // treeSecciones
            // 
            this.treeSecciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.treeSecciones.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeSecciones.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeSecciones.FullRowSelect = true;
            this.treeSecciones.HideSelection = false;
            this.treeSecciones.ItemHeight = 32;
            this.treeSecciones.Location = new System.Drawing.Point(0, 0);
            this.treeSecciones.Name = "treeSecciones";
            this.treeSecciones.Size = new System.Drawing.Size(209, 450);
            this.treeSecciones.TabIndex = 0;
            // 
            // rtbContenido
            // 
            this.rtbContenido.BackColor = System.Drawing.Color.White;
            this.rtbContenido.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContenido.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbContenido.Location = new System.Drawing.Point(209, 0);
            this.rtbContenido.Name = "rtbContenido";
            this.rtbContenido.ReadOnly = true;
            this.rtbContenido.Size = new System.Drawing.Size(591, 450);
            this.rtbContenido.TabIndex = 1;
            this.rtbContenido.Text = "";
            // 
            // FrmManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbContenido);
            this.Controls.Add(this.treeSecciones);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmManual";
            this.Text = "Manual del Sistema";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeSecciones;
        private System.Windows.Forms.RichTextBox rtbContenido;
    }
}