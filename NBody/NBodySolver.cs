namespace NBody;

using System;
using System.Threading.Tasks;

public class NBodySolver
{
    private static Body[]? _bodies;
    private static int _dt;
    private static double _errorDistance;

    private readonly ForceCalculator[] _recalcingCallables;
    private readonly BodyMover[] _movingCallables;

    private readonly Task[] _forceCalculatingTasks;
    private readonly Task[] _movingTasks;
    private readonly TaskFactory _taskFactory;

    public NBodySolver(Point[] bodiesCoords, NBodySettings settings)
    {
        _bodies = new Body[bodiesCoords.Length];
        for (int i = 0; i < _bodies.Length; i++)
        {
            _bodies[i] = new Body(bodiesCoords[i], settings.BodyMass);
        }

        _dt = settings.DeltaTime;
        _errorDistance = settings.ErrorDistance;

        int[][] recalcingRanges = Helpers.GetRanges(0, _bodies.Length - 2, settings.ThreadsNum);
        int[][] movingRanges = Helpers.GetRanges(1, _bodies.Length, settings.ThreadsNum);
        
        _recalcingCallables = new ForceCalculator[recalcingRanges.Length];
        _movingCallables = new BodyMover[movingRanges.Length];
        
        for (int i = 0; i < recalcingRanges.Length; i++)
        {
            _recalcingCallables[i] = new ForceCalculator(recalcingRanges[i][0], recalcingRanges[i][1]);
            _movingCallables[i] = new BodyMover(movingRanges[i][0], movingRanges[i][1]);
        }

        _taskFactory = new TaskFactory();
        _forceCalculatingTasks = new Task[settings.ThreadsNum];
        _movingTasks = new Task[settings.ThreadsNum];
    }

    public void CalculateBodiesCoords()
    {
        RecalculateBodiesForces();
        MoveNBodies();
    }

    private void RecalculateBodiesForces()
    {
        for (int i = 0; i < _recalcingCallables.Length; i++)
        {
            _forceCalculatingTasks[i] = _taskFactory.StartNew(_recalcingCallables[i].Call);
        }

        Task.WaitAll(_forceCalculatingTasks);
    }

    private void MoveNBodies()
    {
        for (int i = 0; i < _movingCallables.Length; i++)
        {
            _movingTasks[i] = _taskFactory.StartNew(_movingCallables[i].Call);
        }

        Task.WaitAll(_movingTasks);
    }

    private class ForceCalculator
    {
        private readonly int leftIndex;
        private readonly int rightIndex;

        public ForceCalculator(int leftIndex, int rightIndex)
        {
            this.leftIndex = leftIndex;
            this.rightIndex = rightIndex;
        }

        public void Call()
        {
            double distance;
            double magnitude;
            Point direction;
            for (int k = leftIndex; k <= rightIndex; k++)
            {
                for (int l = k + 1; l < _bodies.Length; l++)
                {
                    distance = Physics.GetDistance(_bodies[k], _bodies[l]);
                    magnitude = distance < _errorDistance ? 0.0 : Physics.GetGravityMagnitude(_bodies[k].Mass, _bodies[l].Mass, distance);
                    direction = Physics.GetDirection(_bodies[k], _bodies[l]);

                    _bodies[k].Force.x += magnitude * direction.x / distance;
                    _bodies[k].Force.y += magnitude * direction.y / distance;

                    lock (this)
                    {
                        _bodies[l].Force.x -= magnitude * direction.x / distance;
                        _bodies[l].Force.y -= magnitude * direction.y / distance;
                    }
                }
            }
        }
    }

    private class BodyMover
    {
        private readonly int leftIndex;
        private readonly int rightIndex;

        public BodyMover(int rangeStart, int rangeEnd)
        {
            this.leftIndex = rangeStart - 1;
            this.rightIndex = rangeEnd - 1;
        }

        public void Call()
        {
            Point deltaV; // dv = F / m * dt
            Point deltaP; // dp = (v + dv / 2) * dt

            for (int i = leftIndex; i <= rightIndex; i++)
            {
                Body current = _bodies[i];
                deltaV = Physics.GetDv(current, _dt);
                deltaP = Physics.GetDp(current, _dt, deltaV);

                current.Velocity.x += deltaV.x;
                current.Velocity.y += deltaV.y;

                current.Position.x += deltaP.x;
                current.Position.y += deltaP.y;
                //if (i is 1 or 0) Console.WriteLine($"p{i}: {current.Position.x}, {current.Position.y}");
                current.Force.x = current.Force.y = 0.0;
            }
        }
    }
    
    
}
