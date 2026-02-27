using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BPFC_System
{
    public static class ControlExtensions
    {
        public static void EnableDoubleBuffering(this Control control)
        {
            typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(control, true, null);
        }

        public static void EnableDoubleBuffering(this Form form)
        {
            foreach (Control control in GetAllControls(form))
            {
                EnableDoubleBuffering(control);
            }
        }

        private static IEnumerable<Control> GetAllControls(Control control)
        {
            foreach (Control ctrl in control.Controls)
            {
                yield return ctrl;

                foreach (Control childCtrl in GetAllControls(ctrl))
                {
                    yield return childCtrl;
                }
            }
        }
    }
}