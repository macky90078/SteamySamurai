using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
	[Header("Sources")]
	[SerializeField]
	private AudioSource audioSource;
	[SerializeField]
	private AudioSource sfxSource;

	[Header("Clips")]
	[SerializeField]
	private AudioClip buttonClickClip;
	[SerializeField]
	private AudioClip freezeClip;
	[SerializeField]
	private List<AudioClip> smashClips;
	[SerializeField]
	private AudioClip pullClip;

	public void PlayButtonClick()
	{
		this.sfxSource.clip = this.buttonClickClip;
		this.sfxSource.Play();
	}

	public void PlayFreeze()
	{
		this.sfxSource.clip = this.freezeClip;
		this.sfxSource.Play();
	}

	public void PlaySmash()
	{
		int rand = Random.Range(0, this.smashClips.Count);
		this.sfxSource.clip = this.smashClips[rand];
		this.sfxSource.Play();
	}

	public void PlayPull()
	{
		this.sfxSource.clip = pullClip;
		this.sfxSource.Play();
	}
}
