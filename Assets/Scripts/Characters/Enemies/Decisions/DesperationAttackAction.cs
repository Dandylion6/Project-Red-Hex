using UnityEngine;

[CreateAssetMenu(fileName = "New Desperation Attack", menuName = "Data/Items/Actions/Desperation Attack")]
public class DesperationAttackAction : RangedAttackData
{
    [Header("Desperation Data")]
    [SerializeField][Range(0.0f, 100.0f)][Tooltip("The threshold at which the enemy will use the desperation attack")] private float desperationThreshold = 50.0f;


    public float DesperationThreshold => desperationThreshold;
}
