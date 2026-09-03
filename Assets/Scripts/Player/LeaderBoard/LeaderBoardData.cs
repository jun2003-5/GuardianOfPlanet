using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoardData : MonoBehaviour
{
    public enum BoardType {InfiniteStage, TotalDamage};
    public BoardType typeOfData;

    public TextMeshProUGUI UserRank;
    public TextMeshProUGUI UserName;
    public TextMeshProUGUI UserScore;

    public GameObject Focus;

    public void setUserData(string rank, string name, string score)
    {
        UserRank.text = rank;
        UserName.text = name.Substring(0, name.IndexOf("#"));

        if(typeOfData == BoardType.InfiniteStage) {
            UserScore.text = "스테이지 " + score;
        } else {
            UserScore.text = score;
        }
    }
}
