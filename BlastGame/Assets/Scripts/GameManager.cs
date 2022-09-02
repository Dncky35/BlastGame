using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static int Number_of_Rows { get; set; }
    public static int Number_of_Columns { get; set; }
    public static int K { get; set; }
    public static int A { get; set; }
    public static int B { get; set; }
    public static int C { get; set; }
    public static bool CanPlayerPlay { get; set; }

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        CanPlayerPlay = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleWhenSceneHasChange;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleWhenSceneHasChange;
    }

    #region HANDLERS

    private void HandleWhenSceneHasChange(Scene scene, LoadSceneMode mode)
    {

    }

    #endregion
}
