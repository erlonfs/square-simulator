using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projeto2015
{
	public class Program
	{
		private static ICollection<IElement> _elements = new List<IElement>();

		private static double _frames { get { return 1000D / (_msNextFrame + _msMainLoop); } }
		private static double _msNextFrame;
		private static double _msBase { get { return 125; } }
		private static double _msGameVelocity { get { return 30; } }
		private static double _msMainLoop = 0;

		private static TimeSpan _timeGameUpdate { get; set; }
		private static TimeSpan _timeMainLoop { get; set; }
		private static TimeSpan _timeFrameRate { get; set; }

		//milisegundos => 16.6; frames => 60fps
		//milisegundos => 33.2; frames => 30fps
		//milisegundos => 66.4; frames => 15fps
		//milisegundos => 125; frames => 8fps

		static void Main(string[] args)
		{

			Initialize();

			var _stWatch = new Stopwatch();

			var _countFrames = 0;
			var _timesOfMainLoop = new List<double>();

			while (true)
			{
				if (_timeMainLoop < DateTime.Now.TimeOfDay)
				{
					_stWatch.Reset();
					_stWatch.Start();

					Run();

					_stWatch.Stop();
					_msMainLoop = _stWatch.ElapsedMilliseconds;

					_msNextFrame = _msMainLoop - _msBase;
					_timeMainLoop = DateTime.Now.AddMilliseconds(_msNextFrame).TimeOfDay;

					_timesOfMainLoop.Add(_msMainLoop);
					_countFrames++;

				}

				if (_timeFrameRate < DateTime.Now.TimeOfDay)
				{
					ShowFrameRate(_countFrames, _timesOfMainLoop.Average());
					_timeFrameRate = DateTime.Now.AddSeconds(1).TimeOfDay;

					_countFrames = 0;
					_timesOfMainLoop.Clear();
				}

			}
		}

		static void ShowFrameRate(int frames, double msMainLoop)
		{
			//Console.ForegroundColor = ConsoleColor.White;
			//Console.SetCursorPosition(0, 0);
			Console.Title = $".:: square simulator ::.   {frames.ToString("N0")} fps | elapsed main loop {msMainLoop.ToString("N0")} milliseconds";
		}

		static bool Colision(Square square1, Square square2)
		{
			int left1 = square1.X;
			int left2 = square2.X;
			int right1 = square1.X + square1.Width;
			int right2 = square2.X + square2.Width;
			int top1 = square1.Y;
			int top2 = square2.Y;
			int bottom1 = square1.Y + square1.Height;
			int bottom2 = square2.Y + square2.Height;

			if (bottom1 < top2) return false;
			if (top1 > bottom2) return false;

			if (right1 < left2) return false;
			if (left1 > right2) return false;

			return true;

		}

		static void Run()
		{
			//if (mod % 4 == 0)
			//{
			//	Console.Clear();
			//	mod = 0;
			//}

			//mod++;

			if (_timeGameUpdate < DateTime.Now.TimeOfDay)
			{

				Console.Clear();

				_elements.ForEach(q1 =>
				{
					bool colision = false;
					_elements.ForEach(q2 => { if (q2.X != q1.X && q2.Y != q1.Y) { colision = colision || Colision((Square)q1, (Square)q2); } });
					q1.Update(colision);
					q1.Draw();
				});

				_timeGameUpdate = DateTime.Now.AddMilliseconds(_msGameVelocity).TimeOfDay;

			}
		}

		static void Initialize()
		{
			Console.CursorVisible = false;
			_timeMainLoop = DateTime.Now.TimeOfDay;

			_msNextFrame = _msBase;

			var square = new Square();
			square.X = 1;
			square.Y = 10;
			square.Width = 2;
			square.Height = 1;
			square.Cor = ConsoleColor.Gray;
			_elements.Add(square);

			/*square = new Square();
			square.X = 50;
			square.Y = 10;
			square.Width = 2;
			square.Height = 1;
			square.Cor = ConsoleColor.Magenta;
			_elements.Add(square);

			square = new Square();
			square.X = 15;
			square.Y = 15;
			square.Width = 2;
			square.Height = 1;
			square.Cor = ConsoleColor.Cyan;
			_elements.Add(square);*/

			square = new Square();
			square.X = 5;
			square.Y = 25;
			square.Width = 2;
			square.Height = 1;
			square.Cor = ConsoleColor.Green;
			_elements.Add(square);
		}

	}

	public static class System
	{
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}
	}
}
