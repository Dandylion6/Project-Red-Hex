using UnityEngine;

public class ClawSlashSequence : SlashSequence
{
    protected override void PlaySound()
    {
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.RapierHit);
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.WolfAttack);
    }
}
