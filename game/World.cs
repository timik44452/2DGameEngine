using Service;
using System;

public class World
{
    public int CameraCount { get => cameras.Length; }

    private GameObject[] cameras;
    private GameObject[] allObjects;
    private GameObject[] drawbleObjects;
    private GameObject[] physicActiveObjects;
    private GameObject[] lightObjects;
    private GameObject[] collidingObjects;

    public World()
    {
        cameras = new GameObject[0];
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

        var renderer0 = new Renderer(Resourcepack.GetResource<Sprite>("floor"));
        var renderer1 = new Renderer(Resourcepack.GetResource<Sprite>("wall"));
        var renderer2 = new Renderer(Resourcepack.GetResource<Sprite>("tree"));

        int[] room = new int[]
            {
                1, 1, 1, 1, 0, 1, 1, 1, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 1,
                0, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 1, 1, 1, 0, 1, 1, 1, 1
            };

        int size = (int)Math.Sqrt(room.Length);

        for (int idx = 0; idx < room.Length; idx++)
        {
            int x = idx % size;
            int y = idx / size;

            GameObject gameObject = new GameObject();

            gameObject.Layer = 1;
            gameObject.transform.position = new Vector(x - size * 0.5F, y - size * 0.5F);

            if(room[idx] == 1)
            {
                gameObject.AddComponent(renderer1);
            }
            else
            {
                gameObject.AddComponent(renderer0);
            }

            CreateGameObject(gameObject);
        }

        GameObject menu = new GameObject();

        Texture texture = new Texture(16, 16);
        ImageProcessor.Fill(texture, ColorAtlas.Gray);
        Sprite sprite = new Sprite(texture);

        Renderer menuRenderer = new Renderer(sprite);

        menu.Layer = 2;
        menu.AddComponent(menuRenderer);

        CreateGameObject(menu);

        Vector scale = new Vector(0.225F, 1);
        Vector position = new Vector((scale.X - 1) * 0.5F, 0);

        menu.transform.space = Space.Screen;
        menu.transform.scale = scale;
        menu.transform.position = position;

        GameObject player = new GameObject();

        player.Layer = 2;

        player.AddComponent(new PhysicBody());
        player.AddComponent(new Player());
        player.AddComponent(renderer2);

        player.GetComponent<Player>().Speed = 0.1F;

        CreateGameObject(player);
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

        if (gameObject.collider != null)
            Remove(ref collidingObjects, gameObject);

        Remove(ref allObjects, gameObject);
    }

    public void CreateGameObject(GameObject gameObject)
    {
        gameObject.world = this;
        gameObject.Id = allObjects.Length;

        if (gameObject.camera != null)
            Add(ref cameras, gameObject);

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

    public GameObject[] GetCameraObjects()
    {
        return cameras;
    }

    public GameObject[] GetPhysicObjects()
    {
        return physicActiveObjects;
    }

    public GameObject[] GetDrawbleObjects()
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