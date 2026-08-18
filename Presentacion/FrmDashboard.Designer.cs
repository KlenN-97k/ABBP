namespace Presentacion
{
    partial class FrmDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDashboard));
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.cboRangoFecha = new System.Windows.Forms.ComboBox();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.flowTarjetas = new System.Windows.Forms.FlowLayoutPanel();
            this.panelTotal = new System.Windows.Forms.Panel();
            this.lblTotalTitulo = new System.Windows.Forms.Label();
            this.lblTotalValor = new System.Windows.Forms.Label();
            this.panelPendientes = new System.Windows.Forms.Panel();
            this.lblPendientesTitulo = new System.Windows.Forms.Label();
            this.lblPendientesValor = new System.Windows.Forms.Label();
            this.panelResueltos = new System.Windows.Forms.Panel();
            this.lblResueltosTitulo = new System.Windows.Forms.Label();
            this.lblResueltosValor = new System.Windows.Forms.Label();
            this.panelTiempoPromedio = new System.Windows.Forms.Panel();
            this.lblTiempoPromedioTitulo = new System.Windows.Forms.Label();
            this.lblTiempoPromedioValor = new System.Windows.Forms.Label();
            this.tabCharts = new System.Windows.Forms.TabControl();
            this.tabEstado = new System.Windows.Forms.TabPage();
            this.chartEstado = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPrioridad = new System.Windows.Forms.TabPage();
            this.chartPrioridad = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabArea = new System.Windows.Forms.TabPage();
            this.chartArea = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelToolbar.SuspendLayout();
            this.flowTarjetas.SuspendLayout();
            this.panelTotal.SuspendLayout();
            this.panelPendientes.SuspendLayout();
            this.panelResueltos.SuspendLayout();
            this.panelTiempoPromedio.SuspendLayout();
            this.tabCharts.SuspendLayout();
            this.tabEstado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartEstado)).BeginInit();
            this.tabPrioridad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPrioridad)).BeginInit();
            this.tabArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartArea)).BeginInit();
            this.SuspendLayout();
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.White;
            this.panelToolbar.Controls.Add(this.dtpHasta);
            this.panelToolbar.Controls.Add(this.dtpDesde);
            this.panelToolbar.Controls.Add(this.cboRangoFecha);
            this.panelToolbar.Controls.Add(this.btnRefrescar);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(857, 45);
            this.panelToolbar.TabIndex = 0;
            // 
            // dtpHasta
            // 
            this.dtpHasta.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.dtpHasta.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.dtpHasta.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(403, 5);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(181, 34);
            this.dtpHasta.TabIndex = 30;
            this.dtpHasta.Visible = false;
            // 
            // dtpDesde
            // 
            this.dtpDesde.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.dtpDesde.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.dtpDesde.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(216, 4);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(176, 34);
            this.dtpDesde.TabIndex = 29;
            this.dtpDesde.Visible = false;
            // 
            // cboRangoFecha
            // 
            this.cboRangoFecha.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRangoFecha.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRangoFecha.FormattingEnabled = true;
            this.cboRangoFecha.Location = new System.Drawing.Point(12, 4);
            this.cboRangoFecha.Name = "cboRangoFecha";
            this.cboRangoFecha.Size = new System.Drawing.Size(189, 36);
            this.cboRangoFecha.TabIndex = 12;
            // 
            // btnRefrescar
            // 
            this.btnRefrescar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefrescar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefrescar.FlatAppearance.BorderSize = 0;
            this.btnRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefrescar.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefrescar.ForeColor = System.Drawing.Color.White;
            this.btnRefrescar.Location = new System.Drawing.Point(692, 3);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(153, 35);
            this.btnRefrescar.TabIndex = 1;
            this.btnRefrescar.Text = "🔄 Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = false;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);
            // 
            // flowTarjetas
            // 
            this.flowTarjetas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.flowTarjetas.Controls.Add(this.panelTotal);
            this.flowTarjetas.Controls.Add(this.panelPendientes);
            this.flowTarjetas.Controls.Add(this.panelResueltos);
            this.flowTarjetas.Controls.Add(this.panelTiempoPromedio);
            this.flowTarjetas.Controls.Add(this.tabCharts);
            this.flowTarjetas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowTarjetas.Location = new System.Drawing.Point(0, 45);
            this.flowTarjetas.Name = "flowTarjetas";
            this.flowTarjetas.Size = new System.Drawing.Size(857, 505);
            this.flowTarjetas.TabIndex = 1;
            // 
            // panelTotal
            // 
            this.panelTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.panelTotal.Controls.Add(this.lblTotalTitulo);
            this.panelTotal.Controls.Add(this.lblTotalValor);
            this.panelTotal.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTotal.Location = new System.Drawing.Point(3, 3);
            this.panelTotal.Name = "panelTotal";
            this.panelTotal.Size = new System.Drawing.Size(207, 88);
            this.panelTotal.TabIndex = 0;
            // 
            // lblTotalTitulo
            // 
            this.lblTotalTitulo.AutoSize = true;
            this.lblTotalTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalTitulo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTotalTitulo.Location = new System.Drawing.Point(0, 50);
            this.lblTotalTitulo.Name = "lblTotalTitulo";
            this.lblTotalTitulo.Size = new System.Drawing.Size(85, 25);
            this.lblTotalTitulo.TabIndex = 3;
            this.lblTotalTitulo.Text = "📊 Total";
            this.lblTotalTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalValor
            // 
            this.lblTotalValor.AutoSize = true;
            this.lblTotalValor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalValor.Font = new System.Drawing.Font("Segoe UI Black", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalValor.ForeColor = System.Drawing.Color.White;
            this.lblTotalValor.Location = new System.Drawing.Point(0, 0);
            this.lblTotalValor.Name = "lblTotalValor";
            this.lblTotalValor.Size = new System.Drawing.Size(119, 50);
            this.lblTotalValor.TabIndex = 2;
            this.lblTotalValor.Text = "Valor";
            this.lblTotalValor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelPendientes
            // 
            this.panelPendientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.panelPendientes.Controls.Add(this.lblPendientesTitulo);
            this.panelPendientes.Controls.Add(this.lblPendientesValor);
            this.panelPendientes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPendientes.Location = new System.Drawing.Point(216, 3);
            this.panelPendientes.Name = "panelPendientes";
            this.panelPendientes.Size = new System.Drawing.Size(207, 88);
            this.panelPendientes.TabIndex = 1;
            // 
            // lblPendientesTitulo
            // 
            this.lblPendientesTitulo.AutoSize = true;
            this.lblPendientesTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPendientesTitulo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPendientesTitulo.ForeColor = System.Drawing.Color.White;
            this.lblPendientesTitulo.Location = new System.Drawing.Point(0, 50);
            this.lblPendientesTitulo.Name = "lblPendientesTitulo";
            this.lblPendientesTitulo.Size = new System.Drawing.Size(136, 25);
            this.lblPendientesTitulo.TabIndex = 4;
            this.lblPendientesTitulo.Text = "⏳ Pendientes";
            this.lblPendientesTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPendientesValor
            // 
            this.lblPendientesValor.AutoSize = true;
            this.lblPendientesValor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPendientesValor.Font = new System.Drawing.Font("Segoe UI Black", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPendientesValor.ForeColor = System.Drawing.Color.White;
            this.lblPendientesValor.Location = new System.Drawing.Point(0, 0);
            this.lblPendientesValor.Name = "lblPendientesValor";
            this.lblPendientesValor.Size = new System.Drawing.Size(119, 50);
            this.lblPendientesValor.TabIndex = 3;
            this.lblPendientesValor.Text = "Valor";
            this.lblPendientesValor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelResueltos
            // 
            this.panelResueltos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.panelResueltos.Controls.Add(this.lblResueltosTitulo);
            this.panelResueltos.Controls.Add(this.lblResueltosValor);
            this.panelResueltos.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelResueltos.Location = new System.Drawing.Point(429, 3);
            this.panelResueltos.Name = "panelResueltos";
            this.panelResueltos.Size = new System.Drawing.Size(207, 88);
            this.panelResueltos.TabIndex = 2;
            // 
            // lblResueltosTitulo
            // 
            this.lblResueltosTitulo.AutoSize = true;
            this.lblResueltosTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResueltosTitulo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResueltosTitulo.ForeColor = System.Drawing.Color.White;
            this.lblResueltosTitulo.Location = new System.Drawing.Point(0, 50);
            this.lblResueltosTitulo.Name = "lblResueltosTitulo";
            this.lblResueltosTitulo.Size = new System.Drawing.Size(125, 25);
            this.lblResueltosTitulo.TabIndex = 6;
            this.lblResueltosTitulo.Text = "✅ Resueltos";
            this.lblResueltosTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResueltosValor
            // 
            this.lblResueltosValor.AutoSize = true;
            this.lblResueltosValor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblResueltosValor.Font = new System.Drawing.Font("Segoe UI Black", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResueltosValor.ForeColor = System.Drawing.Color.White;
            this.lblResueltosValor.Location = new System.Drawing.Point(0, 0);
            this.lblResueltosValor.Name = "lblResueltosValor";
            this.lblResueltosValor.Size = new System.Drawing.Size(119, 50);
            this.lblResueltosValor.TabIndex = 5;
            this.lblResueltosValor.Text = "Valor";
            this.lblResueltosValor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelTiempoPromedio
            // 
            this.panelTiempoPromedio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(107)))), ((int)(((byte)(154)))));
            this.panelTiempoPromedio.Controls.Add(this.lblTiempoPromedioTitulo);
            this.panelTiempoPromedio.Controls.Add(this.lblTiempoPromedioValor);
            this.panelTiempoPromedio.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTiempoPromedio.Location = new System.Drawing.Point(642, 3);
            this.panelTiempoPromedio.Name = "panelTiempoPromedio";
            this.panelTiempoPromedio.Size = new System.Drawing.Size(207, 88);
            this.panelTiempoPromedio.TabIndex = 3;
            // 
            // lblTiempoPromedioTitulo
            // 
            this.lblTiempoPromedioTitulo.AutoSize = true;
            this.lblTiempoPromedioTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTiempoPromedioTitulo.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTiempoPromedioTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTiempoPromedioTitulo.Location = new System.Drawing.Point(0, 50);
            this.lblTiempoPromedioTitulo.Name = "lblTiempoPromedioTitulo";
            this.lblTiempoPromedioTitulo.Size = new System.Drawing.Size(194, 25);
            this.lblTiempoPromedioTitulo.TabIndex = 6;
            this.lblTiempoPromedioTitulo.Text = "⏱️ Tiempo Promedio";
            this.lblTiempoPromedioTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTiempoPromedioValor
            // 
            this.lblTiempoPromedioValor.AutoSize = true;
            this.lblTiempoPromedioValor.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTiempoPromedioValor.Font = new System.Drawing.Font("Segoe UI Black", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTiempoPromedioValor.ForeColor = System.Drawing.Color.White;
            this.lblTiempoPromedioValor.Location = new System.Drawing.Point(0, 0);
            this.lblTiempoPromedioValor.Name = "lblTiempoPromedioValor";
            this.lblTiempoPromedioValor.Size = new System.Drawing.Size(119, 50);
            this.lblTiempoPromedioValor.TabIndex = 5;
            this.lblTiempoPromedioValor.Text = "Valor";
            this.lblTiempoPromedioValor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabCharts
            // 
            this.tabCharts.Controls.Add(this.tabEstado);
            this.tabCharts.Controls.Add(this.tabPrioridad);
            this.tabCharts.Controls.Add(this.tabArea);
            this.tabCharts.Location = new System.Drawing.Point(3, 97);
            this.tabCharts.Name = "tabCharts";
            this.tabCharts.SelectedIndex = 0;
            this.tabCharts.Size = new System.Drawing.Size(846, 381);
            this.tabCharts.TabIndex = 4;
            // 
            // tabEstado
            // 
            this.tabEstado.Controls.Add(this.chartEstado);
            this.tabEstado.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.tabEstado.Location = new System.Drawing.Point(4, 25);
            this.tabEstado.Name = "tabEstado";
            this.tabEstado.Padding = new System.Windows.Forms.Padding(3);
            this.tabEstado.Size = new System.Drawing.Size(838, 352);
            this.tabEstado.TabIndex = 0;
            this.tabEstado.Text = "Por Estado";
            this.tabEstado.UseVisualStyleBackColor = true;
            this.tabEstado.UseWaitCursor = true;
            // 
            // chartEstado
            // 
            chartArea1.Name = "ChartArea1";
            this.chartEstado.ChartAreas.Add(chartArea1);
            this.chartEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartEstado.Legends.Add(legend1);
            this.chartEstado.Location = new System.Drawing.Point(3, 3);
            this.chartEstado.Name = "chartEstado";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartEstado.Series.Add(series1);
            this.chartEstado.Size = new System.Drawing.Size(832, 346);
            this.chartEstado.TabIndex = 0;
            this.chartEstado.Text = "chart1";
            this.chartEstado.UseWaitCursor = true;
            // 
            // tabPrioridad
            // 
            this.tabPrioridad.Controls.Add(this.chartPrioridad);
            this.tabPrioridad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPrioridad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.tabPrioridad.Location = new System.Drawing.Point(4, 25);
            this.tabPrioridad.Name = "tabPrioridad";
            this.tabPrioridad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrioridad.Size = new System.Drawing.Size(838, 352);
            this.tabPrioridad.TabIndex = 1;
            this.tabPrioridad.Text = "Por Prioridad";
            this.tabPrioridad.UseVisualStyleBackColor = true;
            // 
            // chartPrioridad
            // 
            chartArea2.Name = "ChartArea1";
            this.chartPrioridad.ChartAreas.Add(chartArea2);
            this.chartPrioridad.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartPrioridad.Legends.Add(legend2);
            this.chartPrioridad.Location = new System.Drawing.Point(3, 3);
            this.chartPrioridad.Name = "chartPrioridad";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartPrioridad.Series.Add(series2);
            this.chartPrioridad.Size = new System.Drawing.Size(832, 346);
            this.chartPrioridad.TabIndex = 0;
            this.chartPrioridad.Text = "chart1";
            // 
            // tabArea
            // 
            this.tabArea.Controls.Add(this.chartArea);
            this.tabArea.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabArea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(35)))), ((int)(((byte)(60)))));
            this.tabArea.Location = new System.Drawing.Point(4, 25);
            this.tabArea.Name = "tabArea";
            this.tabArea.Padding = new System.Windows.Forms.Padding(3);
            this.tabArea.Size = new System.Drawing.Size(838, 352);
            this.tabArea.TabIndex = 2;
            this.tabArea.Text = "Por Área";
            this.tabArea.UseVisualStyleBackColor = true;
            // 
            // chartArea
            // 
            chartArea3.Name = "ChartArea1";
            this.chartArea.ChartAreas.Add(chartArea3);
            this.chartArea.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chartArea.Legends.Add(legend3);
            this.chartArea.Location = new System.Drawing.Point(3, 3);
            this.chartArea.Name = "chartArea";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartArea.Series.Add(series3);
            this.chartArea.Size = new System.Drawing.Size(832, 346);
            this.chartArea.TabIndex = 0;
            this.chartArea.Text = "chart1";
            // 
            // FrmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(857, 550);
            this.Controls.Add(this.flowTarjetas);
            this.Controls.Add(this.panelToolbar);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.FrmDashboard_Load);
            this.panelToolbar.ResumeLayout(false);
            this.flowTarjetas.ResumeLayout(false);
            this.panelTotal.ResumeLayout(false);
            this.panelTotal.PerformLayout();
            this.panelPendientes.ResumeLayout(false);
            this.panelPendientes.PerformLayout();
            this.panelResueltos.ResumeLayout(false);
            this.panelResueltos.PerformLayout();
            this.panelTiempoPromedio.ResumeLayout(false);
            this.panelTiempoPromedio.PerformLayout();
            this.tabCharts.ResumeLayout(false);
            this.tabEstado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartEstado)).EndInit();
            this.tabPrioridad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPrioridad)).EndInit();
            this.tabArea.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.FlowLayoutPanel flowTarjetas;
        private System.Windows.Forms.Panel panelTotal;
        private System.Windows.Forms.Panel panelPendientes;
        private System.Windows.Forms.Panel panelResueltos;
        private System.Windows.Forms.Panel panelTiempoPromedio;
        private System.Windows.Forms.Label lblTotalTitulo;
        private System.Windows.Forms.Label lblTotalValor;
        private System.Windows.Forms.Label lblPendientesTitulo;
        private System.Windows.Forms.Label lblPendientesValor;
        private System.Windows.Forms.Label lblResueltosTitulo;
        private System.Windows.Forms.Label lblResueltosValor;
        private System.Windows.Forms.Label lblTiempoPromedioTitulo;
        private System.Windows.Forms.Label lblTiempoPromedioValor;
        private System.Windows.Forms.ComboBox cboRangoFecha;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.TabControl tabCharts;
        private System.Windows.Forms.TabPage tabEstado;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartEstado;
        private System.Windows.Forms.TabPage tabPrioridad;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPrioridad;
        private System.Windows.Forms.TabPage tabArea;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartArea;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}