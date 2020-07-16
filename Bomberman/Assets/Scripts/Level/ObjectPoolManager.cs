using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class ObjectPool2 : MonoBehaviour
//{
//    public GameObject m_OriginalInstance;
//    public int m_Capacity = 10;
//    public bool m_IsExpandable = true;

//    public Transform m_OverrideParentTransform = null;

//    private List<GameObject> m_Pool;

//    public ObjectPool2(GameObject prefab, int capacity = 10, bool expandable = true, Transform parentTransform = null)
//    {
//        Debug.Assert(prefab);
//        m_OriginalInstance = prefab;
//        m_Capacity = capacity;
//        m_IsExpandable = expandable;
//        m_OverrideParentTransform = parentTransform;
//    }

//    private void Populate()
//    {
//        m_Pool = new List<GameObject>(m_Capacity);

//        Transform spawnedObjectParent = m_OverrideParentTransform != null ? m_OverrideParentTransform : this.transform;

//        for(int i = 0; i < m_Capacity; i++)
//        {
//            GameObject newGameObject =(GameObject)Instantiate(m_OriginalInstance, this.transform);
//            newGameObject.SetActive(false);

//            m_Pool.Add(newGameObject);
//        }
//    }

//}

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager sm_Instance;

    public List<ObjectPool> m_ObjectPools;

    private Dictionary<int, ObjectPool> m_PoolMap = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        sm_Instance = this;
        DontDestroyOnLoad(this.gameObject);

        Init();
    }

    public static ObjectPoolManager Instance()
    {
        return sm_Instance;
    }

    private void Init()
    {
        foreach(ObjectPool pool in m_ObjectPools)
            m_PoolMap.Add(pool.m_OriginalInstance.GetInstanceID(), pool);
    }

    public bool Spawn(int prefabID, Vector3 position, Quaternion rotation)
    {
        if(!m_PoolMap.ContainsKey(prefabID))
        {
            Debug.LogError("Trying to spawn object that has no object pool");
            return false;
        }

        return m_PoolMap[prefabID].SpawnObject(position, rotation);
    }

    public bool DespawnPool(int prefabID)
    {
        if(!m_PoolMap.ContainsKey(prefabID))
        {
            Debug.LogError("Trying to object type which has no object pool.");
            return false;
        }

        m_PoolMap[prefabID].DespawnAll();
        return true;
    }

    public void DeallocateAllPools()
    {
        foreach(int id in m_PoolMap.Keys)
            DeallocatePool(id);

        Debug.Log("All object pools have been cleared.");
    }

    public bool DeallocatePool(int prefabID)
    {
        if(!m_PoolMap.ContainsKey(prefabID))
        {
            Debug.LogError("Trying to clear object pool that does not exist.");
            return false;
        }

        m_PoolMap[prefabID].Deallocate();
        return true;
    }
}
