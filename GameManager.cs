
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	//This script to manage the whole game and specially the grounds

	public GameObject groundElement;
	public int gridWidth = 10;
	public int gridHeight = 10;
	public Transform player;
	public int playerScore = 0;
	public Color playerColor;
	public static GameManager instance = null;
	Transform newPlayer;
	public Player newPlayerScript;
	public bool isGamePlaying = false;
	public PlayersManager playersManager;
	public Text playerName;
	string isGenerated = "0";

	void Start()
	{
		Application.targetFrameRate = 60;
		Time.timeScale = 1;
		instance = this;
		Router.GroundWithUID("Generated").GetValueAsync().ContinueWith(task => {
			if (task.IsCompleted)
			{
				DataSnapshot snapshot = task.Result;
				isGenerated= snapshot.Value.ToString();
				GenerateGround();
			}
		});
		Router.Grounds().ChildChanged += HandleChildChanged;


	}

	void GenerateGround()
	{
		for (int i = 0; i < gridHeight; i++)
		{
			for (int j = 0; j < gridWidth; j++)
			{
				GameObject o = Instantiate(groundElement, new Vector3(j * 1.05f, 0, i * .95f), Quaternion.identity);
				//
				if (i % 2 == 0)
				{
					o.transform.position = new Vector3(o.transform.position.x - 0.53f, 0, o.transform.position.z);
				}
				o.name = j + "_" + i;
				o.transform.SetParent(transform);
				if (isGenerated == "0") { 
					Router.Grounds().Child(o.name).SetValueAsync("#fff");
					o.GetComponent<Renderer>().material.color = Color.white;
				}
				else
				{
					Router.GroundWithUID(o.name).GetValueAsync().ContinueWith(task => {

						 if (task.IsCompleted)
						{
							DataSnapshot snapshot = task.Result;
							Color newCol;
							if (ColorUtility.TryParseHtmlString(snapshot.Value.ToString(), out newCol))
								o.GetComponent<Renderer>().material.color = newCol;

						}
					});
				}

			}
		}
		if (isGenerated == "0")
			Router.Grounds().Child("Generated").SetValueAsync("1");
		GenerateBorders();
	}
	void GenerateBorders() {

		// Generate the borders
		Transform LeftCenter = GameObject.Find(0 + "_" + (int)(gridHeight / 2)).transform;
		Transform RightCenter = GameObject.Find((gridWidth - 1) + "_" + (int)(gridHeight / 2)).transform;
		Transform TopCenter = GameObject.Find((int)(gridWidth / 2) + "_" + (gridHeight - 1)).transform;
		Transform BottomCenter = GameObject.Find((int)(gridWidth / 2) + "_" + 0).transform;

		Transform LeftCenterOBJ = new GameObject().transform;
		LeftCenterOBJ.name = "LeftCenter";
		LeftCenterOBJ.gameObject.AddComponent<BoxCollider>();
		LeftCenterOBJ.transform.localScale = new Vector3(0.001f, 10, 100);
		LeftCenterOBJ.transform.localPosition = new Vector3(LeftCenter.position.x - .7f, LeftCenter.position.y, LeftCenter.position.z);

		Transform RightCenterOBJ = new GameObject().transform;
		RightCenterOBJ.name = "RightCenter";
		RightCenterOBJ.gameObject.AddComponent<BoxCollider>();
		RightCenterOBJ.transform.localScale = new Vector3(0.001f, 10, 100);
		RightCenterOBJ.transform.localPosition = new Vector3(RightCenter.position.x + 0.5f, RightCenter.position.y, RightCenter.position.z);

		Transform TopCenterOBJ = new GameObject().transform;
		TopCenterOBJ.name = "TopCenter";
		TopCenterOBJ.gameObject.AddComponent<BoxCollider>();
		TopCenterOBJ.transform.localScale = new Vector3(100, 10, 0.001f);
		TopCenterOBJ.transform.localPosition = new Vector3(TopCenter.position.x, TopCenter.position.y, TopCenter.position.z + 0.5f);


		Transform BottomCenterOBJ = new GameObject().transform;
		BottomCenterOBJ.name = "BottomCenter";
		BottomCenterOBJ.gameObject.AddComponent<BoxCollider>();
		BottomCenterOBJ.transform.localScale = new Vector3(100, 10, 0.001f);
		BottomCenterOBJ.transform.localPosition = new Vector3(BottomCenter.position.x, BottomCenter.position.y, BottomCenter.position.z - 0.5f);


	}
	void HandleChildChanged(object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}

		DataSnapshot snapshot = args.Snapshot;
		string key = snapshot.Key.ToString();
		string value = snapshot.Value.ToString();
		GameObject ground=GameObject.Find(key);
        if (ground != null)
        {
			Color newCol;
			if (ColorUtility.TryParseHtmlString(value, out newCol))
			{
				ground.GetComponent<Renderer>().material.color = newCol;

			}
			

		}

	}
	public void StartNewGame()
	{
		isGamePlaying = true;
		GameObject randomObj = GameObject.Find(Random.Range(0, gridWidth - 1) + "_" + Random.Range(0, gridHeight - 1));
		newPlayer = Instantiate(player);
		newPlayer.localPosition = new Vector3(randomObj.transform.localPosition.x, randomObj.transform.localPosition.y + 1, randomObj.transform.localPosition.z);
		playerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		newPlayer.GetComponent<Renderer>().material.color = playerColor;
		string player_name = playerName.text;
		if (player_name == "")
		{
			player_name = "Jumper.io";
		}

		Player playerModel = new Player("", player_name, "#" + ColorUtility.ToHtmlStringRGB(playerColor), newPlayer.localPosition.x, newPlayer.localPosition.y, newPlayer.localPosition.z, -90, 0, 0, 1, 1, 1, 0);
		playersManager.CreateNewPlayer(playerModel, playerModel.player_id);
		newPlayerScript = playerModel;


	}
	public void RestGame()
	{
		isGamePlaying = false;
		playersManager.DetelePlayer(newPlayerScript.player_id);
		PlayerPrefs.DeleteKey("my_id");
		newPlayerScript.player_score= 0;
		Destroy(newPlayer.gameObject);
		
	}
	void OnDisable()
	{
		PlayerPrefs.DeleteKey("my_id");
		newPlayerScript.player_score = 0;
		playersManager.DetelePlayer(newPlayerScript.player_id);

	}
}
