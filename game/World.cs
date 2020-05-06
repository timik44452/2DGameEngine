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

        var renderer0 = new Renderer(Resourcepack.GetResource<Sprite>("grass"));
        var renderer1 = new Renderer(Resourcepack.GetResource<Sprite>("tree"));

        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                GameObject gameObject = new GameObject();

                gameObject.Layer = 1;
                gameObject.transform.position = new Vector(x, y);
                gameObject.AddComponent(renderer0);

                CreateGameObject(gameObject);
            }
        }

        GameObject _gameObject = new GameObject();

        _gameObject.Layer = 0;
        _gameObject.transform.position = new Vector(3, 4);
        _gameObject.AddComponent(renderer1);

        CreateGameObject(_gameObject);

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