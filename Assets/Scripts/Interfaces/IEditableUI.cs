using UnityEngine;
using System.Collections;
public interface IEditableUI<T>
{
	void Show ();
	void Hide ();
	void AddContent (T content);
	void ClearContent ();
}


