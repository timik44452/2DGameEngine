public class CollisionManager
{
    public void CalculateCollision(GameObject[] physicObjects)
    {
        //for (int i = 0; i < physicObjects.Length; i++)
        //{
        //    PhysicBody physic = physicObjects[i].physic;
        //    Rect currentRect = new Rect(physic.gameObject.transform.position, physic.gameObject.transform.scale * 64);

        //    if(physic.shape == null)
        //    {
        //        continue;
        //    }

        //    if (!physic.velocity.Equals(Vector.zero))
        //    {
        //        for (int t = 0; t < physicObjects.Length; t++)
        //        {
        //            if (t != i)
        //            {
        //                PhysicBody _physic = physicObjects[t].physic;

        //                if (_physic.shape == null)
        //                {
        //                    continue;
        //                }

        //                Rect rect = new Rect(_physic.gameObject.transform.position, _physic.gameObject.transform.scale * 64);

        //                CollisionHandler(_physic, physic);

        //                //if (currentRect.Contain(rect))
        //                //{
        //                //    physic.velocity = Vector.zero;
        //                //    _physic.velocity = Vector.zero;
        //                //    //for (int _i = 0; _i < physic.collision_buffer.Length; _i++)
        //                //    //    if (physic.collision_buffer[_i] && _physic.collision_buffer[_i])
        //                //    //    {
        //                //    //        physicObjects[i].OnCollision(Vector.zero);
        //                //    //        physicObjects[t].OnCollision(Vector.zero);
        //                //    //    }
        //                //}
        //            }
        //        }
        //    }
        //}
    }

    public static void CollisionHandler(PhysicBody body0, PhysicBody body1)
    {
        for(int i = 0; i < 10; i ++)
        {
            float delta_x = (body0.gameObject.transform.position.X - body1.gameObject.transform.position.X) * 32;
            float delta_y = (body0.gameObject.transform.position.Y - body1.gameObject.transform.position.Y) * 32;

            if (System.Math.Abs(delta_x) < 32 &&
                System.Math.Abs(delta_y) < 32)
            {
                if(i == 0)
                {
                    body0.velocity = Vector.zero;
                    body1.velocity = Vector.zero;
                }

                Vector direction = 0.001F * (body1.gameObject.transform.position - body0.gameObject.transform.position).Normalized;

                body0.velocity -= direction;
                body1.velocity += direction;

                //CollisionHandler(body0, body1);
                //physicObjects[i].OnCollision(Vector.zero);
                //physicObjects[t].OnCollision(Vector.zero);
            }
            else
            {
                break;
            }
        }
    }
}