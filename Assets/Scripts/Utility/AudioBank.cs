using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioBank", menuName = "Data/Items/AudioBank")]
public class AudioBank : ScriptableObject
{
    

    [Header("One Shot Clips")]
    [SerializeField] private AudioClip uIClick;
    [SerializeField] private AudioClip bossLunge;


    [Header("Ambience/Loops")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip firstLevelAmbience;
    [SerializeField] private AudioClip thirdLevelMusic;
    

    [Header("Multi Sound Effects")]
    [SerializeField] private AudioClip[] musketFire = new AudioClip[2];
    [SerializeField] private AudioClip[] musketHit = new AudioClip[2];
    [SerializeField] private AudioClip[] rapierHit = new AudioClip[2];
    [SerializeField] private AudioClip[] playerMove = new AudioClip[2];
    [SerializeField] private AudioClip[] playerHeal = new AudioClip[2];
    [SerializeField] private AudioClip[] wolfAttack = new AudioClip[2];

    //one shot getters
    public AudioClip[] PlayerMove => playerMove;
    public AudioClip[] MusketFire => musketFire;
    public AudioClip[] MusketHit => musketHit;
    public AudioClip[] RapierHit => rapierHit;
    public AudioClip[] PlayerHeal => playerHeal;
    public AudioClip[] WolfAttack => wolfAttack;
    public AudioClip UIClick => uIClick;

    //loop getters
    public AudioClip MainMenuMusic => mainMenuMusic;
    public AudioClip FirstLevelAmbience => firstLevelAmbience;
    public AudioClip ThirdLevelMusic => thirdLevelMusic;



    // selecting piece sound
    // 
}
