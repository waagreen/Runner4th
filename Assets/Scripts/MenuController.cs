using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneOrder
{
    FirstLevel,
    SecondLevel,
    ThirdLevel,
}
public class MenuController : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene(SceneOrder.FirstLevel.ToString());
}
