using System.Collections.Generic;

public class GameObject
{
    public static GameObject underMouse { get; set; }

    public int Id;

    public int Layer { get; set; }

    public World world { get; set; }

    public Transform transform { get; private set; }
    public Renderer renderer { get; private set; }
    public PhysicBody physic { get; private set; }
    public Animation animation { get; private set; }
    public Camera camera { get; private set; }
    public Light light { get; private set; }
    public Collider collider { get; private set; }


    private Component[] components = new Component[0];

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

        if(component is Camera)
        {
            camera = component as Camera;
        }
        else if(component is PhysicBody)
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
        else if (component is Collider)
        {
            collider = component as Collider;
        }

        component.gameObject = this;

        var buffer = components;
        components = new Component[buffer.Length + 1];
        buffer.CopyTo(components, 0);
        components[buffer.Length] = component;
    }

    public T GetComponent<T>() where T : Component
    {
        for (int i = 0; i < components.Length; i++)
            if (components[i] is T)
                return (T)components[i];

        return default;
    }

    public IEnumerable<T> GetComponents<T>() where T : Component
    {
        for (int i = 0; i < components.Length; i++)
            if (components[i] is T)
                yield return (T)components[i];
    }

    private void AddTransform()
    {
        AddComponent(new Transform());
    }

    public void OnCollision(Vector point)
    {
        if (!RuntimeUtilities.RunningComponentSystem)
            return;

        for (int i = 0; i < components.Length; i++)
            components[i].OnCollision(point);
    }
    public void Update()
    {
        if (!RuntimeUtilities.RunningComponentSystem)
            return;

        for (int i = 0; i < components.Length; i++)
            components[i].OnRenderer();
    }
    public void FixedUpdate()
    {
        if (!RuntimeUtilities.RunningComponentSystem)
            return;

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

