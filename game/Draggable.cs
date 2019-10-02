class Draggable : Component
{
    public override void FixedUpdate()
    {
        if (gameObject.physic != null )
        {
            if(gameObject.physic.rect.Contain(Input.MousePosition - gameObject.transform.position))
            {
                gameObject.physic.velocity += Vector.up;
            }
        }
    }
}