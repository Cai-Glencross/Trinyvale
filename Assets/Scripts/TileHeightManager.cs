using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHeightManager : MonoBehaviour
{
    public static TileHeightManager Instance { get; set; }

    public List<TileHeightGroup> TileHeightGroups;
    private readonly Dictionary<Sprite, float> _tileHeights = new Dictionary<Sprite, float>();
    private readonly HashSet<Vector3Int> _returnBack = new HashSet<Vector3Int>();

    public Tilemap Tilemap;
    public Tilemap Overlay;

    private void Awake()
    {
        Instance = this;

        if (TileHeightGroups != null)
        {
            foreach (var group in TileHeightGroups)
            {
                foreach (var tile in group.Tiles)
                {
                    _tileHeights.Add(tile.Sprite, tile.Height);
                }
            }
        }
    }

    public void ReportPosition(Vector3 position, Bounds bounds)
    {
        // clear previous overlay tiles
        foreach (var pos in _returnBack)
        {
            Overlay.SetTile(pos, null);
        }
        _returnBack.Clear();

        // project bounds to world position
        bounds.center += position;
        var playerDepth = bounds.min.y;

        // for all affected tiles
        var min = Vector2Int.FloorToInt(bounds.min);
        var max = Vector2Int.FloorToInt(bounds.max);
        for (var y = min.y; y <= max.y; y++)
        {
            var playerLocalHeight = y - playerDepth;
            for (var x = min.x; x <= max.x; x++)
            {
                // get the sprite
                var sprite = Tilemap.GetSprite(new Vector3Int(x, y, 0));
                if (sprite == null)
                    continue;

                // compare heights
                var tileHeight = _tileHeights.ContainsKey(sprite) ? _tileHeights[sprite] : 0f;
                if (tileHeight > playerLocalHeight)
                {
                    var pos = new Vector3Int(x, y, 0);
                    _returnBack.Add(pos);
                    Overlay.SetTile(pos, Tilemap.GetTile(pos));
                }

            }
        }
    }
}