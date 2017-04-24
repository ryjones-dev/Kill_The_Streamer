using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrawTrail : MonoBehaviour
{
	private TrailHandler trailHandler;

    private void Start()
    {
        trailHandler = GetComponent<TrailHandler>();

        // Debug
        int texX = 0;
        int texY = 0;
        Vector2 playerPos = new Vector2(Player.s_Player.FastTransform.Position.x, Player.s_Player.FastTransform.Position.z);
        trailHandler.WorldToTexture(playerPos, out texX, out texY);

        trailHandler.DrawTrail(texX, texY);
    }

	private void Update()
	{
		int texX = 0;
		int texY = 0;
		Vector2 playerPos = new Vector2(Player.s_Player.FastTransform.Position.x, Player.s_Player.FastTransform.Position.z);
		trailHandler.WorldToTexture(playerPos, out texX, out texY);

		trailHandler.DrawTrail(texX, texY);
    }
}
