using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // GameObjects
    public HumanSpawner Spawner;
    public TextMeshProUGUI CakesLeft;
    public TextMeshProUGUI Score;
    public HouseLogic[] Houses;
    public CastleLogic[] Castles;
    public TextMeshProUGUI GameFinalMessage;
    public TextMeshProUGUI GameFinalButtonText;
    public Button GameFinalButton;
    public GameObject GameFinal;
    public CakeClicker Clicker;

    public bool CanUpdate = true;

    public int InvasionAttemptsTotal => Spawner.WaveCount;
    internal int InvasionAttemptsLeft;

    public float WaveTimeInterval = 20.0f; // every 20 seconds
    public float FirstTimeIntervalDelay = 2.0f; // every 20 seconds

    private int CurrentCakes => (int)Math.Floor(CurrentCakesUpdateable);
    public float CurrentCakesUpdateable = 0.0f;
    public float CakeProduction; // per second
    public int CakeCapacity; // max stored cakes
    /// <summary>
    /// per second
    /// </summary>
    public int CurrentScore => (int)Math.Floor(CurrentScoreUpdateable);
    public float CurrentScoreUpdateable = 0;

    public float UpdatesPassed = 0;
    public float UpdateFrequency = 1.0f;

    // Start is called before the first frame update
    public void Start()
    {
        Spawner.Reset();
        UpdatesPassed = 0;
        CurrentScoreUpdateable = 0;
        CurrentCakesUpdateable = 0.0f;
        UpdatesPassed = 0;

        InvasionAttemptsLeft = InvasionAttemptsTotal;

        Houses = GameObject.FindGameObjectsWithTag("House").Select(h => h.GetComponent<HouseLogic>()).ToArray();
        Castles = GameObject.FindGameObjectsWithTag("Castle").Select(h => h.GetComponent<CastleLogic>()).ToArray();
        UpdateCakes();
    }

    private void TrySpawnWave()
    {
        if (UpdatesPassed < FirstTimeIntervalDelay)
            return;

        if (UpdatesPassed > WaveTimeInterval * Spawner.CurrentWave)
        {
            Spawner.CreateNextWave();
            InvasionAttemptsLeft--;
        }
    }

    private void ProduceCakes()
    {
        CurrentCakesUpdateable += CakeProduction;
        CurrentCakesUpdateable = Mathf.Min(CurrentCakesUpdateable, CakeCapacity);
    }

    private void UpdateCakes()
    {
        UpdateCakeCapacity();
        UpdateCakeProduction();
        CurrentCakesUpdateable = Mathf.Min(CurrentCakesUpdateable, CakeCapacity);
    }

    private void UpdateCakeProduction()
    {
        CakeProduction = 
            Houses.Where(h => h.IsStanding).Sum(h => h.CakeProduction) 
            + 
            Castles.Where(c => c.IsStanding).Sum(c => c.CakeProduction);
    }

    private void UpdateCakeCapacity()
    {
        CakeCapacity = 
            Houses.Where(h => h.IsStanding).Sum(h => h.CakeCapacity) 
            + 
            Castles.Where(c => c.IsStanding).Sum(c => c.CakeCapacity);
    }

    // Update is called once per frame
    public void Update()
    {
        if (!CanUpdate)
            return;

        UpdateCakes();

        var timeDiff = Time.timeSinceLevelLoad - UpdatesPassed;
        var timeFrequencyDiff = timeDiff - UpdateFrequency;

        // Update on frequency quota
        if (timeFrequencyDiff > 0.0f)
        {
            UpdatesPassed += UpdateFrequency;
            ProduceCakes();
            UpdateScore();
            TrySpawnWave();
        }
        RedrawUserInterface();
        CheckConditions();
    }

    private void RedrawUserInterface()
    {
        CakesLeft.text = $"Cakes: {CurrentCakes}/{CakeCapacity} ({CakeProduction}/sec)";
        Score.text = $"Legacy: {CurrentScore}";
    }

    private void UpdateScore()
    {
        CurrentScoreUpdateable += CakeProduction;
        if (CurrentCakes == CakeCapacity)
            CurrentScoreUpdateable++;
    }

    internal void UpdateScore(int satisficationLevel, int hungryLevel)
    {
        if (satisficationLevel == hungryLevel)
        {
            CurrentScoreUpdateable += hungryLevel;
        }
        else
        {
            CurrentScoreUpdateable += hungryLevel / 2;
        }
    }

    private void CheckConditions()
    {
        if (HasFailed())
        {
            EnableGameOver();
            return;
        }
        if (HasSurvived())
        {
            EnableGameWin();
            return;
        }
    }

    private bool HasSurvived()
    {
        return InvasionAttemptsLeft <= 0 && Spawner.HasUnsatisfiedHuman() == false;
    }

    private bool HasFailed()
    {
        if (Castles.Any(c => c != null && c.IsStanding))
            return false;
        else
            return true;
    }

    private void EnableGameOver()
    {
        CanUpdate = false;
        Clicker.gameObject.SetActive(false);
        GameFinal.SetActive(true);
        ScoreboardExtensions.SaveScore(CurrentScore, $"Failed after {UpdatesPassed} seconds");
        GameFinalMessage.text = $"Humans devoured all of the sugarkind.";
        GameFinalButtonText.text = "BACK TO MENU";
        GameFinalButton.onClick.AddListener(OnFinalConfirm);
    }

    private void EnableGameWin()
    {
        CanUpdate = false;
        Clicker.gameObject.SetActive(false);
        GameFinal.SetActive(true);
        ScoreboardExtensions.SaveScore(CurrentScore, $"Heroically saved Sugarlings");
        GameFinalMessage.text = $"You have heroically saved all of sugarkind.";
        GameFinalButtonText.text = "BACK TO MENU";
        GameFinalButton.onClick.AddListener(OnFinalConfirm);
    }

    public void OnFinalConfirm()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    internal bool CanBuildCake(int cakeCost)
    {
        if (CurrentCakesUpdateable - cakeCost > 0.0f)
            return true;
        return false;
    }

    internal void BuildCake(int cakeCost)
    {
        CurrentCakesUpdateable -= cakeCost;
    }
}