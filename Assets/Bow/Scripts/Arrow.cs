/*
    Copyright (C) 2016 FusionEd

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */


using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{
	private bool isAttached = false;

	private bool isFired = false;

	void OnTriggerStay()
    {
		AttachArrow ();
	}

	void OnTriggerEnter()
    {
		AttachArrow ();
	}

	void Update()
    {
		if (isFired && transform.GetComponent<Rigidbody> ().velocity.magnitude > 5f)
        {
			transform.LookAt (transform.position + transform.GetComponent<Rigidbody> ().velocity);
		}
	}

	public void Fired()
    {
		isFired =  true; 
	}

	private void AttachArrow()
    {
		//var device = SteamVR_Controller.Input((int)ArrowManager.Instance.trackedObj.index);
		//if (!isAttached && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger))
  //      {
		//	ArrowManager.Instance.AttachBowToArrow ();
		//	isAttached = true;
		//}
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == null)
        {
            return;
        }

        if (collision.collider.gameObject == null)
        {
            return;
        }

        if (collision.collider.gameObject.name == "Golden Bow")
        {
            return;
        }

        BasicEnemyController enemy = collision.collider.gameObject.GetComponent<BasicEnemyController>();

        if (enemy != null)
        {
            enemy.dealDamage(100);
        }

        Destroy(gameObject);
    }
}
