using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Org_Conversation : Deeper_Component
{
    public Dialogue_Org_Chain FirstChain;

    private void Start()
    {
        Initialize(1000);

        FirstChain= transform.GetChild(0).GetComponent<Dialogue_Org_Chain>();

    }

    public void Fire()
    {
        GameObject.Find("Managers_Game").GetComponent<Game_DialogueManager>().RegisterDialogue(this);
    }
}
