using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSceneManager : MonoBehaviour
{
  public SceneManagement sceneManagement;
  private void Start() => sceneManagement = GetComponent<GameManager>().sceneManagement;

    public void LoadScene(string name) => sceneManagement.LoadScene(name);
}
