using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Sprite[] spritePool;

	void Start () 
	{
		spriteRenderer.sprite = spritePool[Random.Range (0,spritePool.Length)];
	}
}
