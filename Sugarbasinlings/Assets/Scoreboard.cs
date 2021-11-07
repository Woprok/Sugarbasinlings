using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ScoreboardExtensions
{
    private static readonly string ScoreSaved = "scoreSaved";
    private static readonly string ScoreValue = "scoreValue"; //append with 0-4
    private static readonly string ScoreText = "scoreText"; //append with 0-4
    private const int MaxSavedScores = 5;
    private const int MaxSavedScores2 = 4;

    public static bool HasScoreSaved()
    {
        return !string.IsNullOrEmpty(PlayerPrefs.GetString(ScoreSaved, defaultValue: null));
    }

    public static string GetLeaderboardData()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Scoreboard:");
        List<Tuple<int, string>> scores = new List<Tuple<int, string>>();
        
        for (int i = 0; i<MaxSavedScores; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString($"{ScoreText}{i}", defaultValue: null)))
            {
                scores.Add(
                    new Tuple<int, string>(
                        PlayerPrefs.GetInt($"{ScoreValue}{i}"),
                        PlayerPrefs.GetString($"{ScoreText}{i}", defaultValue: null)
                        ));
            }
        }

        int j = 0;
        foreach (var pair in scores.OrderByDescending(x => x.Item1))
        {
            SaveScore(pair.Item1, pair.Item2, j++);
            builder.AppendLine($"{j}. {pair.Item1.ToString().PadLeft(6)} {pair.Item2}");
        }

        return builder.ToString();
    }

    public static void SaveScore(int score, string text, int position = MaxSavedScores2)
    {
        PlayerPrefs.SetString(ScoreSaved, $"{true}");
        PlayerPrefs.SetInt($"{ScoreValue}{position}", score);
        PlayerPrefs.SetString($"{ScoreText}{position}", text);
    }
}