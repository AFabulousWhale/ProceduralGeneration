using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGeneration : MonoBehaviour
{
    Renderer thisRenderer;

    public int textureWidth, textureHeight, intSeed;
    int tileSize = 5;
    public string inputSeed;
    public float perlinScale, xOffset, yOffset, seedValX, seedValY, speed;
    bool GeneratedWorld = false;

    Texture2D perlinTexture;
    void Start()
    {
        thisRenderer = GetComponent<Renderer>(); //gets the renderer of the texture at the start
    }

    private void Update()
    {
        if (GeneratedWorld)
        {
            if (Input.GetAxis("Horizontal") > 0) //moving right
            {
                xOffset += speed;
                thisRenderer.material.mainTexture = GenerateTexture();
            }
            if (Input.GetAxis("Horizontal") < 0) //moving left
            {
                xOffset -= speed;
                thisRenderer.material.mainTexture = GenerateTexture();
            }

            if (Input.GetAxis("Vertical") > 0) //moving up
            {
                yOffset += speed;
                thisRenderer.material.mainTexture = GenerateTexture();
            }
            if (Input.GetAxis("Vertical") < 0) //moving down
            {
                yOffset -= speed;
                thisRenderer.material.mainTexture = GenerateTexture();
            }
        }
    }

    public void ChangeSeed(string seed)
    {
        inputSeed = seed;
    }

    public void DisableMovement(bool canMove)
    {
        GeneratedWorld = canMove;
    }

    Texture2D GenerateTexture()
    {
        perlinTexture = new Texture2D(textureWidth, textureHeight); //generates a new noise textures of the designated width and height

        for (int x = 0; x < textureWidth; x++) //creates a for loop through each pixel space within the texture grid
        {
            for (int y = 0; y < textureHeight; y++)
            {
                perlinTexture.SetPixel(x, y, GenerateColours(x, y)); //generates a new color on the gray scale for that pixel based on the perlin output
            }
        }
        perlinTexture.filterMode = FilterMode.Point;
        perlinTexture.Apply();
        return perlinTexture;
    } 

    Color GenerateColours(int x, int y)
    {
        float xCoord = xOffset + (((((float)x + ((seedValX * perlinScale) * textureWidth)) / textureWidth) * perlinScale)); //gets the current coordinate of the grid and divides it by the size of the grid to get a float value between 0 and 1 for the perlin noise scale
        float yCoord = yOffset + (((((float)y + ((seedValY * perlinScale) * textureHeight)) / textureHeight) * perlinScale)); //adds the random.value based on the seed the player inputs
        float perlinOutput = Mathf.PerlinNoise(xCoord, yCoord);

        if(perlinOutput < 0.3)
        {
            return new Color32(32,129,195,255); //will be water
        }
        else if (perlinOutput < 0.4)
        {
            return new Color32(229, 232, 182, 255); //will be sand
        }
        else if(perlinOutput < 0.8)
        {
            return new Color32(49,233,129,255); //will be green (land)
        }
        else
        {
            return new Color32(63,97,45,255); //will be dark green for forest
        }
    }

    public void ResetCoords()
    {
        xOffset = 0;
        yOffset = 0;
    }

    public void RandomisePerlin()
    {
        intSeed = inputSeed.GetHashCode(); //sets whatever the user types to a hashcode which will be used as the seed
        if(inputSeed == "")
        {
            intSeed = Random.Range(1, 999999999);
        }
        Random.InitState(intSeed);
        seedValX = Random.value;
        seedValY = Random.value;
        GeneratedWorld = true;
        thisRenderer.material.mainTexture = GenerateTexture();
    }
}
