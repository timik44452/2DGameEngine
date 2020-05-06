using System;

public class PhysicBody : Component
{


    public bool IsStatic = false;

    public Vector velocity;

    public Shape shape { get; private set;  } = null;

    public bool[] collision_buffer;

    public Rect rect { get; private set; }

    public float Damping
    {
        get
        {
            return damping;
        }
        set
        {
            damping = Math.Max(Math.Min(damping, 1), 0);
        }
    }

    private float damping = 0.95F;

    private const int collision_buffer_size = 64;

    public PhysicBody()
    {
        Recalculate();
    }

    public void SetShape(Shape shape)
    {
        this.shape = shape;

        Recalculate();
    }

    public override void Update()
    {
        if (!IsStatic)
        {
            //velocity -= Vector.up * 0.098F;
            gameObject.transform.position += velocity;

            velocity = velocity * damping;
        }
    }

    public void Recalculate()
    {
        if (shape != null)
        {
            if (collision_buffer == null)
                collision_buffer = new bool[collision_buffer_size * collision_buffer_size];

            for (int i = 0; i < collision_buffer.Length; i++)
                collision_buffer[i] = false;

            shape.CopyTo(collision_buffer_size, collision_buffer_size, collision_buffer);

            rect = new Rect(Vector.one * collision_buffer_size * -0.5F, Vector.one * collision_buffer_size * 0.5F);
        }
        else
        {
            rect = Rect.Empty;
        }
    }

    public override void OnCollision(Vector point)
    {
        //gameObject.GetComponent<PhysicBody>().IsStatic = true;
    }
}