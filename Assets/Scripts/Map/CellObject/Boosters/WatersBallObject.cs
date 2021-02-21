﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatersBallObject : BoosterObject
{
    public override void AddInInventory()
    {
        BoosterInventory inventory = new BoosterInventory(BoostersDataBase);
        inventory.Load(new JsonSaveLoad());

        foreach (var data in BoostersDataBase.Data)
        {
            if (data.Booster.Equals(Booster))
            {
                inventory.Add(data);
                break;
            }
        }

        inventory.Save(new JsonSaveLoad());
    }

    public override void Triggered(CellObject cellObject)
    {
        if (cellObject is Player == false)
            return;

        gameObject.SetActive(false);
    }
}
