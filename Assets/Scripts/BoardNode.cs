using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNode 
{
    //  1 - up
    //6   2
    //5   3
    //  4 - down
    private BoardNode[] neighbours = new BoardNode[6];
    public int Id { get; private set; }
    public (int x, int y) Coordinates { get; private set; }

    public BoardNode(int id, (int x, int y) coordinates)
    {
        Id = id;
        Coordinates = coordinates;
    }
    public void SetNode(BoardNode node, int index) => neighbours[index - 1] = node;

    public string AllNeighBoursToString() => $"{Id}, {Coordinates}: 1 - {neighbours[0]?.Id}, 2 - {neighbours[1]?.Id}, 3 - {neighbours[2]?.Id}" +
        $", 4 - {neighbours[3]?.Id}, 5 - {neighbours[4]?.Id}, 6 - {neighbours[5]?.Id}";
}
