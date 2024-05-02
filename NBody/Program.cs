using NBody;


Body b1 = new Body(new Point(1, 2),  1000);
Body b2 = new Body(new Point(2, 3),  1000);
Body b3 = new Body(new Point(10, 5),  1000);
Body[] bodies = new [] {b1, b2};

Point p1 = new Point(1, 2);
Point p2 = new Point(2, 3);
Point p3 = new Point(10, 5);

Point[] bodiesCoords = new[] { p1, p2 };

NBodySettings nBodySettings = new NBodySettings(1000, 1, 5, 2);

NBodySimulation nBodySimulation = new NBodySimulation(bodies, 1, 1);

NBodySolver nBodySolver = new NBodySolver(bodiesCoords, nBodySettings);


for (int t = 0; t < 1000000; t = t + 1)
{nBodySolver.RecalcBodiesCoords();}

Console.WriteLine("done");
nBodySimulation.Simulate(1000000);
Console.WriteLine($"{b1.Position.x}, {b1.Position.y}");
Console.WriteLine($"{b2.Position.x}, {b2.Position.y}");

