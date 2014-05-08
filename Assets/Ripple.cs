using UnityEngine;
using System.Collections;

public class Ripple : MonoBehaviour {

	// Use this for initialization
	Texture2D texture;
	public int res;
	public int drawres;

	private float scale;
	private int delaycounter;

	private float[,] values1;
	private float[,] values2;
	private bool[,] wall;

	public float dampening;
	public float dropheight;
	public float drawlimit;
	public int delay;

	private Color c;

	void Start () {
		// Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
		texture = new Texture2D(res, res, TextureFormat.ARGB32, false);
		texture.filterMode = FilterMode.Point;
		values1 = new float[res,res];
		values2 = new float[res,res];
		wall = new bool[res, res];

		c = Color.white;
		c.a = 1;

		scale = drawres / res;
	
	}
	
	// Update is called once per frame
	void Update () {

			int x = (int)(Input.mousePosition.x / scale);
			int y = (int)(res - (Screen.height - Input.mousePosition.y) / scale);

			if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.S)) {
				setWall(x,y);
				setWall(x + 1, y);
				setWall(x + 1, y + 1);
				setWall(x, y + 1);
			}
			else if (Input.GetMouseButton(0)) {
				ripple(x,y);
				ripple(x + 1, y);
				ripple(x - 1, y);
				ripple(x, y + 1);
				ripple(x, y - 1);

			}

			updateripples ();
			delaycounter = 0;
	}

	void setWall(int x, int y)
	{
		if (insideGrid (x, y))
			wall[x, y] = true;
	}

	void ripple(int x, int y)
	{
		if(insideGrid(x,y))
		   values1[x,y] = dropheight;
	}

	bool insideGrid(int x, int y)
	{
		return (x > 0 && x < res - 1) && (y > 0 && y < res - 1);
	}

	void updateripples(){
		float value;

		for (int x = 1; x < res - 1; x++) {
			for (int y = 1; y < res -1; y++) {
				if(!wall[x,y]){
					value = (  values1[ x + 1, y]
					         + values1[ x - 1, y]
					         + values1[ x, y + 1]
					         + values1[ x, y - 1]) / 4 - values2[x,y];

					value = value * dampening;

					values2[x,y] = value;

					c = Color.black;
					c.a = 0;

					if(value > drawlimit)
					{
						c = Color.white;
						c.a = value - drawlimit;
					}
				}
				else
					c = Color.black;

				texture.SetPixel(x, y, c);
			}
		}

		texture.Apply ();

		float[,] temp = values1;
		values1 = values2;
		values2 = temp;

	}

	void OnGUI(){
		GUI.DrawTexture (new Rect (0, 0, drawres, drawres), texture);
	}
}
