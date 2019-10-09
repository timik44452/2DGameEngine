﻿
using System;

public class World
{
    private GameObject camera;
    private GameObject[] allObjects;
    private GameObject[] drawbleObjects;
    private GameObject[] physicActiveObjects;
    private GameObject[] lightObjects;

    public World()
    {
        allObjects = new GameObject[0];
        drawbleObjects = new GameObject[0];
        physicActiveObjects = new GameObject[0];
        lightObjects = new GameObject[0];

        Fill();
    }

    private void Fill()
    {
        GameObject cameraObject = new GameObject();
        cameraObject.AddComponent(new Camera());
        cameraObject.GetComponent<Camera>().viewport = new Rect(-20, -12, 40, 24);

        var renderer = new Renderer(Resourcepack.GetResource<Sprite>("grass"));

        for (int x = -100; x < 100; x++)
        {
            for (int y = -100; y < 100; y++)
            {
                GameObject gameObject = new GameObject();

                gameObject.Layer = 1;
                gameObject.transform.position = new Vector(x, y);
                gameObject.AddComponent(renderer);

                CreateGameObject(gameObject);
            }
        }

        CreateGameObject(cameraObject);
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject.renderer != null)
            Remove(ref drawbleObjects, gameObject);

        if (gameObject.physic != null)
            Remove(ref physicActiveObjects, gameObject);

        if (gameObject.light != null)
            Remove(ref lightObjects, gameObject);

        Remove(ref allObjects, gameObject);
    }

    public void CreateGameObject(GameObject gameObject)
    {
        gameObject.world = this;
        gameObject.Id = allObjects.Length;

        if (gameObject.camera != null)
            camera = gameObject;

        if (gameObject.renderer != null)
            Add(ref drawbleObjects, gameObject);

        if (gameObject.physic != null)
            Add(ref physicActiveObjects, gameObject);

        if (gameObject.light != null)
            Add(ref lightObjects, gameObject);

        Add(ref allObjects, gameObject);

        gameObject.OnCreated();
    }

    private void Remove(ref GameObject[] array, GameObject gameObject)
    {
        var buffer = array;
        array = new GameObject[buffer.Length - 1];

        int _i = 0;
        for (int i = 0; i < buffer.Length; i++)
        {
            if (!buffer[i].Equals(gameObject))
            {
                array[_i] = buffer[i];
                _i++;
            }
        }
    }   

    private void Add(ref GameObject[] array, GameObject gameObject)
    {
        var buffer = array;
        array = new GameObject[buffer.Length + 1];
        buffer.CopyTo(array, 0);
        array[buffer.Length] = gameObject;
    }

    public GameObject GetCameraObject()
    {
        return camera;
    }

    public GameObject[] GetPhysicObjects()
    {
        return physicActiveObjects;
    }

    public GameObject[] GetViewedObjects()
    {
        return drawbleObjects;
    }

    public GameObject[] GetLightObjects()
    {
        return lightObjects;
    }

    public GameObject[] GetAllObjects()
    {
        return allObjects;
    }
}