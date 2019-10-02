
public class Component
{
    public GameObject gameObject { get; set; }

    public virtual void OnCreated()
    {
    }
    public virtual void OnDraw()
    {
    }
    public virtual void FixedUpdate()
    {
    }
    public virtual void Update()
    {
    }
    public virtual void OnCollision(Vector point)
    {
    }
}

