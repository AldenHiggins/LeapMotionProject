//========= Copyright 2014, Valve Corporation, All rights reserved. ===========
//
// Purpose: Displays 2d content on a large virtual screen.
//
//=============================================================================

using UnityEngine;
using System.Collections;
using Valve.VR;

public class SteamVR_Overlay : MonoBehaviour
{
	public Texture texture;
	public bool curved = true;
	public bool antialias = true;
	public float scale = 3.0f;			// size of overlay view
	public float distance = 1.25f;		// distance from surface
	public float alpha = 1.0f;			// opacity 0..1

	public Vector4 uvOffset = new Vector4(0, 0, 1, 1);

	public float gridDivs = 20;
	public float gridWidth = 0.01f;
	public float gridScale = 20.0f;

	public float radius { get; private set; } // function of distance
	public float zoffset { get; private set; } // local space head pos

	public struct EyeData
	{
		public Matrix4x4 invProj;
		public Vector4[] kernel;
	}

	public EyeData[] eyeData { get; private set; }

	static public SteamVR_Overlay instance { get; private set; }

	void OnEnable()
	{
		var vr = SteamVR.instance;
		if (vr != null)
		{
			var proj = Matrix4x4.Perspective(vr.fieldOfView, vr.aspect, 0.1f, 1000.0f);
			var invProj = proj.inverse;

			eyeData = new EyeData[2];
			for (int i = 0; i < 2; i++)
			{
				var eye = (Hmd_Eye)i;

				uint x = 0, y = 0, w = 0, h = 0;
				vr.hmd.GetEyeOutputViewport(eye, ref x, ref y, ref w, ref h);

				eyeData[i].kernel = new Vector4[] { // AA sub-pixel sampling (2x2 RGSS)
				new Vector4( 0.125f / w, -0.375f / h,  0.375f / w,  0.125f / h),
				new Vector4(-0.125f / w,  0.375f / h, -0.375f / w, -0.125f / h)};

				eyeData[i].invProj = invProj;
			}
		}

		SteamVR_Overlay.instance = this;
		SteamVR_Utils.Event.Listen("new_poses", OnNewPoses);
	}

	void OnDisable()
	{
		SteamVR_Utils.Event.Remove("new_poses", OnNewPoses);
		SteamVR_Overlay.instance = null;
		SteamVR.SafeClearOverlay();
	}

	private void OnNewPoses(params object[] args)
	{
		var poses = (TrackedDevicePose_t[])args[0];
		if (poses.Length <= OpenVR.k_unTrackedDeviceIndex_Hmd)
			return;

		if (!poses[OpenVR.k_unTrackedDeviceIndex_Hmd].bPoseIsValid)
			return;

		var hmd = new SteamVR_Utils.RigidTransform(poses[OpenVR.k_unTrackedDeviceIndex_Hmd].mDeviceToAbsoluteTracking);
		zoffset = hmd.pos.z;
	}

	public void UpdateOverlay(SteamVR vr)
	{
		if (texture != null)
		{
			var settings = new Compositor_OverlaySettings();
			settings.size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(Compositor_OverlaySettings));
			settings.curved = curved;
			settings.antialias = antialias;
			settings.scale = scale;
			settings.distance = distance;
			settings.alpha = alpha;
			settings.uOffset = uvOffset.x;
			settings.vOffset = uvOffset.y;
			settings.uScale = uvOffset.z;
			settings.vScale = uvOffset.w;
			settings.gridDivs = gridDivs;
			settings.gridWidth = gridWidth;
			settings.gridScale = gridScale;

			var vrcam = SteamVR_Render.Top();
			if (vrcam != null && vrcam.origin != null)
			{
				var offset = new SteamVR_Utils.RigidTransform(vrcam.origin, transform);
				offset.pos.x /= vrcam.origin.localScale.x;
				offset.pos.y /= vrcam.origin.localScale.y;
				offset.pos.z /= vrcam.origin.localScale.z;
				settings.transform = offset.ToHmdMatrix44();

				// The overlay transform is always rendered in standing space, so we transform it here
				// to seated space when using the seated universe origin in Unity.
				if (SteamVR_Render.instance.trackingSpace == TrackingUniverseOrigin.TrackingUniverseSeated)
				{
					var seated = vr.hmd.GetSeatedZeroPoseToStandingAbsoluteTrackingPose();
					offset = new SteamVR_Utils.RigidTransform(seated) * offset;
					settings.transform = offset.ToHmdMatrix44();
				}
			}

			vr.compositor.SetOverlay(texture.GetNativeTexturePtr(), ref settings);
		}
		else
		{
			vr.compositor.ClearOverlay();
		}
	}
}

