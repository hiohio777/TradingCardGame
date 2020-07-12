using System;
using UnityEngine;

public class CommonBattelScene : BaseBattelScene
{

    public static CommonBattelScene CreatPrefab(Action<ScenesEnum> startNewScene, IBattel battelData, IUser player,
              BattelFieldFactory battelFieldFactory) =>
     (Instantiate(Resources.Load<CommonBattelScene>($"BattleScene/CommonBattelScene")).Initialize(startNewScene, battelData, player,
     battelFieldFactory) as CommonBattelScene)
     .Build();

    private CommonBattelScene Build()
    {
        return this;
    }

    private void StartBattel()
    {

    }

    public override void FinishBattel()
    {
        throw new NotImplementedException();
    }
}
