using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class s_SceneManager : MonoBehaviour
{
  public void ChangeScene(string name)
  {
    SceneManager.LoadScene(name);
  }

}