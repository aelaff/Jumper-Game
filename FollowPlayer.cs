using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	public Transform player;
	
	// Update is called once per frame
	void Update()
	{
		//for making the text and camera following the main player 
		if (GameManager.instance.isGamePlaying)
		{
			if(player==null)
				player = GameObject.FindWithTag("Player").transform;
			transform.position = new Vector3(player.position.x, player.position.y+1, player.position.z + 0.8f);

		}


	}

}
