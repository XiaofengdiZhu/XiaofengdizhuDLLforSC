using Engine;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class PlayGluttonousSnake
    {
        protected Engine.Random m_random = new Engine.Random();
        private int m_width;
        private int m_height;
        public List<Snake> m_snakes;
        private readonly TerrainType[,] m_layerTerrain;
        private int[,] m_layerSnake;
        private bool[,] m_layerSnakeHead;
        private Color[,] m_layerOutput;
        public bool m_stop = false;
        private CommonMethod commonMethod = new CommonMethod();

        public enum SnakeType
        {
            None,
            Player0,
            Player1,
            Player2,
            Player3,
            Computer
        }

        private readonly Color[] m_snakeBodyColor = new Color[]{
            new Color(0,0,0),
            new Color(255,255,255),
            new Color(255,255,255),
            new Color(255,255,255),
            new Color(255,255,255),
            new Color(255,0,0)
        };

        private readonly Color[] m_snakeHeadColor = new Color[]{
            new Color(0,0,0),
            new Color(160,160,160),
            new Color(160,160,160),
            new Color(160,160,160),
            new Color(160,160,160),
            new Color(255,128,128)
        };

        public enum SnakeStatus
        {
            Dead,
            Living
        }

        public enum TerrainType
        {
            None,
            Wall,
            Fruit
        }

        public class Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static readonly Point Zero = new Point(0, 0);
            public static readonly Point Up = new Point(0, -1);
            public static readonly Point Down = new Point(0, 1);
            public static readonly Point Right = new Point(1, 0);
            public static readonly Point Left = new Point(-1, 0);

            public static Point operator +(Point p1, Point p2)
            {
                return new Point(p1.X + p2.X, p1.Y + p2.Y);
            }

            public static Point operator -(Point p1, Point p2)
            {
                return new Point(p1.X - p2.X, p1.Y - p2.Y);
            }

            public static Point operator -(Point p1)
            {
                return new Point(-p1.X, -p1.Y);
            }

            public static bool operator ==(Point p1, Point p2)
            {
                return p1.X == p2.X && p1.Y == p2.Y;
            }

            public override bool Equals(object obj)
            {
                return obj is Point && Equals((Point)obj);
            }

            public bool Equals(Point p1)
            {
                return X == p1.X && Y == p1.Y;
            }

            public static bool operator !=(Point p1, Point p2)
            {
                return p1.X != p2.X || p1.Y != p2.Y;
            }

            public override string ToString()
            {
                return X + "," + Y;
            }

            public override int GetHashCode()
            {
                return X + Y;
            }

            public Point TurnRight()
            {
                if (this == Up)
                {
                    return Right;
                }
                else if (this == Down)
                {
                    return Left;
                }
                else if (this == Right)
                {
                    return Down;
                }
                else if (this == Left)
                {
                    return Up;
                }
                else return Zero;
            }

            public Point TurnLeft()
            {
                if (this == Up)
                {
                    return Left;
                }
                else if (this == Down)
                {
                    return Right;
                }
                else if (this == Right)
                {
                    return Up;
                }
                else if (this == Left)
                {
                    return Down;
                }
                else return Zero;
            }
        }

        public class Snake
        {
            public int Index;
            public SnakeType Type;
            public SnakeStatus Status;
            public Point Direction;
            public Point LastDirection;
            public Point Head;
            public List<Point> Body = new List<Point>();
        }

        public PlayGluttonousSnake(int width, int height)
        {
            m_width = width;
            m_height = height;
            m_layerTerrain = new TerrainType[m_width, m_height];
            m_layerSnake = new int[m_width, m_height];
            m_layerSnakeHead = new bool[m_width, m_height];
            m_layerOutput = new Color[m_width, m_height];
            m_snakes = new List<Snake>
            {
                new Snake() { Index = 0, Type = SnakeType.None, Status = SnakeStatus.Dead }
            };
        }

        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public Color[,] OutputLayer
        {
            get { return m_layerOutput; }
        }

        public List<Snake> Snakes
        {
            get { return m_snakes; }
        }

        public bool isPointInRange(Point point)
        {
            return point.X >= 0 && point.X < m_width && point.Y >= 0 && point.Y < m_height;
        }

        public bool isPointInRange(int x, int y)
        {
            return x >= 0 && x < m_width && y >= 0 && y < m_height;
        }

        //每帧执行
        public void UpdatePlayerSnakesDirection()
        {
            foreach (Snake snake in m_snakes)
            {
                int playerIndex = -1;
                switch (snake.Type)
                {
                    case SnakeType.None:
                        continue;
                    case SnakeType.Computer:
                        continue;
                    case SnakeType.Player0:
                        playerIndex = 0;
                        break;

                    case SnakeType.Player1:
                        playerIndex = 1;
                        break;

                    case SnakeType.Player2:
                        playerIndex = 2;
                        break;

                    case SnakeType.Player3:
                        playerIndex = 3;
                        break;
                }
                if (playerIndex > -1)
                {
                    if (playerIndex >= commonMethod.componentPlayers.Count) continue;
                    Point direction = snake.Direction;
                    Vector3 vector3 = commonMethod.componentPlayers[playerIndex].ComponentInput.PlayerInput.Move;
                    if (vector3.X > vector3.Z)
                    {
                        if (vector3.X > 0)
                        {
                            direction = Point.Right;
                        }
                        else
                        {
                            direction = Point.Down;
                        }
                    }
                    else if (vector3.X < vector3.Z)
                    {
                        if (vector3.Z > 0)
                        {
                            direction = Point.Up;
                        }
                        else
                        {
                            direction = Point.Left;
                        }
                    }
                    if (direction != snake.LastDirection && direction != -snake.LastDirection)
                    {
                        snake.Direction = direction;
                    }
                }
            }
        }

        //每次刷新前执行
        public void UpdateComputerSnakesDirection()
        {
            foreach (Snake snake in m_snakes)
            {
                if (snake.Type != SnakeType.Computer) continue;
                Point fruitDirection = FindFruitDirection(snake.Head, snake.LastDirection, snake.Index);
                Point direction = fruitDirection;
                if (direction == Point.Zero || direction == snake.LastDirection || direction == -snake.LastDirection)
                {
                    fruitDirection = snake.LastDirection;
                    direction = snake.LastDirection;
                }
                Point nextPosition = snake.Head + direction;
                int triedTimes = 0;
                while (
                    !isPointInRange(nextPosition)
                    || (m_layerSnake[nextPosition.X, nextPosition.Y] > 0 && m_layerSnake[nextPosition.X, nextPosition.Y] != snake.Index)
                    || m_layerTerrain[nextPosition.X, nextPosition.Y] == TerrainType.Wall
                    || (
                        m_layerSnake[nextPosition.X, nextPosition.Y] == 0 && m_layerTerrain[nextPosition.X, nextPosition.Y] != TerrainType.Wall
                        && (
                            (isPointInRange(nextPosition.X + direction.X, nextPosition.Y + direction.Y) && m_layerSnakeHead[nextPosition.X + direction.X, nextPosition.Y + direction.Y])
                            || (isPointInRange(nextPosition.X + direction.TurnLeft().X, nextPosition.Y + direction.TurnLeft().Y) && m_layerSnakeHead[nextPosition.X + direction.TurnLeft().X, nextPosition.Y + direction.TurnLeft().Y])
                            || (isPointInRange(nextPosition.X + direction.TurnRight().X, nextPosition.Y + direction.TurnRight().Y) && m_layerSnakeHead[nextPosition.X + direction.TurnRight().X, nextPosition.Y + direction.TurnRight().Y])
                        )
                    )
                )
                {
                    if (triedTimes == 2)
                    {
                        direction = snake.LastDirection;
                        nextPosition = snake.Head + direction;
                        break;
                    }
                    else if (triedTimes == 0)
                    {
                        triedTimes++;
                        if (fruitDirection == snake.LastDirection || fruitDirection == snake.LastDirection.TurnLeft())
                        {
                            direction = snake.LastDirection.TurnRight();
                        }
                        else
                        {
                            direction = snake.LastDirection.TurnLeft();
                        }
                        nextPosition = snake.Head + direction;
                    }
                    else if (triedTimes == 1)
                    {
                        triedTimes++;
                        if (direction == snake.LastDirection.TurnRight())
                        {
                            direction = snake.LastDirection.TurnLeft();
                        }
                        else if (direction == snake.LastDirection.TurnLeft())
                        {
                            direction = snake.LastDirection.TurnRight();
                        }
                        nextPosition = snake.Head + direction;
                    }
                }
                if (direction == Point.Zero || direction == -snake.LastDirection)
                {
                    snake.Direction = snake.LastDirection;
                }
                else
                {
                    snake.Direction = direction;
                }
            }
        }

        public void MoveSnakes()
        {
            var deadSnakes = new List<Snake>();
            foreach (Snake snake in m_snakes)
            {
                if (snake.Status == SnakeStatus.Dead) continue;
                snake.Body.Insert(0, snake.Head);
                snake.Head += snake.Direction;
                snake.LastDirection = snake.Direction;
                if (!isPointInRange(snake.Head) || m_layerTerrain[snake.Head.X, snake.Head.Y] == TerrainType.Wall)
                {
                    snake.Status = SnakeStatus.Dead;
                    deadSnakes.Add(snake);
                    continue;
                }
                if (m_layerTerrain[snake.Head.X, snake.Head.Y] != TerrainType.Fruit)
                {
                    snake.Body.RemoveAt(snake.Body.Count - 1);
                }
                else
                {
                    m_layerTerrain[snake.Head.X, snake.Head.Y] = TerrainType.None;
                }
            }
            UpdateLayerSnakeAndLayerSnakeHead();
            foreach (Snake snake in m_snakes)
            {
                if (snake.Status == SnakeStatus.Dead) continue;
                if (m_layerSnake[snake.Head.X, snake.Head.Y] != snake.Index)
                {
                    snake.Status = SnakeStatus.Dead;
                    deadSnakes.Add(snake);
                }
            }
            UpdateLayerSnakeAndLayerSnakeHead();
            deadSnakes = deadSnakes.OrderBy(s => s.Index).ToList();
            foreach (Snake snake in deadSnakes)
            {
                foreach (Point point in snake.Body)
                {
                    AddFruit(point);
                }
                m_snakes.Remove(snake);
                AddSnake(snake.Type);
            }
        }

        public void UpdateLayerSnakeAndLayerSnakeHead()
        {
            m_layerSnake = new int[m_width, m_height];
            m_layerSnakeHead = new bool[m_width, m_height];
            foreach (Snake snake in m_snakes)
            {
                if (snake.Status == SnakeStatus.Dead || snake.Type == SnakeType.None) continue;
                foreach (Point point in snake.Body)
                {
                    m_layerSnake[point.X, point.Y] = snake.Index;
                }
                if (m_layerSnake[snake.Head.X, snake.Head.Y] == 0) m_layerSnake[snake.Head.X, snake.Head.Y] = snake.Index;
                m_layerSnakeHead[snake.Head.X, snake.Head.Y] = snake.Index > 0;
            }
        }

        public void UpdateLayerOutput()
        {
            m_layerOutput = new Color[m_width, m_height];
            foreach (Snake snake in m_snakes)
            {
                if (snake.Status == SnakeStatus.Dead || snake.Type == SnakeType.None) continue;
                foreach (Point point in snake.Body)
                {
                    m_layerOutput[point.X, point.Y] = m_snakeBodyColor[(int)snake.Type];
                }
                m_layerOutput[snake.Head.X, snake.Head.Y] = m_snakeHeadColor[(int)snake.Type];
            }
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    if (m_layerTerrain[x, y] == TerrainType.Wall) m_layerOutput[x, y] = new Color(255, 255, 255);
                    if (m_layerTerrain[x, y] == TerrainType.Fruit) m_layerOutput[x, y] = new Color(0, 255, 0);
                }
            }
        }

        public void Update()
        {
            if (m_stop) return;
            UpdateComputerSnakesDirection();
            MoveSnakes();
            AddFruitRandomly();
            AddFruitRandomly();
            UpdateLayerOutput();
        }

        public Point FindBlankRandomly(bool allowFruit)
        {
            for (int triedTimes = 0; triedTimes < 500; triedTimes++)
            {
                int x = m_random.Int(0, m_width - 1);
                int y = m_random.Int(0, m_height - 1);
                if (allowFruit ? (m_layerTerrain[x, y] != TerrainType.Wall) : (m_layerTerrain[x, y] == TerrainType.None) && m_layerSnake[x, y] == 0)
                {
                    return new Point(x, y);
                }
            }
            return Point.Zero;
        }

        public void AddSnake(SnakeType type)
        {
            Point position = Point.Zero;
            var directions = new List<Point>();
            int triedTimes = 0;
            while (true)
            {
                triedTimes++;
                position = FindBlankRandomly(true);
                if (isPointInRange(position.X + 1, position.Y) && isPointInRange(position.X - 1, position.Y) && m_layerTerrain[position.X + 1, position.Y] != TerrainType.Wall && m_layerSnake[position.X + 1, position.Y] == 0 && m_layerTerrain[position.X - 1, position.Y] != TerrainType.Wall && m_layerSnake[position.X - 1, position.Y] == 0)
                {
                    directions.Add(new Point(1, 0));
                    directions.Add(new Point(-1, 0));
                }
                if (isPointInRange(position.X, position.Y + 1) && isPointInRange(position.X, position.Y - 1) && m_layerTerrain[position.X, position.Y + 1] != TerrainType.Wall && m_layerSnake[position.X, position.Y + 1] == 0 && m_layerTerrain[position.X, position.Y - 1] != TerrainType.Wall && m_layerSnake[position.X, position.Y - 1] == 0)
                {
                    directions.Add(new Point(0, 1));
                    directions.Add(new Point(0, -1));
                }
                if (directions.Count > 0) break;
                if (triedTimes > 1000) return;
            }
            int index = m_random.Int();
            var snake = new Snake()
            {
                Index = index,
                Type = type,
                Status = SnakeStatus.Living,
                Direction = directions[m_random.Int(0, directions.Count - 1)],
                Head = position
            };
            snake.LastDirection = snake.Direction;
            snake.Body.Add(position - snake.Direction);
            m_snakes.Add(snake);
        }

        public void AddFruit(int x, int y)
        {
            m_layerTerrain[x, y] = TerrainType.Fruit;
        }

        public void AddFruit(Point position)
        {
            m_layerTerrain[position.X, position.Y] = TerrainType.Fruit;
        }

        public void AddFruitRandomly()
        {
            Point position = FindBlankRandomly(false);
            if (position != Point.Zero) m_layerTerrain[position.X, position.Y] = TerrainType.Fruit;
        }

        public Point FindFruitDirection(Point position, Point originDirection, int snakeIndex)
        {
            var close = new Dictionary<Point, int>() { { position, 0 } };
            var open = new Dictionary<Point, int>();
            bool flag = m_random.Bool();
            var fourDirection = new List<Point>() {
                originDirection,
                flag ? originDirection.TurnRight() : originDirection.TurnLeft(),
                flag ? originDirection.TurnLeft() : originDirection.TurnRight(),
                -originDirection
            };
            //fourDirection.Sort(delegate (Point a, Point b) { return m_random.Int(-1, 1); });
            for (int directionIndex = 0; directionIndex < 4; directionIndex++)
            {
                Point nextPosition = position + fourDirection[directionIndex];
                if (!isPointInRange(nextPosition) || (m_layerSnake[nextPosition.X, nextPosition.Y] > 0 && m_layerSnake[nextPosition.X, nextPosition.Y] != snakeIndex) || m_layerTerrain[nextPosition.X, nextPosition.Y] == TerrainType.Wall)
                {
                    continue;
                }
                else
                {
                    if (m_layerTerrain[nextPosition.X, nextPosition.Y] == TerrainType.Fruit)
                    {
                        return fourDirection[directionIndex];
                    }
                    else
                    {
                        if (!open.ContainsKey(nextPosition)) open.Add(nextPosition, 1);
                    }
                }
            }
            Point fruitPosition = Point.Zero;
            bool founded = false;
            int circled = 0;
            while (open.Count > 0)
            {
                circled++;
                KeyValuePair<Point, int> minOpen = open.First();
                foreach (KeyValuePair<Point, int> dic in open)
                {
                    if (dic.Value < minOpen.Value)
                    {
                        minOpen = dic;
                    }
                }
                for (int directionIndex = 0; directionIndex < 4; directionIndex++)
                {
                    Point nextPosition = minOpen.Key + fourDirection[directionIndex];
                    if (!isPointInRange(nextPosition) || (m_layerSnake[nextPosition.X, nextPosition.Y] > 0 && m_layerSnake[nextPosition.X, nextPosition.Y] != snakeIndex) || m_layerTerrain[nextPosition.X, nextPosition.Y] == TerrainType.Wall)
                    {
                        continue;
                    }
                    else
                    {
                        if (close.ContainsKey(nextPosition))
                        {
                            if (minOpen.Value + 1 < close[nextPosition])
                            {
                                close[nextPosition] = minOpen.Value + 1;
                            }
                        }
                        else if (m_layerTerrain[nextPosition.X, nextPosition.Y] == TerrainType.Fruit)
                        {
                            fruitPosition = nextPosition;
                            founded = true;
                            break;
                        }
                        else if (!open.ContainsKey(nextPosition))
                        {
                            open.Add(nextPosition, minOpen.Value + 1);
                        }
                    }
                }
                if (!close.ContainsKey(minOpen.Key)) close.Add(minOpen.Key, minOpen.Value);
                open.Remove(minOpen.Key);
                if (founded)
                {
                    if (!close.ContainsKey(fruitPosition)) close.Add(fruitPosition, minOpen.Value + 1);
                    break;
                }
                if (circled > 1000) break;
            }
            if (founded)
            {
                Point previousPosition = fruitPosition;
                circled = 0;
                while (close[previousPosition] > 1)
                {
                    circled++;
                    var previousPositions = new Point[5];
                    previousPositions[4] = previousPosition;
                    for (int directionIndex = 0; directionIndex < 4; directionIndex++)
                    {
                        previousPositions[directionIndex] = previousPosition + fourDirection[directionIndex];
                        if (close.ContainsKey(previousPositions[directionIndex]) && close[previousPositions[directionIndex]] < close[previousPositions[4]])
                        {
                            previousPositions[4] = previousPositions[directionIndex];
                        }
                    }
                    previousPosition = previousPositions[4];
                    if (circled > 1000) break;
                }
                return previousPosition - position;
            }
            return Point.Zero;
        }
    }
}