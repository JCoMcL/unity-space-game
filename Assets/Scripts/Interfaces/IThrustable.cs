using UnityEngine;
using System.Collections;

public interface IThrustable<T1, T2>
{
	void Thrust (T1 rb, T2 force);
}

