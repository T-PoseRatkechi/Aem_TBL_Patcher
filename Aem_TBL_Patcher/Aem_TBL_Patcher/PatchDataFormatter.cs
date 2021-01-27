using System;
using System.Collections.Generic;
using System.Text;

namespace Aem_TBL_Patcher
{
    class PatchDataFormatter
    {
        public static string ByteArrayToHexText(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", " ");
        }

        public static string ByteArrayToNameText(byte[] ba)
        {
            string name = null;
            for (int i = 0; i < ba.Length; i++)
            {
                if (ba[i] <= 0x7f)
                    name += Encoding.ASCII.GetString(new byte[] { ba[i] });
                else if (i != ba.Length - 1)
                {
                    name += $"[{BitConverter.ToString(ba[i..(i + 2)]).Replace("-", " ")}]";
                    i++;
                }
            }

            return name;
        }
    }
}
