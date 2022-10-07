using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerGameData", menuName = "UntitledEndlessRunner/PlayerGameData", order = 0)]

public class PlayerGameData : ScriptableObject
{
    public int currentReservedCoins;
    public int totalCoins;

    public int currentBestDistance;

}
