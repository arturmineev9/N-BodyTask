namespace NBody;

public class NBodySimulation(Body[] bodies, double dt)
{
    private Body[] _bodies = bodies;
    private double _dt = dt;
    private const double G = 6.67e-11;

    public void Simulate(double time)
    {
        for (double t = 0; t < time; t += _dt)
        {
            CalculateForces();
            MoveBodies();
        }
    }

    private void CalculateForces()
    {
        double distance, magnitude;
        Point direction;

        for (int i = 0; i < _bodies.Length; i++)
        {
            for (int j = i + 1; j < _bodies.Length; j++)
            {
                distance = Math.Sqrt(Math.Pow(_bodies[i].Position.x - _bodies[j].Position.x, 2) +
                                     Math.Pow(_bodies[i].Position.y - _bodies[j].Position.y, 2));
                magnitude = G * _bodies[i].Mass * _bodies[j].Mass / Math.Pow(distance, 2);
                direction = new Point(_bodies[j].Position.x - _bodies[i].Position.x,
                    _bodies[j].Position.y - _bodies[i].Position.y);

                _bodies[i].Force.x += magnitude * direction.x / distance;
                _bodies[j].Force.x -= magnitude * direction.x / distance;
                _bodies[i].Force.y += magnitude * direction.y / distance;
                _bodies[j].Force.y -= magnitude * direction.y / distance;
            }
        }
    }

    public void MoveBodies()
    {
        Point deltaV; // dv = F / m * dt
        Point deltaP; // dp = (v + dv / 2) * dt

        for (int i = 0; i < _bodies.Length; i++)
        {
            deltaV = new Point(_bodies[i].Force.x / _bodies[i].Mass * _dt,
                _bodies[i].Force.y / _bodies[i].Mass * _dt);
            deltaP = new Point((_bodies[i].Velocity.x + deltaV.x / 2) * _dt,
                (_bodies[i].Velocity.x + deltaV.x / 2) * _dt);

            _bodies[i].Velocity.x += deltaV.x;
            _bodies[i].Velocity.y += deltaV.y;
            _bodies[i].Position.x += deltaP.x;
            _bodies[i].Position.y += deltaP.y;

            /*Console.WriteLine($"{i + 1} тело, {_bodies[i].Position.x}, {_bodies[i].Position.y}");
            if (i == 1)
            {
                Console.WriteLine();
            }
            _bodies[i].Force.x = _bodies[i].Force.y = 0.0;*/
        }
    }
}