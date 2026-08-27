namespace Presentacion
{
    partial class FrmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnAuditoria = new System.Windows.Forms.Button();
            this.panelFooterUsuario = new System.Windows.Forms.Panel();
            this.btnMiPerfil = new System.Windows.Forms.Button();
            this.btnAcercaDe = new System.Windows.Forms.Button();
            this.lblUsuarioSidebar = new System.Windows.Forms.Label();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.btnGuias = new System.Windows.Forms.Button();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnReportes = new System.Windows.Forms.Button();
            this.btnAreas = new System.Windows.Forms.Button();
            this.btnUsuarios = new System.Windows.Forms.Button();
            this.btnIncidencias = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelSidebar.SuspendLayout();
            this.panelFooterUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.panelSidebar.Controls.Add(this.btnAuditoria);
            this.panelSidebar.Controls.Add(this.panelFooterUsuario);
            this.panelSidebar.Controls.Add(this.btnGuias);
            this.panelSidebar.Controls.Add(this.btnDashboard);
            this.panelSidebar.Controls.Add(this.btnReportes);
            this.panelSidebar.Controls.Add(this.btnAreas);
            this.panelSidebar.Controls.Add(this.btnUsuarios);
            this.panelSidebar.Controls.Add(this.btnIncidencias);
            this.panelSidebar.Controls.Add(this.picLogo);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(352, 881);
            this.panelSidebar.TabIndex = 0;
            this.panelSidebar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSidebar_Paint);
            // 
            // btnAuditoria
            // 
            this.btnAuditoria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnAuditoria.FlatAppearance.BorderSize = 0;
            this.btnAuditoria.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnAuditoria.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnAuditoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAuditoria.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuditoria.ForeColor = System.Drawing.Color.White;
            this.btnAuditoria.Location = new System.Drawing.Point(23, 535);
            this.btnAuditoria.Name = "btnAuditoria";
            this.btnAuditoria.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAuditoria.Size = new System.Drawing.Size(303, 52);
            this.btnAuditoria.TabIndex = 11;
            this.btnAuditoria.Text = "📋 Auditoría";
            this.btnAuditoria.UseVisualStyleBackColor = false;
            this.btnAuditoria.Click += new System.EventHandler(this.btnAuditoria_Click);
            // 
            // panelFooterUsuario
            // 
            this.panelFooterUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.panelFooterUsuario.Controls.Add(this.btnMiPerfil);
            this.panelFooterUsuario.Controls.Add(this.btnAcercaDe);
            this.panelFooterUsuario.Controls.Add(this.lblUsuarioSidebar);
            this.panelFooterUsuario.Controls.Add(this.btnCerrarSesion);
            this.panelFooterUsuario.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooterUsuario.Location = new System.Drawing.Point(0, 722);
            this.panelFooterUsuario.Name = "panelFooterUsuario";
            this.panelFooterUsuario.Size = new System.Drawing.Size(352, 159);
            this.panelFooterUsuario.TabIndex = 10;
            // 
            // btnMiPerfil
            // 
            this.btnMiPerfil.FlatAppearance.BorderSize = 0;
            this.btnMiPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMiPerfil.Image = global::Presentacion.Properties.Resources.usuario;
            this.btnMiPerfil.Location = new System.Drawing.Point(23, 13);
            this.btnMiPerfil.Name = "btnMiPerfil";
            this.btnMiPerfil.Size = new System.Drawing.Size(98, 72);
            this.btnMiPerfil.TabIndex = 11;
            this.btnMiPerfil.UseVisualStyleBackColor = true;
            this.btnMiPerfil.Click += new System.EventHandler(this.btnMiPerfil_Click);
            // 
            // btnAcercaDe
            // 
            this.btnAcercaDe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnAcercaDe.FlatAppearance.BorderSize = 0;
            this.btnAcercaDe.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnAcercaDe.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnAcercaDe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcercaDe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcercaDe.ForeColor = System.Drawing.Color.White;
            this.btnAcercaDe.Location = new System.Drawing.Point(3, 91);
            this.btnAcercaDe.Name = "btnAcercaDe";
            this.btnAcercaDe.Size = new System.Drawing.Size(164, 46);
            this.btnAcercaDe.TabIndex = 10;
            this.btnAcercaDe.Text = "ℹ️AcercaDe";
            this.btnAcercaDe.UseVisualStyleBackColor = false;
            this.btnAcercaDe.Click += new System.EventHandler(this.btnAcercaDe_Click);
            // 
            // lblUsuarioSidebar
            // 
            this.lblUsuarioSidebar.AutoSize = true;
            this.lblUsuarioSidebar.ForeColor = System.Drawing.Color.White;
            this.lblUsuarioSidebar.Location = new System.Drawing.Point(145, 24);
            this.lblUsuarioSidebar.Name = "lblUsuarioSidebar";
            this.lblUsuarioSidebar.Size = new System.Drawing.Size(79, 31);
            this.lblUsuarioSidebar.TabIndex = 1;
            this.lblUsuarioSidebar.Text = "label1";
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnCerrarSesion.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnCerrarSesion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesion.Location = new System.Drawing.Point(173, 91);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(164, 46);
            this.btnCerrarSesion.TabIndex = 9;
            this.btnCerrarSesion.Text = "🚪 Cerrar Sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // btnGuias
            // 
            this.btnGuias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnGuias.FlatAppearance.BorderSize = 0;
            this.btnGuias.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnGuias.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnGuias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuias.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuias.ForeColor = System.Drawing.Color.White;
            this.btnGuias.Location = new System.Drawing.Point(23, 389);
            this.btnGuias.Name = "btnGuias";
            this.btnGuias.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnGuias.Size = new System.Drawing.Size(303, 52);
            this.btnGuias.TabIndex = 8;
            this.btnGuias.Text = "📖 Guías";
            this.btnGuias.UseVisualStyleBackColor = false;
            this.btnGuias.Click += new System.EventHandler(this.btnGuias_Click);
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnDashboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(23, 462);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(303, 52);
            this.btnDashboard.TabIndex = 7;
            this.btnDashboard.Text = "📊 Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // btnReportes
            // 
            this.btnReportes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnReportes.FlatAppearance.BorderSize = 0;
            this.btnReportes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnReportes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportes.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportes.ForeColor = System.Drawing.Color.White;
            this.btnReportes.Location = new System.Drawing.Point(23, 612);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnReportes.Size = new System.Drawing.Size(303, 52);
            this.btnReportes.TabIndex = 6;
            this.btnReportes.Text = "🗒️Manual ";
            this.btnReportes.UseVisualStyleBackColor = false;
            this.btnReportes.Click += new System.EventHandler(this.btnReportes_Click);
            // 
            // btnAreas
            // 
            this.btnAreas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnAreas.FlatAppearance.BorderSize = 0;
            this.btnAreas.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnAreas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnAreas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreas.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAreas.ForeColor = System.Drawing.Color.White;
            this.btnAreas.Location = new System.Drawing.Point(23, 317);
            this.btnAreas.Name = "btnAreas";
            this.btnAreas.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAreas.Size = new System.Drawing.Size(303, 52);
            this.btnAreas.TabIndex = 5;
            this.btnAreas.Text = "🏢 Áreas";
            this.btnAreas.UseVisualStyleBackColor = false;
            this.btnAreas.Click += new System.EventHandler(this.btnAreas_Click);
            // 
            // btnUsuarios
            // 
            this.btnUsuarios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnUsuarios.FlatAppearance.BorderSize = 0;
            this.btnUsuarios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnUsuarios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsuarios.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUsuarios.ForeColor = System.Drawing.Color.White;
            this.btnUsuarios.Location = new System.Drawing.Point(23, 244);
            this.btnUsuarios.Name = "btnUsuarios";
            this.btnUsuarios.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnUsuarios.Size = new System.Drawing.Size(303, 52);
            this.btnUsuarios.TabIndex = 4;
            this.btnUsuarios.Text = "👥 Usuarios";
            this.btnUsuarios.UseVisualStyleBackColor = false;
            this.btnUsuarios.Click += new System.EventHandler(this.btnUsuarios_Click);
            // 
            // btnIncidencias
            // 
            this.btnIncidencias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.btnIncidencias.FlatAppearance.BorderSize = 0;
            this.btnIncidencias.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnIncidencias.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnIncidencias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIncidencias.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncidencias.ForeColor = System.Drawing.Color.White;
            this.btnIncidencias.Location = new System.Drawing.Point(23, 166);
            this.btnIncidencias.Name = "btnIncidencias";
            this.btnIncidencias.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnIncidencias.Size = new System.Drawing.Size(303, 52);
            this.btnIncidencias.TabIndex = 3;
            this.btnIncidencias.Text = "🛠️ Incidencias";
            this.btnIncidencias.UseVisualStyleBackColor = false;
            this.btnIncidencias.Click += new System.EventHandler(this.btnIncidencias_Click);
            // 
            // picLogo
            // 
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLogo.Image = global::Presentacion.Properties.Resources.Logo3;
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.picLogo.MaximumSize = new System.Drawing.Size(0, 174);
            this.picLogo.MinimumSize = new System.Drawing.Size(0, 174);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(352, 174);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(1444, 881);
            this.Controls.Add(this.panelSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sistema de Gestión de Incidencias - APPB";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.Resize += new System.EventHandler(this.FrmPrincipal_Resize);
            this.panelSidebar.ResumeLayout(false);
            this.panelFooterUsuario.ResumeLayout(false);
            this.panelFooterUsuario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnCerrarSesion;
        private System.Windows.Forms.Button btnGuias;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.Button btnAreas;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnIncidencias;
        private System.Windows.Forms.Panel panelFooterUsuario;
        private System.Windows.Forms.Label lblUsuarioSidebar;
        private System.Windows.Forms.Button btnAuditoria;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnAcercaDe;
        private System.Windows.Forms.Button btnMiPerfil;
    }
}