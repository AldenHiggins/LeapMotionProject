using UnityEngine;

public abstract class AAttack : MonoBehaviour
{
    abstract public void inactiveFunction();

    public void releaseFunction(OVRInput.Controller hand)
    {
        releaseFunctionConcrete(OVRInput.GetLocalControllerPosition(hand), getWorldPosition(hand), OVRInput.GetLocalControllerRotation(hand), getWorldRotation(hand));
    }

    public void holdFunction(OVRInput.Controller hand)
    {
        holdFunctionConcrete(OVRInput.GetLocalControllerPosition(hand), getWorldPosition(hand), OVRInput.GetLocalControllerRotation(hand), getWorldRotation(hand));
    }

    abstract public void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot);

    abstract public void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot);

    // Helper function to get the controller's world position
    private Vector3 getWorldPosition(OVRInput.Controller hand)
    {
        return GetObjects.instance.getPlayer().gameObject.transform.parent.TransformPoint(OVRInput.GetLocalControllerPosition(hand));
    }

    // Helper function to get the controller's world rotation
    private Quaternion getWorldRotation(OVRInput.Controller hand)
    {
        return GetObjects.instance.getPlayer().gameObject.transform.parent.rotation * OVRInput.GetLocalControllerRotation(hand);
    }
}


