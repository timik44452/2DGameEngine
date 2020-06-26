using System.Collections.Generic;

public abstract class Component
{
    public GameObject gameObject { get; set; }

    private List<Property> properties = new List<Property>();

    public List<Property> GetProperties()
    {
        return properties;
    }

    protected void SetPropertyValue(string propertyName, object value)
    {
        Property property = properties.Find(x => x.Name == propertyName);

        if (property == null)
        {
            property = new Property(propertyName);

            properties.Add(property);
        }

        property.Value = value;
    }
    protected void CreateProperty(string propertyName, object value)
    {
        Property property = properties.Find(x => x.Name == propertyName);

        if (property == null)
        {
            property = new Property(propertyName)
            {
                Value = value
            };

            properties.Add(property);
        }
    }
    protected bool TryGetPropertyValue<T>(string propertyName, out T destination)
    {
        Property property = properties.Find(x => x.Name == propertyName);

        if (property != null && property.Value is T)
        {
            destination = (T)property.Value;

            return true;
        }

        destination = default;

        return false;
    }
    protected T GetPropertyValue<T>(string propertyName)
    {
        Property property = properties.Find(x => x.Name == propertyName);

        if (property != null)
        {
            if (!(property.Value is T))
                throw new System.Exception($"Property {propertyName} cann't be converted to {typeof(T)}");

            return (T)property.Value;
        }

        throw new System.Exception($"Property {propertyName} hasn't finded");
    }

    public virtual void OnCreated()
    {
    }
    public virtual void FixedUpdate()
    {
    }
    public virtual void OnRenderer()
    {
    }
    public virtual void OnCollision(Vector point)
    {
    }
}

