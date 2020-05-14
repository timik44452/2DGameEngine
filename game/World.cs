public class World
{
    private GameObject camera;
    private GameObject[] allObjects;
    private GameObject[] drawbleObjects;
    private GameObject[] physicActiveObjects;
    private GameObject[] lightObjects;
    private GameObject[] collidingObjects;

    public World()
    {
        allObjects = new GameObject[0];
        drawbleObjects = new GameObject[0];
        physicActiveObjects = new GameObject[0];
        lightObjects = new GameObject[0];
        collidingObjects = new GameObject[0];

        Fill();
    }

    private void Fill()
    {
        GameObject cameraObject = new GameObject();
        cameraObject.AddComponent(new Camera());
        cameraObject.GetComponent<Camera>().viewport = new Rect(-20, -12, 40, 24);

        var renderer0 = new Renderer(Resourcepack.GetResource<Sprite>("grass"));
        var renderer1 = new Renderer(Resourcepack.GetResource<Sprite>("tree"));
        var renderer2 = new Renderer(Resourcepack.GetResource<Sprite>("wx"));

        //var physicBody = new PhysicBody
        //{
        //    velocity = Vector.right * 0.02f,
        //    Damping = 1
        //};

        //GameObject _gameObject = new GameObject();

        //_gameObject.Layer = 1;
        //_gameObject.transform.position = new Vector(0, 0);
        //_gameObject.AddComponent(renderer1);
        ////_gameObject.AddComponent(physicBody);

        //CreateGameObject(_gameObject);

        CreateGameObject(cameraObject);

        GameObject[] randomObjects = new GameObject[10];
        for (int i = 0; i < 2; i++)
        {
            randomObjects[i] = new GameObject();
            randomObjects[i].Layer = 1;
            randomObjects[i].AddComponent(renderer1);
            randomObjects[i].AddComponent(new BoxCollider(new Vector(-0.5, -0.5), new Vector(0.5, -0.5), new Vector(0.5, 0.5), new Vector(-0.5, 0.5)));
            randomObjects[i].AddComponent(new PhysicBody { Damping = 1, velocity = Vector.up * 0.02f });

            CreateGameObject(randomObjects[i]);
        }
        randomObjects[0].transform.position = new Vector(0.5, -1.5);
        randomObjects[1].transform.position = new Vector(0, 1);
        randomObjects[1].physic.velocity = Vector.zero;
        //randomObjects[2].transform.position = new Vector(1, 1);
        //randomObjects[3].transform.position = new Vector(-1, 1);
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject.renderer != null)
            Remove(ref drawbleObjects, gameObject);

        if (gameObject.physic != null)
            Remove(ref physicActiveObjects, gameObject);

        if (gameObject.light != null)
            Remove(ref lightObjects, gameObject);

        if (gameObject.collider != null)
            Remove(ref collidingObjects, gameObject);

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

        if (gameObject.collider != null)
            Add(ref collidingObjects, gameObject);

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

    public GameObject[] GetCollidingObjects()
    {
        return collidingObjects;
    }

    public GameObject[] GetAllObjects()
    {
        return allObjects;
    }
}