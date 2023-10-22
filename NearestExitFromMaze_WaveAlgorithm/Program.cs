int[] entrance = new int[]
{
    4,
    5
};
string maze = "[[\"+\",\"+\",\"+\",\"+\",\".\",\".\",\"+\",\"+\"],[\"+\",\"+\",\".\",\"+\",\"+\",\".\",\"+\",\"+\"],[\".\",\"+\",\"+\",\".\",\"+\",\".\",\"+\",\"+\"],[\"+\",\"+\",\"+\",\"+\",\"+\",\".\",\"+\",\"+\"],[\"+\",\"+\",\"+\",\"+\",\"+\",\".\",\".\",\"+\"],[\"+\",\".\",\"+\",\"+\",\"+\",\"+\",\"+\",\"+\"]]";
var mazeChars = InputsTranslator.GetCharArray(maze);

Solution solution = new Solution();
solution.NearestExit(mazeChars, entrance);

public static class InputsTranslator
{
    public static char[][] GetCharArray(string maze)
    {
        maze = maze.Substring(1, maze.Length - 2);
        maze = maze.Replace("\"", "");

        string[] mazeSplitted = maze.Split("],[");

        char[][] result = new char[mazeSplitted.Length][];
        for (int i = 0; i < mazeSplitted.Length; i++)
        {
            result[i] = mazeSplitted[i]
                .Replace("[", "")
                .Replace("]", "")
                .Replace(",", "")
                .ToCharArray();
        }

        return result;
    }
}

public class Solution
{
    int?[,] mazeMap;

    public int NearestExit(char[][] maze, int[] entrance)
    {
        Cell initCell = new Cell { RowIndex = entrance[0], ColIndex = entrance[1], Value = 0 };
        mazeMap = ConvertMazeIntoMap(maze, initCell);

        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(initCell);
        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            List<Cell> nearCells = GetNearCells(currentCell, mazeMap);
            if (nearCells.Count == 0 && queue.Count == 0)
            {
                return -1;
            }
            else if (ContainsExit(nearCells, initCell))
            {
                return currentCell.Value + 1;
            }
            else
            {
                SetMapValues(nearCells,
                    mazeMap,
                    currentCell.Value);

                foreach (Cell cell in nearCells)
                {
                    queue.Enqueue(cell);
                }
            }
        }

        return -1;
    }

    public class Cell
    {
        public int RowIndex;
        public int ColIndex;
        public int Value;
    }

    private int?[,] ConvertMazeIntoMap(char[][] maze, Cell initCell)
    {
        int?[,] map = new int?[maze.Length, maze[0].Length];
        for (int i = 0; i < maze.Length; i++)
        {
            for (int j = 0; j < maze[0].Length; j++)
            {
                if (maze[i][j] == '+')
                {
                    map[i, j] = -1;
                }
                else
                {
                    map[i, j] = null;
                }
            }
        }

        map[initCell.RowIndex, initCell.ColIndex] = 0;

        return map;
    }

    private List<Cell> GetNearCells(Cell currentCell, int?[,] mazeMap)
    {
        List<Cell?> nearCells = new()
        {
            GetCell(currentCell.RowIndex + 1, currentCell.ColIndex, mazeMap),
            GetCell(currentCell.RowIndex, currentCell.ColIndex - 1, mazeMap),
            GetCell(currentCell.RowIndex - 1, currentCell.ColIndex, mazeMap),
            GetCell(currentCell.RowIndex, currentCell.ColIndex + 1, mazeMap)
        };

        List<Cell> cellsToFill = new List<Cell>();

        foreach (Cell? cell in nearCells)
        {
            bool cellIsOk = cell != null
                && mazeMap[cell.RowIndex,cell.ColIndex] == null;
            if (cellIsOk)
            {
                cell.Value = currentCell.Value + 1;
                cellsToFill.Add(cell);
            }
        }

        return cellsToFill;
    }

    private bool ContainsExit(List<Cell> cells, Cell initCell)
    {
        foreach (Cell cell in cells)
        {
            bool isExit = cell.RowIndex == 0
                || cell.RowIndex == mazeMap.GetLength(0) - 1
                || cell.ColIndex == 0
                || cell.ColIndex == mazeMap.GetLength(1) - 1;
            isExit = isExit && !(cell.RowIndex == initCell.RowIndex && cell.ColIndex == initCell.ColIndex);
            if (isExit)
            {
                return true;
            }
        }
        return false;
    }

    private Cell? GetCell(int rowIndex, int colIndex, int?[,] mazeMap)
    {
        bool isCellNull = rowIndex < 0
            || rowIndex > mazeMap.GetLength(0) - 1
            || colIndex < 0
            || colIndex > mazeMap.GetLength(1) - 1
            || mazeMap[rowIndex, colIndex] == -1;

        if (isCellNull)
        {
            return null;
        }
        else
        {
            return new Cell
            {
                ColIndex = colIndex,
                RowIndex = rowIndex
            };
        }
    }

    private void SetMapValues(List<Cell> cells, int?[,] mazeMap, int currentValue)
    {
        foreach (Cell cell in cells)
        {
            mazeMap[cell.RowIndex, cell.ColIndex] = currentValue + 1;
        }
    }
}