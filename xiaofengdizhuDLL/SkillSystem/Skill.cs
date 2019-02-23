namespace Game
{
    public abstract class Skill
    {
        public CommonSubsystems subsystems = new CommonSubsystems();
        public CommonMethod commonMethod = new CommonMethod();

        public virtual string Name
        {
            get { return null; }
        }

        public virtual bool Input()
        {
            return false;
        }

        public virtual void Action()
        {
        }

        public ComponentPlayer componentPlayer
        {
            get { return subsystems.componentPlayer; }
        }
    }
}