using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Living
{

    private Animator animator;

    public override void Initialize()
    {
        base.Initialize();
        animator = GetComponent<Animator>();
    }

    public override void LocalUpdate()
    {
        base.LocalUpdate();

        animator.SetBool("walking", currentVelocity.magnitude > 0);

    }

}
