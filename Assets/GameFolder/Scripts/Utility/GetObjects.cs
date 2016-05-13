﻿using UnityEngine;
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
        GameObject[] root = GameObject.FindGameObjectsWithTag("Root");
        Transform xform = root[0].transform;
        return xform.root;
    }

    public static GameObject getCamera()
    {
        GameObject firstFoundOfName = null;
        findFirstObjectWithName("Camera (head)", getRootTransform(), ref firstFoundOfName);

        Debug.Log("First found one is named: " + firstFoundOfName.name);

        return firstFoundOfName;
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
