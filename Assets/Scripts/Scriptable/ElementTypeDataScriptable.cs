using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementTypeData", menuName = "SandSoft/ElementTypeData", order = 0)]
public class ElementTypeDataScriptable : ScriptableObject
{
    [SerializeField] private List<ElementTypeEntry> availableTextures;


    public Texture2D GetTextureByID(int id)
    {
        return availableTextures.FirstOrDefault(x => x.id == id).texture;
    }
}