using UnityEngine;
using System.Collections;

public class PlaceOilSlick : AAttack
{
	public GameObject oilSlick;
	public GameObject oilSlickPending;
	private bool isInstantiated = false;
	private GameObject createdOilSlick;
	public DefensiveAbilities defense;
	private AudioSource source;
	public AudioClip placeOilSlickSound;

	void Start()
	{
		source = gameObject.GetComponent<AudioSource> ();
	}

//    public override void chargingFunction(HandModel[] hands){

//    }
	
//    public override void chargedFunction(HandModel[] hands){}
	
//    public override void releaseFunction(HandModel[] hands){
//        if (defense.getNumSlicksLeft () > 0) {
//            GameObject oilSlickFinal = (GameObject)Instantiate (oilSlick);
//            source.PlayOneShot(placeOilSlickSound);
//            oilSlickFinal.transform.position = defense.getRayHit().point + new Vector3(0.0f, 0.1f, 0.0f);
//            Destroy (createdOilSlick);

//            OilSlick oilSlickScript = (OilSlick)oilSlickFinal.GetComponent (typeof(OilSlick));
//            oilSlickScript.enabled = true;

//            isInstantiated = false;
//            //TODO: change this
////			defense.slickUsed();
//        }
//    }

//    public override void holdGestureFunction(HandModel[] hands){
//        if (defense.getNumSlicksLeft () > 0) {
//            if (!isInstantiated) {
//                createdOilSlick = (GameObject)Instantiate (oilSlickPending);
//                isInstantiated = true;
//            }
//            createdOilSlick.transform.position = defense.getRayHit().point + new Vector3(0.0f, 0.1f, 0.0f);
//        }
//    }
	
//    public override void inactiveFunction(){
//        if (isInstantiated){
//            isInstantiated = false;
//            Destroy (createdOilSlick);
//        }
//    }
}
	