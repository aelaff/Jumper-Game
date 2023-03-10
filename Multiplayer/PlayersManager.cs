using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PlayersManager : MonoBehaviour
{
	// This script for Managing players objects and leaderboard items
	public List<Player> allPlayers = new List<Player>();
	public Transform leaderboardElement;
	public Transform leaderboardPanel;
	public Transform otherPlayer;

	
	void Awake()
	{
		

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://jumper-4cf67.firebaseio.com/players");
		Router.Players().ChildAdded += HandleChildAdded;
		Router.Players().ChildChanged += HandleChildChanged;
		Router.Players().ChildRemoved += HandleChildRemoved;

	}
	

	public void CreateNewPlayer(Player player, string uid)
	{
		string playerJSON = JsonUtility.ToJson(player);
		string key = Router.PlayerWithUID(uid).Push().Key;
		player.player_id = key;
		playerJSON = JsonUtility.ToJson(player);
		Router.PlayerWithUID(key).SetRawJsonValueAsync(playerJSON);
		PlayerPrefs.SetString("my_id",key);
		leaderboardPanel.parent.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
	}
	
	
	void HandleChildAdded(object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
        // Do something with the data in args.Snapshot
        DataSnapshot snapshot = args.Snapshot;
        var playerDict = (IDictionary<string, object>)snapshot.Value;
        Player newPlayer = new Player(playerDict);
		AddToLeaderboard(newPlayer);
		if (newPlayer.player_id != PlayerPrefs.GetString("my_id", "")) { 
			AddNewPlayer(newPlayer);
		}

	}
    void HandleChildChanged(object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		DataSnapshot snapshot = args.Snapshot;
		var playerDict = (IDictionary<string, object>)snapshot.Value;
		Player returnedPlayer = new Player(playerDict);
		ChangedInLeaderboard(returnedPlayer.player_id, returnedPlayer.player_score);
		ChangePlayer(returnedPlayer);
	}

	void HandleChildRemoved(object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		// Do something with the data in args.Snapshot
		DataSnapshot snapshot = args.Snapshot;
		var playerDict = (IDictionary<string, object>)snapshot.Value;
		Player returnedPlayer = new Player(playerDict);
		RemoveFromLeaderboard(returnedPlayer.player_id);
		RemovePlayer(returnedPlayer);
	}

	
	void AddToLeaderboard(Player player) {
		RectTransform playerLeaderboard = Instantiate(leaderboardElement) as RectTransform;

		Color newCol;
		if (ColorUtility.TryParseHtmlString(player.player_color, out newCol))
		{
			playerLeaderboard.GetComponent<UnityEngine.UI.Image>().color =newCol;

		}
		
		playerLeaderboard.name= player.player_id;
		playerLeaderboard.GetChild(0).GetComponent<Text>().text = player.player_name;
		playerLeaderboard.GetChild(1).GetComponent<Text>().text = player.player_score + "";
		playerLeaderboard.SetParent(leaderboardPanel);
		

	}
	void RemoveFromLeaderboard(string player_id)
	{
		RectTransform  child = leaderboardPanel.Find(player_id) as RectTransform;
		if(child!=null)
			Destroy(child.gameObject);

	}
	void ChangedInLeaderboard(string player_id,int player_score)
	{
		RectTransform child = leaderboardPanel.Find(player_id)as RectTransform;
		if (child != null)
			child.GetChild(1).GetComponent<Text>().text = player_score + "";

	}
	void AddNewPlayer(Player p) {
		Transform player = Instantiate(otherPlayer);
		player.localPosition = new Vector3((float)p.player_position_x, (float)p.player_position_y, (float)p.player_position_z);
		player.localRotation = Quaternion.Euler( new Vector3((float)p.player_rotation_z, (float)p.player_rotation_y, (float)p.player_rotation_z));
		player.localScale = new Vector3((float)p.player_scale_x, (float)p.player_scale_y, (float)p.player_scale_z);
		player.transform.SetParent(transform);
		player.name = p.player_id;
		player.transform.GetChild(0).GetComponent<TextMesh>().text = p.player_name;
		Color newCol;
		if (ColorUtility.TryParseHtmlString(p.player_color, out newCol))
		{
			player.transform.GetChild(1).GetComponent<Renderer>().material.color = newCol;

		}
	}
	void ChangePlayer(Player p)
	{
        Transform child = transform.Find(p.player_id);
        if (child != null)
        {
            child.localPosition = new Vector3((float)p.player_position_x, (float)p.player_position_y, (float)p.player_position_z);
            child.localRotation = Quaternion.Euler(new Vector3((float)p.player_rotation_z, (float)p.player_rotation_y, (float)p.player_rotation_z));
            child.localScale = new Vector3((float)p.player_scale_x, (float)p.player_scale_y, (float)p.player_scale_z);
        }
    }
	void RemovePlayer(Player p)
	{
		Transform child = transform.Find(p.player_id);
		if (child != null)
		{
			GameManager.instance.newPlayerScript.player_score+=p.player_score;

			Destroy(child.gameObject);
		}

	}

    public void DetelePlayer(string uid)
    {
		Router.PlayerWithUID(uid).RemoveValueAsync();
    }
}
