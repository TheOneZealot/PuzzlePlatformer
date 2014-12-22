using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;

public class TileMap : MonoBehaviour 
{
    public string fileName;
    public GameObject player;
    public GameObject tile;
    public GameObject[,] tiles;
    public int width = 10, height = 10;
    public Dictionary<string, int[,]> layers;

    public void DeleteTiles()
    {
        int tempChildCount = transform.childCount;
        for (int i = tempChildCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public void CreateTiles()
    {
        ParseMap();

        foreach (var pair in layers)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (pair.Value[x, y] > 0)
                    {
                        GameObject newTile = Instantiate(tile, new Vector3(x, y), Quaternion.identity) as GameObject;
                        newTile.transform.parent = transform;
                    }
                }
            }
        }
    }

    public bool IsTileSolid(int x, int y)
    {
        foreach (var pair in layers)
        {
            if (pair.Value[x, y] > 0)
            {
                return true;
            }
        }
        return false;
    }

    /**************
	* Map Parsing *
	**************/
    void ParseMap()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(ReadFile(fileName));

        XmlNode map = doc.DocumentElement;
        width = Convert.ToInt32(map.Attributes.GetNamedItem("width").Value);
        height = Convert.ToInt32(map.Attributes.GetNamedItem("height").Value);

        XmlNodeList layer = doc.GetElementsByTagName("layer");
        layers = new Dictionary<string, int[,]>();
        foreach (XmlNode l in layer)
        {
            string key = l.Attributes.GetNamedItem("name").Value;
            int[,] value = new int[width, height];

            int x = 0, y = height - 1;
            foreach (XmlNode t in l.SelectNodes("data/tile"))
            {
                if (x >= width)
                {
                    x = 0;
                    y--;
                }
                value[x, y] = Convert.ToInt32(t.Attributes.GetNamedItem("gid").Value);
                x++;
            }

            layers.Add(key, value);
        }
    }

    private string ReadFile(string fn)
    {
        try
        {
            string text = File.ReadAllText(String.Format("Assets/Resources/Maps/{0}.tmx", fn));
            return text;
        }
        catch (Exception e)
        {
            print(e.Message);
            return null;
        }
    }
}