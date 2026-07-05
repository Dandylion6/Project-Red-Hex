using UnityEngine;

[CreateAssetMenu(fileName = "AudioBank", menuName = "Data/Items/AudioBank")]
public class AudioBank : ScriptableObject
{
    [Header("UI Clips")]
    [SerializeField] private AudioClip uIClick;
    [SerializeField] private AudioClip berryClick;
    [SerializeField] private AudioClip musketClick;
    [SerializeField] private AudioClip buttonHover;
    [SerializeField] private AudioClip[] turnChange;

    [Header("Ambience/Loops")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip firstLevelAmbience;
    [SerializeField] private AudioClip secondLevelAmbience;
    [SerializeField] private AudioClip thirdLevelMusic;

    [Header("Landing Clips")]
    [SerializeField] private AudioClip[] onGrassLand;
    [SerializeField] private AudioClip[] onGravelLand;
    [SerializeField] private AudioClip[] onRockLand;

    [Header("One Shot Clips")]
    [SerializeField] private AudioClip[] selectPiece;
    [SerializeField] private AudioClip[] healing;
    [SerializeField] private AudioClip[] damageTaken;
    [SerializeField] private AudioClip[] eatBerries;
    [SerializeField] private AudioClip[] musketFire;
    [SerializeField] private AudioClip[] musketHit;
    [SerializeField] private AudioClip[] rapierHit;
    [SerializeField] private AudioClip[] bossLunge;
    [SerializeField] private AudioClip[] bossSummon;
    [SerializeField] private AudioClip[] bossClaw;
    [SerializeField] private AudioClip[] wolfAttack;


    public AudioClip UIClick => uIClick;
    public AudioClip BerryClick => berryClick;
    public AudioClip MusketClick => musketClick;
    public AudioClip ButtonHover => buttonHover;
    public AudioClip[] TurnChange => turnChange;

    public AudioClip[] OnGrassLand => onGrassLand;
    public AudioClip[] OnRockLand => onRockLand;
    public AudioClip[] OnGravelLand => onGravelLand;

    public AudioClip[] SelectPiece => selectPiece;
    public AudioClip[] MusketFire => musketFire;
    public AudioClip[] MusketHit => musketHit;
    public AudioClip[] RapierHit => rapierHit;
    public AudioClip[] Healing => healing;
    public AudioClip[] DamageTaken => damageTaken;
    public AudioClip[] EatBerries => eatBerries;
    public AudioClip[] BossLunge => bossLunge;
    public AudioClip[] BossSummon => bossSummon;
    public AudioClip[] BossClaw => bossClaw;
    public AudioClip[] WolfAttack => wolfAttack;
    

    public AudioClip MainMenuMusic => mainMenuMusic;
    public AudioClip FirstLevelAmbience => firstLevelAmbience;
    public AudioClip SecondLevelAmbience => secondLevelAmbience;
    public AudioClip ThirdLevelMusic => thirdLevelMusic;
}
