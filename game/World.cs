
using System;

public class World
{
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

        //PhysMake();
        Fill();
        //Generate();
    }

    private void Generate()
    {
        GameObject gameObject0 = new GameObject();
        GameObject gameObject1 = new GameObject();
        GameObject gameObject2 = new GameObject();

        gameObject0.transform.position = new Vector(10, 10);
        gameObject1.transform.position = new Vector(20, 10);

        gameObject0.AddComponent(new Player());



        Renderer renderer = new Renderer();

        gameObject0.AddComponent(renderer);
        gameObject1.AddComponent(renderer);
        gameObject2.AddComponent(new Light());

        gameObject2.GetComponent<Light>().lightType = Light.LightType.Directional;


        CreateGameObject(gameObject0);
        CreateGameObject(gameObject1);
    }

    
    private void PhysMake()
    {
        GameObject gameObject0 = new GameObject();
        GameObject gameObject1 = new GameObject();

        Renderer renderer = new Renderer(Resourcepack.GetSprite("grass"));

        gameObject0.AddComponent(new PhysicBody());
        gameObject1.AddComponent(new PhysicBody());

        gameObject0.AddComponent(renderer);
        gameObject1.AddComponent(renderer);

        gameObject0.transform.position = new Vector(10, 10);
        gameObject1.transform.position = new Vector(30, 10);

        gameObject0.GetComponent<PhysicBody>().SetShape(ShapeAtlas.Quad);
        gameObject1.GetComponent<PhysicBody>().SetShape(ShapeAtlas.Quad);

        gameObject0.GetComponent<PhysicBody>().velocity = Vector.right * 1;
        gameObject1.GetComponent<PhysicBody>().velocity = Vector.right * -1;

        gameObject0.AddComponent(new Draggable());
        gameObject1.AddComponent(new Draggable());

        for (float alpha = 0; alpha < 1F; alpha+=0.01F)
        {
            float x = (float)Math.Sin(alpha * Math.PI * 2) * 15;
            float y = (float)Math.Cos(alpha * Math.PI * 2) * 15;

            GameObject gameObject = new GameObject();

            gameObject.transform.position = new Vector(20 + x, 20 + y);

            gameObject.AddComponent(renderer);

            gameObject.AddComponent(new PhysicBody());

            
            gameObject.GetComponent<PhysicBody>().IsStatic = true;
            gameObject.GetComponent<PhysicBody>().SetShape(ShapeAtlas.Quad);

            CreateGameObject(gameObject);

        }

        CreateGameObject(gameObject0);
        CreateGameObject(gameObject1);
    }
    private void Fill()
    {
        GameObject player = new GameObject();
        GameObject particle = new GameObject();
        GameObject gameObject = new GameObject();

        gameObject.transform.position = new Vector(25, 10);


        gameObject.AddComponent(new PhysicBody());
        //gameObject.GetComponent<PhysicBody>().IsStatic;
        gameObject.GetComponent<PhysicBody>().SetShape(ShapeAtlas.Quad);
        gameObject.AddComponent(new Renderer(Resourcepack.GetSprite("grass")));


        particle.transform.position = new Vector(25, 15);


        player.transform.position = new Vector(10, 10);

        player.AddComponent(new Player());
        player.AddComponent(new Animation(Resourcepack.GetSprites("27", "28", "29", "30", "31", "32", "33", "34", "35")));
        player.AddComponent(new Renderer());
        player.AddComponent(new PhysicBody());
        player.GetComponent<PhysicBody>().SetShape(ShapeAtlas.Quad);

        float speed = 5F;

        player.GetComponent<Player>().Speed = 0.02F * speed;
        player.GetComponent<Animation>().Speed = 0.2F * speed;

        player.Layer = 10;

        CreateGameObject(gameObject);
        CreateGameObject(particle);
        CreateGameObject(player);
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