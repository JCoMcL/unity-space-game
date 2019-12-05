using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour 
{
	[SerializeField]
	GameObject loadScreen;
	[SerializeField]
	Slider slider;
	public void LoadScene(int index)
	{	
		loadScreen.SetActive (true);
		StartCoroutine (LoadSceneAsync (index));
	}

	IEnumerator LoadSceneAsync(int index)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync (index);
		while (!operation.isDone) 
		{
			float progress = operation.progress / 0.9f;
			slider.value = progress;
			yield return null;
		}	
	}

}
