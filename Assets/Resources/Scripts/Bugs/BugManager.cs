using System.Collections.Generic;
using UnityEngine;

public class BugManager : MonoBehaviour
{
    private List<BugBehaviour> _bugs = new List<BugBehaviour>();

    [SerializeField] 
    private List<BugType> addAtStartBugs = new List<BugType>();

    public void Start() // idk if this should be awake
    {
        //foreach (var addAtStartBug in addAtStartBugs)
        //{
        //    var bug = AddBug(addAtStartBug);
            
        //    // todo mb this should  be active by default
        //    bug.Pause();
        //}
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    var bug = GetBug(BugType.LowGravity);
        //    if (bug.IsActive)
        //        bug.Pause();
        //    else
        //        bug.Continue();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    var bug = GetBug(BugType.NoCollision);
        //    if (bug.IsActive)
        //        bug.Pause();
        //    else
        //        bug.Continue();
        //}
        UpdateBugs();
    }

    private void UpdateBugs()
    {
        foreach (var bug in _bugs)
        {
            if (bug.IsActive)
                bug.Update();
        }
    }

    public void OnDestroy()
    {
        foreach (var bugBehaviour in _bugs)
        {
            bugBehaviour.Remove();
        }
        _bugs.Clear();
    }

    public void RemoveBug(BugType type)
    {
        var bugIndex = _bugs.FindIndex(x => x.Type == type);
        if (bugIndex == -1)
        {
            return;
        }
        _bugs[bugIndex].Remove();
        _bugs.RemoveAt(bugIndex);
    }

    public BugBehaviour AddBug(BugType type)
    {
        _bugs.Add(GetNewBugByType(type));
        return _bugs[^1];
    }

    public BugBehaviour GetBug(BugType type)
    {
        return _bugs.Find(x => x.Type == type);
    }

    private BugBehaviour GetNewBugByType(BugType type)
    {
         switch (type)
         {
             case BugType.NoCollision:
                 return new NoCollisionBug(GameObject.FindWithTag("Player"));
             case BugType.LowGravity:
                 return new LowGravityBug(GameObject.FindWithTag("Player"));
             default:
                 Debug.LogError("Bug type is not registered", this);
                 return null;
         }       
    }
}
