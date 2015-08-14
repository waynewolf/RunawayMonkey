using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace UnitTest {
	internal class SpriteTileTest {
		
		[Test]
		public void SizeFromBoundingVolume() {
			Vector2 size = SpriteTile.VisualSizeFromBoundingVolume(new Vector2(20, 20), 45f);
			Assert.AreEqual(20 * 0.7071067f, size.x);
		}
	}
}