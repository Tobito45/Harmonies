using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGraph
{
    private int height, width, size;
    private BoardNode[] nodes;

    public IEnumerable<BoardNode> GetNodes => nodes;
    public BoardGraph(int height, int width)
    {
        this.height = height;
        this.width = width;
        CreateNodes();
    }

    private void CreateNodes()
    {
        size = height * width - (width / 2);
        nodes = new BoardNode[size];

        InitNodes();
        CreateDownConnect();
        CreateDiagonalConnect();
    }

    private void CreateDiagonalConnect()
    {
        int breakIndex = height + (height - 2);
        for (int i = height; i < nodes.Length;)
        {
            if (i - height >= 0)
            {
                nodes[i].SetNode(nodes[i - height], 6);
                nodes[i - height].SetNode(nodes[i], 3);
            }
            if (i - (height - 1) >= 0)
            {
                nodes[i].SetNode(nodes[i - (height - 1)], 5);
                nodes[i - (height - 1)].SetNode(nodes[i], 2);
            }
            if (i + height < size)
            {
                nodes[i].SetNode(nodes[i + height], 3);
                nodes[i + height].SetNode(nodes[i], 6);

            }
            if (i + (height - 1) < size)
            {
                nodes[i].SetNode(nodes[i + (height - 1)], 2);
                nodes[i + (height - 1)].SetNode(nodes[i], 5);
            }

            if (i < breakIndex)
                i++;
            else
            {
                i += height + 1;
                breakIndex += height + 1 + (height - 2);
            }
        }
    }

    private void CreateDownConnect()
    {
        int breakIndex = 0;
        bool dirtyFlagBreakIndex = true;
        for (int i = 0; i < nodes.Length; i++)
        {
            if (i != breakIndex)
            {
                nodes[i].SetNode(nodes[i - 1], 1);
                nodes[i - 1].SetNode(nodes[i], 4);
            }
            else
            {
                if (dirtyFlagBreakIndex)
                    breakIndex += height;
                else
                    breakIndex += (height - 1);

                dirtyFlagBreakIndex = !dirtyFlagBreakIndex;
            }
        }
    }

    private void InitNodes()
    {
        int breakIndex = height - 1;
        bool dirtyFlagBreakIndex = true;
        int x = 1, y = 1;
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new BoardNode(i + 1, (x, y));
            x += 2;
            if (i == breakIndex)
            {
                y++;
                if (dirtyFlagBreakIndex)
                {
                    x = 2;
                    breakIndex += height - 1;
                }
                else
                {
                    x = 1;
                    breakIndex += height;
                }
                dirtyFlagBreakIndex = !dirtyFlagBreakIndex;

            }
        }
    }
}
