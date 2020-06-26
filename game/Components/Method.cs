using System;

public class Method
{
    public string Name { get; }

    public Action callback;

    public Method(string name, Action action)
    {
        callback = action;
    }
}