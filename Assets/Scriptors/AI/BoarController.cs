using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarController : AIController {

    protected override void DeathState()
    {
        M_NavMeshAgent.enabled = false;
        M_Animator.SetTrigger("Death");
        AIState = AIState.DEATH;
        StartCoroutine(Dead());
    }

    protected override void PlayAttackAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.BoarAttack, M_Transform.position);
    }

    protected override void PlayDeathAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.BoarDeath, M_Transform.position);
    }

    protected override void PlayHitAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.BoarInjured, M_Transform.position);
    }
}
