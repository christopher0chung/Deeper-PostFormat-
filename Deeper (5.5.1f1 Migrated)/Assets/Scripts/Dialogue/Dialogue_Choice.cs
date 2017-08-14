using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Choice : Dialogue
{
    public CharactersEnum choice;
    public string choice1;
    public string choice2;

    [Header("Links to Dialogue_Org_Chain")]
    public Dialogue_Org_Chain ChainLink1;
    public Dialogue_Org_Chain ChainLink2;
}
