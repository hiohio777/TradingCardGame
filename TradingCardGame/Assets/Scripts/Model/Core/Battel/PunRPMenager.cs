﻿using Newtonsoft.Json;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PunRPMenager : MonoBehaviourPunCallbacks
{
    private RatingBattelScene battelScene;

    private void Awake()
    {
        battelScene = FindObjectOfType<RatingBattelScene>();
        battelScene.Battel.SendReportRPC += ToCallRPC;
        photonView.RPC("STARTBATTEL", RpcTarget.Others, battelScene.Battel.Player.Report);
    }

    public override void OnDisable()
    {
        battelScene.Battel.SendReportRPC -= ToCallRPC;
        base.OnDisable();
    }

    private void ToCallRPC(string nameRPC)
    {
        var nameRPCUpper = nameRPC.ToUpper();
        Debug.Log($"PunRPC: {nameRPCUpper}");
        var data = string.Empty;
        switch (nameRPCUpper)
        {
            case "STARTING_HAND":
                data = GetDataSTARTING_HAND(battelScene.Battel.Player);
                break;
            case "FORTUNE":
                data = battelScene.Battel.Player.Fortune.ToString();
                break;
            case "RESERVE":
                data = GetDataRESERVE(battelScene.Battel.Player);
                break;
            case "TACTICS":
                data = GetDataTACTICS(battelScene.Battel.Player);
                break;
            default:
                photonView.RPC("ReportReadiness", RpcTarget.Others);
                return;
        }

        photonView.RPC(nameRPCUpper, RpcTarget.Others, data);
    }

    private string GetDataSTARTING_HAND(IBattelPerson player)
    {
        var cardReserv = new List<string>()
        {
            player.ReservCards[0].Combat.Name,
            player.ReservCards[1].Combat.Name,
            player.ReservCards[2].Combat.Name,
            player.ReservCards[3].Combat.Name,
            player.ReservCards[4].Combat.Name,
            player.ReservCards[5].Combat.Name,
        };
        return JsonConvert.SerializeObject(cardReserv);
    }

    private string GetDataRESERVE(IBattelPerson player)
    {
        var cardsAttack = new List<string>()
        {
            player.AttackCards[0].BattelCard.Combat.Name,
            player.AttackCards[1].BattelCard.Combat.Name,
            player.AttackCards[2].BattelCard.Combat.Name,
            player.AttackCards[3].BattelCard.Combat.Name,
        };
        var cardReserv = new List<string>()
        {
            player.ReservCards[0].Combat.Name,
            player.ReservCards[1].Combat.Name,
            player.ReservCards[2].Combat.Name,
            player.ReservCards[3].Combat.Name,
            player.ReservCards[4].Combat.Name,
            player.ReservCards[5].Combat.Name,
        };
        return new PersonDATAREPORT(cardsAttack, cardReserv).GetJsonString();
    }

    private string GetDataTACTICS(IBattelPerson player)
    {
        var cards = new List<int>();
        foreach (var item in player.AttackCards)
        {
            if (item.AttackCardTarget != null) cards.Add(item.AttackCardTarget.Id);
            else cards.Add(-1);
        }
        return JsonConvert.SerializeObject(cards);
    }

    [PunRPC]
    private void STARTBATTEL(string data)
    {
        // Получение данных от противника для начала битвы
        var dataTemp = JsonConvert.DeserializeObject<StartBattelDATAREPORT>(data);
        battelScene.Battel.Enemy.Creat(dataTemp.name, dataTemp.fraction, dataTemp.cards);
        battelScene.StartBattel();
    }

    [PunRPC]
    private void STARTING_HAND(string data)
    {
        battelScene.Battel.Enemy.Report = data;
        battelScene.Battel.Enemy.NewStartingHand();
        battelScene.Battel.ReportReadinessEnemy();
    }

    [PunRPC]
    private void FORTUNE(string data)
    {
        // Передана удача(её значение в начале битвы)
        if (data == "True")
        {
            battelScene.Battel.Enemy.Fortune = true;
            battelScene.Battel.Player.Fortune = false;
        }
        else
        {
            battelScene.Battel.Enemy.Fortune = false;
            battelScene.Battel.Player.Fortune = true;
        }
    }

    [PunRPC]
    private void RESERVE(string data)
    {
        battelScene.Battel.Enemy.Report = data;
        battelScene.Battel.ReportReadinessEnemy();
    }

    [PunRPC]
    private void TACTICS(string data)
    {
        // внедрить данные о действиях врага в карты на поле боя
        battelScene.Battel.Enemy.Report = data;
        battelScene.Battel.ReportReadinessEnemy();
    }

    [PunRPC]
    private void ReportReadiness()
    {
        //Получен сигнал об окончании фазы боя противником
        battelScene.Battel.ReportReadinessEnemy();
    }
}