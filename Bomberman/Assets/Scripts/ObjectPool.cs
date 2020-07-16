using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject m_OriginalInstance;
    public int m_StartSize = 10;
    public bool m_IsExpandable = true;

    public Transform m_OverrideParentGameObject = null;

    private List<GameObject> m_Pool;

    private void Awake()
    {
        Debug.Assert(m_OriginalInstance);
        
        m_Pool = new List<GameObject>(m_StartSize);

        Transform spawnedObjectParent = m_OverrideParentGameObject != null ? m_OverrideParentGameObject : this.transform;

        for(int i = 0; i < m_StartSize; i++)
        {
            GameObject newGameObject =(GameObject)Instantiate(m_OriginalInstance, this.transform);
            newGameObject.SetActive(false);
           
            m_Pool.Add(newGameObject);
        }
    }

    public bool SpawnObject(Vector3 position, Quaternion rotation)
    {
        for(int i = 0; i < m_Pool.Count; i++)
        {
            GameObject obj = m_Pool[i];
            if(!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);

                return true;
            }
        }

        if(m_IsExpandable)
        {
            Transform spawnedObjectParent = m_OverrideParentGameObject != null ? m_OverrideParentGameObject : this.transform;

            GameObject obj = Instantiate(m_OriginalInstance, spawnedObjectParent);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            m_Pool.Add(obj);

            return true;
        }

        Debug.LogWarning("Object Pool maxed out: " + this.gameObject.name);
        return false;
    }

    public void DespawnAll()
    {
        foreach(GameObject item in m_Pool)
        {
            if(item.activeInHierarchy)
                item.SetActive(false);
        }
    }

    public void Deallocate()
    {
        m_Pool.Clear();
    }
}
