using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private List<ILogicUpdate> components = new List<ILogicUpdate>();
    public Movement _movement
    {
        get => GenericNotImplementedError<Movement>.TryGet(movement, transform.parent.name);
      
        private set => movement = value;
    }

    public CollisionSenses _collisionSenses
    {
        get => GenericNotImplementedError<CollisionSenses>.TryGet(collisionSenses, transform.parent.name);
        private set => collisionSenses = value;
    }

    public Combat _combat
    {
        get => GenericNotImplementedError<Combat>.TryGet(combat, transform.parent.name);
        private set => combat = value;
    }

    public Stats _stats
    {
        get => GenericNotImplementedError<Stats>.TryGet(stats, transform.parent.name);
        private set => stats = value;
    }

    private Movement movement;
    private CollisionSenses collisionSenses;
    private Combat combat;
    private Stats stats;
    private void Awake()
    {
        _movement = GetComponentInChildren<Movement>();
        _collisionSenses = GetComponentInChildren<CollisionSenses>();
        _combat = GetComponentInChildren<Combat>();
        _stats = GetComponentInChildren<Stats>();
    }

    public void LogicUpdate()
    {
        foreach (ILogicUpdate component in components) 
        { 
            component.LogicUpdate();
        }
    }

    public void AddComponent(ILogicUpdate component)
    {
        if(!components.Contains(component))
        {
            components.Add(component);
        }
    }
}
