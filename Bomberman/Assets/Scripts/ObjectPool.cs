using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject originalInstance;
    public int size;
    public bool isExpandable;

    private List<GameObject> _pool;


    private void Awake()
    {
        _pool = new List<GameObject>(size);

        for (int i = 0; i < size; i++)
        {
            GameObject newGameObject = (GameObject)Instantiate(originalInstance);
            newGameObject.SetActive(false);
            // _pool[i] = newGameObject;
            _pool.Add(newGameObject);
        }
    }

    public bool SpawnObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            GameObject obj = _pool[i];
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);

                return true;
            }
        }

        if (isExpandable)
        {
            GameObject obj = Instantiate(originalInstance);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            _pool.Add(obj);

            return true;
        }

        return false;
    }
}
