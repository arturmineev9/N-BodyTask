namespace NBody;

public interface ISimulate
{
    public void Simulate(double time);
    public void CalculateForces();
    public void MoveBodies();
}