using UnityEngine;


[System.Serializable]
public class Card 
{
    public string name;
    public string type;
    public int value;
    public Sprite sprite;

    public Card(string name,string type, int value, Sprite sprite)
    {
        this.name = name;
        this.type = type;
        this.value = value;
        this.sprite = sprite;
    }
}
