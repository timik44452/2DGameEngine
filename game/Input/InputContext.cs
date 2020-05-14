using System;
using System.Collections.Generic;

public enum KeyState
{
    None = 0,
    KeyDown = 1,
    Pressed = 2,
    KeyUp = 3
}

public class InputContext
{
    public class KeyItem
    {
        public string name;
        public KeyState state;

        public KeyItem(string name)
        {
            this.name = name;
        }
    }

    public Vector mousePosition = Vector.zero;

    private List<KeyItem> keys = new List<KeyItem>();


    public KeyState GetKeyState(string name)
    {
        foreach (KeyItem item in keys)
        {
            if (item.name == name)
                return item.state;
        }

        return KeyState.None;
    }

    public void Update()
    {
        foreach (KeyItem item in keys)
        {
            if (item.state == KeyState.KeyDown)
            {
                item.state = KeyState.Pressed;
            }
            else if (item.state == KeyState.KeyUp)
            {
                item.state = KeyState.None;
            }
        }
    }

    public void KeyDown(string key)
    {
        KeyItem item = GetKeyOrCreate(key);

        item.state = KeyState.KeyDown;
    }

    public void KeyUp(string key)
    {
        KeyItem item = GetKeyOrCreate(key);

        item.state = KeyState.KeyUp;
    }

    public bool ContainsKey(string key)
    {
        return keys.Find(x => x.name == key) != null;
    }

    private KeyItem GetKeyOrCreate(string name)
    {
        KeyItem item = keys.Find(x => x.name == name);

        if (item == null)
        {
            item = new KeyItem(name);
            keys.Add(item);
        }

        return item;
    }
}