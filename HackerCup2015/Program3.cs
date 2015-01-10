using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace HackerCup2015
{
    // A-maze-ing
    internal class Program3
    {
        public class MazeWinException : Exception
        {
            
        }

        public class Maze
        {
            private readonly char[,] _maze;
            private Tuple<int, int> _coordsS;
            
            public Maze(char[,] maze)
            {
                _maze = maze;

                for (int i = 0; i < _maze.GetLength(0); i++)
                {
                    for (int j = 0; j < _maze.GetLength(1); j++)
                    {
                        if (_maze[i, j] == 'S')
                        {
                            _coordsS = Tuple.Create(i, j);
                            return;
                        }
                    }
                }

            }

            public string GetHash()
            {
                var charArray = new char[_maze.GetLength(0)*_maze.GetLength(1)];
                Buffer.BlockCopy(_maze, 0, charArray, 0, _maze.Length * 2);
                return new string(charArray);
            }

            // Moving S is the key
            public List<Maze> GetNextMoves()
            {
                var res = new List<Maze>();
                var nextMoves = new List<Tuple<int, int>>
                {
                    Tuple.Create(_coordsS.Item1 + 1, _coordsS.Item2),
                    Tuple.Create(_coordsS.Item1 - 1, _coordsS.Item2),
                    Tuple.Create(_coordsS.Item1, _coordsS.Item2 + 1),
                    Tuple.Create(_coordsS.Item1, _coordsS.Item2 - 1)
                };

                ApplyTurretMove();
                var invalidBlocks = GetBlockingBlocks();

                foreach (var nextMove in nextMoves)
                {
                    if (nextMove.Item1 < invalidBlocks.GetLength(0) && nextMove.Item1 >= 0
                        && nextMove.Item2 < invalidBlocks.GetLength(1) && nextMove.Item2 >= 0
                        && !invalidBlocks[nextMove.Item1, nextMove.Item2])
                    {
                        Maze toAdd = Copy();

                        toAdd.ApplyPlayerMove(Tuple.Create(nextMove.Item1, nextMove.Item2));
                        res.Add(toAdd);
                    }
                }
                
                return res;
            }

            private void ApplyPlayerMove(Tuple<int, int> coords)
            {
                var curPlayer = _maze[_coordsS.Item1, _coordsS.Item2];
                var dest = _maze[coords.Item1, coords.Item2];

                if (dest == 'G')
                {
                    throw new MazeWinException();
                }

                _maze[_coordsS.Item1, _coordsS.Item2] = dest;
                _maze[coords.Item1, coords.Item2] = curPlayer;
                _coordsS = coords;
            }

            private Maze Copy()
            {
                var copiedMaze = new char[_maze.GetLength(0), _maze.GetLength(1)];

                Array.Copy(_maze, copiedMaze, _maze.Length);

                return new Maze(copiedMaze);
            }

            private void ApplyTurretMove()
            {
                for (int i = 0; i < _maze.GetLength(0); i++)
                {
                    for (int j = 0; j < _maze.GetLength(1); j++)
                    {
                        var c = _maze[i, j];

                        switch (c)
                        {
                            case '>':
                                _maze[i, j] = 'v';
                                break;
                            case 'v':
                                _maze[i, j] = '<';
                                break;
                            case '<':
                                _maze[i, j] = '^';
                                break;
                            case '^':
                                _maze[i, j] = '>';
                                break;
                        }
                    }
                }
            }

            private bool[,] GetBlockingBlocks()
            {
                var res = new bool[_maze.GetLength(0), _maze.GetLength(1)];

                for (int i = 0; i < _maze.GetLength(0); i++)
                {
                    for (int j = 0; j < _maze.GetLength(1); j++)
                    {
                        var c = _maze[i, j];

                        switch (c)
                        {
                            case '#':
                                res[i, j] = true;
                                break;
                            case '>':
                                res[i, j] = true;

                                // Blast
                                for (int k = j + 1; k < _maze.GetLength(1); k++)
                                {
                                    char c2 = _maze[i, k];

                                    if (c2 == '#' || c2 == '>' || c2 == 'v' || c2 == '<' || c2 == '^')
                                    {
                                        break;
                                    }

                                    res[i, k] = true;
                                }
                                break;
                            case 'v':
                                res[i, j] = true;

                                // Blast
                                for (int k = i + 1; k < _maze.GetLength(0); k++)
                                {
                                    char c2 = _maze[k, j];

                                    if (c2 == '#' || c2 == '>' || c2 == 'v' || c2 == '<' || c2 == '^')
                                    {
                                        break;
                                    }

                                    res[k, j] = true;
                                }
                                break;
                            case '<':
                                res[i, j] = true;

                                // Blast
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    char c2 = _maze[i, k];

                                    if (c2 == '#' || c2 == '>' || c2 == 'v' || c2 == '<' || c2 == '^')
                                    {
                                        break;
                                    }

                                    res[i, k] = true;
                                }

                                break;
                            case '^':
                                res[i, j] = true;

                                // Blast
                                for (int k = i - 1; k >= 0; k--)
                                {
                                    char c2 = _maze[k, j];

                                    if (c2 == '#' || c2 == '>' || c2 == 'v' || c2 == '<' || c2 == '^')
                                    {
                                        break;
                                    }

                                    res[k, j] = true;
                                }
                                break;
                        }
                    }
                }

                return res;
            }
        }

        private static void Main()
        {
            const string inputPath = "test3.in";
            const string outputPath = "test3.out";
            var sb = new StringBuilder();

            using (var sr = new StreamReader(inputPath))
            {
                string[] input = sr.ReadToEnd()
                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                int nbCase = int.Parse(input[0]);
                int lineCounter = 1;

                for (int i = 0; i < nbCase; i++)
                {
                    string[] coords = input[lineCounter++]
                        .Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    int x = int.Parse(coords[0]), y = int.Parse(coords[1]);
                    var maze = new char[x, y];

                    for (int j = 0; j < x; j++)
                    {
                        string line = input[lineCounter++];

                        for (int k = 0; k < y; k++)
                        {
                            maze[j, k] = line[k];
                        }
                    }

                    var m = new Maze(maze);
                    var res = Treatment(m);
                    var toWriteLine = string.Format("Case #{0}: {1}", i + 1, res == 0 ? "impossible" : res.ToString(CultureInfo.InvariantCulture));

                    Console.WriteLine(toWriteLine);
                    sb.AppendLine(toWriteLine);
                }
            }

            var toWrite = sb.ToString();

            using (var sw = new StreamWriter(outputPath))
            {
                sw.Write(toWrite);
            }

            Console.ReadLine();
        }

        private static int Treatment(Maze initial)
        {
            var alreadyChecked = new HashSet<string> { initial.GetHash() };
            var next = new List<Maze> { initial };
            var toAdd = new List<Maze>();
            int count = 0;

            try
            {
                while (true)
                {
                    count++;

                    if (next.Count == 0)
                    {
                        return 0;
                    }

                    foreach (var maze in next)
                    {
                        var nextMoves = maze.GetNextMoves();

                        toAdd.AddRange(nextMoves.Where(n => alreadyChecked.Add(n.GetHash())));
                    }

                    next.Clear();
                    next.AddRange(toAdd);
                    toAdd.Clear();
                }
            }
            catch (MazeWinException)
            {
                return count;
            }
        }
    }
}
