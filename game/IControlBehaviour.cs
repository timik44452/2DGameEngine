using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    public interface IControlBehaviour
    {
        void OnLoad(object sender);
        void OnClosed(object sender);
        void OnPaint(object sender);
    }
}
