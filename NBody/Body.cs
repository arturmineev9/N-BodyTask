namespace NBody;

public class Body
{
    public Point Position { get; set; }
    public Point Velocity { get; set; }
    public Point Force { get; set; }
    public double Mass { get; set; }

    public Body(Point position, double mass)
    {
        Position = new Point(position.x, position.y);
        Velocity = new Point(0.0, 0.0);
        Force = new Point(0.0, 0.0);
        Mass = mass;
    }
}