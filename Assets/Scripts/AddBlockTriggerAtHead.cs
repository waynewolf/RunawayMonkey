using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class AddBlockTriggerAtHead : MonoBehaviour {
	
	void Start () {
		BoxCollider2D boxColliderAttached = GetComponent<BoxCollider2D>();
		if (!boxColliderAttached) {
			Debug.LogError("The game object " + gameObject.name +
			               " must attach a boxcollider that is the same width of the sprite");
			return;
		}
		BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
		boxCollider.isTrigger = true;
		// programmablly add trigger, so the size is not affected by the
		// scale of the attached game object, otherwise, monkey may be
		// blocked far away which causes confusion. This is the reason
		// why we don't use a child gameobject like 'block check' thing.
		float sizeY = Mathf.Max(boxColliderAttached.size.y - 0.02f, 0.1f);
		boxCollider.size = new Vector2(0.06f, sizeY);
		boxCollider.offset = new Vector2(-0.5f * boxColliderAttached.size.x + 0.01f, 0f);
	}

}
