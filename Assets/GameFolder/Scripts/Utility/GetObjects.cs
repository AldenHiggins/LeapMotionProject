using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GetObjects
{
    public static GameLogic getGame()
    {
        GameLogic returnGame = null;
        findFirstObjectOfType(ref returnGame, getRootTransform());
        return returnGame;
    }

    public static PlayerLogic getPlayer()
    {
        PlayerLogic returnPlayer = null;
        findFirstObjectOfType(ref returnPlayer, getRootTransform());
        return returnPlayer;
    }

    public static ControllableUnit getControllableUnit()
    {
        ControllableUnit returnUnit = null;
        findFirstObjectOfType(ref returnUnit, getRootTransform());
        return returnUnit;
    }

    public static Transform getRootTransform()
    {
        GameObject[] root = GameObject.FindGameObjectsWithTag("Root");
        Transform xform = root[0].transform;
        return xform.root;
    }

    public static Transform getAttackContainer()
    {
        GameObject attackContainer = null;
        findNameInChildren("Attacks", getRootTransform(), ref attackContainer);

        if (attackContainer != null)
        {
            return attackContainer.transform;
        }

        attackContainer = new GameObject();
        attackContainer.name = "Attacks";
        attackContainer.transform.parent = getRootTransform();

        return attackContainer.transform;
    }

    public static Transform getDefenseContainer()
    {
        GameObject defenseContainer = null;
        findNameInChildren("Defenses", getRootTransform(), ref defenseContainer);

        if (defenseContainer != null)
        {
            return defenseContainer.transform;
        }

        defenseContainer = new GameObject();
        defenseContainer.name = "Defenses";
        defenseContainer.transform.parent = getRootTransform();

        return defenseContainer.transform;
    }

    public static Transform getAttackParticleContainer()
    {
        GameObject attackParticleContainer = null;
        findNameInChildren("AttackParticles", getRootTransform(), ref attackParticleContainer);

        if (attackParticleContainer != null)
        {
            return attackParticleContainer.transform;
        }

        attackParticleContainer = new GameObject();
        attackParticleContainer.name = "AttackParticles";
        attackParticleContainer.transform.parent = getRootTransform();

        return attackParticleContainer.transform;
    }

    public static GameObject getCamera()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("CenterEyeAnchor", getRootTransform(), ref firstFoundOfName);
        return firstFoundOfName;
    }

    public static GameObject getSpawnedEnemies()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("SpawnedEnemies", getRootTransform(), ref firstFoundOfName);
        return firstFoundOfName;
    }

    public static GameObject getGoalPosition()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("EnemyGoal", getRootTransform(), ref firstFoundOfName);
        return firstFoundOfName;
    }

    public static GameObject getEnemyWaves()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("EnemyWaves", getRootTransform(), ref firstFoundOfName);
        return firstFoundOfName;
    }

    public static Transform getMiscContainer()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("MiscGameObjects", getRootTransform(), ref firstFoundOfName);
        return firstFoundOfName.transform;
    }

    public static GameObject getMovingObjectsContainer()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("MovingObjects", getRootTransform(), ref firstFoundOfName);
        return firstFoundOfName;
    }

    public static DefensiveGrid getDefensiveGrid()
    {
        DefensiveGrid returnGrid = null;
        findFirstObjectOfType(ref returnGrid, getRootTransform());
        return returnGrid;
    }

    public static LevelBounds GetLevelBounds()
    {
        LevelBounds returnBounds = null;
        findFirstObjectOfType(ref returnBounds, getRootTransform());
        return returnBounds;
    }

    public static GameObject getScene()
    {
        GameObject[] sceneRootObjects = UnityEngine.SceneManagement.SceneManager.GetSceneAt(2).GetRootGameObjects();
        return sceneRootObjects[0];
    }

    public static GameObject getPauseMenu()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("PauseMenu", getRootTransform(), ref firstFoundOfName);
        return firstFoundOfName;
    }

    // Try and find an object with a specific name ONLY within the direct children of the supplied transform
    public static void findNameInChildren(string name, Transform findObjectInThis, ref GameObject foundObject)
    {
        for (int childIndex = 0; childIndex < findObjectInThis.childCount; childIndex++)
        {
            GameObject childObject = findObjectInThis.GetChild(childIndex).gameObject;
            if (childObject.name == name)
            {
                foundObject = childObject;
            }
        }
    }

    public static void findFirstObjectWithName(string name, Transform findObjectInThis, ref GameObject foundObject)
    {
        GameObject thisGameobject = findObjectInThis.gameObject;

        if (thisGameobject.name == name)
        {
            foundObject = thisGameobject;
            return;
        }

        for (int childIndex = 0; childIndex < findObjectInThis.childCount; childIndex++)
        {
            findFirstObjectWithName(name, findObjectInThis.GetChild(childIndex), ref foundObject);
        }
    }

    public static bool findFirstObjectOfType<FindType>(ref FindType foundObject, Transform findObjectInThis)
    {
        FindType potentialObject = findObjectInThis.gameObject.GetComponent<FindType>();

        if (potentialObject != null)
        {
            foundObject = potentialObject;
            return true;
        }

        for (int childIndex = 0; childIndex < findObjectInThis.childCount; childIndex++)
        {
            bool found = findFirstObjectOfType(ref foundObject, findObjectInThis.GetChild(childIndex));

            if (found)
            {
                return true;
            }
        }

        return false;
    }

    public static void findAllObjectsOfType<FindType>(Transform findButtonsInThis, List<FindType> buttons)
    {
        FindType button = findButtonsInThis.gameObject.GetComponent<FindType>();
        if (button != null)
        {
            buttons.Add(button);
        }

        for (int childIndex = 0; childIndex < findButtonsInThis.childCount; childIndex++)
        {
            findAllObjectsOfType(findButtonsInThis.GetChild(childIndex), buttons);
        }
    }
}
