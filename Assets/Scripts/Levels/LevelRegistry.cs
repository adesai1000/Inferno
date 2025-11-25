using System;
using UnityEngine;

[Serializable]
public struct LevelInfo
{
    public string scene;   // EXACT scene name (Build Settings)
    public string title;   // display name e.g., "Level 2 — Lust"
}

[CreateAssetMenu(fileName = "LevelRegistry", menuName = "Game/Level Registry")]
public class LevelRegistry : ScriptableObject
{
    public LevelInfo[] levels = new LevelInfo[6]; // set in Inspector
}
