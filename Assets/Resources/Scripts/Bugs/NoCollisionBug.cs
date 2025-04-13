using UnityEngine;

public class NoCollisionBug : BugBehaviour
{
    private readonly GameObject _player;
    private readonly int _buggedLayer;
    private readonly int _normalLayer;
    
    public NoCollisionBug(GameObject player) : base(BugType.NoCollision)
    {
        _buggedLayer = LayerMask.NameToLayer("BuggedCollision");
        _player = player;
        _normalLayer = _player.layer;
        
        ApplyEffect();
    }

    public override void Pause()
    {
        _player.layer = _normalLayer;
        _isActive = false;
    }

    public override void Remove()
    {
        if (_player != null)
            _player.layer = _normalLayer;
    }

    public override void Continue()
    {
        ApplyEffect();
    }

    private void ApplyEffect()
    {
        _player.layer = _buggedLayer;
        _isActive = true;
    }
}