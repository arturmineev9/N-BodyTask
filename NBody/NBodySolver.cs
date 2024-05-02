namespace NBody;

using System;
using System.Threading.Tasks;

public class NBodySolver
{
    private static Body[]? _bodies;
    private static int _dt;
    private static readonly double _errorDistance = 5;

    private readonly RecalcingCallable[] _recalcingCallables;
    private readonly MovingCallable[] _movingCallables;

    private readonly Task[] _recalcingTasks;
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
        //_errorDistance = settings.ErrorDistance;

        int[][] recalcingRanges = Helpers.Ranges(0, _bodies.Length - 2, settings.ThreadsNum);
        int[][] movingRanges = Helpers.Ranges(1, _bodies.Length, settings.ThreadsNum);

        _recalcingCallables = new RecalcingCallable[recalcingRanges.Length];
        _movingCallables = new MovingCallable[movingRanges.Length];
        for (int i = 0; i < recalcingRanges.Length; i++)
        {
            _recalcingCallables[i] = new RecalcingCallable(recalcingRanges[i][0], recalcingRanges[i][1]);
            _movingCallables[i] = new MovingCallable(movingRanges[i][0], movingRanges[i][1]);
        }

        _taskFactory = new TaskFactory();
        _recalcingTasks = new Task[settings.ThreadsNum];
        _movingTasks = new Task[settings.ThreadsNum];
    }

    public int N() => _bodies.Length;
    
    public int BodyX(int index) => (int)_bodies[index].Position.x;

    public int BodyY(int index) => (int)_bodies[index].Position.y;

    public void RecalcBodiesCoords()
    {
        RecalculateBodiesForces();
        MoveNBodies();
    }

    private void RecalculateBodiesForces()
    {
        for (int i = 0; i < _recalcingCallables.Length; i++)
        {
            _recalcingTasks[i] = _taskFactory.StartNew(_recalcingCallables[i].Call);
        }

        Task.WaitAll(_recalcingTasks);
    }

    private void MoveNBodies()
    {
        for (int i = 0; i < _movingCallables.Length; i++)
        {
            _movingTasks[i] = _taskFactory.StartNew(_movingCallables[i].Call);
        }

        Task.WaitAll(_movingTasks);
    }

    private class RecalcingCallable
    {
        private readonly int leftIndex;
        private readonly int rightIndex;

        public RecalcingCallable(int leftIndex, int rightIndex)
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
                    distance = GetDistance(_bodies[k], _bodies[l]);
                    magnitude = distance < _errorDistance ? 0.0 : GetGravityMagnitude(_bodies[k].Mass, _bodies[l].Mass, distance);
                    direction = GetDirection(_bodies[k], _bodies[l]);

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

    private class MovingCallable
    {
        private readonly int leftIndex;
        private readonly int rightIndex;

        public MovingCallable(int rangeStart, int rangeEnd)
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
                deltaV = GetDv(current, _dt);
                deltaP = GetDp(current, _dt, deltaV);

                current.Velocity.x += deltaV.x;
                current.Velocity.y += deltaV.y;

                current.Position.x += deltaP.x;
                current.Position.y += deltaP.y;

                current.Force.x = current.Force.y = 0.0;
            }
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
        return 1 * m1 * m2 / Math.Pow(r, 2);
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
