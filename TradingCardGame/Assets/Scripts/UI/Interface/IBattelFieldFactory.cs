using System;
using UnityEngine;

public interface IBattelFieldFactory
{
    IBattelDataPanel GetBattelDataPanel(IBattel battel);
    IPersonsPanel GetPersonsPanel(Transform parent, IBattelPerson player, IBattelPerson enemy);
    IStartingHandPanel GetStartingHandPanel(Transform parent, IBattelPerson person, Action accept);
    IBattleFieldCards GetBattleFieldCards();
    IFinishBattel GetFinishTrainingBattel(IBattel battel, TypePersonEnum loser, Action continueAct);
}
