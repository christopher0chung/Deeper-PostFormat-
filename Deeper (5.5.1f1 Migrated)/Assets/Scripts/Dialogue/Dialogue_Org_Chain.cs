using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Org_Chain : MonoBehaviour
{
    public List<Dialogue> LinesAndChoices;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            LinesAndChoices.Add(transform.GetChild(i).GetComponent<Dialogue>());

            Dialogue_Line l = LinesAndChoices[i] as Dialogue_Line;
            Dialogue_LineTerminal t = LinesAndChoices[i] as Dialogue_LineTerminal;

            if (l != null)
            {
                Debug.Assert(l.line.Substring(l.line.Length - 1, 1) == " ", "Last char in " + transform.GetChild(i).gameObject.name + " is not a space");
            }
            else if (t != null)
            {
                Debug.Assert(t.line.Substring(t.line.Length - 1, 1) == " ", "Last char in " + transform.GetChild(i).gameObject.name + " is not a space");
            }
        }
    }
}

public class Dialogue: MonoBehaviour {
    public float delayTime;
}