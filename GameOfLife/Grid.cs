using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.CodeDom.Compiler;

namespace GameOfLife
{
	public class Grid
	{
		public Point Size { get; private set; }
		private Cell[,] _cells;
		private bool[,] _nextCellStates;

		private TimeSpan _updateTimer;

		public Grid()
		{
			Size = new Point(Game1.CellsX, Game1.CellsY);

			_cells = new Cell[Size.X, Size.Y];
			_nextCellStates = new bool[Size.X, Size.Y];

			for (int i = 0; i < Size.X; i++)
			{
				for (int j = 0; j < Size.Y; j++)
				{
					_cells[i, j] = new Cell(new Point(i, j));
					_nextCellStates[i, j] = false;
				}
			}

			_updateTimer = TimeSpan.Zero;
		}

		public void Clear()
		{
			for (int i = 0; i < Size.X; i++)
			{
				for (int j = 0; j < Size.Y; j++)
				{
					_nextCellStates[i, j] = false;
				}
			}

			SetNextState();
		}

		public void SetNextState()
		{
			for (int i = 0; i < Size.X; i++)
			{
				for (int j = 0; j < Size.Y; j++)
				{
					_cells[i, j].IsAlive = _nextCellStates[i, j];
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (var cell in _cells)
			{
				cell.Draw(spriteBatch);
			}

			for (int i = 0; i < Size.X; i++)
			{
				spriteBatch.Draw(Game1.Pixel, new Rectangle(i * Game1.CellSize - 1, 0, 1, Size.Y * Game1.CellSize), Color.DarkGray);
			}

			for (int j = 0; j < Size.Y; j++)
			{
				spriteBatch.Draw(Game1.Pixel, new Rectangle(0, j * Game1.CellSize - 1, Size.X * Game1.CellSize, 1), Color.DarkGray);
			}

		}

		public void Update(GameTime gameTime)
		{
			MouseState mouseState = Mouse.GetState();

			foreach (var cell in _cells)
			{
				cell.Update(mouseState);
			}

			if (Game1.Paused)
				return;

			_updateTimer += gameTime.ElapsedGameTime;

			if (_updateTimer.TotalMilliseconds > 1000f / Game1.UPS)
			{
				_updateTimer = TimeSpan.Zero;


				// loop through every cell on the grid
				for (int i = 0; i < Size.X; i++)
				{
					for (int j = 0; j < Size.Y; j++)
					{
						bool living = _cells[i, j].IsAlive;
						int count = GetLivingNeighbors(i, j);
						bool result = false;

						// apply the rules and set the next state
						if (living && count < 2)
						{
							result = false;
						}

						if (living && (count == 2 || count == 3))
						{
							result = true;
						}

						if (living && count > 3)
						{
							result = false;
						}

						if (!living && count == 3)
						{
							result = true;
						}

						_nextCellStates[i, j] = result;
					}
				}

				SetNextState();
			}
		}

		public int GetLivingNeighbors(int x, int y)
		{
			int count = 0;

			// the first if statement checks for the borders of the move

			// check cell on the right
			if (x != Size.X - 1)
				if (_cells[x + 1, y].IsAlive)
					count++;

			// check cell on the bottom right
			if (x != Size.X - 1 && y != Size.Y - 1)
				if (_cells[x + 1, y + 1].IsAlive)
					count++;

			//check cell on the bottom
			if (y != Size.Y - 1)
				if (_cells[x, y + 1].IsAlive)
					count++;

			// check cell on the bottom left
			if (x != 0 && y != Size.Y - 1)
				if (_cells[x - 1, y + 1].IsAlive)
					count++;

			// check cells on the left
			if (x != 0)
				if (_cells[x - 1, y].IsAlive)
					count++;

			// check cell on the top left
			if (x != 0 && y != 0)
				if (_cells[x - 1, y - 1].IsAlive)
					count++;

			// check cell on the top
			if (y != 0)
				if (_cells[x, y - 1].IsAlive)
					count++;

			// check cell on the top right
			if (x != Size.X - 1 && y != 0)
				if (_cells[x + 1, y - 1].IsAlive)
					count++;

			return count;
		}
	}
}
