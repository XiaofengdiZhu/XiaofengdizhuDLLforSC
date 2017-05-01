using Engine;
using Engine.Input;
using GameEntitySystem;

namespace Game
{
    public class SkillFireBall : Skill
    {
        public override string Name {
            get { return "FireBall"; }
        }
        public override bool Input()
        {
            return InputManager.IsKeyDownOnce(Key.Y);
        }
        public override void Action()
        {
            Vector3 position = componentPlayer.ComponentBody.Position;
            subsystems.explosions.AddExplosion((int)position.X, (int)position.Y + 10, (int)position.Z, 500, false, false);
        }
    }
}
