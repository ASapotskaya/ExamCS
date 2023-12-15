using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamCS
{
	class Dispatcher
	{
		public string Name { get; set; }          // Имя диспетчера
		public int Penalty { get; set; }          // Счетчик штрафных очков
		private int adjustment;                   // Корректировка погодных условий
		private static Random r;

		public Dispatcher(string name)
		{
			Name = name;
			r = new Random();
			adjustment = r.Next(-200, 200);
			Penalty = 0;
		}
		                                             // Фукция вывода рекомендуемой высоты полета
		public void RecomendedHight(int speed, int height)
		{
			int recomended = 7 * speed - adjustment; // Значение рекомендованой высоты

			int difference;                          // Разница между рекомендованой и текущей высотой
			if (height > recomended)                 // Вычитаем из большего меньшее
				difference = height - recomended;
			else
				difference = recomended - height;

			Console.WriteLine($"Диспетчер {Name}: Рекомендуемая высота полета: {recomended} м.");

			if (speed > 1000)// Превышение максимальной скорости
			{
				Penalty += 100;
				Console.WriteLine($"Диспетчер {Name}: Немедленно снизьте скорость!");
				Console.Beep();
			}

			if (difference >= 300 && difference < 600) Penalty += 25;
			else if (difference >= 600 && difference < 1000) Penalty += 50;
			else if (difference >= 1000 || (speed <= 0 && height <= 0))
                throw new AirplaneCrushed("Самолет разбился");          // Генерирует исключение «Самолет раз­бил­ся»

            if (Penalty >= 1000)
				throw new Unsuitable("Непригоден к полетам");           // Генерирует исключение «Непригоден к полетам»
		}
	}
}
