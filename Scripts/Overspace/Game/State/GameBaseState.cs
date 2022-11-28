namespace Overspace.Game.State
{
    public abstract class GameBaseState
    {
        public virtual string StateName => "UNDEFINED";

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Tick();
    }
}