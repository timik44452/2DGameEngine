using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollisionTest
{
    public partial class Form1 : Form
    {
        public GameObject body0;
        public GameObject body1;

        private Graphics graphics;

        private bool[] collisionBuffer0;
        private bool[] collisionBuffer1;

        public Form1()
        {
            InitializeComponent();

            body0 = new GameObject();
            body1 = new GameObject();

            body0.AddComponent(new PhysicBody());
            body1.AddComponent(new PhysicBody());

            body0.transform.scale = Vector.one * 70;
            body1.transform.scale = Vector.one * 70;

            graphics = CreateGraphics();

            body0.transform.position = new Vector(Width * 0.25F, Height) * 0.5F;
            body1.transform.position = new Vector(Width, Height) * 0.5F;

            body0.GetComponent<PhysicBody>().velocity = Vector.right * 10;
            body1.GetComponent<PhysicBody>().velocity = -Vector.right * 10;

            collisionBuffer0 = new bool[64 * 64];
            //collisionBuffer1 = new bool[64 * 64];

            body0.GetComponent<PhysicBody>().Recalculate();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            ClearCBuffer();

            graphics.FillRectangle(Brushes.Black, 0, 0, Width, Height);

            //DrawBody(body0.GetComponent<PhysicBody>(), collisionBuffer0);
            //DrawBody(body1.GetComponent<PhysicBody>(), collisionBuffer1);

            DrawBuffer(Pens.Orange, body0.GetComponent<PhysicBody>().collision_buffer);

            if (IsContact())
            {
                body0.GetComponent<PhysicBody>().velocity = Vector.zero;
                body1.GetComponent<PhysicBody>().velocity = Vector.zero;
            }

            body0.Update();
            body1.Update();
        }

        //private void DrawBody(PhysicBody body, bool[] collisionBuffer)
        //{
        //    Vector scale = body.gameObject.transform.scale;
        //    Vector position = body.gameObject.transform.position;
        //    Shape shape = body.shape;

        //    for (int i = 1; i < shape.links.Length; i++)
        //    {
        //        Vector vertex0 = shape.vertexes[shape.links[i - 1]];
        //        Vector vertex1 = shape.vertexes[shape.links[i]];

        //        vertex0.X *= scale.X;
        //        vertex0.Y *= scale.Y;
        //        vertex1.X *= scale.X;
        //        vertex1.Y *= scale.Y;

        //        vertex0 += position;
        //        vertex1 += position;

        //        for(float alpha = 0; alpha < 1F; alpha += 0.01F)
        //        {
        //            Vector point = Vector.Lerp(vertex0, vertex1, alpha);

        //            if(point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height)
        //            {
        //                int index = (int)(point.X + point.Y * Width);
        //                collisionBuffer[index] = true;
        //            }

        //            //graphics.DrawEllipse(Pens.Green, point.X, point.Y, 1, 1);
        //        }
                
        //        graphics.DrawLine(Pens.Green, vertex0.X, vertex0.Y, vertex1.X, vertex1.Y);
        //    }
        //}

        private void DrawBuffer(Pen pen, bool[] collisionBuffer)
        {
            //for(int x = 0; x < Width; x++)
            //    for(int y = 0; y < Height;y++)
            //    {
            //        int idx = x + y * Width;

            //        if(collisionBuffer0[idx] && collisionBuffer1[idx])
            //        {
            //            graphics.DrawEllipse(pen, x, y, 1, 1);
            //        }
            //    }
            //for (int i = 0; i < collisionBuffer0.Length; i++)
            //{
            //    if (collisionBuffer0[i] && collisionBuffer1[i])
            //    {
            //        graphics.DrawEllipse(pen, i % Width, i / Width, 1, 1);
            //    }
            //}

            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64; y++)
                {
                if (collisionBuffer[x + y * 63])
                {
                    graphics.DrawEllipse(pen, x, y, 1, 1);
                }
            }
        }

        private void ClearCBuffer()
        {
            for(int i = 0; i < collisionBuffer0.Length; i++)
            {
                collisionBuffer0[i] = false;
                //collisionBuffer1[i] = false;
            }
        }

        public bool IsContact()
        {
            for (int i = 0; i < collisionBuffer0.Length; i++)
            {
                if (collisionBuffer0[i] && collisionBuffer1[i])
                    return true;
            }
            return false;
        }
    }
}
