using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConstantsRef
{
	public static class Tags
	{
		public static readonly string Untagged = "Untagged";
		public static readonly string Respawn = "Respawn";
		public static readonly string Finish = "Finish";
		public static readonly string EditorOnly = "EditorOnly";
		public static readonly string MainCamera = "MainCamera";
		public static readonly string Player = "Player";
		public static readonly string GameController = "GameController";
	}

	public static class Layers
	{
		public static readonly int Default = 0;
		public static readonly int TransparentFX = 1;
		public static readonly int IgnoreRaycast = 2;
		public static readonly int Water = 4;
		public static readonly int UI = 5;
		public static readonly int PostProcessing = 8;

		public static class ByName
		{
			public static readonly string Default = "Default";
			public static readonly string TransparentFX = "TransparentFX";
			public static readonly string IgnoreRaycast = "Ignore Raycast";
			public static readonly string Water = "Water";
			public static readonly string UI = "UI";
			public static readonly string PostProcessing = "PostProcessing";
		}

		public static bool IsValueOnMask(int layer, LayerMask mask)
		{
			return (mask == (mask | (1 << layer)));
		}
	}

	public static class SortingLayers
	{
		public static readonly SortingLayer Default = SortingLayer.layers[0];
		public static readonly SortingLayer Background = SortingLayer.layers[1];
		public static readonly SortingLayer Clouds = SortingLayer.layers[2];
		public static readonly SortingLayer Tiles = SortingLayer.layers[3];
		public static readonly SortingLayer Characters = SortingLayer.layers[4];
		public static readonly SortingLayer Foreground = SortingLayer.layers[5];

		public static class ByValue
		{
			public static readonly int Default = 0;
			public static readonly int Background = 1;
			public static readonly int Clouds = 2;
			public static readonly int Tiles = 3;
			public static readonly int Characters = 4;
			public static readonly int Foreground = 5;
		}

		public static class ByName
		{
			public static readonly string Default = "Default";
			public static readonly string Background = "Background";
			public static readonly string Clouds = "Clouds";
			public static readonly string Tiles = "Tiles";
			public static readonly string Characters = "Characters";
			public static readonly string Foreground = "Foreground";
		}
	}

	public static class Scenes
	{
		public static class SampleScene
		{
			public static readonly int index = 0;
			public static readonly string name = "SampleScene";
			public static readonly bool isLoaded = true;
			public static readonly int rootCount = 4;
			public static readonly GameObject[] root = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();
		}
	}

}