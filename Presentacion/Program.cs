using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading; 
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    internal static class Program
    {
        // Creamos una llave única para tu aplicación
        static Mutex mutex = new Mutex(true, "{APPB-Sistema-Incidencias-2027}");

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Intentamos tomar el control del Mutex. Si devuelve true, somos la primera instancia.
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Forzar a WinForms a usar seguridad TLS 1.2 (Requisito indispensable para que el Bot no falle)
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    using (FrmSplash splash = new FrmSplash())
                    {
                        splash.ShowDialog();
                    }

                    Application.Run(new FrmLogin());
                }
                finally
                {
                    // Al cerrar el programa por completo, liberamos la llave
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                // Si la llave ya estaba tomada (el programa ya estaba abierto), mostramos este mensaje y no hacemos nada más
                MessageBox.Show("El sistema ya se encuentra en ejecución.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}