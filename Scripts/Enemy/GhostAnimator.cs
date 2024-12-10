using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GhostAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Setup(bool isAttack)
    {
        _animator.SetBool(AnimatorData.Params.IsAttacked, isAttack);
    }
}