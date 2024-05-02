using NBody;


Body b1 = new Body(new Point(1, 2),  1000);
Body b2 = new Body(new Point(2, 3),  1000);
Body b3 = new Body(new Point(10, 5),  100);
Body[] bodies = new [] {b1, b2};

NBodySimulation nBodySimulation = new NBodySimulation(bodies, 0.4, 1);
nBodySimulation.Simulate(10000000);
Console.WriteLine($"{b1.Position.x}, {b1.Position.y}");
Console.WriteLine($"{b2.Position.x}, {b2.Position.y}");

