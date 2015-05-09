//========= Copyright 2014, Valve Corporation, All rights reserved. ===========
//
// Purpose: Handles rendering to the game view window
//
//=============================================================================

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SteamVR_GameView : MonoBehaviour
{
	static Material overlayMaterial;
	public bool stop = false;
	public float scale = 1.5f;

	public void OnPostRender()
	{
		if (stop)
			return;
		var blitMaterial = SteamVR_Camera.blitMaterial;
		var vr = SteamVR.instance;
		var camera = GetComponent<Camera>();

		var x0 = -scale;
		var x1 = scale;

		float y0, y1;
		float aspect = scale * camera.aspect / vr.aspect;
		if (SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL"))
		{
			y0 =  aspect;
			y1 = -aspect;
		}
		else
		{
			y0 = -aspect;
			y1 =  aspect;
		}

		blitMaterial.mainTexture = SteamVR_Camera.GetSceneTexture(camera.hdr);

		GL.PushMatrix();
		GL.LoadOrtho();
		blitMaterial.SetPass(0);
		GL.Begin(GL.QUADS);
		GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(x0, y0, 0);
		GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(x1, y0, 0);
		GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(x1, y1, 0);
		GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(x0, y1, 0);
		GL.End();
		GL.PopMatrix();

		var overlay = SteamVR_Overlay.instance;
		if (overlay && overlay.texture)
		{
			if (!overlayMaterial)
			{
				overlayMaterial = new Material("Shader \"SteamVR_Viewport\" {" + "\n" +
					"Properties { _MainTex (\"Base (RGB)\", 2D) = \"white\" {} }" + "\n" +
					"SubShader { Pass {" + "\n" +
					"	Blend SrcAlpha OneMinusSrcAlpha" + "\n" +
					"	ZTest Always Cull Off ZWrite Off Fog { Mode Off }" + "\n" +
					"	BindChannels { Bind \"vertex\", vertex Bind \"texcoord\", texcoord0 }" + "\n" +
					"	SetTexture [_MainTex] { combine texture }" + "\n" +
					"} } }");
				overlayMaterial.hideFlags = HideFlags.HideAndDontSave;
				overlayMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
			}

			var texture = overlay.texture;
			overlayMaterial.mainTexture = texture;

			var u0 = 0.0f;
			var v0 = 1.0f - (float)Screen.height / texture.height;
			var u1 = (float)Screen.width / texture.width;
			var v1 = 1.0f;

			u0 += overlay.uvOffset.x;
			u1 += overlay.uvOffset.x;
			v0 += overlay.uvOffset.y;
			v1 += overlay.uvOffset.y;

			if (Screen.width < texture.width)
			{
				var offset = (float)(texture.width - Screen.width) / (2 * texture.width);
				u0 += offset;
				u1 += offset;
			}
			if (Screen.height < texture.height)
			{
				var offset = (float)(texture.height - Screen.height) / (2 * texture.height);
				v0 -= offset;
				v1 -= offset;
			}

			GL.PushMatrix();
			GL.LoadOrtho();
			overlayMaterial.SetPass(0);
			GL.Begin(GL.QUADS);
			GL.TexCoord2(u0, v0); GL.Vertex3(0, 0, 0);
			GL.TexCoord2(u1, v0); GL.Vertex3(1, 0, 0);
			GL.TexCoord2(u1, v1); GL.Vertex3(1, 1, 0);
			GL.TexCoord2(u0, v1); GL.Vertex3(0, 1, 0);
			GL.End();
			GL.PopMatrix();
		}
	}
}

