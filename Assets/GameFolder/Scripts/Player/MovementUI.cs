using UnityEngine;
using System.Collections;

public class MovementUI : MonoBehaviour 
{
//    public OVRManager ovrManage;
//    public OVRCameraRig cameraRig;
//    private GameObject viewConeMesh;

//    // Use this for initialization
//    void Start () 
//    {
////		viewConeMesh = transform.GetChild (0).gameObject;
////		Vector3 IRCameraPos = Vector3.zero;
////		//		Quaternion IRCameraRot = Quaternion.identity;
////		//		
//        float cameraHFov = OVRManager.tracker.frustum.fov.x;
//        float cameraVFov = OVRManager.tracker.frustum.fov.y;
//        float cameraNearZ = OVRManager.tracker.frustum.nearZ;
//        float cameraFarZ = OVRManager.tracker.frustum.farZ;
////		
//////		GetIRCamera (ref IRCameraPos, ref IRCameraRot, ref cameraHFov, ref cameraVFov, ref cameraNearZ, ref cameraFarZ);
////		
////		IRCameraPos.z *= -1;
////		
//////		transform.localPosition = IRCameraPos;
//////		transform.rotation = IRCameraRot;
////
////		
//        float horizontalScale = Mathf.Tan (cameraHFov / 2f);
//        float verticalScale = Mathf.Tan (cameraVFov / 2f);
////
////		
//        transform.GetChild(0).localScale = new Vector3 (horizontalScale * cameraFarZ, verticalScale * cameraFarZ, cameraFarZ);
////


////		tracker.frustum.
////		cameraRig.
//    }
	
//    // Update is called once per frame
//    void Update () 
//    {
//        OVRPose pose = OVRManager.tracker.GetPose ();
//        transform.localPosition = pose.position;
//        transform.rotation = pose.orientation;
//    }
}
