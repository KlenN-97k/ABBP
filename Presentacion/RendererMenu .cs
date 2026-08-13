using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public class RendererMenu : ToolStripProfessionalRenderer
    {
        public RendererMenu() : base(new ColoresMenu()) { }
    }

    public class ColoresMenu : ProfessionalColorTable
    {
        private static readonly Color AzulMarino = Color.FromArgb(21, 50, 80);
        private static readonly Color AzulAcero = Color.FromArgb(43, 107, 154);

        public override Color MenuStripGradientBegin => AzulMarino;
        public override Color MenuStripGradientEnd => AzulMarino;
        public override Color MenuItemSelected => AzulAcero;
        public override Color MenuItemSelectedGradientBegin => AzulAcero;
        public override Color MenuItemSelectedGradientEnd => AzulAcero;
        public override Color MenuItemPressedGradientBegin => AzulAcero;
        public override Color MenuItemPressedGradientEnd => AzulAcero;
        public override Color MenuBorder => AzulMarino;
        public override Color MenuItemBorder => AzulAcero;
        public override Color ImageMarginGradientBegin => AzulMarino;
        public override Color ImageMarginGradientMiddle => AzulMarino;
        public override Color ImageMarginGradientEnd => AzulMarino;
    }
}
