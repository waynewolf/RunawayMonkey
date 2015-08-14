using UnityEngine;
using System.Collections;

/// <summary>
/// Sprite tile. Drag this script to a game object, the sprite tiles
/// will be generated based on the scale of the game object.
/// </summary>
[RequireComponent (typeof (SpriteRenderer))]
public class SpriteTile : MonoBehaviour {
	private SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();

		if (GetSpriteAlignment(spriteRenderer.sprite.pivot, 
		                       spriteRenderer.sprite.rect.center) != SpriteAlignment.Center) {
			Debug.LogError("You forgot change the sprite pivot to Center.");
		}

		// Clear rotation before creating child gameobjects
		Quaternion orignalRotation = transform.rotation;
		transform.rotation = Quaternion.identity;

		Vector3 originalTileSize = new Vector3(spriteRenderer.bounds.size.x / transform.localScale.x,
		                                 	   spriteRenderer.bounds.size.y / transform.localScale.y,
		                                       1);

		int xTileNumber = (int)Mathf.Floor(Mathf.Abs (transform.localScale.x));
		int yTileNumber = (int)Mathf.Floor(Mathf.Abs (transform.localScale.y));
		Vector3 tileScale = Vector3.one;
		tileScale.x = transform.localScale.x / xTileNumber;
		tileScale.y = transform.localScale.y / yTileNumber;
		Vector3 stretchedTileSize = Vector3.Scale(originalTileSize, tileScale);

		Vector3 startPos = transform.position - 0.5f * spriteRenderer.bounds.size;

		// Generate a child prefab of the sprite renderer
		GameObject childPrefab = new GameObject();
		SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
		childPrefab.transform.position = transform.position;
		childSprite.sprite = spriteRenderer.sprite;
		childSprite.sortingLayerID = spriteRenderer.sortingLayerID;
		childSprite.sortingOrder = spriteRenderer.sortingOrder;

		// Loop through and create repeated tiles in original size
		GameObject child;
		for (int i = 0; i < yTileNumber; i++) {
			for (int j = 0; j < xTileNumber; j++) {
				child = Instantiate(childPrefab) as GameObject;
				child.transform.position = startPos + Vector3.Scale (new Vector3(j + 0.5f, i + 0.5f, 1f), stretchedTileSize);
				child.transform.localScale = tileScale;
				child.transform.parent = transform;
			}
		}
		Destroy(childPrefab);

		// Restore original rotation to the root game object
		transform.rotation = orignalRotation;
		spriteRenderer.enabled = false;
	}

	// this is old fashioned code from internet, use the one down below in unity 5
	public static SpriteAlignment GetSpriteAlignment(GameObject spriteObject) {
		BoxCollider2D boxCollider = spriteObject.AddComponent<BoxCollider2D> ();
		float colX = boxCollider.offset.x;
		float colY = boxCollider.offset.y;

		if (colX > 0f && colY < 0f)
			return (SpriteAlignment.TopLeft);
		else if (colX < 0 && colY < 0)
			return (SpriteAlignment.TopRight);
		else if (colX == 0 && colY < 0)
			return (SpriteAlignment.TopCenter);
		else if (colX > 0 && colY == 0)
			return (SpriteAlignment.LeftCenter);
		else if (colX < 0 && colY == 0)
			return (SpriteAlignment.RightCenter);
		else if (colX > 0 && colY > 0)
			return (SpriteAlignment.BottomLeft);
		else if (colX < 0 && colY > 0)
			return (SpriteAlignment.BottomRight);
		else if (colX == 0 && colY > 0)
			return (SpriteAlignment.BottomCenter);
		else if (colX == 0 && colY == 0)
			return (SpriteAlignment.Center);
		else
			return (SpriteAlignment.Custom);
	}

	public static SpriteAlignment GetSpriteAlignment(Vector2 spirtePivot, Vector2 spriteCenter) {
		float colX = spriteCenter.x - spirtePivot.x;
		float colY = spriteCenter.y - spirtePivot.y;
		
		if (colX > 0f && colY < 0f)
			return (SpriteAlignment.TopLeft);
		else if (colX < 0 && colY < 0)
			return (SpriteAlignment.TopRight);
		else if (colX == 0 && colY < 0)
			return (SpriteAlignment.TopCenter);
		else if (colX > 0 && colY == 0)
			return (SpriteAlignment.LeftCenter);
		else if (colX < 0 && colY == 0)
			return (SpriteAlignment.RightCenter);
		else if (colX > 0 && colY > 0)
			return (SpriteAlignment.BottomLeft);
		else if (colX < 0 && colY > 0)
			return (SpriteAlignment.BottomRight);
		else if (colX == 0 && colY > 0)
			return (SpriteAlignment.BottomCenter);
		else if (colX == 0 && colY == 0)
			return (SpriteAlignment.Center);
		else
			return (SpriteAlignment.Custom);
	}

	public static Vector2 VisualSizeFromBoundingVolume (Vector3 size, float eulerAngleZ) {
		eulerAngleZ = Mathf.Repeat(eulerAngleZ, 360f);
		float x = size.x, y = size.y;
		float sinz = Mathf.Sin (Mathf.Deg2Rad * eulerAngleZ);
		float cosz = Mathf.Cos (Mathf.Deg2Rad * eulerAngleZ);
		if (Mathf.Abs (sinz) - Mathf.Abs (cosz) < float.Epsilon) {
			if (x - y < float.Epsilon) {
				Debug.LogWarning("Infinite solutions");
				return new Vector2(x * 0.7071067f, x * 0.7071067f);
			} else {
				Debug.LogError("No solutions");
				return Vector2.zero;
			}
		}
		float sinzSquare = sinz * sinz;
		float coszSquare = cosz * cosz;
		float w = (x * cosz - y * sinz) / (coszSquare - sinzSquare);
		float h = (y * cosz - x * sinz) / (coszSquare - sinzSquare);

		return new Vector2(w, h);
	}
}

