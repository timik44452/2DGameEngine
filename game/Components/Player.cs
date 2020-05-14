
public class Player : Component
{
    public float Speed = 0.4F;

    public override void Update()
    {
        float dx = 0;
        float dy = 0;

        if (Input.IsKeyPress("A"))
            dx = -1;
        if (Input.IsKeyPress("D"))
            dx = 1;
        if (Input.IsKeyPress("S"))
            dy = 1;
        if (Input.IsKeyPress("W"))
            dy = -1;

        if (Input.IsMousePress(MouseButton.LeftButton))
        {
            Vector d = Input.MousePosition - gameObject.transform.position;

            dx = d.X / System.Math.Abs(d.X);
            dy = d.Y / System.Math.Abs(d.Y);
        }


        Move(dx, dy);
    }

    public void Move(float deltaX, float deltaY)
    {
        //if (deltaX != 0)
        //{
        //    gameObject.transform.scale = new Vector(deltaX, 1);
        //}

        if (deltaY != 0 || deltaX != 0)
            gameObject.GetComponent<Animation>()?.Steep();

        gameObject.GetComponent<PhysicBody>().velocity = new Vector(deltaX, deltaY) * Speed;
    }
}