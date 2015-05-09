//========= Copyright 2014, Valve Corporation, All rights reserved. ===========
//
// Purpose: Masks for working with controller buttons.
//
//=============================================================================

using UnityEngine;
using Valve.VR;

public class SteamVR_Controller
{
	public class ButtonMask
	{
		public const ulong System			= (1ul << (int)EVRButtonId.k_EButton_System);
		public const ulong ApplicationMenu	= (1ul << (int)EVRButtonId.k_EButton_ApplicationMenu);
		public const ulong Grip				= (1ul << (int)EVRButtonId.k_EButton_Grip);
		public const ulong Axis0			= (1ul << (int)EVRButtonId.k_EButton_Axis0);
		public const ulong Axis1			= (1ul << (int)EVRButtonId.k_EButton_Axis1);
		public const ulong Axis2			= (1ul << (int)EVRButtonId.k_EButton_Axis2);
		public const ulong Axis3			= (1ul << (int)EVRButtonId.k_EButton_Axis3);
		public const ulong Axis4			= (1ul << (int)EVRButtonId.k_EButton_Axis4);
		public const ulong Touchpad			= (1ul << (int)EVRButtonId.k_EButton_SteamVR_Touchpad);
		public const ulong Trigger			= (1ul << (int)EVRButtonId.k_EButton_SteamVR_Trigger);
	}
}

