using System.Diagnostics;
using NBody;


Body b1 = new Body(new Point(1, 2),  1000);
Body b2 = new Body(new Point(2, 3),  1000);
Body b3 = new Body(new Point(10, 5),  1000);
Body[] bodies = new [] {b1, b2};

Random random = new Random();
Point p1 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p2 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p3 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p4 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p5 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p6 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p7 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p8 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p9 = new Point(random.Next(1, 100), random.Next(1, 100));
Point p10 = new Point(random.Next(1, 100), random.Next(1, 100));



Point[] bodiesCoords = new[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10};

NBodySettings nBodySettings = new NBodySettings(10000, 1, 1, 3);

NBodySimulation nBodySimulation = new NBodySimulation(bodies, 1, 1);

NBodySolver nBodySolver = new NBodySolver(bodiesCoords, nBodySettings);

Console.WriteLine();

foreach (int[] subArray in Helpers.GetRanges(0,14, 3))
         
{
    foreach (int number in subArray)
    {
        Console.Write(number + " ");
    }
    Console.WriteLine();
}


Stopwatch stopwatch = new Stopwatch();

// Начинаем измерение времени
stopwatch.Start();

// Здесь может быть ваш код, время выполнения которого вы хотите измерить

// Останавливаем секундомер


// Выводим затраченное время

for (int t = 0; t < 100000; t = t + 1)
{
    nBodySolver.CalculateBodiesCoords();
    //Console.WriteLine($"p1: {p1.x}, {p1.y}");
    //Console.WriteLine($"p2: {p2.x}, {p2.y}");
    Console.WriteLine();
}
stopwatch.Stop();
Console.WriteLine("Время: " + stopwatch.Elapsed);
Console.WriteLine("done");
/*nBodySimulation.Simulate(1000000);
Console.WriteLine($"{b1.Position.x}, {b1.Position.y}");
Console.WriteLine($"{b2.Position.x}, {b2.Position.y}");
*/

#region Console.WriteLine()
        /*
        foreach (int[] subArray in recalcingRanges)
         
        {
            foreach (int number in subArray)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        foreach (int[] subArray in movingRanges)
        {
            foreach (int number in subArray)
            {
                Console.Write(number + " ");
            }
            Console.WriteLine();
        }*/
        #endregion