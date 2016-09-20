using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class CubemapRenderer : MonoBehaviour {

	public Cubemap cubemap;

	void Save(CubemapFace face) {
		var tex = new Texture2D (1024, 1024);// (cubemap.texelSize.x, cubemap.texelSize.y, TextureFormat.ARGB32, false);
		tex.SetPixels (cubemap.GetPixels (face));
		File.WriteAllBytes (Application.streamingAssetsPath + "/" + face + ".png", tex.EncodeToPNG ());
	}



	[ContextMenu("render")]
	void Render() {
		GetComponent<Camera> ().RenderToCubemap (cubemap);

		Save (CubemapFace.NegativeX);
		Save (CubemapFace.NegativeY);
		Save (CubemapFace.NegativeZ);
		Save (CubemapFace.PositiveX);
		Save (CubemapFace.PositiveY);
		Save (CubemapFace.PositiveZ);
	}
}
