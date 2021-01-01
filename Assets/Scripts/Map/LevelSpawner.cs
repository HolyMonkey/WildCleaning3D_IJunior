﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private CurrentLevelLoader _levelLoader;
    [SerializeField] private GameCell _floor;
    [SerializeField] private PlayerInitializer _playerInitializer;

    public event UnityAction<CellObject> CellObjectSpawned;
    public event UnityAction SpawnCompleted;

    public List<GameCell> InstCells { get; private set; }

    private void Start()
    {
        InstCells = new List<GameCell>();

        foreach (Vector2Int cell in _levelLoader.CurrentLevel.Map.Keys)
        {
            GameCell gameCell = SpawnCell(cell, _floor);
            SpawnLevelObject(_levelLoader.CurrentLevel.Map[cell], gameCell);
        }

        InitAdjacentCells(InstCells);
        SpawnCompleted?.Invoke();
    }

    private void SpawnLevelObject(CellData cellData, GameCell targetCell)
    {
        if (cellData.LevelObject == null)
            return;

        var inst = Instantiate(cellData.LevelObject.Prefab, targetCell.Position.ToVector3(0.5f), Quaternion.identity);
        inst.Init(targetCell);
        cellData.Parameters?.Apply(inst);
        CellObjectSpawned?.Invoke(inst);

        if (inst.TryGetComponent(out Player player))
            _playerInitializer.SetPlayer(player);
    }

    private GameCell SpawnCell(Vector2Int cell, GameCell template)
    {
        GameCell gameCell = Instantiate(template, cell.ToVector3(), Quaternion.identity);
        gameCell.Init(cell);
        InstCells.Add(gameCell);
        return gameCell;
    }

    private void InitAdjacentCells(List<GameCell> cells)
    {
        foreach (GameCell gameCell in cells)
        {
            var adjacentDirections = new List<Vector2Int>() { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
            var adjacentCells = GetAdjacentCells(gameCell.Position, adjacentDirections);
            gameCell.InitAdjacentCells(adjacentCells);
        }
    }

    private Dictionary<Vector2Int, GameCell> GetAdjacentCells(Vector2Int fromPosition, List<Vector2Int> adjacentDirections)
    {
        var adjacentCells = new Dictionary<Vector2Int, GameCell>();

        foreach (Vector2Int direction in adjacentDirections)
        {
            GameCell adjacentCell = InstCells.Find((cell) => cell.Position == fromPosition + direction);
            if (adjacentCell != null)
                adjacentCells.Add(direction, adjacentCell);
        }

        return adjacentCells;
    }
}