using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour
{
	private Texture2D m_mapTexture;
	private Color[] m_mapTextureArray;

	private Renderer textureRenderer;

	private void Awake()
	{
		m_mapTexture = new Texture2D (1024, 1024); // For now
		m_mapTextureArray = new Color[1024 * 1024];
	}

	private void Start()
	{
		// For debugging
		textureRenderer = GetComponent<Renderer>();

		Color[] trailTexture = new Color[1024 * 1024];

		for (int i = 0; i < 1024 * 1024; i++)
		{
			trailTexture [i] = new Color (1.0f, 0.0f, 0.0f, 1.0f);
		}

		DrawTrail (trailTexture, 0, 0);
	}

	public void DrawTrail(Color[] trailColors, int screenX, int screenY)
	{
		int xIndex = screenX % 1024;
		int yIndex = screenY / 1024;

		int index = xIndex + yIndex * 1024;
		bool[] circle = new bool[49] { 
			false, false, true, true, true, false, false,
			false, true, true, true, true, true, false,
			true, true, true, true, true, true, true,
			true, true, true, true, true, true, true,
			true, true, true, true, true, true, true,
			false, true, true, true, true, true, false,
			false, false, true, true, true, false, false
		};

		// Do bounds checking for location + i + j * 1024
		// Set the location to be the player's screen coordinates

		int location = 1024 * 10 + 10;
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {
				if (circle [i * 7 + j]) {
					m_mapTextureArray [location + i + j * 1024] = new Color (1, 1, 1);
				}
			}
		}


//		for(int y = yIndex; y < trailTexture.height; y++)
//		{
//			for (int x = xIndex; x < trailTexture.width; x++)
//			{
//				
//			}
//		}
	}

	private void LateUpdate()
	{
		m_mapTexture.SetPixels (m_mapTextureArray);
		m_mapTexture.Apply ();

		textureRenderer.material.mainTexture = m_mapTexture;
	}
}
