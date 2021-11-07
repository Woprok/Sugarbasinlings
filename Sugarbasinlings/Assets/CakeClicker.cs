using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeClicker : MonoBehaviour
{
    public Game MainGameController;

    public GameObject SmallCake;
    public GameObject MediumCake;
    public GameObject BigCake;

    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {        
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && MainGameController.CanBuildCake(SmallCake.GetComponent<CakeLogic>().SpawnCost))
        {
            MainGameController.BuildCake(SmallCake.GetComponent<CakeLogic>().SpawnCost);
            GameObject newCake = SmallCake;

            var spawnedCake = Instantiate(newCake, new Vector2(mouseWorldPosition.x, mouseWorldPosition.y), newCake.transform.rotation);
            spawnedCake.transform.SetParent(this.gameObject.transform);
        }
        else if (Input.GetMouseButtonDown(2) && MainGameController.CanBuildCake(MediumCake.GetComponent<CakeLogic>().SpawnCost))
        {
            MainGameController.BuildCake(MediumCake.GetComponent<CakeLogic>().SpawnCost);
            GameObject newCake = MediumCake;

            var spawnedCake = Instantiate(newCake, new Vector2(mouseWorldPosition.x, mouseWorldPosition.y), newCake.transform.rotation);
            spawnedCake.transform.SetParent(this.gameObject.transform);
        }
        else if (Input.GetMouseButtonDown(1) && MainGameController.CanBuildCake(BigCake.GetComponent<CakeLogic>().SpawnCost))
        {
            MainGameController.BuildCake(BigCake.GetComponent<CakeLogic>().SpawnCost);
            GameObject newCake = BigCake;

            var spawnedCake = Instantiate(newCake, new Vector2(mouseWorldPosition.x, mouseWorldPosition.y), newCake.transform.rotation);
            spawnedCake.transform.SetParent(this.gameObject.transform);
        }
    }
}
