using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct EnumObj<T, S> where T : Object where S : System.Enum
{
    public S Type;
    public T obj;
}
[System.Serializable]
public class MaskingObjectPool
{
    public GameObject obj;
    [SerializeField] private int maxPool = 5;
    [SerializeField] private Queue<MaskingPoolItem> queueObjects = new Queue<MaskingPoolItem>();

    private System.Action<GameObject> OnPoolSpawned;

    // Initialize pool objects
    public virtual void Initialize()
    {
        // Instante obj and set inactive
        for (int i = 0; i < maxPool; i++)
        {
            SpawnPoolObject();
        }
    }

    // Spawn new pool object
    void SpawnPoolObject()
    {
        GameObject objPool = MonoBehaviour.Instantiate(obj);
        //Add PoolItem script
        MaskingPoolItem pl = objPool.AddComponent<MaskingPoolItem>();
        pl.Subscribe(OnPoolItemInactive);
        objPool.gameObject.SetActive(false);
        OnPoolSpawned?.Invoke(objPool);
    }

    void OnPoolItemInactive(MaskingPoolItem item)
    {
        queueObjects.Enqueue(item);
    }

    // Get objectfrom pool
    public GameObject GetObj(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        // spawn new item if queue is empty
        if (queueObjects.Count <= 0)
        {
            maxPool++;
            SpawnPoolObject();
        }

        // get the object from queue
        GameObject obj = queueObjects.Dequeue().gameObject;
        // set its position
        // and rotation
        // and parenting
        obj.gameObject.transform.position = position;
        obj.gameObject.transform.rotation = rotation;
        if (parent)
            obj.transform.SetParent(parent);

        // enable object
        obj.SetActive(true);

        return obj;
    }

    public GameObject GetObj() => GetObj(Vector3.zero, Quaternion.identity);
    // Subscribe events
    public void SubscribeEventObjectSpawned(System.Action<GameObject> action)
    {
        OnPoolSpawned += action;
    }
}
