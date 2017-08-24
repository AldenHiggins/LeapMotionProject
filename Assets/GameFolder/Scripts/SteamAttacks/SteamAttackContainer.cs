using UnityEngine;
using System.Collections.Generic;
//using Valve.VR;

public class SteamAttackContainer : MonoBehaviour
{
    public SteamAttacks triggerAttack;
    public SteamAttacks gripAttack;
    //public SteamVR_TrackedObject left;
    //public SteamVR_TrackedObject right;

    public SteamAttacks offensiveAttack;
    public SteamAttacks defensiveAttack;
    public SteamAttacks defensiveObjectSwitchAttack;
    GameLogic game;

    public GameObject bow;
    public GameObject controllerModel;
    public ArrowManager arrowManager;

    List<int> controllerIndices = new List<int>();

    void Start()
    {
        game = GetObjects.getGame();
    }

    private void OnDeviceConnected(params object[] args)
    {
        //var index = (int)args[0];

        //var system = OpenVR.System;
        //if (system == null || system.GetTrackedDeviceClass((uint)index) != ETrackedDeviceClass.Controller)
        //    return;

        //var connected = (bool)args[1];
        //if (connected)
        //{
        //    //Debug.Log(string.Format("Controller {0} connected.", index));
        //    //PrintControllerStatus(index);
        //    controllerIndices.Add(index);
        //}
        //else
        //{
        //    //Debug.Log(string.Format("Controller {0} disconnected.", index));
        //    //PrintControllerStatus(index);
        //    controllerIndices.Remove(index);
        //}
    }

    void OnEnable()
    {
        //SteamVR_Utils.Event.Listen("device_connected", OnDeviceConnected);
    }

    void OnDisable()
    {
        //SteamVR_Utils.Event.Remove("device_connected", OnDeviceConnected);
    }

    void PrintControllerStatus(int index)
    {
        //var device = SteamVR_Controller.Input(index);
        //Debug.Log("index: " + device.index);
        //Debug.Log("connected: " + device.connected);
        //Debug.Log("hasTracking: " + device.hasTracking);
        //Debug.Log("outOfRange: " + device.outOfRange);
        //Debug.Log("calibrating: " + device.calibrating);
        //Debug.Log("uninitialized: " + device.uninitialized);
        //Debug.Log("pos: " + device.transform.pos);
        //Debug.Log("rot: " + device.transform.rot.eulerAngles);
        //Debug.Log("velocity: " + device.velocity);
        //Debug.Log("angularVelocity: " + device.angularVelocity);

        //var l = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        //var r = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        //Debug.Log((l == r) ? "first" : (l == index) ? "left" : "right");
    }

    //EVRButtonId[] buttonIds = new EVRButtonId[] {
    //    EVRButtonId.k_EButton_ApplicationMenu,
    //    EVRButtonId.k_EButton_Grip,
    //    EVRButtonId.k_EButton_SteamVR_Touchpad,
    //    EVRButtonId.k_EButton_SteamVR_Trigger
    //};

    //EVRButtonId[] axisIds = new EVRButtonId[] {
    //    EVRButtonId.k_EButton_SteamVR_Touchpad,
    //    EVRButtonId.k_EButton_SteamVR_Trigger
    //};

    public Transform point, pointer;

    void Update()
    {
        //// Switch out the attacks based on what mode we're in
        //// Defensive mode
        //if (game.roundActive == false)
        //{
        //    defensiveAttack.holdFunction(0, right);
        //    triggerAttack = defensiveAttack;
        //    gripAttack = defensiveObjectSwitchAttack;

        //    bow.SetActive(false);
        //    controllerModel.SetActive(true);
        //    arrowManager.enabled = false;

        //    GetObjects.getPlayer().gameObject.transform.localScale = new Vector3(18.0f, 18.0f, 18.0f);
        //}
        //// Offensive mode
        //else
        //{
        //    triggerAttack = new EmptySteamAttack();
        //    gripAttack = new EmptySteamAttack();

        //    bow.SetActive(true);
        //    controllerModel.SetActive(false);
        //    arrowManager.enabled = true;

        //    GetObjects.getPlayer().gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //}


        //foreach (var index in controllerIndices)
        //{
        //    SteamVR_TrackedObject thisController = right;

        //    if (index == SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost))
        //    {
        //        thisController = left;
        //    }

        //    var overlay = SteamVR_Overlay.instance;
        //    if (overlay && point && pointer)
        //    {
        //        var t = SteamVR_Controller.Input(index).transform;
        //        pointer.transform.localPosition = t.pos;
        //        pointer.transform.localRotation = t.rot;

        //        var results = new SteamVR_Overlay.IntersectionResults();
        //        var hit = overlay.ComputeIntersection(t.pos, t.rot * Vector3.forward, ref results);
        //        if (hit)
        //        {
        //            point.transform.localPosition = results.point;
        //            point.transform.localRotation = Quaternion.LookRotation(results.normal);
        //        }

        //        continue;
        //    }

        //    foreach (var buttonId in buttonIds)
        //    {
        //        if (SteamVR_Controller.Input(index).GetPressDown(buttonId))
        //        {
        //            if (buttonId == EVRButtonId.k_EButton_SteamVR_Trigger)
        //            {
        //                SteamVR_Controller.Input(index).TriggerHapticPulse();
        //                triggerAttack.releaseFunction(0, thisController);
        //            }
        //            else if (buttonId == EVRButtonId.k_EButton_SteamVR_Touchpad)
        //            {
        //                game.startRound();
        //            }
        //            else if (buttonId == EVRButtonId.k_EButton_Grip)
        //            {
        //                gripAttack.releaseFunction(0, thisController);
        //            }
        //        }
        //        if (SteamVR_Controller.Input(index).GetPressUp(buttonId))
        //        {
        //            //Debug.Log(buttonId + " press up");
                    
        //        }
        //        if (SteamVR_Controller.Input(index).GetPress(buttonId))
        //        {
        //            //Debug.Log(buttonId);
    
        //        }
        //    }

        //    foreach (var buttonId in axisIds)
        //    {
        //        if (SteamVR_Controller.Input(index).GetTouchDown(buttonId))
        //            //Debug.Log(buttonId + " touch down");
        //        if (SteamVR_Controller.Input(index).GetTouchUp(buttonId))
        //            //Debug.Log(buttonId + " touch up");
        //        if (SteamVR_Controller.Input(index).GetTouch(buttonId))
        //        {
        //            var axis = SteamVR_Controller.Input(index).GetAxis(buttonId);
        //            Debug.Log("axis: " + axis);

        //        }
        //    }
        //}
    }
}


