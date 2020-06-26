using System;

public class Property
{
    public event Action OnPropertyChanged;

    public string Name { get; set; }

    public object Value 
    { 
        get => value;
        set
        {
            this.value = value;
            OnPropertyChanged?.Invoke();
        }
    }

    private object value;

    public Property(string name)
    {
        Name = name;
    }
}