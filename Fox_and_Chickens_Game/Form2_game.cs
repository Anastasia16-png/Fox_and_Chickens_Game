using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Fox_and_Chickens_Game
{
    public partial class Form2_game : Form
    {

        private const int BoardSize = 7;
        private const int CellSize = 90;
        private const int TopOffset = 35;

        private List<Point> chickenPositions;
        private List<Point> foxPositions;
        private bool isFoxTurn; // змінна для визначення черги ходу лисиці
        private int foxSelected;
        private int eatenChickenCount;
        private int chickenSelected;

        public Form2_game()
        {
            InitializeComponent();
            InitializeGraphics();
        }

        private void InitializeGraphics()
        {
            foxPositions = new List<Point>();
            chickenPositions = new List<Point>();
            isFoxTurn = false;
            foxSelected = -1;
            eatenChickenCount = 0;
            chickenSelected = -1;

            // ініціалізація позицій курей
            chickenPositions.Add(new Point(3, 0));
            chickenPositions.Add(new Point(3, 1));
            chickenPositions.Add(new Point(3, 2));
            chickenPositions.Add(new Point(3, 3));
            chickenPositions.Add(new Point(3, 4));
            chickenPositions.Add(new Point(3, 5));
            chickenPositions.Add(new Point(3, 6));
            chickenPositions.Add(new Point(4, 0));
            chickenPositions.Add(new Point(4, 1));
            chickenPositions.Add(new Point(4, 2));
            chickenPositions.Add(new Point(4, 3));
            chickenPositions.Add(new Point(4, 4));
            chickenPositions.Add(new Point(4, 5));
            chickenPositions.Add(new Point(4, 6));
            chickenPositions.Add(new Point(5, 2));
            chickenPositions.Add(new Point(5, 3));
            chickenPositions.Add(new Point(5, 4));
            chickenPositions.Add(new Point(6, 2));
            chickenPositions.Add(new Point(6, 3));
            chickenPositions.Add(new Point(6, 4));

            // ініціалізація позицій лисиць
            foxPositions.Add(new Point(2, 4));
            foxPositions.Add(new Point(2, 2));

            // розмір дошки відповідно до кількості комірок і розміру комірок 
            int boardWidth = BoardSize * CellSize;
            int boardHeight = BoardSize * CellSize + TopOffset;

            // розмір форми відповідно до поля
            this.ClientSize = new Size(boardWidth, boardHeight);

            // початкова дошка
            DrawBoard();
        }
        // пле гри
        private void DrawBoard()
        {
            var boardImage = new Bitmap(BoardSize * CellSize, BoardSize * CellSize + TopOffset);
            var g = Graphics.FromImage(boardImage);
            g.Clear(Color.White);

            // намальовані клітинки
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    bool isHighlighted = IsHighlighted(row, col);
                    DrawCell(g, row, col, isHighlighted);
                }
            }

            // намальовані кури
            foreach (var position in chickenPositions)
            {
                DrawChicken(g, position.X, position.Y);
            }

            // намальовані лисиці
            foreach (var position in foxPositions)
            {
                DrawFox(g, position.X, position.Y);
            }

            // намальовані у горі форми виведення поточних хідів ( лисиці чи курки)
            DrawCurrentTurn(g, isFoxTurn);
            // намальоване вікно поряд з поточними ходами про к-ть з'їдених курей
            DrawEatenChickenCount(g, eatenChickenCount);

            // намальоване зображення дошки на формі (клітикок)
            boardPictureBox.Image = boardImage;
        }

        private bool IsHighlighted(int row, int col)
        {
            return (row == 0 && col == 0) ||
                   (row == 0 && col == 1) ||
                   (row == 1 && col == 0) ||
                   (row == 1 && col == 1) ||
                   (row == 0 && col == 6) ||
                   (row == 0 && col == 5) ||
                   (row == 1 && col == 5) ||
                   (row == 1 && col == 6) ||
                   (row == 6 && col == 0) ||
                   (row == 5 && col == 0) ||
                   (row == 5 && col == 1) ||
                   (row == 6 && col == 1) ||
                   (row == 5 && col == 5) ||
                   (row == 5 && col == 6) ||
                   (row == 6 && col == 5) ||
                   (row == 6 && col == 6);
        }
        // метод, що створює клітинки 
        private void DrawCell(Graphics g, int row, int col, bool isHighlighted)
        {
            int x = col * CellSize;
            int y = row * CellSize + TopOffset;
            int cellOffset = isHighlighted ? 8 : 0;

            if (isHighlighted)
            {
                g.FillRectangle(Brushes.DarkSeaGreen, x, y, CellSize, CellSize); 
            }
            else
            {
                g.FillRectangle(Brushes.White, x, y, CellSize, CellSize);
            }

            g.DrawRectangle(Pens.Black, x, y, CellSize, CellSize);

        }
        private void DrawChicken(Graphics g, int row, int col)
        {
            var chickenRect = new Rectangle(col * CellSize, row * CellSize + TopOffset, CellSize, CellSize);
            var chickenImage = Properties.Resources.chicken2;
            g.DrawImage(chickenImage, chickenRect);
        }
        private void DrawFox(Graphics g, int row, int col)
        {
            var foxRect = new Rectangle(col * CellSize, row * CellSize + TopOffset, CellSize, CellSize);
            var foxImage = Properties.Resources.fox2;
            g.DrawImage(foxImage, foxRect);
        }
        // метод для відображення ходу
        private void DrawCurrentTurn(Graphics g, bool isFoxTurn)
        {
            string turnText = isFoxTurn ? "Хід лисиці" : "Хід курки";
            g.DrawString(turnText, new Font("Arial", 12), Brushes.Black, 10, 5);

        }
        // метод для відображення скільки курей було з'їдено
        private void DrawEatenChickenCount(Graphics g, int count)
        {
            g.DrawString("Кількість з'їдених курей: " + count, new Font("Arial", 12), Brushes.Black, 130, 5);
        }
        // метод з логікою ходів лисиць
        private void MoveFox(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (foxSelected != -1)
            {

                if (IsValidMoveForFoxMove(fromRow, fromCol, toRow, toCol))
                {
                    if (IsCellEmpty(toRow, toCol))
                    {
                        // переміщення лисиці на нову клітинку
                        foxPositions[foxSelected] = new Point(toRow, toCol);
                    }
                    else
                    {
                        MessageBox.Show("Хід не можливий, оберіть іншу клітинку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (IsValidMoveForFoxEat(fromRow, fromCol, toRow, toCol))
                {
                    // логіка переміщення лисиці з поїданням курки
                    // перевірка, чи між лисицею і цільовою клітинкою є курка
                    int chickenRow = (fromRow + toRow) / 2;
                    int chickenCol = (fromCol + toCol) / 2;

                    if (chickenPositions.Contains(new Point(chickenRow, chickenCol)))
                    {
                        // видалення з'їденої курки
                        chickenPositions.Remove(new Point(chickenRow, chickenCol));

                        // оновлення рахунку з'їдених курей
                        eatenChickenCount++;

                        MessageBox.Show("Лисиця з'їла курку!", "Поїдання курки", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // переміщення лисиці
                    foxPositions[foxSelected] = new Point(toRow, toCol);
                }
                else
                {
                    MessageBox.Show("Неприпустимий хід лисицею, оберіть іншу клітинку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // скидання вибору лисиці
                foxSelected = -1;

                // оновити дошку після ходу
                DrawBoard();

                // перевірка результату гри
                CheckGameResult();

                // хід належить курям
                isFoxTurn = false;
            }
            else
            {
                MessageBox.Show("Оберіть лисицю.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // метод з логікою ходів курей
        private void MoveChicken(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (IsValidMoveForCh(fromRow, fromCol, toRow, toCol))
            {
                var chickenPosition = new Point(toRow, toCol);
                chickenPositions[chickenSelected] = chickenPosition;

                // скидання вибору курки
                chickenSelected = -1;

                DrawBoard();
                CheckGameResult();
                isFoxTurn = true;

            }
            else
            {
                MessageBox.Show("Неприпустимий хід курки, оберіть іншу клітинку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsCellEmpty(int row, int col)
        {
            // перевірка чи клітинка призначення пуста (без лисиць та курей)
            return !chickenPositions.Any(chicken => chicken.X == row && chicken.Y == col) &&
                   !foxPositions.Any(fox => fox.X == row && fox.Y == col);
        }

        private bool IsValidMoveForCh(int fromRow, int fromCol, int toRow, int toCol)
        {
            // чи хід знаходиться в межах дошки
            if (toRow < 0 || toRow >= BoardSize || toCol < 0 || toCol >= BoardSize)
                return false;

            // чи клітинка призначення порожня
            if (!IsCellEmpty(toRow, toCol))
                return false;

            // чи хід знаходиться на відстані одного кроку
            if (Math.Abs(toCol - fromCol) > 1 || Math.Abs(toRow - fromRow) > 1)
                return false;

            // курка рухається вгору, вправо або вліво
            if (toRow > fromRow || Math.Abs(toCol - fromCol) > 1)
                return false;

            return true;
        }
        private bool IsValidMoveForFoxEat(int fromRow, int fromCol, int toRow, int toCol)
        {
            // чи хід знаходиться в межах дошки
            if (toRow < 0 || toRow >= BoardSize || toCol < 0 || toCol >= BoardSize)
                return false;

            // чи є поруч з лисицею курка
            int chickenRow = (toRow + fromRow) / 2;
            int chickenCol = (toCol + fromCol) / 2;

            // чи є курка між початковою і цільовою клітинками
            if (chickenPositions.Contains(new Point(chickenRow, chickenCol)))
            {
                // чи клітинка призначення порожня
                if (IsCellEmpty(chickenRow, chickenCol))
                {
                    if (IsCellEmpty(chickenRow, chickenCol))
                        return true;
                }
            }
            return true; 
        }
        private bool IsValidMoveForFoxMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            // чи хід в межах дошки
            if (toRow < 0 || toRow >= BoardSize || toCol < 0 || toCol >= BoardSize)
                return false;

            //чи клітинка призначення порожня
            if (isFoxTurn)
            {
                foreach (var position in foxPositions)
                {
                    if (position.X == toRow && position.Y == toCol)
                        return false;
                }
            }
            else
            {
                foreach (var position in chickenPositions)
                {
                    if (position.X == toRow && position.Y == toCol)
                        return false;
                }
                // чи клітинка, на яку рухається лисиця, порожня
                if (IsCellEmpty(toRow, toCol)) 
                    return false;
            }
            // чи хід знаходиться на відстані одного кроку
            if (Math.Abs(toRow - fromRow) > 1 || Math.Abs(toCol - fromCol) > 1)
                return false;

            return true;
        }
        private void boardPictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseArgs = (MouseEventArgs)e;
            int x = mouseArgs.X;
            int y = mouseArgs.Y - TopOffset;

            int row = y / CellSize;
            int col = x / CellSize;

            if (isFoxTurn)
            {
                if (foxSelected == -1)
                {
                    // вибір лисиці
                    if (foxPositions.Contains(new Point(row, col)))
                    {
                        foxSelected = foxPositions.IndexOf(new Point(row, col));
                    }
                }
                else
                {
                    // переміщення вибраної лисиці
                    int fromRow = foxPositions[foxSelected].X;
                    int fromCol = foxPositions[foxSelected].Y;
                    MoveFox(fromRow, fromCol, row, col);
                }
            }
            else
            {
                // переміщення курки
                if (chickenSelected == -1)
                {
                    if (chickenPositions.Contains(new Point(row, col)))
                    {
                        chickenSelected = chickenPositions.IndexOf(new Point(row, col));
                    }
                }
                else
                {
                    int fromRow = chickenPositions[chickenSelected].X;
                    int fromCol = chickenPositions[chickenSelected].Y;
                    MoveChicken(fromRow, fromCol, row, col);
                }
            }
        }
    
        private void CheckGameResult()
        {
            // перевірка закінчення гри (хто переміг)
            if (CheckFoxWin())
            {
                MessageBox.Show("Fox Wins!");
                InitializeGraphics(); // починається нова гра після перемоги лисиці
            }
            else if (CheckChickenWin())
            {
                MessageBox.Show("Chicken Wins!");
                InitializeGraphics(); 
            }
        }
        //метод, що перевіряє перемогу курей
        private bool CheckChickenWin()
        {
            int requiredPositionsCount = 9;
            int[] requiredPositionsCol = { 2, 3, 4 };
            int[] requiredPositionsRow = { 0, 1, 2 };
            // перевірка чи всі потрібні клітинки зайняті курями для перемоги
            int occupiedCount = 0;
            foreach (int col in requiredPositionsCol)
            {
                foreach (int row in requiredPositionsRow)
                {
                    if (chickenPositions.Contains(new Point(row, col)))
                    {
                        occupiedCount++;
                    }
                }
            }

            return occupiedCount == requiredPositionsCount;
        }
        // метод що перевіряє перемогу лисиць
        private bool CheckFoxWin()
        {
            // чи з'їли лисиці 12 курей
            return eatenChickenCount >= 12;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            InitializeGraphics();
        }
    }
}
