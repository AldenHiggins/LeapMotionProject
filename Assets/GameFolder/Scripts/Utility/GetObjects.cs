using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GetObjects
{
    public static GameLogic getGame()
    {
        GameLogic returnGame = new GameLogic();
        findFirstObjectOfType<GameLogic>(ref returnGame, getRootTransform());
        return returnGame;
    }

    public static PlayerLogic getPlayer()
    {
        PlayerLogic returnPlayer = new PlayerLogic();
        findFirstObjectOfType<PlayerLogic>(ref returnPlayer, getRootTransform());
        return returnPlayer;
    }

    public static Transform getRootTransform()
    {
        Transform xform = UnityEngine.Object.FindObjectOfType<Transform>();
        return xform.root;
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
