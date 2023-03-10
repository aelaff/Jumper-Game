using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{
    // Player model class

	public string player_id;
    public string player_name;
    public string player_color;
    public double player_position_x;
    public double player_position_y;
    public double player_position_z;
    public double player_rotation_x;
    public double player_rotation_y;
    public double player_rotation_z;
    public double player_scale_x;
    public double player_scale_y;
    public double player_scale_z;
    public int player_score;

    public Player(string player_id, string player_name, string player_color, double player_position_x, double player_position_y, double player_position_z, double player_rotation_x, double player_rotation_y, double player_rotation_z, double player_scale_x, double player_scale_y, double player_scale_z, int player_score)
    {
        this.player_id = player_id;
        this.player_name = player_name;
        this.player_color = player_color;
        this.player_position_x = player_position_x;
        this.player_position_y = player_position_y;
        this.player_position_z = player_position_z;
        this.player_rotation_x = player_rotation_x;
        this.player_rotation_y = player_rotation_y;
        this.player_rotation_z = player_rotation_z;
        this.player_scale_x = player_scale_x;
        this.player_scale_y = player_scale_y;
        this.player_scale_z = player_scale_z;
        this.player_score = player_score;
    }
    public Player FromOtherPlayer(Player otherPlayer)
    {
        this.player_id = otherPlayer.player_id;
        this.player_name = otherPlayer.player_name;
        this.player_color = otherPlayer.player_color;
        this.player_position_x = otherPlayer.player_position_x;
        this.player_position_y = otherPlayer.player_position_y;
        this.player_position_z = otherPlayer.player_position_z;
        this.player_rotation_x = otherPlayer.player_rotation_x;
        this.player_rotation_y = otherPlayer.player_rotation_y;
        this.player_rotation_z = otherPlayer.player_rotation_z;
        this.player_scale_x = otherPlayer.player_scale_x;
        this.player_scale_y = otherPlayer.player_scale_y;
        this.player_scale_z = otherPlayer.player_scale_z;
        this.player_score = otherPlayer.player_score;
        return this;
    }


    public Player(IDictionary<string ,object> dict) {
        this.player_id = dict["player_id"].ToString();
        this.player_name = dict["player_name"].ToString();
        this.player_color = dict["player_color"].ToString();
        this.player_position_x = Convert.ToDouble(dict["player_position_x"]);
        this.player_position_y = Convert.ToDouble(dict["player_position_y"]);
        this.player_position_z = Convert.ToDouble(dict["player_position_z"]);
        this.player_rotation_x = Convert.ToDouble(dict["player_rotation_x"]);
        this.player_rotation_y = Convert.ToDouble(dict["player_rotation_y"]);
        this.player_rotation_z = Convert.ToDouble(dict["player_rotation_z"]);
        this.player_scale_x = Convert.ToDouble(dict["player_scale_x"]);
        this.player_scale_y = Convert.ToDouble(dict["player_scale_y"]);
        this.player_scale_z = Convert.ToDouble(dict["player_scale_z"]);
        this.player_score = Convert.ToInt32(dict["player_score"]);
    }
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["player_id"] = player_id;
        result["player_name"] = player_name;
        result["player_color"] = player_color;
        result["player_position_x"] = player_position_x;
        result["player_position_y"] = player_position_y;
        result["player_position_z"] = player_position_z;
        result["player_rotation_x"] = player_rotation_x;
        result["player_rotation_y"] = player_rotation_y;
        result["player_rotation_z"] = player_rotation_z;
        result["player_scale_x"] = player_scale_x;
        result["player_scale_y"] = player_scale_y;
        result["player_scale_z"] = player_scale_z;
        result["player_score"] = player_score;

        return result;
    }
}
