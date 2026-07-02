using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioBank", menuName = "Data/Items/AudioBank")]
public class AudioBank : ScriptableObject
{
    

    [Header("One Shot Clips")]
    [SerializeField] private AudioClip playerMove;
    [SerializeField] private AudioClip musketFire;
    [SerializeField] private AudioClip musketHit;
    [SerializeField] private AudioClip rapierHit;
    [SerializeField] private AudioClip playerHeal;
    [SerializeField] private AudioClip uIClick;

    [Header("Ambience/Loops")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip firstLevelAmbience;
    [SerializeField] private AudioClip thirdLevelMusic;

    //one shot getters
    public AudioClip PlayerMove => playerMove;
    public AudioClip MusketFire => musketFire;
    public AudioClip MusketHit => musketHit;
    public AudioClip RapierHit => rapierHit;
    public AudioClip PlayerHeal => playerHeal;
    public AudioClip UIClick => uIClick;

    //loop getters
    public AudioClip MainMenuMusic => mainMenuMusic;
    public AudioClip FirstLevelAmbience => firstLevelAmbience;
    public AudioClip ThirdLevelMusic => thirdLevelMusic;



    // selecting piece sound
    // 
}
