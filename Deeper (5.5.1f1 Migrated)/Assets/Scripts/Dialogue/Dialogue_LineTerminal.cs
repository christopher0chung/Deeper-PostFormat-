using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_LineTerminal : Dialogue {
    public CharactersEnum speaker;
    public string line;
    [Header ("Link to Dialogue_Org_Chain")]
    public Dialogue_Org_Chain ChainLink;
}
