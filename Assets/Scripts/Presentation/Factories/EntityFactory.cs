using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using static UnityEngine.GraphicsBuffer;

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
        GameObjectCreationParameters paramsNow = new GameObjectCreationParameters();
        //paramsNow.
        var prefub = _entities.Find(e=>e.Model.EntityType==entityType);
        if (prefub != null)
        {
            return _container.InstantiatePrefabForComponent<Entity>(prefub);
            //return _container.Instantiate<Entity>();
            //var some = _container.InstantiatePrefab(prefub.gameObject, _container.DefaultParent).;
            //GameObject.Destroy(some.);
            //var root = SceneManager.GetActiveScene().GetRootGameObjects()[0].transform.parent;
            //var obj = _container.InstantiatePrefab(prefub);
            //Transform.DontDestroyOnLoad(obj, false);
            //SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
            //obj.gameObject.se
            //return _container.InstantiatePrefab(prefub.gameObject).GetComponent<Entity>();
            //return obj.GetComponent<Entity>();
        }

        return null;
    }
}
