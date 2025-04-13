
public abstract class BugBehaviour
{
    private readonly BugType _type;
    public BugType Type => _type;
    
    protected bool _isActive;

    public bool IsActive => _isActive;
 
    protected BugBehaviour(BugType type)
    {
        _type = type;
        _isActive = true;
    }
    
    public virtual void Update(){}
    public virtual void Pause(){}
    public virtual void Continue(){}
    public virtual void Remove(){}
}

public enum BugType
{
    LowGravity,
    NoCollision,
    SoundIncreasing,
}
