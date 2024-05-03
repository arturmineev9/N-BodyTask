namespace NBody;

public class NBodySimulation(Body[] bodies, double dt, double eps) : ISimulate
{
    private Body[] _bodies = bodies;
    private double _dt = dt;
    private const double G = 6.67e-11;
    private readonly double _eps = eps;

    public void Simulate(double time)
    {
        for (double t = 0; t < time; t += _dt)
        {
            CalculateForces();
            MoveBodies();
        }
    }

    public void CalculateForces()
    {
        double distance, magnitude;
        Point direction;

        for (int i = 0; i < _bodies.Length; i++)
        {
            Body curr = _bodies[i];
            for (int j = i + 1; j < _bodies.Length; j++)
            {
                Body other = _bodies[j];

                distance = GetDistance(curr, other);
                magnitude = distance < eps ? 0.0 : GetGravityMagnitude(curr.Mass, other.Mass, distance);
                direction = GetDirection(curr, other);

                curr.Force.x += magnitude * direction.x / distance;
                other.Force.x -= magnitude * direction.x / distance;
                curr.Force.y += magnitude * direction.y / distance;
                other.Force.y -= magnitude * direction.y / distance;
            }
        }
    }

    public void MoveBodies()
    {
        Point deltaV; // dv = F / m * dt
        Point deltaP; // dp = (v + dv / 2) * dt

        for (int i = 0; i < _bodies.Length; i++)
        {
            Body curr = _bodies[i];
            deltaV = GetDv(curr, _dt);
            deltaP = GetDp(curr, _dt, deltaV);

            curr.Velocity.x += deltaV.x;
            curr.Velocity.y += deltaV.y;
            curr.Position.x += deltaP.x;
            curr.Position.y += deltaP.y;

            /*Console.WriteLine($"{i + 1} тело, {_bodies[i].Position.x}, {_bodies[i].Position.y}");
            if (i == 1)
            {
                Console.WriteLine();
            }*/
            curr.Force.x = curr.Force.y = 0.0;
        }
    }

    private static Point GetDv(Body body, double dt) // dv = F / m * dt
    {
        return new Point(body.Force.x / body.Mass * dt, body.Force.y / body.Mass * dt);
    }

    private static Point GetDp(Body body, double dt, Point dv) // dp = (v + dv / 2) * dt
    {
        return new Point((body.Velocity.x + dv.x / 2) * dt, (body.Velocity.x + dv.x / 2) * dt);
    }

    private static double GetGravityMagnitude(double m1, double m2, double r)
    {
        return G * m1 * m2 / Math.Pow(r, 2);
    }

    private static Point GetDirection(Body curr, Body other)
    {
        return new Point(other.Position.x - curr.Position.x,
            other.Position.y - curr.Position.y);
    }

    private static double GetDistance(Body curr, Body other)
    {
        return Math.Sqrt(Math.Pow(curr.Position.x - other.Position.x, 2) +
                         Math.Pow(curr.Position.y - other.Position.y, 2));
    }
}