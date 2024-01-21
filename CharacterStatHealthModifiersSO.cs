using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifiersSO : CharacterStatModifier
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Player health = character.GetComponent<Player>();
        if (health != null)
            health.AddHealth((int)val);
    }
}