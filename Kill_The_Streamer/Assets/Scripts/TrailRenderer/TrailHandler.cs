using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour
{
    private Vector4 corners;
    private float xWidth;
    private float yWidth;
    private const int TEXTURE_WIDTH = 1024;

    private Texture2D m_mapTexture;
    private Color32[] m_mapTextureArray;

    private Renderer textureRenderer;

    private bool[] circle;

    private int alphaFadeIndex = 0;

    private void Awake()
    {
        m_mapTexture = new Texture2D(TEXTURE_WIDTH, TEXTURE_WIDTH);
        m_mapTextureArray = new Color32[TEXTURE_WIDTH * TEXTURE_WIDTH];

        circle = new bool[49]
        {
            false, false, true, true, true, false, false,
            false, true, true, true, true, true, false,
            true, true, true, true, true, true, true,
            true, true, true, true, true, true, true,
            true, true, true, true, true, true, true,
            false, true, true, true, true, true, false,
            false, false, true, true, true, false, false
        };
    }

    private void Start()
    {
        textureRenderer = GetComponent<Renderer>();
        textureRenderer.material.mainTexture = m_mapTexture;

        corners = new Vector4(
            textureRenderer.bounds.min.x,
            textureRenderer.bounds.max.x,
            textureRenderer.bounds.min.z,
            textureRenderer.bounds.max.z);
        xWidth = corners[1] - corners[0];
        yWidth = corners[3] - corners[2];
    }

    public void DrawTrail(int screenX, int screenY)
    {
        int location = 1024 * screenY + screenX;
        for (int y = 0; y < 7; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                if (circle[y * 7 + x] && location + x + y * TEXTURE_WIDTH >= 0 && location + x + y * TEXTURE_WIDTH < m_mapTextureArray.Length)
                {
                    m_mapTextureArray[location + x + y * TEXTURE_WIDTH] = new Color32(255, 255, 255, 255);
                }
            }
        }
    }

    /// <summary>
	/// Takes a world space coordinate and puts it in texture space
	/// </summary>
	/// <param name="WorldCoords">World coords.</param>
	/// <param name="textureX">Resulting X Coordinate on the Texture</param>
	/// <param name="textureY">Resulting Y Coordinate on the Texture</param>
	public void WorldToTexture(Vector2 WorldCoords, out int textureX, out int textureY)
    {
        float relX = WorldCoords.x - corners[0];
        float relY = WorldCoords.y - corners[2];

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
	public void WorldToTexture(Vector2 WorldCoords, out int textureIndex)
    {
        float relX = WorldCoords.x - corners[0];
        float relY = WorldCoords.y - corners[2];

        float ratioX = relX / xWidth;
        float ratioY = relY / yWidth;

        textureIndex = (int)(ratioX * TEXTURE_WIDTH) + TEXTURE_WIDTH * (int)(ratioY * TEXTURE_WIDTH);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        int alphaFadeStart = alphaFadeIndex;
        for (int i = alphaFadeIndex; i < m_mapTextureArray.Length / 2 + alphaFadeStart; i++)
        {
            if (m_mapTextureArray[i].a > 0)
            {
                m_mapTextureArray[i].a -= 5;
            }

            alphaFadeIndex++;
        }

        alphaFadeIndex %= m_mapTextureArray.Length;
    }

    private void LateUpdate()
    {
        m_mapTexture.SetPixels32(m_mapTextureArray);
        m_mapTexture.Apply();
    }
}
