using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switch_to_control_scene: MonoBehaviour {
	public void Load_Scene(string scene)
	{
		// Only specifying the sceneName or sceneBuildIndex will load the scene with the Single mode
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}
}
