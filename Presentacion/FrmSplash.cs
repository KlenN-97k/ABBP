using System;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class FrmSplash : Form
    {
        public FrmSplash()
        {
            InitializeComponent();

            // 1. Inicializamos la ventana completamente transparente
            this.Opacity = 0.0;

            // Configuración del Timer y la barra
            timerSplash.Interval = 30;
            progressBar1.Value = 0;

            timerSplash.Tick += (s, e) =>
            {
                // 2. Efecto Fade-In: Aumentamos la opacidad poco a poco
                if (this.Opacity < 1.0)
                {
                    this.Opacity += 0.05; // Suma 5% de visibilidad en cada latido
                }

                // 3. Lógica de la barra de progreso (se ejecuta simultáneamente)
                if (progressBar1.Value < 100)
                {
                    progressBar1.Value += 2;
                }
                else
                {
                    // Cuando la barra se llena, detenemos el Timer y cerramos
                    timerSplash.Stop();
                    this.Close();
                }
            };

            timerSplash.Start();
        }

        private void label9_Click(object sender, EventArgs e)
        {
        }
    }
}