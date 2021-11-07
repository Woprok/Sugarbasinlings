using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeLogic : MonoBehaviour
{
    public int SpawnCost = 0;
    public int FillLevel = 0;

    // Start is called before the first frame update
    public void Start()
    {
        //Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Houses"));
    }

    public void EatCake(HumanLogic who)
    {
        who.OnSatisficationChange(FillLevel);
        Destroy(this.gameObject);
    }
}
