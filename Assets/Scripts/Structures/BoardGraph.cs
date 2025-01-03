using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.Structures
{
    public class BoardGraph<T>
    {
        private int _height, _width, _size;
        private BoardNode<T>[] _nodes;
        public int Count => _nodes.Length;
        public IEnumerable<BoardNode<T>> GetNodes =>  _nodes;
        public IEnumerable<(BoardNode<T> node, int index)> GetNodesWithIndex()
        {
            for (int i = 0; i < _nodes.Length; i++)
                yield return (_nodes[i], i);
        }
        public BoardNode<T> GetNodeByIndex(int index) => _nodes[index];
        public BoardGraph(int height, int width)
        {
            _height = height;
            _width = width;
            CreateNodes();
        }

        private void CreateNodes()
        {
            _size = _height * _width - (_width / 2);
            _nodes = new BoardNode<T>[_size];

            InitNodes();
            CreateDownConnect();
            CreateDiagonalConnect();
        }

        private void CreateDiagonalConnect()
        {
            int breakIndex = _height + (_height - 2);
            for (int i = _height; i < _nodes.Length;)
            {
                if (i - _height >= 0)
                {
                    _nodes[i].SetNode(_nodes[i - _height], 6);
                    _nodes[i - _height].SetNode(_nodes[i], 3);
                }
                if (i - (_height - 1) >= 0)
                {
                    _nodes[i].SetNode(_nodes[i - (_height - 1)], 5);
                    _nodes[i - (_height - 1)].SetNode(_nodes[i], 2);
                }
                if (i + _height < _size)
                {
                    _nodes[i].SetNode(_nodes[i + _height], 3);
                    _nodes[i + _height].SetNode(_nodes[i], 6);

                }
                if (i + (_height - 1) < _size)
                {
                    _nodes[i].SetNode(_nodes[i + (_height - 1)], 2);
                    _nodes[i + (_height - 1)].SetNode(_nodes[i], 5);
                }

                if (i < breakIndex)
                    i++;
                else
                {
                    i += _height + 1;
                    breakIndex += _height + 1 + (_height - 2);
                }
            }
        }

        private void CreateDownConnect()
        {
            int breakIndex = 0;
            bool dirtyFlagBreakIndex = true;
            for (int i = 0; i < _nodes.Length; i++)
            {
                if (i != breakIndex)
                {
                    _nodes[i].SetNode(_nodes[i - 1], 1);
                    _nodes[i - 1].SetNode(_nodes[i], 4);
                }
                else
                {
                    if (dirtyFlagBreakIndex)
                        breakIndex += _height;
                    else
                        breakIndex += (_height - 1);

                    dirtyFlagBreakIndex = !dirtyFlagBreakIndex;
                }
            }
        }

        private void InitNodes()
        {
            int breakIndex = _height - 1;
            bool dirtyFlagBreakIndex = true;
            int x = 1, y = 1;
            for (int i = 0; i < _nodes.Length; i++)
            {
                _nodes[i] = new BoardNode<T>(i + 1, (x, y));
                x += 2;
                if (i == breakIndex)
                {
                    y++;
                    if (dirtyFlagBreakIndex)
                    {
                        x = 2;
                        breakIndex += _height - 1;
                    }
                    else
                    {
                        x = 1;
                        breakIndex += _height;
                    }
                    dirtyFlagBreakIndex = !dirtyFlagBreakIndex;

                }
            }
        }
    }
}
