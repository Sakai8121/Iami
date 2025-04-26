#nullable enable

using UnityEngine;
using unityroom.Api;

public class ScoreHolder
{
    private const string KeyPrefix = "HighScore_";

    // ハイスコアを保存
    public void SaveHighScore(StageIndex stage, float score)
    {
        int currentHighScore = GetHighScore(stage);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetFloat(GetKey(stage), score);
            PlayerPrefs.Save();
            if(stage == StageIndex.Fourth)
                UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreAsc);
            else if(stage == StageIndex.Fifth)
                UnityroomApiClient.Instance.SendScore(2, score, ScoreboardWriteMode.HighScoreAsc);
        }
    }

    // ハイスコアを取得
    public int GetHighScore(StageIndex stage)
    {
        return PlayerPrefs.GetInt(GetKey(stage), 0);
    }

    // PlayerPrefsのキーを生成
    private string GetKey(StageIndex stage)
    {
        return $"{KeyPrefix}{stage}";
    }

    // 全ステージのハイスコアをリセット（オプション）
    // public static void ResetAllHighScores()
    // {
    //     foreach (StageIndex stage in System.Enum.GetValues(typeof(StageIndex)))
    //     {
    //         PlayerPrefs.DeleteKey(GetKey(stage));
    //     }
    //     PlayerPrefs.Save();
    // }
}
