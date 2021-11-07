using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class HumanLogic : MonoBehaviour
{
    public Sprite secondary;
    public RuntimeAnimatorController secondaryController;

    internal static Random TargetRandom = new Random();
    public GameObject CurrentTarget;
    public bool IsFull => SatisficationLevel >= HungryLevel;
    public int HungryLevel = 1;
    public int SatisficationLevel = 0;
    public float SpeedBase = 1.0f;
    public float MinimumSpeed = 1.0f;
    internal float CurrentSpeed;
    public float SpeedDecreasePerSatisficationLevel = 0.2f;

    internal float InitialPositionX;
    internal HumanSpawner parent;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        InitialPositionX = transform.position.x;
        CurrentSpeed = SpeedBase;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTarget == null && IsFull == false)
        {
            ChooseTarget();
        }
        else
        {
            FollowPath();
        }
    }

    private void ChooseTarget()
    {
        var currentHouses = GameObject.FindGameObjectsWithTag("House");
        if (currentHouses.Length != 0)
        {
            CurrentTarget = currentHouses[TargetRandom.Next(currentHouses.Length)];
            return;
        }
        var currentCastles = GameObject.FindGameObjectsWithTag("Castle");
        if (currentCastles.Length != 0)
        {
            CurrentTarget = currentCastles[TargetRandom.Next(currentCastles.Length)];
        }
    }

    private void FollowPath()
    {
        if (!IsFull)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(CurrentTarget.transform.position.x, CurrentTarget.transform.position.y, 0), Time.deltaTime * CurrentSpeed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(InitialPositionX, transform.position.y, 0), Time.deltaTime * CurrentSpeed);
            if (transform.position.x == InitialPositionX)
            {
                parent.DestroyHuman(gameObject);
            }
        }
    }

    public void OnSatisficationChange(int fillValue)
    {
        SatisficationLevel += fillValue;
                
        CurrentSpeed = Math.Max(SpeedBase - SpeedDecreasePerSatisficationLevel * SatisficationLevel, MinimumSpeed);

        if (IsFull)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = secondary;
            gameObject.GetComponent<Animator>().runtimeAnimatorController = secondaryController;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsFull)
        {
            switch (collision.gameObject.tag)
            {
                case "Cake":
                    collision.gameObject.GetComponent<CakeLogic>().EatCake(this.gameObject.GetComponent<HumanLogic>());
                    break;
                case "House":
                    collision.gameObject.GetComponent<HouseLogic>().EatHouse(this.gameObject.GetComponent<HumanLogic>());
                    break;
                case "Castle" when CurrentTarget.tag == "Castle":
                    collision.gameObject.GetComponent<CastleLogic>().EatCastle(this.gameObject.GetComponent<HumanLogic>());
                    break;
            }
        }
    }
}
