using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStartLevelAttack : AAttack
{
    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(2).name);
        SceneManager.LoadScene("LevelSelectTest", LoadSceneMode.Additive);
        GetObjects.getMovingObjectsContainer().transform.position = Vector3.zero;
        return;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }
}
