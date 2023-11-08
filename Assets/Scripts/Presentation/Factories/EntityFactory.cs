using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EntityFactory
{
    private DiContainer _container;
    private List<Entity> _entities;
    public EntityFactory(DiContainer diContainer, List<Entity> entities)
    {
        _container = diContainer;
        _entities = entities;
    }

    public Entity Spawn(EntityType entityType)
    {
        var prefub = _entities.Find(e=>e.Model.EntityType==entityType);
        if (prefub != null)
        {
            return _container.InstantiatePrefabForComponent<Entity>(prefub);
        }

        return null;
    }
}
