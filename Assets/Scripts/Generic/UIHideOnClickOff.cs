using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHideOnClickOff : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
	[SerializeField]
	AbsUIContentPanel panel;
	bool active;
	void Update()
	{
		if (Input.GetMouseButtonDown (0) && !active) 
			{GetPanel ().Hide ();}
	}
	public void OnPointerExit(PointerEventData eventData)
	{active = false;}
	public void  OnPointerEnter(PointerEventData eventData)
		{active = true;}
	AbsUIContentPanel GetPanel()
	{
		if (panel == null) 
			{panel = GetComponent<AbsUIContentPanel> ();}
		return panel;
	}
}
