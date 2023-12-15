using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCS
{
	class AirplaneCrushed : Exception
	{
		public AirplaneCrushed(string message)
			: base(message)
		{ }
	}
	class Unsuitable : Exception
	{
		public Unsuitable(string message)
			: base(message)
		{ }
	}

	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				// В процессе тренировки пилотов самолета используется только один объект самолета
				Airplane plane = new Airplane();
				plane.Showmenu();
			}
			catch (AirplaneCrushed ac)
			{
				Console.WriteLine(ac.Message);
				Console.Beep();
			}
			catch (Unsuitable u)
			{
				Console.WriteLine(u.Message);
				Console.Beep();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
