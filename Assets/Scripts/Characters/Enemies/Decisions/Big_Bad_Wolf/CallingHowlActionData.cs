using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CallingHowlPoint
{
    [Header("Calling Howl Point Settings")]
    [SerializeField][Range(0, 6)] [Tooltip("Amount of dire wolves to spawn.")] private int spawnAmount = 1;
    [SerializeField][Range(0.0f, 100.0f)] [Tooltip("Health percentage at which to trigger the howl.")] private float triggerHealthPercentage = 0.75f;

    
    public int SpawnAmount => spawnAmount;
    public float TriggerHealthPercentage => triggerHealthPercentage;
}


[CreateAssetMenu(fileName = "New Calling Howl Action", menuName = "Data/Items/Actions/Calling Howl")]
public class CallingHowlActionData : ItemData
{
    [Header("Calling Howl Data")] 
    [SerializeField] private List<CallingHowlPoint> callingHowlPoints = new();


    public IReadOnlyList<CallingHowlPoint> CallingHowlPoints => callingHowlPoints;
}
