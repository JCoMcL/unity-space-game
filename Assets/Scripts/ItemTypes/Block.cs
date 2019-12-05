using UnityEngine;
using System.Collections;

public class Block: ItemType
{
	public override Vector2 GetTextureCoordinates ()
	{
		textureCoords = new Vector2 (0, 0);
		return base.GetTextureCoordinates ();
	}
}
