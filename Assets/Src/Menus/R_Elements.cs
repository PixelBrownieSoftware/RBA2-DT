using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Registers/Elements")]
public class R_Elements : R_Default
{
    public List<S_Element> elementsList = new List<S_Element>();
    public S_Element GetElement(string elementName) {
        return elementsList.Find(x => x.name == elementName);
    }
}
