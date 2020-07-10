using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject originalInstance;
    public int startSize;
    public bool isExpandable;

    public Transform overrideParentGameobject = null;

    private List<GameObject> _pool;

    private void Awake()
    {
        Debug.Assert(originalInstance);

        _pool = new List<GameObject>(startSize);

        Transform spawnedObjectParent = overrideParentGameobject != null ? overrideParentGameobject : this.transform;

        for(int i = 0; i < startSize; i++)
        {
            GameObject newGameObject =(GameObject)Instantiate(originalInstance, this.transform);
            newGameObject.SetActive(false);
           
            _pool.Add(newGameObject);
        }
    }

    public bool SpawnObject(Vector3 position, Quaternion rotation)
    {
        for(int i = 0; i < _pool.Count; i++)
        {
            GameObject obj = _pool[i];
            if(!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);

                return true;
            }
        }

        if(isExpandable)
        {
            Transform spawnedObjectParent = overrideParentGameobject != null ? overrideParentGameobject : this.transform;

            GameObject obj = Instantiate(originalInstance, spawnedObjectParent);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            _pool.Add(obj);

            return true;
        }

        Debug.LogWarning("Object Pool maxed out: " + this.gameObject.name);
        return false;
    }

    public void DespawnAll()
    {
        foreach(GameObject item in _pool)
        {
            if(item.activeInHierarchy)
                item.SetActive(false);
        }
    }

    public void Deallocate()
    {
        _pool.Clear();
    }
}
