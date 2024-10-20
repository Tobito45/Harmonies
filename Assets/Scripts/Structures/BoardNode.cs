using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Harmonies.Structures
{
    public class BoardNode
    {
        //  1 - up
        //6   2
        //5   3
        //  4 - down
        private BoardNode[] _neighbours = new BoardNode[6];
        public int Id { get; private set; }
        public (int x, int y) Coordinates { get; private set; }
        public GameCell GameCell { get; set; }

        private List<int> _indexes = new(); //future blocks in enum
        public int IndexesCount => _indexes.Count;
        public int GetIndex(int index) => _indexes[index];
        public void AddNewIndex(int index) => _indexes.Add(index);

        public BoardNode(int id, (int x, int y) coordinates)
        {
            Id = id;
            Coordinates = coordinates;
        }
        /// <summary>
        ///   1 - up
        /// 6   2
        /// 5   3
        ///   4 - down
        /// </summary>
        public void SetNode(BoardNode node, int index) => _neighbours[index - 1] = node;

        /// <summary>
        ///  1 - up 
        /// 6   2
        /// 5   3
        ///  4 - down
        /// </summary>
        public BoardNode GetNode(int index) => _neighbours[index - 1];

        public string AllNeighBoursToString() => $"{Id}, {Coordinates}: 1 - {_neighbours[0]?.Id}, 2 - {_neighbours[1]?.Id}, 3 - {_neighbours[2]?.Id}" +
            $", 4 - {_neighbours[3]?.Id}, 5 - {_neighbours[4]?.Id}, 6 - {_neighbours[5]?.Id}";

        public IEnumerator GetEnumerator() => _neighbours.GetEnumerator();

        public BoardNode this[int index]
        {
            get => GetNode(index);
        }

    }
}
