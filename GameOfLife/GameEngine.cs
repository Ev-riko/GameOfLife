using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class GameEngine
    {
        private bool[,] field;
        private readonly int rows;
        private readonly int cols;
        public int currentGeneration { get; private set; }


        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];
            Random rnd = new Random();

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next(0, density) == 0;
                }
            }
        }

        private int CountNeighours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                    count += field[(x + i + cols) % cols, (y + j + rows) % rows] && !(i == 0 && j == 0) ? 1 : 0;
            return count;
            //var col = (x + i + cols) % cols;
            //var row = (y + j + rows) % rows;

            //var isSelfChecking = col == x && row == y;
            //var hasLife = field[col, row];

            //if (!isSelfChecking && hasLife)
            //{
            //    count++;
            //}
        }

        public void NextGeneration()
        {
            var newField = new bool[cols, rows];
            currentGeneration++;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighoursCount = CountNeighours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && (neighoursCount == 3))
                        newField[x, y] = true;
                    else if (hasLife && neighoursCount < 2 || neighoursCount > 3)
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];                    
                }
            }
            field = newField;            
        }

        public bool[,] GetCurrentGeneratiton()
        {
            bool[,] result = new bool[cols, rows];
            Array.Copy(field, result, field.Length);
            return result;
        }

        private bool ValidateCellPosition(int x, int y) => x >= 0 && y >= 0 && x < cols && y < rows;

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                field[x, y] = state;
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }
    }
}
