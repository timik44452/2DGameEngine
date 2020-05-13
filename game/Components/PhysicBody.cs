public class PhysicBody : Component
{
    public bool IsStatic = false;

    public Vector velocity;

    public Rect rect { get; private set; }

    public float Damping
    {
        get
        {
            return damping;
        }
        set
        {
            damping = GameMath.Clamp(value, 0, 1);
        }
    }

    private float damping = 0.95F;

    public PhysicBody()
    {
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
}