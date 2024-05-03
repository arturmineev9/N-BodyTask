namespace NBody;

public static class Physics
{
    private const double G = 6.67e-11;

    public static Point GetDv(Body body, double dt) // dv = F / m * dt
    {
        return new Point(body.Force.x / body.Mass * dt, body.Force.y / body.Mass * dt);
    }

    public static Point GetDp(Body body, double dt, Point dv) // dp = (v + dv / 2) * dt
    {
        return new Point((body.Velocity.x + dv.x / 2) * dt, (body.Velocity.x + dv.x / 2) * dt);
    }

    public static double GetGravityMagnitude(double m1, double m2, double r)
    {
        return G * m1 * m2 / Math.Pow(r, 2);
    }

    public static Point GetDirection(Body curr, Body other)
    {
        return new Point(other.Position.x - curr.Position.x,
            other.Position.y - curr.Position.y);
    }

    public static double GetDistance(Body curr, Body other)
    {
        return Math.Sqrt(Math.Pow(curr.Position.x - other.Position.x, 2) +
                         Math.Pow(curr.Position.y - other.Position.y, 2));
    }
}