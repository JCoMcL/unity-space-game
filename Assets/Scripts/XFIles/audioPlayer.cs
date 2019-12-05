using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class audioPlayer : MonoBehaviour {

	AudioSource sfx;
	[Range(0,100)] public int pitchRange;
	[Range(0,100)] public int pitchRandom;
	public int pitchBase;
	[Range(0,100)] public int volumeRange;
	[Range(0,100)] public int volumeRandom;
	public int volumeBase;

	void Start () 
	{
		sfx = GetComponent<AudioSource>();
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		sfx.pitch = 	(collision.relativeVelocity.magnitude*pitchRange + 100 * (100-pitchRange)) * Random.Range(100-pitchRandom, 100+pitchRandom) / 100000 + pitchBase/100;
		sfx.volume = 	(collision.relativeVelocity.magnitude*volumeRange + 100 * (100-volumeRange)) * Random.Range(100-volumeRandom, 100+volumeRandom) / 100000 + volumeBase/10000;
		sfx.Play();
	}
}
