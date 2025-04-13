using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private List<Tilemap> tilemapsTorecolor;
    
    private void Awake()
    {
        if (!LoadManager.Instance.IsLoaded)
            return;
        
        Color color = LoadManager.Instance.Data.Settings.Color;
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        foreach (var tilemap in tilemapsTorecolor)
        {
            tilemap.color = color;
        }
    }
}