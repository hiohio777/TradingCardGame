using System;
using UnityEngine;

public class CommonBattelScene : BaseBattelScene
{

    public static CommonBattelScene CreatPrefab(IBattel battelData, IUserData player,
              BattelFieldFactory battelFieldFactory) =>
     (Instantiate(Resources.Load<CommonBattelScene>($"BattleScene/CommonBattelScene")).Initialize(battelData, player,
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
