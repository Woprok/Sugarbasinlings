using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLogic : MonoBehaviour
{
    public int CakeCapacity = 10;
    public float CakeProduction = 1.0f;
    public int FillLevel = 10;
    public bool IsStanding = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EatCastle(HumanLogic who)
    {
        who.OnSatisficationChange(FillLevel);
        IsStanding = false;
        Destroy(this.gameObject);
    }
}
