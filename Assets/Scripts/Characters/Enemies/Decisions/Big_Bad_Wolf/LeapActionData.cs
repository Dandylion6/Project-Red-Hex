using UnityEngine;

[CreateAssetMenu(fileName = "New Leap Action", menuName = "Data/Items/Actions/Leap")]
public class LeapActionData : ItemData
{
    [Header("Leap Data")]
    [SerializeField][Tooltip("The distance the player is from the enemy in which the leap will start counting turns")] private int triggerDistance = 2;
    [SerializeField][Tooltip("The number of turns to commit to the leap")] private int turnsToCommit = 2;


    public int TriggerDistance => triggerDistance;
    public int TurnsToCommit => turnsToCommit;
}
