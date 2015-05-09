﻿//========= Copyright 2014, Valve Corporation, All rights reserved. ===========
//
// Purpose: Render model of associated tracked object
//
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[ExecuteInEditMode, RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class SteamVR_RenderModel : MonoBehaviour
{
	public SteamVR_TrackedObject.EIndex index;
	public string modelOverride;

	public class RenderModel
	{
		public RenderModel(Mesh mesh, Texture2D texture)
		{
			this.mesh = mesh;
			this.material = new Material(Shader.Find("Unlit/Texture"));
			this.material.mainTexture = texture;
		}
		public Mesh mesh { get; private set; }
		public Material material { get; private set; }
	}

	public static Hashtable models = new Hashtable();

	private void OnDeviceConnected(params object[] args)
	{
		var i = (int)args[0];
		if (i != (int)index)
			return;

		var connected = (bool)args[1];
		if (connected)
		{
			var vr = SteamVR.instance;
			var error = Valve.VR.TrackedPropertyError.TrackedProp_Success;
			var capactiy = vr.hmd.GetStringTrackedDeviceProperty((uint)i, Valve.VR.TrackedDeviceProperty.Prop_RenderModelName_String, null, 0, ref error);
			if (capactiy <= 1)
			{
				Debug.LogError("Failed to get render model name for tracked object " + index);
				return;
			}

			var buffer = new System.Text.StringBuilder((int)capactiy);
			vr.hmd.GetStringTrackedDeviceProperty((uint)i, Valve.VR.TrackedDeviceProperty.Prop_RenderModelName_String, buffer, capactiy, ref error);

			SetModel(buffer.ToString());
		}
		else
		{
			GetComponent<MeshFilter>().mesh = null;
		}
	}

	private void SetModel(string renderModelName)
	{
		if (!models.Contains(renderModelName))
		{
			Debug.Log("Loading render model " + renderModelName);

			var vr = SteamVR.instance;
			if (vr == null)
				return;

			var renderModel = new Valve.VR.RenderModel_t();
			if (!vr.hmd.LoadRenderModel(renderModelName, ref renderModel))
			{
				Debug.LogError("Failed to load render model " + renderModelName);
				return;
			}

			var vertices = new Vector3[renderModel.unVertexCount];
			var normals = new Vector3[renderModel.unVertexCount];
			var uv = new Vector2[renderModel.unVertexCount];

			var type = typeof(Valve.VR.RenderModel_Vertex_t);
			for (int iVert = 0; iVert < renderModel.unVertexCount; iVert++)
			{
				var ptr = new System.IntPtr(renderModel.rVertexData.ToInt64() + iVert * Marshal.SizeOf(type));
				var vert = (Valve.VR.RenderModel_Vertex_t)Marshal.PtrToStructure(ptr, type);

				vertices[iVert] = new Vector3(vert.vPosition.v[0], vert.vPosition.v[1], -vert.vPosition.v[2]);
				normals[iVert] = new Vector3(vert.vNormal.v[0], vert.vNormal.v[1], -vert.vNormal.v[2]);
				uv[iVert] = new Vector2(vert.rfTextureCoord[0], vert.rfTextureCoord[1]);
			}

			int indexCount = (int)renderModel.unTriangleCount * 3;
			var indices = new short[indexCount];
			Marshal.Copy(renderModel.rIndexData, indices, 0, indices.Length);

			var triangles = new int[indexCount];
			for (int iTri = 0; iTri < renderModel.unTriangleCount; iTri++)
			{
				triangles[iTri * 3 + 0] = (int)indices[iTri * 3 + 2];
				triangles[iTri * 3 + 1] = (int)indices[iTri * 3 + 1];
				triangles[iTri * 3 + 2] = (int)indices[iTri * 3 + 0];
			}

			var mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = triangles;

			var textureMapData = new byte[renderModel.diffuseTexture.unWidth * renderModel.diffuseTexture.unHeight * 4]; // RGBA
			Marshal.Copy(renderModel.diffuseTexture.rubTextureMapData, textureMapData, 0, textureMapData.Length);

			var colors = new Color32[renderModel.diffuseTexture.unWidth * renderModel.diffuseTexture.unHeight];
			int iColor = 0;
			for (int iHeight = 0; iHeight < renderModel.diffuseTexture.unHeight; iHeight++)
			{
				for (int iWidth = 0; iWidth < renderModel.diffuseTexture.unWidth; iWidth++)
				{
					var r = textureMapData[iColor++];
					var g = textureMapData[iColor++];
					var b = textureMapData[iColor++];
					var a = textureMapData[iColor++];
					colors[iHeight * renderModel.diffuseTexture.unWidth + iWidth] = new Color32(r, g, b, a);
				}
			}

			var texture = new Texture2D(renderModel.diffuseTexture.unWidth, renderModel.diffuseTexture.unHeight, TextureFormat.ARGB32, true);
			texture.SetPixels32(colors);
			texture.Apply();

			models[renderModelName] = new RenderModel(mesh, texture);

			vr.hmd.FreeRenderModel(ref renderModel);
		}

		var model = models[renderModelName] as RenderModel;
		GetComponent<MeshFilter>().mesh = model.mesh;
		GetComponent<MeshRenderer>().material = model.material;
	}

	void OnEnable()
	{
		if (!string.IsNullOrEmpty(modelOverride))
			SetModel(modelOverride);
		else
			SteamVR_Utils.Event.Listen("device_connected", OnDeviceConnected);
	}

	void OnDisable()
	{
		SteamVR_Utils.Event.Remove("device_connected", OnDeviceConnected);
		GetComponent<MeshFilter>().mesh = null;
	}

#if UNITY_EDITOR
	void Update()
	{
		if (!Application.isPlaying && !string.IsNullOrEmpty(modelOverride))
			SetModel(modelOverride);
	}
#endif
}

