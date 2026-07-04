using UnityEngine;

public class ClawSlashSequence : SlashSequence
{
    protected override void PlaySound()
    {
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.BossClaw);
    }
}
