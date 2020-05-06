
public static class ShapeAtlas
{
    public static Shape Quad => new Shape(
        new int[] 
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0
        }, 
        new Vector[] 
        {
            new Vector(-1F, -1F),
            new Vector(-1F,  1F),
            new Vector( 1F,  1F),
            new Vector( 1F, -1F)
        });
}