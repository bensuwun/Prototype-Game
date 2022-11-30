using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CustomSceneManager
{
    /**
        Used to load scenes in the background until it finishes.
		Plan to use in DialogueTrigger.cs to transition to Battle Scene.
    */
    public static IEnumerator LoadAsyncAdditiveScene(string sceneName)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		while (!asyncLoad.isDone) {
			// Can do loading screen here
			yield return null;
		}
	}

	/**
		Used to unload additive scenes.
		Plan to use on CustomTyper.cs to unload the battle scene.
		NOTE: Only works if there is another running scene aside from the given parameter scene
	**/
	public static IEnumerator UnloadSceneAsync(string sceneName)
	{
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

		while (!asyncUnload.isDone) {
			// Can do loading screen here
			yield return null;
		}
	}

	/**
		NOTE: Only works if there is another running scene aside from the given parameter scene
	*/
	public static IEnumerator ReloadAdditiveSceneAsync(string sceneName) 
	{
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		while (!asyncLoad.isDone && !asyncUnload.isDone) {
			// Can do loading screen here
			yield return null;
		}
	}
}