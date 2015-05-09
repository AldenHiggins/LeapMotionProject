SteamVR plugin for Unity - v1.0
Copyright 2014-2015, Valve Corporation, All rights reserved.


Quickstart:

To use, simply add the SteamVR_Camera script to your Camera object(s).  Everything else gets set up at
runtime.  See the included quickstart guide for more details.


Requirements:

The SteamVR runtime must be installed.  This can be found in Steam under Tools.

The plugin currently only supports Windows / DX11.


Upgrading from previous versions:

The easiest and safest thing to do is to delete your SteamVR folder, and any files and folders in your
Plugins directory called 'openvr_api', 'steam_api' or 'steam_unity' (and variants).  Additionally, verify there
are no SteamVR files found in Assets/Editor.  Then import the new unitypackage into your project.

This latest version has been greatly simplified.  SteamVR_CameraEye has been removed as well as the menu
option from SteamVR_Setup to 'Setup Selected Camera(s)'.  The SteamVR_Camera object is instead rendered twice
(once per eye) and the game view rendering handled in SteamVR_GameView.  SteamVR_Camera now has 'head' and
'origin' properties for accessing the associated Transforms, and 'offset' has been deprecated in favor of using
'head'.  By pressing the 'Expand' button below the SteamVR logo in SteamVR_Camera's Inspector, these objects are
automatically created.  This is useful for attaching objects appropriately, and removes the need for managing
separate FollowHead and FollowEyes arrays. Similarly, the RenderComponents list is no longer needed as the
SteamVR_Camera is itself used to render each eye.  And finally, the button below the SteamVR logo will change to
'Collapse' to restore the camera to its previous setup.

SteamVR_Camera's Overlay support has been broken out into a separate SteamVR_Overlay component.  This can be
added to any object in your scene.  If you wish to use it in some scenes, but not others, it is good practice
to add the component to each of your scenes and ensure its Texture is set to None in those that you do not wish
it rendered in.

The experimental binaural audio support has been removed as there are better plugins on the Unity Asset Store now,
and this was an incomplete and unsupported solution.


Files:

Assets/Plugins/openvr_api.cs - This direct wrapper for the native SteamVR SDK support mirrors SteamVR.h and  
is the only script required.  It exposes all functionality provided by SteamVR.  It is not recommended you make  
changes to this file.  It should be kept in sync with the associated openvr_api dll.

The remaining files found in Assets/SteamVR/Scripts are provided as a reference implementation, and to get you  
up and running quickly and easily.  You are encouraged to modify these to suit your project's unique needs,  
and provide feedback at http://steamcommunity.com/app/250820 or http://steamcommunity.com/app/358720/discussions

Assets/SteamVR/Scenes/example.unity - A sample scene demonstrating the functionality provided by this plugin.   
This also shows you how to set up a separate camera for rendering gui elements, and handle events to display  
hmd status.


Details:

Note that these scripts are a work in progress. Many of these will change in future releases and we will not necessarily
be able to maintain compatibility with this version.

Assets/SteamVR/Scripts/SteamVR.cs - Handles initialization and shutdown of subsystems.  Use SteamVR.instance
to access.  This may return null if initialization fails for any reason.  Use SteamVR.active to determine if
VR has been initialized without attempting to initialized it in the process.

Assets/SteamVR/Scripts/SteamVR_Camera.cs - Adds VR support to your existing camera object.

To combat stretching incurred by distortion correction, we render scenes at a higher resolution off-screen.
Since all camera's in Unity are rendered sequentially, we share a single static render texture across each
eye camera.  SteamVR provides a recommended render target size as a minimum to account for distortion,
however, rendering to a higher resolution provides additional multisampling benefits at the associated
expense.  This can be controlled via SteamVR_Camera.sceneResolutionScale.

Note: Both GUILayer and FlareLayer are not compatible with SteamVR_Camera since they render in screen space
rather than world space. These are automatically moved the SteamVR_GameView object which itself is automatically
added to the SteamVR_Camera's parent 'head' object.  The AudioListener also gets transferred to the head in order
for audio to be properly spacialized.

Assets/SteamVR/Scripts/SteamVR_Overlay.cs - This component is provided to assist in rendering 2D content in VR.
The specified texture is composited into the scene on a virtual curved surface using a special render path for
increased fidelity.  See the [Status] prefab in the example scene for how to set this up.  Since it uses GUIText,
it should be dragged into the Hierarchy window rather than into the Scene window so it retains its default position
at the origin.

Assets/SteamVR/Scripts/SteamVR_TrackedObject.cs - Add this to any object that you want to use tracking.  The
hmd has one set up for it automatically.  For controllers, select the index of the object to map to.  In general
you should parent these objects to the camera's 'origin' object so they track in the same space.  However, if
that is inconvenient, you can specify the 'origin' in the TrackedObject itself.

Assets/SteamVR/Scripts/SteamVR_RenderModel.cs - Dynamically creates associated SteamVR provided models for tracked
objects.  See <SteamVR Runtime Path>/resources/rendermodels for the full list of overrides.

Assets/SteamVR/Scripts/SteamVR_Utils.cs - Various bits for working with the SteamVR API in Unity including a  
simple event system, a RigidTransform class for working with vector/quaternion pairs, matrix conversions, and  
other useful functions.


Prefabs:

[CameraRig] - This is the camera setup used by the example scene.  It is simply a default camera with the
SteamVR_Camera component added to it, and the Expand button clicked.  It also includes a full set of Tracked Devices
which will display and follow any connected tracked devices (e.g. controllers, base stations and cameras).

[Status] - This is a set of components for providing various status info.  You can add it to your scene to get
notifications for leaving the tracking bounds, et cetera.  It also includes an escape menu for tweaking options.
Note: It uses the SteamVR_Overlay component, which is rather expensive rendering-wise.

[SteamVR] - This object controls some global settings for SteamVR, most notably Tracking Space.  Legacy projects
that want their viewed automatically centered on startup if not configured or to use the seated calibrated position
should switch Tracking Space to Seated.  This object is created automatically on startup if not added and defaults
to Standing Tracking Space.  It also provides the ability to set special masks for rendering each eye (in case you
want to do something differently per-eye) and some simple help text that demonstrates rendering only to the
companion window (which can be cleared or customized here).


GUILayer, GUIText, and GUITexture:

The recommended way for drawing 2D content is through SteamVR_Overlay.  There is an example of how to set this up
in the example scene.  GUIText and GUITexture use their Transform to determine where they are drawn, so these
objects will need to live near the origin.  You will need to set up a separate camera using a Target Texture.  To
keep it from rendering other elements of your scene, you should create a unique layer used by all of your gui
elements, and set the camera's Culling Mask to only draw those items.  Set its depth to -1 to ensure it gets
updated before composited into the final view.


OnGUI:

Assets/SteamVR/Scripts/SteamVR_Menu.cs demonstrates use of OnGUI with SteamVR_Camera's overlay texture.  The  
key is to set RenderTexture.active and restore it afterward.  Beware when also using a camera to render to the  
same texture as it may clear your content.


Camera layering:

One powerful feature of Unity is its ability to layer cameras to render scenes (e.g. drawing a skybox scene
with one camera, the rest of the environment with a second, and maybe a third for a 3D hud).  This is performed
by setting the latter cameras to only clear the depth buffer, and leveraging the cameras' cullingMask to control
which items get rendered per-camera, and depth to control order.


Camera scale:

Setting SteamVR_Camera's gameObject scale will result in the world appearing (inversely) larger or smaller.
This can be used to powerful effect, and is useful for allowing you to build skybox geometry at a sane scale
while still making it feel far away.  Similarly, it allows you to build geometry at scales the physics engine
and nav mesh generation prefers, while visually feeling much smaller or larger.  Of course, if you are building
geometry to real-world scale you should leave this at its default of 1,1,1.  Once a SteamVR_Camera has been
expanded, its 'origin' Transform should be scaled instead.


Camera masking:

By manually adding a GameObject with the SteamVR_Render component on it to your scene, you can specify a left
and right culling mask to use to control rendering per eye if necessary.


Events:

SteamVR_Camera fires off several events.  These can be handled by registering for them through  
SteamVR_Utils.Event.Listen.  Be sure to remove your handler when no longer needed.  The best pattern is to  
Listen and Remove in OnEnable and OnDisable respectively.

"initializing" - This event is sent when the hmd's tracking status changes to or from Unitialized.

"calibrating" - This event is sent when starting or stopping calibration with the new state.

"out_of_range" - This event is sent when losing or reacquiring absolute positional tracking.  This will 
never fire for the Rift DK1 since it does not have positional tracking.  For camera based trackers, this 
happens when the hmd exits and enters the camera's view.

"device_connected" - This event is sent when devices are connected or disconnected.  The device index is passed
as the first argument, and the connected status (true / false) as the second argument.

Feel free to leverage this system to fire off events of your own.  SteamVR_Utils.Event.Send takes any number  
of parameters, and passes them on to all registered callbacks.

A helper class has been included called SteamVR_Status which leverages these events to display hmd status to  
the user.  Examples of this can be found in the example scene.  SteamVR_StatusText specifically, leverages this
functionality to wrap up GUIText display, overriding SetAlpha.


Keybindings (if using the [Status] prefab):

Escape/Start - toggle menu
PageUp/PageDown - adjust scale
Home - reset scale
I - toggle frame stats on/off


Deploying on Steam:

If you are releasing your game on Steam (i.e. have a Steam ID and are calling Steam_Init through the  
Steamworks SDK), then you may want to check ISteamUtils::IsSteamRunningInVRMode() in order to determine if you  
should automatically launch into VR mode or not.


Known Issues:

* If Unity finds an Assets\Plugins\x86 folder, it will ignore all files in Assets\Plugins.  You will need to
either move openv_api.dll into the x86 subfolder, or move the dlls in the x86 folder up a level and delete
the x86 folder.


Troubleshooting:

* HmdError_Init_VRClientDLLNotFound - Make sure the SteamVR runtime is insteall.  This can be found in Steam
under Tools.  Try uninstalling and reinstalling SteamVR.  Try deleting <user>/AppData/Local/OpenVR/openvrpaths.vrpath
and relaunching Steam to regenerate this file.

* HmdError_Init_HmdNotFound - SteamVR cannot detect your VR headset, ensure the USB cable is plugged in.
If that doesn't work, try deleting your Steam/config/steamvr.cfg.

* HmdError_Init_InterfaceNotFound - Make sure your SteamVR runtime is up to date.

* HmdError_IPC_ConnectFailed - SteamVR launches a separate process called vrserver.exe which directly talks
to the hardware.  Games communicate to vrserver through vrclient.dll over IPC.  This error is usually due
to the communication pipe between the two having closed.  Use task manager to verify there are no rogue apps
that got stuck trying to shut down.  Often it's just a matter of the connection timing out the first time
due to long load times.  Launching a second time usually resolves this.

* "Not using DXGI 1.1" - Older versions of Unity used DXGI 1.0 which doesn't support functionality the compositor
requires to operate properly.  To fix this, we've added a hook to Steam to force DXGI 1.1.  To enable this hook
set the environement variable ForceDXGICreateFactory1 = 1 and launch the Unity Editor or your standalone builds
via Steam by manually adding them using the "Add Game..." button found in the lower left of the Library tab.

* Core Parking often causes hitching.  The easiest way to disable core parking is to download the tool called
Core Parking Manager, slide the slider to 100% and click Apply.

