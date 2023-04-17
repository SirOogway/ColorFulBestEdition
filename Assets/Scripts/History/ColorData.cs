using System.Collections.Generic;

[System.Serializable]
public class ColorData
{
    Stack<string> hexModels;

    public ColorData() => hexModels = new Stack<string>();
    
    public void SetHexModel(string hexModel) => hexModels.Push(hexModel);

    public Stack<string> GetHexModels() => hexModels;
}
