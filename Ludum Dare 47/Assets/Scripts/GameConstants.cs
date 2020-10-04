using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    public static List<Color> MatchGameColorList = new List<Color>()
    {
        Color.red, Color.blue, Color.green, Color.magenta, Color.cyan, Color.black
    };

    public static List<MatchStageInfo> MatchGameStageInfo = new List<MatchStageInfo>()
    {
        new MatchStageInfo(2,2,2),
        new MatchStageInfo(4,2,3),
        new MatchStageInfo(6,4,3),
        new MatchStageInfo(8,6,4),
        new MatchStageInfo(10,8,4),
        new MatchStageInfo(12,10,5),
        new MatchStageInfo(14,12,5)
    };
}
