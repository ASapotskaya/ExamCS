using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExamCS
{
		class Airplane
		{
			private List<Dispatcher> dispatchers;                  // Список текущих диспетчеров
			private int CurrentSpeed;                              // Текущая скорость
			private int CurrentHeight;                             // Текущая Высота
			private int TotalPenalty;                              // Общая сумма штрафных очков
			private bool IsSpeedGained;                            // Показывает, набрана ли максимальная скорость
			private bool IsFlyBegin;                               // Показывает, начался ли полет

			private delegate void ChangeDelegate(int speed, int height);
			private event ChangeDelegate ChangeEvent;

			public Airplane()
			{
				dispatchers = new List<Dispatcher>();
				CurrentSpeed = 0;
				CurrentHeight = 0;
				TotalPenalty = 0;
				IsSpeedGained = false;
				IsFlyBegin = false;
			}
			                                                         // Функция для добавления диспечтера
			public void AddDispatcher(string name)
			{
				Dispatcher d = new Dispatcher(name);
				ChangeEvent += d.RecomendedHight;                    // Подписка на событие
				dispatchers.Add(d);                                  // Добавление в список
				Console.WriteLine($"\nДиспетчер {name} добавлен!\n");
			}
			                                                         // Функция для удаление диспетчера
			public void DeleteDispatcher(int position)
			{
				if (dispatchers.Count == 0)                          // Если в списке пусто
				{
					Console.WriteLine("Сначала добавьте диспетчера!");
					Console.Beep();
				}
				else if (position == -1)
				{
					Console.WriteLine("\nОтмена.\n");
					return;
				}
				else if (position >= 0 && position <= dispatchers.Count - 1)
				{
					ChangeEvent -= dispatchers[position].RecomendedHight;                 // Отписка от события
					Console.WriteLine($"Диспетчер {dispatchers[position].Name} удален!\a");
					TotalPenalty += dispatchers[position].Penalty;                       // Сохранение штрафных очков, полученных от удаляемого диспетчера
					dispatchers.RemoveAt(position);                                      // Удаление из списка
				}
				else
				{
					Console.WriteLine("Такого диспетчера не существует!");
					Console.Beep();
				}
			}
			                                                // Функция для вывода всех диспетчеровна экран
			public void PrintDispatchers()
			{
				Console.WriteLine();
				Console.WriteLine("0. Отмена");
				foreach (Dispatcher i in dispatchers)
					Console.WriteLine($"{dispatchers.IndexOf(i) + 1}. {i.Name}");
			}

			public void Fly()
			{
				Console.WriteLine("Управление:\n+(плюс) - добавить нового диспетчера," +
					"\n-(минус) - удалить выбраного диспетчера,\n" +
					"\nСтрелка вправо - увеличить скорость самолета на 50," +
					"\nСтрелка влево - уменьшить скорость самолета на 50," +
					"\nShift + стрелка вправо - увеличить скорость самолета на 150," +
					"\nShift + стрелка влево - уменьшить скорость самолета на 150,\n" +
					"\nСтрелка вверх - увеличить высоту самолета на 250," +
					"\nСтрелка вниз - уменьшить высоту самолета на 250," +
					"\nShift + стрелка вверх - увеличить высоту самолета на 500," +
					"\nShift + стрелка вниз - уменьшить высоту самолета на 500.\n");
				Console.WriteLine("Задача пилота — взлететь на самолете, набрать максимальную (1000 км/ч.) скорость, а затем посадить самолет.");

				ConsoleKeyInfo key;

				while (true)
				{
					key = Console.ReadKey();

					if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
					{
						if (key.Key == ConsoleKey.RightArrow) CurrentSpeed += 150;
						else if (key.Key == ConsoleKey.LeftArrow) CurrentSpeed -= 150;
						else if (key.Key == ConsoleKey.UpArrow) CurrentHeight += 500;
						else if (key.Key == ConsoleKey.DownArrow) CurrentHeight -= 500;
					}
					else
					{
						if (key.Key == ConsoleKey.RightArrow) CurrentSpeed += 50;
						else if (key.Key == ConsoleKey.LeftArrow) CurrentSpeed -= 50;
						else if (key.Key == ConsoleKey.UpArrow) CurrentHeight += 250;
						else if (key.Key == ConsoleKey.DownArrow) CurrentHeight -= 250;
						else if (key.Key == ConsoleKey.OemPlus || key.Key == ConsoleKey.Add)
						{
							Console.Write($"\nВведите имя диспетчера: ");
							AddDispatcher(Console.ReadLine());
						}
						else if (key.Key == ConsoleKey.OemMinus || key.Key == ConsoleKey.Subtract)
						{
							PrintDispatchers();
							Console.Write($"Введите номер диспетчера, которого хотите удалить: ");
							DeleteDispatcher(Convert.ToInt32(Console.ReadLine()) - 1);
						}
					}
					// Управление самолетом диспетчерами начинается
				
					if(dispatchers.Count >= 2 && CurrentSpeed >= 50)
					{
						Console.WriteLine();
						if (!IsFlyBegin)                                                             // Оповещение о начале полета
							Console.WriteLine("Полет начался!\a");
						IsFlyBegin = true;

						// В процессе полета самолет автоматически сообщает
						// всем диспетчерам все изменения в скорости
						// и высоте полета с помощью делегатов
						ChangeEvent(CurrentSpeed, CurrentHeight);
						if (CurrentSpeed == 1000)
						{
							IsSpeedGained = true;
							Console.WriteLine("\nВы набрали максимальную скорость. Ваша задача - посадить самолет!\a");
						}
						else if (IsSpeedGained && CurrentSpeed <= 50)// Управление самолетом диспетчерами прекращается
						{
							Console.WriteLine("\nПолет закончился!\a");
						

							break;// Выход из цикла
						}
					}
					else
					{
						Console.WriteLine("Добавлено недостаточно диспетчеров. Вы в тренировочном режиме.");
						Console.WriteLine($"Скорость: {CurrentSpeed} км/ч Высота: {CurrentHeight} м");
					}
				// Перебор всех диспетчеров в коллекции и суммирование 
				// всех штрафныех очков в общую сумму
				string current_directory = Directory.GetCurrentDirectory();
				string filename = "Penalty.txt";

				StreamWriter sw = new StreamWriter(filename);

				foreach (Dispatcher i in dispatchers)
				{
					TotalPenalty += i.Penalty;
					Console.WriteLine($"{i.Name}: {i.Penalty}");
					sw.WriteLine($"{i.Name}: {i.Penalty}"); //запись в файл
				}
				sw.Close();
				Console.WriteLine($"штрафные очки: {TotalPenalty}");
				Console.WriteLine($"Скорость: {CurrentSpeed} км/ч Высота: {CurrentHeight} м");
			}
			}
		}

	
}
