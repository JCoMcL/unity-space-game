using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsUIContentPanel : MonoBehaviour 
{
	public abstract void Show ();
	public abstract void Hide ();
	public abstract void AddContent (GameObject content);
	public abstract void ClearContent ();
}
