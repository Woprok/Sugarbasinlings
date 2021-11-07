using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Wave
{
    public int Small;
    public int Medium;
    public int Big;
    public int Boss;
}

public class HumanSpawner : MonoBehaviour
{
    public GameObject HumanSmall;
    public GameObject HumanMedium;
    public GameObject HumanBig;
    public GameObject HumanBoss;

    public Wave[] Waves;

    public Game MainGameController;

    public List<GameObject> Humans = new List<GameObject>();
    public List<HumanLogic> HumanLogics = new List<HumanLogic>();

    public MeshCollider SpawnArea;

    public int CurrentWave = 0;
    public int WaveCount => Waves.Length;

    public void CreateNextWave()
    {
        for (int i = 0; i < Waves[CurrentWave].Small; i++)
            CreateHuman(HumanSmall);
        for (int i = 0; i < Waves[CurrentWave].Medium; i++)
            CreateHuman(HumanMedium);
        for (int i = 0; i < Waves[CurrentWave].Big; i++)
            CreateHuman(HumanBig);
        for (int i = 0; i < Waves[CurrentWave].Boss; i++)
            CreateHuman(HumanBoss);
        CurrentWave++;
    }

    public void CreateHuman(GameObject type)
    {
        GameObject newHuman = type;

        var screenX = Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x);
        var screenY = Random.Range(SpawnArea.bounds.min.y, SpawnArea.bounds.max.y);

        var human = Instantiate(newHuman, new Vector2(screenX, screenY), newHuman.transform.rotation);
        Humans.Add(human);
        var humanLogic = human.GetComponent<HumanLogic>();
        humanLogic.parent = this;
        HumanLogics.Add(humanLogic);
        human.transform.SetParent(this.gameObject.transform);
    }

    internal void Reset()
    {
        CurrentWave = 0;
    }

    public bool HasUnsatisfiedHuman() => HumanLogics.Any(hl => hl.IsFull == false);

    public void DestroyHuman(GameObject human)
    {
        var humanLogic = human.GetComponent<HumanLogic>();
        MainGameController.UpdateScore(humanLogic.SatisficationLevel, humanLogic.HungryLevel);
        HumanLogics.Remove(humanLogic);
        Humans.Remove(human);
        Destroy(human);
    }
}