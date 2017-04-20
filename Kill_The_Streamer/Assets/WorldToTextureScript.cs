using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToTextureScript : MonoBehaviour {

	public Vector4 corners;
	public float xWidth;
	public float yWidth;
	public const int TEXTURE_WIDTH = 1024;


	// Use this for initialization
	void Start () {
		Renderer rend = this.GetComponent<Renderer> ();
		corners = new Vector4 (
			rend.bounds.min.x,
			rend.bounds.max.x,
			rend.bounds.min.z,
			rend.bounds.max.z);
		xWidth = corners [1] - corners [0];
		yWidth = corners [3] - corners [2];
	}

	/// <summary>
	/// Takes a world space coordinate and puts it in texture space
	/// </summary>
	/// <param name="WorldCoords">World coords.</param>
	/// <param name="textureX">Resulting X Coordinate on the Texture</param>
	/// <param name="textureY">Resulting Y Coordinate on the Texture</param>
	public void WorldToTexture(Vector2 WorldCoords, out int textureX, out int textureY){
		float relX = WorldCoords.x - corners [0];
		float relY = WorldCoords.y - corners [2];

		float ratioX = relX / xWidth;
		float ratioY = relY / yWidth;

		textureX = (int)(ratioX * TEXTURE_WIDTH);
		textureY = (int)(ratioY * TEXTURE_WIDTH);
	}

	/// <summary>
	/// Takes a world space coordinate and puts it in texture space
	/// </summary>
	/// <param name="WorldCoords">World coords.</param>
	/// <param name="textureIndex">The index in a color array</param>
	public void WorldToTexture(Vector2 WorldCoords, out int textureIndex){
		float relX = WorldCoords.x - corners [0];
		float relY = WorldCoords.y - corners [2];

		float ratioX = relX / xWidth;
		float ratioY = relY / yWidth;

		textureIndex = (int)(ratioX * TEXTURE_WIDTH) + TEXTURE_WIDTH * (int)(ratioY * TEXTURE_WIDTH);
	}

	void Update(){
		//Debug, remove after implementation utilizing this.
		int texX = 0;
		int texY = 0;
		Vector2 playerPos = new Vector2 (Player.s_Player.FastTransform.Position.x, Player.s_Player.FastTransform.Position.z);
		WorldToTexture (playerPos, out texX, out texY);
		Debug.Log ("X: " + texX + " Y: " + texY);
	}
}
