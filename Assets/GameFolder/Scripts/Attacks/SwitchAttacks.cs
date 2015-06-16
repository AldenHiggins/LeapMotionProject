using UnityEngine;
using System.Collections;

public class SwitchAttacks : AAttack
{
	public GameObject attackChoices;
	public OffensiveAbilities offense;

	private int currentAttackIndex = 0;

	void Start()
	{}

	void Update()
	{}


    //public override void chargingFunction(HandModel[] hands)
    //{

    //}
	
    //public override void chargedFunction(HandModel[] hands){}
	
    //public override void releaseFunction(HandModel[] hands)
    //{
    //    currentAttackIndex++;
    //    if (currentAttackIndex >= attackChoices.transform.childCount)
    //    {
    //        currentAttackIndex = 0;
    //    }
    //    AttackContainer newAttackContainer = (AttackContainer)attackChoices.transform.GetChild (currentAttackIndex).gameObject.GetComponent (typeof(AttackContainer));
    //    offense.rightHandFlipAttack = newAttackContainer.thisAttack;
    //    offense.leftHandFlipAttack = newAttackContainer.thisAttack;
    //}
	
	
    //public override void holdGestureFunction(HandModel[] hands){}
	
    //public override void inactiveFunction(){}
}


