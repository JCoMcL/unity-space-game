using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour 
{
	public ParticleSystem particles;
	public Flares flare;
	public Color color;
	ParticleSystem.EmissionModule em;
	ParticleSystem.VelocityOverLifetimeModule vl;
	public float multiplier;
	float speedMax;
	float speedMin;
	float rate;

	void Start () 
	{
		flare = (Flares)FindObjectOfType(typeof(Flares));
		Camera.main.GetComponent<PostRender> ().EarlyUpdate += Reset;
		em = particles.emission;
		vl = particles.velocityOverLifetime;
		speedMax = vl.z.constantMax;
		speedMin = vl.z.constantMin;
		rate = em.rateOverTimeMultiplier;
	}
	void Update () 
	{
		multiplier *= 0.2f;
		if (multiplier > 0) 
		{
			em.enabled = true;
			em.rateOverTimeMultiplier = rate * multiplier;

			ParticleSystem.MinMaxCurve z = vl.z;
			z.constantMax = speedMax * multiplier;
			z.constantMin = speedMin * multiplier;
			vl.z = z;

			flare.Flicker (transform.position,multiplier * 0.5f,color);
		} 
		else 
		{	em.enabled = false;}
	}
	void Reset(){multiplier = 0;}
}
