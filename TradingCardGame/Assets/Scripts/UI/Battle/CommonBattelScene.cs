using System;
using UnityEngine;

public class CommonBattelScene : BaseBattel
{

    public static CommonBattelScene CreatPrefab(IBattel battelData, IUserData player,
              BattelFieldFactory battelFieldFactory, ICardFactory<IAttackCard> cardFactory) =>
     (Instantiate(Resources.Load<CommonBattelScene>($"BattleScene/CommonBattelScene")).Initialize(battelData, player,
     battelFieldFactory, cardFactory) as CommonBattelScene)
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
