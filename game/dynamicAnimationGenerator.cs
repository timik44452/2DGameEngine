using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class dynamicAnimationGenerator
{
    public static Sprite[] GetFrames()
    {
        int w = 64;
        int h = 64;
        Sprite[] spirtes = new Sprite[25];

        for (int i = 0; i < spirtes.Length; i++)
        {
            float alpha = i / (float)spirtes.Length;

            spirtes[i] = new Sprite(w, h);

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                   float l = new Vector(x - w / 2, y - h / 2).Length;
                    if (l < alpha * w / 2)
                    {
                        spirtes[i].SetColor(x, y, ColorAtlas.Orange);
                    }
                    else
                    {
                        spirtes[i].SetColor(x, y, ColorAtlas.Black);
                    }
                }



        }

        return spirtes;
    }
}