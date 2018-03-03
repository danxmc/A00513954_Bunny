using UnityEngine;
using UnityEngine.SceneManagement;		// Required to switch scenes
using System.Collections;

public class LevelManager : MonoBehaviour {
    private LevelManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string name)
    {

		//Debug.Log ("New Level load: " + name);
		//	Application.LoadLevel (name);    -- This method was deprecated a long time ago
		SceneManager.LoadScene(name);
	}

	public void EndGame()
    {
		//Debug.Log ("Quit requested");
		Application.Quit ();
	}

    // Added these functions to our previous LevelManager script.
    public void LoadNextLevel()
    {
        // Load the next scene in the build order
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
