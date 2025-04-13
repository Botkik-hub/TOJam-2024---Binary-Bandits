using UnityEngine;

public class LowGravityBug : BugBehaviour
{
    private const float GravityScale= 0.3f;
    
    private float _normalScale = 1;
    private readonly Rigidbody2D _rigidbody2D;
    
    public LowGravityBug(GameObject player) : base(BugType.LowGravity)
    {
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        ApplyEffect();
    }

    public override void Pause()
    {
        _rigidbody2D.gravityScale = _normalScale;
        _isActive = false;
    }

    public override void Remove()
    {
        if (_rigidbody2D != null)
            _rigidbody2D.gravityScale = _normalScale;
    }

    public override void Continue()
    {
        ApplyEffect();
    }

    private void ApplyEffect()
    {
        _normalScale = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = GravityScale;
        _isActive = true;
    }
}