using System;

public class Animation : Component
{
    public bool IsPlaying { get; private set; }
    public float Speed = 1F;
    public Sprite[] frames;
    private float timer = 0;


    public Animation(params Sprite[] sprites)
    {
        frames = sprites;
    }

    public void Play()
    {
        IsPlaying = true;
    }

    public void Pause()
    {
        IsPlaying = false;
    }

    public void Stop()
    {
        IsPlaying = false;
        timer = 0;
    }

    public override void OnRenderer()
    {
        if(gameObject.renderer == null)
        {
            return;
        }

        if (IsPlaying)
        {
            Steep();
        }

        gameObject.renderer.sprite = frames[(int)(timer * frames.Length)];
    }

    public void Steep()
    {
        timer += Speed * 0.1F;

        if (timer >= 1F)
            timer = 0;
        else if (timer < 0)
            timer = 0.99F;
    }
}