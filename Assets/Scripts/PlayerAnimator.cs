using UnityEngine;
using StarterAssets; 
using System.Collections;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private StarterAssetsInputs _input;
    private Coroutine jumpCoroutine;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (_input == null) return;

        // --- 1. HANDLE WALKING ---
        bool isMoving = _input.move != Vector2.zero;
        _animator.SetBool("isWalk", isMoving);

        // --- 2. HANDLE JUMPING ---
        if (_input.jump && jumpCoroutine == null)
        {
            // ✅ CRITICAL FIX: Turn off the jump input immediately!
            // This prevents it from looping forever.
            _input.jump = false; 

            jumpCoroutine = StartCoroutine(JumpSequence());
        }
    }

    private IEnumerator JumpSequence()
    {
        // 1. Play Animation
        _animator.SetBool("isJump", true);

        // 2. Wait for your animation duration
        yield return new WaitForSeconds(0.75f);

        // 3. Stop Animation
        _animator.SetBool("isJump", false);

        // 4. Reset routine
        jumpCoroutine = null;
    }
}