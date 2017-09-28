using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLogic : MonoBehaviour
{
    private GameObject lightingContainer;
    private GameObject sceneGeometryContainer;
    private GameObject alliesContainer;
    private Transform enemyContainer;
    private Transform attackParticlesContainer;

	// Use this for initialization
	void Start ()
    {
        GetObjects.findNameInChildren("Lighting", transform, ref lightingContainer);
        GetObjects.findNameInChildren("SceneGeometry", transform, ref sceneGeometryContainer);
        GetObjects.findNameInChildren("Allies", transform, ref alliesContainer);

        enemyContainer = GetObjects.instance.getSpawnedEnemies();
        attackParticlesContainer = GetObjects.instance.getAttackParticleContainer();

        EventManager.StartListening(GameEvents.GamePause, pauseGame);
        EventManager.StartListening(GameEvents.GameResume, resumeGame);
    }
	
	void pauseGame()
    {
        lightingContainer.SetActive(false);
        sceneGeometryContainer.SetActive(false);

        enableDisableRenderers(false, enemyContainer.gameObject);
        enableDisableRenderers(false, alliesContainer);
        enableDisableRenderers(false, attackParticlesContainer.gameObject);
    }

    void resumeGame()
    {
        lightingContainer.SetActive(true);
        sceneGeometryContainer.SetActive(true);

        enableDisableRenderers(true, enemyContainer.gameObject);
        enableDisableRenderers(true, alliesContainer);
        enableDisableRenderers(true, attackParticlesContainer.gameObject);
    }

    void enableDisableRenderers(bool enableRenderers, GameObject rendererContainer)
    {
        Renderer[] renderers = rendererContainer.GetComponentsInChildren<Renderer>();
        for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
        {
            renderers[rendererIndex].enabled = enableRenderers;
        }
    }
}
