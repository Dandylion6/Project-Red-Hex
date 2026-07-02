using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);

    public void TakeDamage(int damage, AudioClip clip);

    public void TakeDamage(int damage, AudioClip[] clipList);
}
