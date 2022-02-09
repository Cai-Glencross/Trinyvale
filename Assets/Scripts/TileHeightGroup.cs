using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile Depth Group")]
public class TileHeightGroup : ScriptableObject
{
    public List<TileHeight> Tiles;

    [System.Serializable]
    public class TileHeight
    {
        public Sprite Sprite;
        public float Height;

        public TileHeight(Sprite sprite, float height)
        {
            Sprite = sprite;
            Height = height;
        }
    }
}