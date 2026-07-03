using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

[CreateAssetMenu(fileName = "AudioBank", menuName = "Data/Items/AudioBank")]
public class AudioBank : ScriptableObject
{
    

    [Header("One Shot Clips")]
    [SerializeField] private AudioClip uIClick;
    [SerializeField] private AudioClip bossLunge;
    [SerializeField] private AudioClip bossSummon;
    [SerializeField] private AudioClip berryClick;
    [SerializeField] private AudioClip musketClick;
    [SerializeField] private AudioClip buttonHover;

    [Header("Ambience/Loops")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip firstLevelAmbience;
    [SerializeField] private AudioClip secondLevelAmbience;
    [SerializeField] private AudioClip thirdLevelMusic;
    

    [Header("Multi Sound Effects")]
    [SerializeField] private AudioClip[] musketFire = new AudioClip[2];
    [SerializeField] private AudioClip[] musketHit = new AudioClip[2];
    [SerializeField] private AudioClip[] rapierHit = new AudioClip[2];
    [SerializeField] private AudioClip[] playerMove = new AudioClip[2];
    [SerializeField] private AudioClip[] playerHeal = new AudioClip[2];
    [SerializeField] private AudioClip[] wolfAttack = new AudioClip[2];


    //one shot getters
    public AudioClip UIClick => uIClick;

    public AudioClip BossLunge => bossLunge;
    public AudioClip BossSummon => bossSummon;
    public AudioClip BerryClick => berryClick;
    public AudioClip MusketClick => musketClick;
    public AudioClip ButtonHover => buttonHover;

    //one shot random getters
    public AudioClip[] PlayerMove => playerMove;
    public AudioClip[] MusketFire => musketFire;
    public AudioClip[] MusketHit => musketHit;
    public AudioClip[] RapierHit => rapierHit;
    public AudioClip[] PlayerHeal => playerHeal;
    public AudioClip[] WolfAttack => wolfAttack;
    

    //loop getters
    public AudioClip MainMenuMusic => mainMenuMusic;
    public AudioClip FirstLevelAmbience => firstLevelAmbience;
    public AudioClip SecondLevelAmbience => secondLevelAmbience;
    public AudioClip ThirdLevelMusic => thirdLevelMusic;



    // selecting piece sound
    // 
}
