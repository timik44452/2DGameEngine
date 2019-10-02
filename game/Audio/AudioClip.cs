using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    public struct AudioClip
    {
        public int[] data;
        public int[] durations;

        public AudioClip(int[] data, int[] durations)
        {
            this.data = data;
            this.durations = durations;
        }
    }
}
