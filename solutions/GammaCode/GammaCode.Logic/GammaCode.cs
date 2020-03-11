using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaCode.Logic
{
    public static class GammaCode
    { 
        public static byte[] XorBytesWithGamma(byte[] data, int key)
        {
            var rand = new Random(key);
            byte[] gamma = new byte[data.Length];
            rand.NextBytes(gamma);

            byte[] xoredData = new byte[data.Length];
            for (int i = 0; i < data.Length; ++i)
            {
                xoredData[i] = (byte)(data[i] ^ gamma[i]);
            }

            return xoredData;
        }
    }
}
