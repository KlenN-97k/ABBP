namespace Presentacion
{
    partial class FrmPerfil
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPerfil));
            this.lblTelegramStatus = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelSeguridad = new System.Windows.Forms.Panel();
            this.lblOjoConfirmar = new System.Windows.Forms.Label();
            this.lblOjoActual = new System.Windows.Forms.Label();
            this.lblOjoNueva = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPasswordActual = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPasswordNueva = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtConfirmarPasswordNueva = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnQuitarFoto = new System.Windows.Forms.Button();
            this.btnCambiarPassword = new System.Windows.Forms.Button();
            this.picFoto = new System.Windows.Forms.PictureBox();
            this.btnCambiarFoto = new System.Windows.Forms.Button();
            this.btnGuardarPerfil = new System.Windows.Forms.Button();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsuarioLogin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelInfoPersonal = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRol = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panelSeguridad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
            this.panel1.SuspendLayout();
            this.panelInfoPersonal.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTelegramStatus
            // 
            this.lblTelegramStatus.AutoSize = true;
            this.lblTelegramStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelegramStatus.ForeColor = System.Drawing.Color.White;
            this.lblTelegramStatus.Location = new System.Drawing.Point(10, 458);
            this.lblTelegramStatus.Name = "lblTelegramStatus";
            this.lblTelegramStatus.Size = new System.Drawing.Size(123, 28);
            this.lblTelegramStatus.TabIndex = 33;
            this.lblTelegramStatus.Text = "👤 Nombre";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panelSeguridad
            // 
            this.panelSeguridad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.panelSeguridad.Controls.Add(this.lblOjoConfirmar);
            this.panelSeguridad.Controls.Add(this.lblOjoActual);
            this.panelSeguridad.Controls.Add(this.lblOjoNueva);
            this.panelSeguridad.Controls.Add(this.label11);
            this.panelSeguridad.Controls.Add(this.label10);
            this.panelSeguridad.Controls.Add(this.txtPasswordActual);
            this.panelSeguridad.Controls.Add(this.label9);
            this.panelSeguridad.Controls.Add(this.txtPasswordNueva);
            this.panelSeguridad.Controls.Add(this.label8);
            this.panelSeguridad.Controls.Add(this.txtConfirmarPasswordNueva);
            this.panelSeguridad.Location = new System.Drawing.Point(224, 267);
            this.panelSeguridad.Name = "panelSeguridad";
            this.panelSeguridad.Size = new System.Drawing.Size(596, 212);
            this.panelSeguridad.TabIndex = 75;
            // 
            // lblOjoConfirmar
            // 
            this.lblOjoConfirmar.AutoSize = true;
            this.lblOjoConfirmar.BackColor = System.Drawing.Color.Transparent;
            this.lblOjoConfirmar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblOjoConfirmar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOjoConfirmar.ForeColor = System.Drawing.Color.White;
            this.lblOjoConfirmar.Location = new System.Drawing.Point(480, 169);
            this.lblOjoConfirmar.Name = "lblOjoConfirmar";
            this.lblOjoConfirmar.Size = new System.Drawing.Size(40, 28);
            this.lblOjoConfirmar.TabIndex = 85;
            this.lblOjoConfirmar.Text = "👁";
            // 
            // lblOjoActual
            // 
            this.lblOjoActual.AutoSize = true;
            this.lblOjoActual.BackColor = System.Drawing.Color.Transparent;
            this.lblOjoActual.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblOjoActual.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOjoActual.ForeColor = System.Drawing.Color.White;
            this.lblOjoActual.Location = new System.Drawing.Point(480, 69);
            this.lblOjoActual.Name = "lblOjoActual";
            this.lblOjoActual.Size = new System.Drawing.Size(40, 28);
            this.lblOjoActual.TabIndex = 84;
            this.lblOjoActual.Text = "👁";
            // 
            // lblOjoNueva
            // 
            this.lblOjoNueva.AutoSize = true;
            this.lblOjoNueva.BackColor = System.Drawing.Color.Transparent;
            this.lblOjoNueva.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblOjoNueva.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOjoNueva.ForeColor = System.Drawing.Color.White;
            this.lblOjoNueva.Location = new System.Drawing.Point(480, 114);
            this.lblOjoNueva.Name = "lblOjoNueva";
            this.lblOjoNueva.Size = new System.Drawing.Size(40, 28);
            this.lblOjoNueva.TabIndex = 83;
            this.lblOjoNueva.Text = "👁";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Black", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(22, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(417, 41);
            this.label11.TabIndex = 80;
            this.label11.Text = "🔒 Seguridad y Contraseña";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(24, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(219, 28);
            this.label10.TabIndex = 66;
            this.label10.Text = "🔒 Contraseña Actual";
            // 
            // txtPasswordActual
            // 
            this.txtPasswordActual.BackColor = System.Drawing.SystemColors.Window;
            this.txtPasswordActual.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasswordActual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtPasswordActual.Location = new System.Drawing.Point(306, 66);
            this.txtPasswordActual.Name = "txtPasswordActual";
            this.txtPasswordActual.Size = new System.Drawing.Size(155, 34);
            this.txtPasswordActual.TabIndex = 67;
            this.txtPasswordActual.UseSystemPasswordChar = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(24, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(219, 28);
            this.label9.TabIndex = 68;
            this.label9.Text = "🔑 Nueva Contraseña";
            // 
            // txtPasswordNueva
            // 
            this.txtPasswordNueva.BackColor = System.Drawing.SystemColors.Window;
            this.txtPasswordNueva.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasswordNueva.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtPasswordNueva.Location = new System.Drawing.Point(306, 114);
            this.txtPasswordNueva.Name = "txtPasswordNueva";
            this.txtPasswordNueva.Size = new System.Drawing.Size(155, 34);
            this.txtPasswordNueva.TabIndex = 69;
            this.txtPasswordNueva.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(24, 169);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(320, 28);
            this.label8.TabIndex = 70;
            this.label8.Text = "🔑 Confirmar Nueva Contraseña";
            // 
            // txtConfirmarPasswordNueva
            // 
            this.txtConfirmarPasswordNueva.BackColor = System.Drawing.SystemColors.Window;
            this.txtConfirmarPasswordNueva.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmarPasswordNueva.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtConfirmarPasswordNueva.Location = new System.Drawing.Point(306, 169);
            this.txtConfirmarPasswordNueva.Name = "txtConfirmarPasswordNueva";
            this.txtConfirmarPasswordNueva.Size = new System.Drawing.Size(155, 34);
            this.txtConfirmarPasswordNueva.TabIndex = 71;
            this.txtConfirmarPasswordNueva.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Segoe UI Black", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(325, 50);
            this.label6.TabIndex = 74;
            this.label6.Text = "Perfil de Usuario";
            // 
            // btnQuitarFoto
            // 
            this.btnQuitarFoto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnQuitarFoto.FlatAppearance.BorderSize = 0;
            this.btnQuitarFoto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnQuitarFoto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnQuitarFoto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuitarFoto.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuitarFoto.ForeColor = System.Drawing.Color.White;
            this.btnQuitarFoto.Location = new System.Drawing.Point(15, 327);
            this.btnQuitarFoto.Name = "btnQuitarFoto";
            this.btnQuitarFoto.Size = new System.Drawing.Size(185, 46);
            this.btnQuitarFoto.TabIndex = 55;
            this.btnQuitarFoto.Text = "🗑 Quitar Foto";
            this.btnQuitarFoto.UseVisualStyleBackColor = false;
            this.btnQuitarFoto.Click += new System.EventHandler(this.btnQuitarFoto_Click_1);
            // 
            // btnCambiarPassword
            // 
            this.btnCambiarPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnCambiarPassword.FlatAppearance.BorderSize = 0;
            this.btnCambiarPassword.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnCambiarPassword.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnCambiarPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarPassword.ForeColor = System.Drawing.Color.White;
            this.btnCambiarPassword.Location = new System.Drawing.Point(224, 485);
            this.btnCambiarPassword.Name = "btnCambiarPassword";
            this.btnCambiarPassword.Size = new System.Drawing.Size(279, 46);
            this.btnCambiarPassword.TabIndex = 73;
            this.btnCambiarPassword.Text = "🔄 Cambiar Contraseña";
            this.btnCambiarPassword.UseVisualStyleBackColor = false;
            this.btnCambiarPassword.Click += new System.EventHandler(this.btnCambiarPassword_Click_1);
            // 
            // picFoto
            // 
            this.picFoto.BackColor = System.Drawing.Color.Transparent;
            this.picFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picFoto.Image = global::Presentacion.Properties.Resources.usuario;
            this.picFoto.Location = new System.Drawing.Point(15, 92);
            this.picFoto.Name = "picFoto";
            this.picFoto.Size = new System.Drawing.Size(185, 124);
            this.picFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFoto.TabIndex = 53;
            this.picFoto.TabStop = false;
            // 
            // btnCambiarFoto
            // 
            this.btnCambiarFoto.BackColor = System.Drawing.Color.Green;
            this.btnCambiarFoto.FlatAppearance.BorderSize = 0;
            this.btnCambiarFoto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnCambiarFoto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnCambiarFoto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarFoto.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarFoto.ForeColor = System.Drawing.Color.White;
            this.btnCambiarFoto.Location = new System.Drawing.Point(15, 255);
            this.btnCambiarFoto.Name = "btnCambiarFoto";
            this.btnCambiarFoto.Size = new System.Drawing.Size(185, 46);
            this.btnCambiarFoto.TabIndex = 54;
            this.btnCambiarFoto.Text = "📷 Cambiar Foto";
            this.btnCambiarFoto.UseVisualStyleBackColor = false;
            this.btnCambiarFoto.Click += new System.EventHandler(this.btnCambiarFoto_Click_1);
            // 
            // btnGuardarPerfil
            // 
            this.btnGuardarPerfil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.btnGuardarPerfil.FlatAppearance.BorderSize = 0;
            this.btnGuardarPerfil.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnGuardarPerfil.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.btnGuardarPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarPerfil.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarPerfil.ForeColor = System.Drawing.Color.White;
            this.btnGuardarPerfil.Location = new System.Drawing.Point(530, 485);
            this.btnGuardarPerfil.Name = "btnGuardarPerfil";
            this.btnGuardarPerfil.Size = new System.Drawing.Size(290, 46);
            this.btnGuardarPerfil.TabIndex = 72;
            this.btnGuardarPerfil.Text = "💾 Guardar Cambios";
            this.btnGuardarPerfil.UseVisualStyleBackColor = false;
            this.btnGuardarPerfil.Click += new System.EventHandler(this.btnGuardarPerfil_Click);
            // 
            // txtNombre
            // 
            this.txtNombre.BackColor = System.Drawing.SystemColors.Window;
            this.txtNombre.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtNombre.Location = new System.Drawing.Point(124, 74);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(155, 34);
            this.txtNombre.TabIndex = 63;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(310, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 28);
            this.label4.TabIndex = 62;
            this.label4.Text = "🆔 Usuario";
            // 
            // txtCorreo
            // 
            this.txtCorreo.BackColor = System.Drawing.SystemColors.Window;
            this.txtCorreo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCorreo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtCorreo.Location = new System.Drawing.Point(413, 74);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(155, 34);
            this.txtCorreo.TabIndex = 61;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(310, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 28);
            this.label3.TabIndex = 60;
            this.label3.Text = "✉️ Correo";
            // 
            // txtApellido
            // 
            this.txtApellido.BackColor = System.Drawing.SystemColors.Window;
            this.txtApellido.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApellido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtApellido.Location = new System.Drawing.Point(124, 128);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(155, 34);
            this.txtApellido.TabIndex = 59;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(16, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 28);
            this.label2.TabIndex = 58;
            this.label2.Text = "👤 Apellido";
            // 
            // txtUsuarioLogin
            // 
            this.txtUsuarioLogin.BackColor = System.Drawing.SystemColors.Window;
            this.txtUsuarioLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuarioLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtUsuarioLogin.Location = new System.Drawing.Point(413, 128);
            this.txtUsuarioLogin.Name = "txtUsuarioLogin";
            this.txtUsuarioLogin.ReadOnly = true;
            this.txtUsuarioLogin.Size = new System.Drawing.Size(155, 34);
            this.txtUsuarioLogin.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 28);
            this.label1.TabIndex = 56;
            this.label1.Text = "👤 Nombre";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.panel1.Controls.Add(this.picFoto);
            this.panel1.Controls.Add(this.btnQuitarFoto);
            this.panel1.Controls.Add(this.btnCambiarFoto);
            this.panel1.Controls.Add(this.lblTelegramStatus);
            this.panel1.Location = new System.Drawing.Point(8, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 495);
            this.panel1.TabIndex = 76;
            // 
            // panelInfoPersonal
            // 
            this.panelInfoPersonal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.panelInfoPersonal.Controls.Add(this.label5);
            this.panelInfoPersonal.Controls.Add(this.txtRol);
            this.panelInfoPersonal.Controls.Add(this.label7);
            this.panelInfoPersonal.Controls.Add(this.txtApellido);
            this.panelInfoPersonal.Controls.Add(this.label1);
            this.panelInfoPersonal.Controls.Add(this.label2);
            this.panelInfoPersonal.Controls.Add(this.txtNombre);
            this.panelInfoPersonal.Controls.Add(this.label3);
            this.panelInfoPersonal.Controls.Add(this.txtCorreo);
            this.panelInfoPersonal.Controls.Add(this.label4);
            this.panelInfoPersonal.Controls.Add(this.txtUsuarioLogin);
            this.panelInfoPersonal.Location = new System.Drawing.Point(224, 36);
            this.panelInfoPersonal.Name = "panelInfoPersonal";
            this.panelInfoPersonal.Size = new System.Drawing.Size(596, 225);
            this.panelInfoPersonal.TabIndex = 77;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(18, 181);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(3);
            this.label5.Size = new System.Drawing.Size(83, 34);
            this.label5.TabIndex = 64;
            this.label5.Text = "🏷️ Rol";
            // 
            // txtRol
            // 
            this.txtRol.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtRol.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRol.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.txtRol.Location = new System.Drawing.Point(124, 181);
            this.txtRol.Name = "txtRol";
            this.txtRol.ReadOnly = true;
            this.txtRol.Size = new System.Drawing.Size(155, 27);
            this.txtRol.TabIndex = 65;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Black", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(14, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(386, 41);
            this.label7.TabIndex = 78;
            this.label7.Text = "ℹ️ Información Personal";
            // 
            // FrmPerfil
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(842, 633);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelSeguridad);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCambiarPassword);
            this.Controls.Add(this.btnGuardarPerfil);
            this.Controls.Add(this.panelInfoPersonal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPerfil";
            this.Text = "Mi Perfil";
            this.Load += new System.EventHandler(this.FrmPerfil_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panelSeguridad.ResumeLayout(false);
            this.panelSeguridad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelInfoPersonal.ResumeLayout(false);
            this.panelInfoPersonal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTelegramStatus;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picFoto;
        private System.Windows.Forms.Button btnQuitarFoto;
        private System.Windows.Forms.Button btnCambiarFoto;
        private System.Windows.Forms.Panel panelSeguridad;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCambiarPassword;
        private System.Windows.Forms.Button btnGuardarPerfil;
        private System.Windows.Forms.TextBox txtConfirmarPasswordNueva;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPasswordNueva;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPasswordActual;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsuarioLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelInfoPersonal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRol;
        private System.Windows.Forms.Label lblOjoActual;
        private System.Windows.Forms.Label lblOjoNueva;
        private System.Windows.Forms.Label lblOjoConfirmar;
    }
}