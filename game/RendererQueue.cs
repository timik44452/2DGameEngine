using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RendererQueue
{
    public Vertex[] VertexBuffer;
    public RendererQueueItem[] RendererItems;

    private int RendererQueueIndex = 0;

    public RendererQueue()
    {
        VertexBuffer = new Vertex[0];
        RendererItems = new RendererQueueItem[0];
    }

    public void ResetQueueIndex()
    {
        RendererQueueIndex = 0;
    }

    public void SetVertexBufferItem(int i, float z, Sprite sprite, Vector point0, Vector point1)
    {
        //      p1
        // d----b
        // |   /|
        // |  / |
        // | /  |
        // |/   |
        // a ---c
        //p0

        VertexBuffer[i].x = point0.X;
        VertexBuffer[i].y = point0.Y;
        
        VertexBuffer[i + 1].x = point1.X;
        VertexBuffer[i + 1].y = point1.Y;

        VertexBuffer[i + 2].x = point1.X;
        VertexBuffer[i + 2].y = point0.Y;


        VertexBuffer[i + 3].x = point0.X;
        VertexBuffer[i + 3].y = point0.Y;

        VertexBuffer[i + 4].x = point0.X;
        VertexBuffer[i + 4].y = point1.Y;

        VertexBuffer[i + 5].x = point1.X;
        VertexBuffer[i + 5].y = point1.Y;

        VertexBuffer[i].z = z;
        VertexBuffer[i + 1].z = z;
        VertexBuffer[i + 2].z = z;
        VertexBuffer[i + 3].z = z;
        VertexBuffer[i + 4].z = z;
        VertexBuffer[i + 5].z = z;

        if (RendererItems.Length > 0 && sprite.UID != RendererItems[0].sprite.UID)
            RendererQueueIndex++;

        if(RendererQueueIndex >= RendererItems.Length)
        {
            var RendererItemsTemp = RendererItems;
            RendererItems = new RendererQueueItem[RendererQueueIndex + 1];
            RendererItemsTemp.CopyTo(RendererItems, 0);

            RendererItems[RendererQueueIndex].VertexIndex = i;
        }

        RendererItems[RendererQueueIndex].sprite = sprite;
        RendererItems[RendererQueueIndex].VertexCount = i + 6;
    }

    public void CheckVertexBuffer(int size)
    {
        if (size > VertexBuffer.Length)
        {
            VertexBuffer = new Vertex[size];

            // d----b
            // |   /|
            // |  / |
            // | /  |
            // |/   |
            // a ---c

            for (int i = 0; i < size; i += 6)
            {
                //a
                VertexBuffer[i].u = 0;
                VertexBuffer[i].v = 1;

                //b
                VertexBuffer[i + 1].u = 1;
                VertexBuffer[i + 1].v = 0;

                //c
                VertexBuffer[i + 2].u = 1;
                VertexBuffer[i + 2].v = 1;

                //a
                VertexBuffer[i + 3].u = 0;
                VertexBuffer[i + 3].v = 1;

                //d
                VertexBuffer[i + 4].u = 0;
                VertexBuffer[i + 4].v = 0;

                //b
                VertexBuffer[i + 5].u = 1;
                VertexBuffer[i + 5].v = 0;
            }
        }
    }
}