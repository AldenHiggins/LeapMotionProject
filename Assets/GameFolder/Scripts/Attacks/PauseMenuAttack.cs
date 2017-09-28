using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuAttack : AAttack
{
    private bool menuActive = false;

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        menuActive = !menuActive;

        GetObjects.getPauseMenu().SetActive(menuActive);
        GetObjects.getScene().SetActive(!menuActive);
        return;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }
}
