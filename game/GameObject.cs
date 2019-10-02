
using System;

public class GameObject
{
    public int Id;

    private Component[] components = new Component[0];

    public int Layer { get; set; }

    public World world { get; set; }
    public Transform transform { get; private set; }
    public Renderer renderer { get; private set; }
    public PhysicBody physic { get; private set; }
    public Animation animation { get; private set; }
    public Light light { get; private set; }


    public GameObject()
    {
        AddTransform();
    }

    public void AddComponent(Component component)
    {
        if (component == null)
        {
            return;
        }

        if(component is PhysicBody)
        {
            physic = component as PhysicBody;
        }
        else if (component is Renderer)
        {
            renderer = component as Renderer;
        }
        else if (component is Animation)
        {
            animation = component as Animation;
        }
        else if (component is Transform)
        {
            transform = component as Transform;
        }
        else if (component is Light)
        {
            light = component as Light;
        }

        component.gameObject = this;

        var buffer = components;
        components = new Component[buffer.Length + 1];
        buffer.CopyTo(components, 0);
        components[buffer.Length] = component;
    }

    public T GetComponent<T>()
    {
        for (int i = 0; i < components.Length; i++)
            if (components[i].GetType().Equals(typeof(T)))
                return (T)(object)components[i];

        return default;
    }

    private void AddTransform()
    {
        AddComponent(new Transform());
    }

    public void OnCollision(Vector point)
    {
        for (int i = 0; i < components.Length; i++)
            components[i].OnCollision(point);
    }
    public void Update()
    {
        for (int i = 0; i < components.Length; i++)
            components[i].Update();
    }
    public void FixedUpdate()
    {
        for (int i = 0; i < components.Length; i++)
            components[i].FixedUpdate();
    }

    public void OnCreated()
    {
        for (int i = 0; i < components.Length; i++)
            components[i].OnCreated();
    }

    public override bool Equals(object obj)
    {
        if(obj is GameObject)
        {
            return (obj as GameObject).Id == Id;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return 2108858624 + Id.GetHashCode();
    }
}

