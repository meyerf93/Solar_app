using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// This script exists in the Persistent scene and manages the content
// based scene's loading.  It works on a principle that the
// Persistent scene will be loaded first, then it loads the scenes that
// contain the player and other visual elements when they are needed.
// At the same time it will unload the scenes that are not needed when
// the player leaves them.
public class SceneController : MonoBehaviour
{
    public event Action BeforeSceneUnload;          // Event delegate that is called just before a scene is unloaded.
    public event Action AfterSceneLoad;             // Event delegate that is called just after a scene is loaded.

    //public GameObject batiment;
    public string startingSceneName = "SecurityRoom";
    // The name of the scene that should be loaded first.


    private IEnumerator Start()
    {

        // Start the first scene loading and wait for it to finish.
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));

        // Once the scene is finished loading, start fading in.
    }


    // This is the main external point of contact and influence from the rest of the project.
    // This will be called by a SceneReaction when the player wants to switch scenes.
    public void FadeAndLoadScene(SceneReaction sceneReaction)
    {
        // If a fade isn't happening then start fading and switching scenes.

        StartCoroutine(FadeAndSwitchScenes(sceneReaction.sceneName));

    }


    // This is the coroutine where the 'building blocks' of the script are put together.
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        // Start fading to black and wait for it to finish before continuing.

        // If this event has any subscribers, call it.
        if (BeforeSceneUnload != null)
            BeforeSceneUnload();

        // Unload the current active scene.
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Start loading the given scene and wait for it to finish.
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // If this event has any subscribers, call it.
        if (AfterSceneLoad != null)
            AfterSceneLoad();

        // Start fading back in and wait for it to finish before exiting the function.
    }


    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Allow the given scene to load over several frames and add it to the already loaded scenes (just the Persistent scene at this point).
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
  
        // Find the scene that was most recently loaded (the one at the last index of the loaded scenes).
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Set the newly loaded scene as the active scene (this marks it as the one to be unloaded next).
        //SceneManager.MoveGameObjectToScene(batiment, newlyLoadedScene);
        SceneManager.SetActiveScene(newlyLoadedScene);

    }


}
