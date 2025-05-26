using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurnMessageTable", menuName = "UI/Turn Message Table")]
public class TurnMessageTable : ScriptableObject
{
    [System.Serializable]
    public class MessageEntry
    {
        public TurnMessageKey key;
        [TextArea]
        public string message;
    }

    public List<MessageEntry> messages = new();

    private Dictionary<TurnMessageKey, string> messageDict;

    public void Initialize()
    {
        messageDict = new Dictionary<TurnMessageKey, string>();
        foreach (var entry in messages)
        {
            if (!messageDict.ContainsKey(entry.key))
                messageDict.Add(entry.key, entry.message);
        }
    }

    public string GetMessage(TurnMessageKey key)
    {
        if (messageDict == null) Initialize();
        return messageDict.TryGetValue(key, out var message) ? message : "";
    }
}
