using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateSamurai : StateMachineBehaviour {

    public float m_damping = 0f;

    private readonly int m_hashHorizontalPara = Animator.StringToHash("Horizontal");
    private readonly int m_hashVerticalPata = Animator.StringToHash("Vertical");

    private PlayerController playerCon;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCon = animator.gameObject.GetComponent<PlayerController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //float horizontal = playerCon.player.GetAxis("MoveHorizontal");
        //float vertical = playerCon.player.GetAxis("MoveVertical");

        Vector2 input = new Vector2(playerCon.m_localVel.x, playerCon.m_localVel.z).normalized;

        animator.SetFloat(m_hashHorizontalPara, input.x, m_damping, Time.deltaTime);
        animator.SetFloat(m_hashVerticalPata, input.y, m_damping, Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
