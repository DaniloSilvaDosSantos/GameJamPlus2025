using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogEntry {
    public string text;
}

[CreateAssetMenu(menuName = "Dialog/DialogData")]
public class DialogData : ScriptableObject
{
    public List<DialogEntry> entries;
}
