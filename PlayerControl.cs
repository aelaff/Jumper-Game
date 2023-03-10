using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	//This script for controlling the player
	Rigidbody rb;
	//Animator anim;
	public float jumpHeight = 1;
	public float speed = 1;
	bool isJumping = false;
	Player myPlayer;
    //float alpha = 0;
    //float hor, ver;


    // Use this for initialization
    void Start() {
		myPlayer = GameManager.instance.newPlayerScript;
		rb = GetComponent<Rigidbody>();
		InvokeRepeating("SendTransformToFirebase",0,.1f);
		



	}

	// Update is called once per frame
	void Update() {

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.velocity = Vector3.up * jumpHeight;
        }
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * speed, rb.velocity.y, Input.GetAxis("Vertical") * Time.deltaTime * speed);



    }
    void SendTransformToFirebase() {

		myPlayer.player_position_x = transform.localPosition.x;
		myPlayer.player_position_y = transform.localPosition.y;
		myPlayer.player_position_z = transform.localPosition.z;
		myPlayer.player_rotation_x = transform.localRotation.x;
		myPlayer.player_rotation_y = transform.localRotation.y;
		myPlayer.player_rotation_z = transform.localRotation.z;
		transform.localScale = new Vector3(50 + myPlayer.player_score / 5, 50 + myPlayer.player_score / 5, 50 + myPlayer.player_score / 5);
		myPlayer.player_scale_x = transform.localScale.x/50;
		myPlayer.player_scale_y = transform.localScale.y/50;
		myPlayer.player_scale_z = transform.localScale.z/50;
		if (transform.hasChanged) { 
			Dictionary<string, object> entryValues = myPlayer.ToDictionary();
			Router.PlayerWithUID(myPlayer.player_id).UpdateChildrenAsync(entryValues);
		}

	}
	void OnCollisionStay(Collision collision) {
		if (collision.gameObject.tag == "ground") {
			isJumping = false;
		}
	}
	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "ground")
		{
			isJumping = true;

		}
	}
	void OnTriggerExit(Collider collision)
	{
		if (collision.tag=="ground"&& isJumping)
		{
			collision.GetComponent<Renderer>().material.color = GameManager.instance.playerColor;
			collision.transform.GetChild(0).GetComponent<Animator>().enabled = true;
			collision.transform.GetChild(0).GetComponent<Animator>().Play(0);
			myPlayer.player_score++;
			
			Router.Grounds().Child(collision.name).SetValueAsync("#"+ColorUtility.ToHtmlStringRGB(GameManager.instance.playerColor));
		}
		
	}
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "enemy" && isJumping)
		{
			GameManager.instance.playersManager.DetelePlayer(collision.transform.parent.name);

		}
	}

}
