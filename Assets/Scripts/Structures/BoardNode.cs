using Harmonies.Cells;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Harmonies.Structures
{
    public class BoardNode<T> // T is data
    {
        //  1 - up
        //6   2
        //5   3
        //  4 - down
        private BoardNode<T>[] _neighbours = new BoardNode<T>[6];
        public int Id { get; private set; }
        public (int x, int y) Coordinates { get; private set; }
        public GameCell GameCell { get; set; }

        private List<T> _indexes = new();
        public int IndexesCount => _indexes.Count;
        public T GetIndex(int index) => _indexes[index];
        public void AddNewIndex(T index) => _indexes.Add(index);

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
        public void SetNode(BoardNode<T> node, int index) => _neighbours[index - 1] = node;

        /// <summary>
        ///  1 - up 
        /// 6   2
        /// 5   3
        ///  4 - down
        /// </summary>
        public BoardNode<T> GetNode(int index) => _neighbours[index - 1];

        public int GetMaxNeighbours => _neighbours.Length;

        public string AllNeighBoursToString() => $"{Id}, {Coordinates}: 1 - {_neighbours[0]?.Id}, 2 - {_neighbours[1]?.Id}, 3 - {_neighbours[2]?.Id}" +
            $", 4 - {_neighbours[3]?.Id}, 5 - {_neighbours[4]?.Id}, 6 - {_neighbours[5]?.Id}";

        public IEnumerator GetEnumerator() => _neighbours.GetEnumerator();

        public BoardNode<T> this[int index]
        {
            get => GetNode(index);
        }

    }
}
