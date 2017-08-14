using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Deeper_ControlRouter : MonoBehaviour {

    #region Internal Dictionaries

    private Dictionary<ControllersEnum, int> _controllerToInt = new Dictionary<ControllersEnum, int>();

    #endregion

    private void Awake()
    {
        Deeper_EventManager.instance.Register<Deeper_Event_ControlAssignment>(ControlAssignmentVotesHandler);
        Deeper_EventManager.instance.Register<Deeper_Event_ControlScheme>(ControlSchemeEventsHandler);        
    }

    private void Start()
    {
        players[0] = player0;
        players[1] = player1;

        _controllerToInt.Add(ControllersEnum.C0, 0);
        _controllerToInt.Add(ControllersEnum.C1, 1);
    }

    #region Control Assignments

    private ControlOrientation _aO;
    private ControlOrientation _activeOrientation
    {
        get
        {
            return _aO;
        }
        set
        {
            //Debug.Log("_aO set");
            _aO = value;
            if (_aO == ControlOrientation.c0Ops_c1Doc)
            {
                Deeper_ServicesLocator.instance.controllerToCharacter.Clear();
                Deeper_ServicesLocator.instance.controllerToCharacter.Add(ControllersEnum.C0, CharactersEnum.Ops);
                Deeper_ServicesLocator.instance.controllerToCharacter.Add(ControllersEnum.C1, CharactersEnum.Doc);

                Deeper_ServicesLocator.instance.characterToController.Clear();
                Deeper_ServicesLocator.instance.characterToController.Add(CharactersEnum.Ops, ControllersEnum.C0);
                Deeper_ServicesLocator.instance.characterToController.Add(CharactersEnum.Doc, ControllersEnum.C1);
            }

            if (_aO == ControlOrientation.c0Doc_c1Ops)
            {
                Deeper_ServicesLocator.instance.controllerToCharacter.Clear();
                Deeper_ServicesLocator.instance.controllerToCharacter.Add(ControllersEnum.C0, CharactersEnum.Doc);
                Deeper_ServicesLocator.instance.controllerToCharacter.Add(ControllersEnum.C1, CharactersEnum.Ops);

                Deeper_ServicesLocator.instance.characterToController.Clear();
                Deeper_ServicesLocator.instance.characterToController.Add(CharactersEnum.Ops, ControllersEnum.C1);
                Deeper_ServicesLocator.instance.characterToController.Add(CharactersEnum.Doc, ControllersEnum.C0);
            }
        }
    }

    private void ControlAssignmentVotesHandler (Deeper_Event e)
    {
        Deeper_Event_ControlAssignment c = (Deeper_Event_ControlAssignment)e;

        VoteAssignment(c.controller, c.character);
    }

    private CharactersEnum[] votes = new CharactersEnum[2];

    private void VoteAssignment (ControllersEnum voteCo, CharactersEnum voteCh)
    {
        if (voteCo == ControllersEnum.C0)
            votes[0] = voteCh;
        else
            votes[1] = voteCh;

        if (votes[0] == CharactersEnum.Ops && votes[1] == CharactersEnum.Doc)
            _activeOrientation = ControlOrientation.c0Ops_c1Doc;
        if (votes[0] == CharactersEnum.Doc && votes[1] == CharactersEnum.Ops)
            _activeOrientation = ControlOrientation.c0Doc_c1Ops;
    }

    #endregion

    private Rewired.Player[] players = new Rewired.Player[2];

    private Rewired.Player player0 { get { return ReInput.isReady ? ReInput.players.GetPlayer(0) : null; } }
    private Rewired.Player player1 { get { return ReInput.isReady ? ReInput.players.GetPlayer(1) : null; } }

    private void ControlSchemeEventsHandler (Deeper_Event e)
    {
        Deeper_Event_ControlScheme cS = (Deeper_Event_ControlScheme)e;
        SetControls(cS.cs);
    }

    private void SetControls (ControlStates cS)
    {
        // If the game interrupts controls, then all maps will be disabled
        if (cS == ControlStates.Interrupted)
        {
            //Debug.Log("Is interrupted");
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[i].controllers.maps.SetMapsEnabled(false, j);
                }
            }
        }
        // Menu applies to both characters
        else if (cS == ControlStates.Menu)
        {
            //Debug.Log("Is menu");
            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[i].controllers.maps.SetMapsEnabled(false, j);
                }
                players[i].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Default));
                players[i].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Menu));
            }
        }

        else
        {
            //Debug.Log(cS);

            #region Ops

            if (cS == ControlStates.Ops_Inside)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(false, j);
                }
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Default));
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Sub));
            }

            if (cS == ControlStates.Ops_Outside)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(false, j);
                }
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Default));
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Character));
            }

            if (cS == ControlStates.Ops_OOC)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(false, j);
                }
            }

            if (cS == ControlStates.Ops_IP)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(false, j);
                }
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Ops)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Default));
            }

            #endregion

            #region Doc

            if (cS == ControlStates.Doc_Inside)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(false, j);
                }
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Default));
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Sub));
            }

            if (cS == ControlStates.Doc_Outside)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(false, j);
                }
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Default));
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Character));
            }

            if (cS == ControlStates.Doc_OOC)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(false, j);
                }
            }

            if (cS == ControlStates.Doc_IP)
            {
                for (int j = 0; j <= 12; j++)
                {
                    players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(false, j);
                }
                players[Deeper_ServicesLocator.instance.GetInt(CharactersEnum.Doc)].controllers.maps.SetMapsEnabled(true, Deeper_RewiredAid.instance.GetMap(ControlMaps.Default));
            }

            #endregion
        }
    }
}
public enum ControlOrientation { c0Doc_c1Ops, c0Ops_c1Doc }
public enum ControlStates { Interrupted, Ops_Outside, Ops_Inside, Ops_OOC, Ops_IP, Doc_Outside, Doc_Inside, Doc_OOC, Doc_IP, Menu}
