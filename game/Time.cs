
public static class Time
{
    public static float deltaTime 
    { 
        get => (float)(Graphic.graphics.new_frame_time - Graphic.graphics.old_frame_time).TotalMilliseconds; 
    }
}