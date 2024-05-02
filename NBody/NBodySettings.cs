namespace NBody;

public class NBodySettings
{
    public readonly double BodyMass;
    public readonly int DeltaTime;
    public readonly double ErrorDistance;
    public readonly int ThreadsNum;

    public NBodySettings(double bodyMass, int deltaTime, double errorDistance, int threadsNum)
    {
        BodyMass = bodyMass;
        DeltaTime = deltaTime;
        ErrorDistance = errorDistance;
        ThreadsNum = threadsNum;
    }
    
}