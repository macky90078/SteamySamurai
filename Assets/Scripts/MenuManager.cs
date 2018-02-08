using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour 
{
	[SerializeField]
	private Button creditsButton;

	[SerializeField]
	private Button creditsCloseButton;

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadGame()
	{
		SceneManager.LoadScene(1);
	}

	public void OpenCreditsPanel()
	{
		this.creditsCloseButton.Select();
	}

	public void CloseCreditsPanel()
	{
		this.creditsButton.Select();
	}
}
