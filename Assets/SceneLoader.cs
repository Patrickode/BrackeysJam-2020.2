using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads a scene by its index. Use a negative index to go that many scenes forward if it was positive.
    /// (Put another way, load thisSceneIndex - <paramref name="index"/> or 
    /// thisSceneIndex + |<paramref name="index"/>|
    /// </summary>
    /// <param name="index"></param>
    public void LoadScene(int index)
    {
        //If index is positive, just load that index.
        if (index >= 0)
        {
            SceneManager.LoadScene(index);
        }
        //If index is negative, instead load the scene that many indices away from this one.
        //I.e., subtract index from this scene's index.
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - index);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
