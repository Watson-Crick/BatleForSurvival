using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannibalController : AIController {

    private BoxCollider boxCollider_Above;
    private BoxCollider boxCollider_Below;

    protected override void Awake()
    {
        base.Awake();
        boxCollider_Above = M_Transform.Find("Armature/Hips/Middle_Spine").GetComponent<BoxCollider>();
        boxCollider_Below = M_Transform.Find("Armature").GetComponent<BoxCollider>();

        //AudioManager.Instance.AddAudioSourceComponent(ClipName.ZombieScream, gameObject, true, true, 0.05f);
    }

    protected override void DeathState()
    {
        M_NavMeshAgent.enabled = false;
        M_Animator.enabled = false;
        AIState = AIState.DEATH;
        boxCollider_Above.enabled = false;
        boxCollider_Below.enabled = false;
        StartCoroutine(Dead());
    }

    protected override void PlayAttackAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.ZombieAttack, M_Transform.position, 0.5f);
    }

    protected override void PlayDeathAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.ZombieDeath, M_Transform.position);
    }

    protected override void PlayHitAudio()
    {
        AudioManager.Instance.PlayAudioClipByEnum(ClipName.ZombieInjured, M_Transform.position);
    }
}
