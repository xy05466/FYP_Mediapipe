using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePointGOPool : MonoBehaviour
{
  [SerializeField] FacePointGO facePointGO;
  [SerializeField] int defaultPoolSize = 10;
  private Stack<FacePointGO> facepointPool;

  private void Awake()
  {
    SetupPool();
  }
  private void SetupPool()
  {
    if (facePointGO == null)
    {
      Debug.LogError("FacePointGO's pooled object is null!");
      return;
    }
    facepointPool = new Stack<FacePointGO>();
    for (int i = 0; i < defaultPoolSize; i++)
    {
      FacePointGO instance = Instantiate(facePointGO);
      instance.objectPool = this;
      instance.gameObject.SetActive(false);
      facepointPool.Push(instance);
    }
  }
  public FacePointGO GetObject()
  {
    if (facePointGO == null)
    {
      Debug.LogError("FacePointGO's pooled object is null!");
      return null;
    }
    if (facepointPool.Count == 0)
    {
      FacePointGO instance = Instantiate(facePointGO);
      instance.objectPool = this;
      return instance;
    }
    FacePointGO nextInstance = facepointPool.Pop();
    nextInstance.gameObject.SetActive(true);
    return nextInstance;
  }
  public void ReturnToPool(FacePointGO backToPool)
  {
    facepointPool.Push(backToPool);
    backToPool.gameObject.SetActive(false);
  }
}
