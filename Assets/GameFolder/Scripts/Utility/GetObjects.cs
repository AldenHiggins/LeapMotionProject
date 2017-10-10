using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetObjects : MonoBehaviour
{
    public static GetObjects instance;

    // Key game objects
    private PlayerLogic player;
    private GameObject mainCamera;
    private ControllableUnit controllableUnit;
    private GameObject goalPosition;
    private DefensiveGrid defensiveGrid;
    private LevelBounds levelBounds;
    private GameObject pauseMenu;
    private VRControls vrControls;

    // UI elements
    private AttackSelection attackSelectionUI;

    // Object containers
    private Transform rootTransform;
    private Transform scene;
    private Transform attackContainer;
    private Transform defenseContainer;
    private Transform attackParticleContainer;
    private Transform miscContainer;
    private Transform spawnedEnemies;
    private Transform enemyWaves;
    private Transform movingObjectsContainer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public PlayerLogic getPlayer()
    {
        if (player)
        {
            return player;
        }

        findFirstObjectOfType(ref player, getRootTransform());

        if (player == null)
        {
            Debug.LogError("PlayerLogic could not be found in scene!");
            return null;
        }
        return player;
    }

    public ControllableUnit getControllableUnit()
    {
        if (controllableUnit)
        {
            return controllableUnit;
        }
        
        findFirstObjectOfType(ref controllableUnit, getScene().transform);

        if (controllableUnit == null)
        {
            Debug.LogError("ControllableUnit could not be found in scene!");
            return null;
        }

        return controllableUnit;
    }

    public Transform getRootTransform()
    {
        if (rootTransform)
        {
            return rootTransform;
        }

        GameObject[] root = GameObject.FindGameObjectsWithTag("Root");

        if (root == null || root.Length == 0)
        {
            Debug.LogError("RootTransform could not be found in scene!");
            return null;
        }

        rootTransform = root[0].transform;
        return rootTransform;
    }

    public Transform getAttackContainer()
    {
        if (attackContainer)
        {
            return attackContainer;
        }

        GameObject attackContainerGameObject = null;
        findNameInChildren("Attacks", getRootTransform(), ref attackContainerGameObject);

        if (attackContainerGameObject != null)
        {
            attackContainer = attackContainerGameObject.transform;
            return attackContainer;
        }

        attackContainerGameObject = new GameObject();
        attackContainerGameObject.name = "Attacks";
        attackContainerGameObject.transform.parent = getRootTransform();
        attackContainer = attackContainerGameObject.transform;
        return attackContainer;
    }

    public Transform getDefenseContainer()
    {
        if (defenseContainer)
        {
            return defenseContainer;
        }

        GameObject defenseContainerGameObject = null;
        findNameInChildren("Defenses", getScene(), ref defenseContainerGameObject);

        if (defenseContainerGameObject != null)
        {
            defenseContainer = defenseContainerGameObject.transform;
            return defenseContainer;
        }

        defenseContainerGameObject = new GameObject();
        defenseContainerGameObject.name = "Defenses";
        defenseContainerGameObject.transform.parent = getScene();
        defenseContainer = defenseContainerGameObject.transform;
        return defenseContainer;
    }

    public Transform getAttackParticleContainer()
    {
        if (attackParticleContainer)
        {
            return attackParticleContainer;
        }

        GameObject attackParticleContainerGameObject = null;
        findNameInChildren("AttackParticles", getScene().transform, ref attackParticleContainerGameObject);

        if (attackParticleContainerGameObject != null)
        {
            attackParticleContainer = attackParticleContainerGameObject.transform;
            return attackParticleContainer;
        }

        attackParticleContainerGameObject = new GameObject();
        attackParticleContainerGameObject.name = "AttackParticles";
        attackParticleContainerGameObject.transform.parent = getScene().transform;
        attackParticleContainerGameObject.transform.localScale = Vector3.one;
        attackParticleContainer = attackParticleContainerGameObject.transform;
        return attackParticleContainer;
    }

    public GameObject getCamera()
    {
        if (mainCamera)
        {
            return mainCamera;
        }

        findFirstObjectWithName("CenterEyeAnchor", getRootTransform(), ref mainCamera);

        if (mainCamera == null)
        {
            Debug.LogError("MainCamera not found in scene!!");
            return null;
        }

        return mainCamera;
    }

    public Transform getSpawnedEnemies()
    {
        if (spawnedEnemies)
        {
            return spawnedEnemies;
        }

        GameObject spawnedEnemiesGameObject = null;
        findFirstObjectWithName("SpawnedEnemies", getScene(), ref spawnedEnemiesGameObject);

        if (spawnedEnemiesGameObject == null)
        {
            Debug.LogError("SpawnedEnemies not found in scene!");
            return null;
        }

        spawnedEnemies = spawnedEnemiesGameObject.transform;
        return spawnedEnemies;
    }

    public GameObject getGoalPosition()
    {
        if (goalPosition)
        {
            return goalPosition;
        }

        findFirstObjectWithName("EnemyGoal", getRootTransform(), ref goalPosition);

        if (goalPosition == null)
        {
            Debug.LogError("GoalPosition not found in scene!");
            return null;
        }

        return goalPosition;
    }

    public Transform getEnemyWaves()
    {
        if (enemyWaves)
        {
            return enemyWaves;
        }

        GameObject enemyWavesGameObject = null;
        findFirstObjectWithName("EnemyWaves", getScene(), ref enemyWavesGameObject);

        if (enemyWaves == null)
        {
            Debug.LogError("EnemyWaves not found in scene!");
            return null;
        }

        enemyWaves = enemyWavesGameObject.transform;
        return enemyWaves;
    }

    public Transform getMiscContainer()
    {
        if (miscContainer)
        {
            return miscContainer;
        }

        GameObject miscContainerGameObject = null;
        findFirstObjectWithName("MiscGameObjects", getRootTransform(), ref miscContainerGameObject);

        if (miscContainerGameObject == null)
        {
            Debug.LogError("MiscGameObjects not found in scene!!");
            return null;
        }

        miscContainer = miscContainerGameObject.transform;
        return miscContainer;
    }

    public Transform getMovingObjectsContainer()
    {
        if (movingObjectsContainer)
        {
            return movingObjectsContainer;
        }

        GameObject movingObjectsContainerGameObject = null;
        findFirstObjectWithName("MovingObjects", getRootTransform(), ref movingObjectsContainerGameObject);

        if (movingObjectsContainerGameObject == null)
        {
            Debug.LogError("MovingObjects not found in scene!");
            return null;
        }

        movingObjectsContainer = movingObjectsContainerGameObject.transform;
        return movingObjectsContainer;
    }

    public DefensiveGrid getDefensiveGrid()
    {
        if (defensiveGrid)
        {
            return defensiveGrid;
        }

        findFirstObjectOfType(ref defensiveGrid, getRootTransform());

        if (defensiveGrid == null)
        {
            Debug.LogError("DefensiveGrid not found in scene!!");
            return null;
        }

        return defensiveGrid;
    }

    public LevelBounds GetLevelBounds()
    {
        if (levelBounds)
        {
            return levelBounds;
        }

        findFirstObjectOfType(ref levelBounds, getScene());

        if (levelBounds == null)
        {
            Debug.LogError("LevelBounds not found in scene!!");
            return null;
        }

        return levelBounds;
    }

    public Transform getScene()
    {
        if (scene)
        {
            return scene;
        }

        GameObject[] sceneRootObjects = UnityEngine.SceneManagement.SceneManager.GetSceneAt(2).GetRootGameObjects();

        // If there are multiple scene root objects look for the one that is named "Scene"
        if (sceneRootObjects == null || sceneRootObjects.Length == 0)
        {
            Debug.LogError("Could not find scene!");
            return null;
        }
        else if (sceneRootObjects.Length == 1)
        {
            scene = sceneRootObjects[0].transform;
        }
        else
        {
            for (int objectIndex = 0; objectIndex < sceneRootObjects.Length; objectIndex++)
            {
                GameObject objectToTest = sceneRootObjects[objectIndex];
                if (objectToTest.name == "Scene")
                {
                    scene = objectToTest.transform;
                    break;
                }
            }
        }
        
        return scene;
    }

    public GameObject getPauseMenu()
    {
        if (pauseMenu)
        {
            return pauseMenu;
        }

        findFirstObjectWithName("PauseMenu", getRootTransform(), ref pauseMenu);

        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu not found in scene!");
            return null;
        }

        return pauseMenu;
    }

    public AttackSelection getAttackSelection()
    {
        if (attackSelectionUI)
        {
            return attackSelectionUI;
        }

        findFirstObjectOfType(ref attackSelectionUI, getRootTransform());

        if (attackSelectionUI == null)
        {
            Debug.LogError("AttackSelection not found in scene!");
            return null;
        }

        return attackSelectionUI;
    }

    public VRControls getVRControls()
    {
        if (vrControls)
        {
            return vrControls;
        }

        findFirstObjectOfType(ref vrControls, getRootTransform());

        if (vrControls == null)
        {
            Debug.LogError("VRControls not found in scene!");
            return null;
        }

        return vrControls;
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
