using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    SceneLoader sceneLoader;
    [SerializeField] int breakableBlocks; // Serialized for debugging purposes (read only)

    void Start()
	{
		sceneLoader = FindObjectOfType<SceneLoader>();
		breakableBlocks = 0;
	}

    public void IncrementBlocks()
    {
		breakableBlocks++;
    }

    public void BlockWasBroken()
    {
		breakableBlocks--;
    }

    public void MoveBallBackToPaddle()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (breakableBlocks == 0)
		{
			sceneLoader.LoadNextScene();
		}
	}



}
