using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLane : MonoBehaviour { // script para modificar a lane das moedas -1 0 1 

	public void PositionLane()
	{
		int randomLane = Random.Range(-1, 2);
		transform.position = new Vector3(randomLane, transform.position.y, transform.position.z);
	}
}