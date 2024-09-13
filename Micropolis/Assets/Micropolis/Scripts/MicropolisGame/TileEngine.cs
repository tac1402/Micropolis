﻿using System;
using System.Collections.Generic;
using MicropolisCore;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MicropolisGame
{
    /// <summary>
    /// Keeps a cached copy of rendered Tiles and provides them for drawing onto a TileMap
    /// </summary>
    public class TileEngine
    {
        /// <summary>
        /// Cached copy of all rendered Tiles
        /// </summary>
        private readonly Dictionary<int, Tile> _tiles = new Dictionary<int, Tile>();

        private string _tileset;

        public TileEngine()
        {
            PreLoadTiles();
            LoadTileset("classic");
        }

        /// <summary>
        /// Preload all tiles into a dictionary for fetching during draw mode
        /// </summary>
		private void PreLoadTiles()
		{
			//TilemapToPng toPng = new TilemapToPng();

			for (var i = 0; i < (int)MapTileCharacters.TILE_COUNT; i++)
			{
				var path = string.Format("Tiles/tiles_{0}", i);
				Tile asset = Resources.Load<Tile>(path);

				/*
                //toPng.ExportAsPng(toPng.TextureFromSprite(asset.sprite), string.Format("tiles_{0}", i));
                Material m = new Material(Shader.Find("Standard"));
                string p = "Assets/Resources/Exported Tilemaps/" + string.Format("tiles_{0}", i) + ".png";
                Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath(p, typeof(Texture2D));
				m.mainTexture =  t;
                toPng.ExportAsMaterial(m, string.Format("m_{0}", i));
                */

				_tiles.Add(i, asset);
			}
		}

		/// <summary>
		/// Fetches a <see cref="Tile"/> object from a cache or adds it 
		/// if it's being requested for the first time.
		/// </summary>
		/// <param name="tileId">Tile id to render from the MicroplisTileCharacter enum</param>
		/// <returns>A <see cref="Tile"/> object that can be renedered onto a <see cref="Tilemap"/></returns>
		public Tile GetTile(int tileId)
        {
            if (_tiles.ContainsKey(tileId))
            {
                return _tiles[tileId];
            }

            throw new ApplicationException("Tile ID: " + tileId + " not found in cache.");
        }

        /// <summary>
        /// Replace all the sprites in the tile array with the
        /// sprite resources from the tileset sprites.
        /// </summary>
        /// <param name="tileset"></param>
        public void LoadTileset(string tileset)
        {
            _tileset = tileset;
            var sprites = Resources.LoadAll<Sprite>("Tilesets/" + _tileset + "/tiles");
            foreach (var tile in _tiles)
            {
                tile.Value.sprite = sprites[tile.Key];
            }
        }
    }
}