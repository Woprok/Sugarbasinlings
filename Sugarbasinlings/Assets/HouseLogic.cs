using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseLogic : MonoBehaviour
{
    public int CakeCapacity = 1;
    public float CakeProduction = 0.1f;
    public int FillLevel = 3;
    public bool IsStanding = true;

    // Start is called before the first frame update
    public void Start()
    {
    }

    public void EatHouse(HumanLogic who)
    {
        who.OnSatisficationChange(FillLevel);
        IsStanding = false;
        Destroy(this.gameObject);
    }
}
