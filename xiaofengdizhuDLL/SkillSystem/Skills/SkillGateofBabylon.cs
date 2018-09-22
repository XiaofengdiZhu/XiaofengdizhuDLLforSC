using Engine;
using Engine.Input;

namespace Game
{
    public class SkillGateofBabylon : Skill
    {
        public override string Name
        {
            get { return "GateofBabylon"; }
        }
        public override bool Input()
        {
            return false;
            //return Keyboard.IsKeyDownOnce(Key.N);
        }
        private int[] some_items = new int[] { 34, 222, 36, 113, 32, 35, 218, 38, 115, 33, 29, 165, 37, 114, 169, 170, 32960, 49344, 65728, 82112, 98496, 74, 40, 42, 85, 109, 79, 22, 43, 102, 103, 111, 149, 30, 219, 171, 172, 80, 81, 220, 82, 116, 122, 123, 221, 124, 125, 192, 16576, 131264 };
        public override void Action()
        {
            ComponentMiner miner = componentPlayer.ComponentMiner;
            Vector3 eyePosition = miner.ComponentCreature.ComponentCreatureModel.EyePosition;
            Vector3 viewDirection = componentPlayer.View.ActiveCamera.ViewPosition;
            Matrix matrix = miner.ComponentCreature.ComponentBody.Matrix;
            Random random = new Random();
            BodyRaycastResult? bodyRaycastResult = miner.PickBody(eyePosition, viewDirection);
            if (bodyRaycastResult.HasValue)
            {
                var bodyPosition = bodyRaycastResult.Value.ComponentBody.Position;
                for (var i = 0; i < 10; i++)
                {
                    var randomRight = random.UniformFloat(-20, 20);
                    var randomUp = random.UniformFloat(-2.5f, 20);
                    if (randomRight < 1.2 && randomUp < 1.2) continue;
                    var firePosition = eyePosition+matrix.Right*randomRight+matrix.Up*randomUp+matrix.Forward*random.UniformFloat(-2, 0.8f);
                    var fireDirection = (Vector3.Normalize(bodyPosition-firePosition)+random.Vector3(0.1f, false))*70;
                    subsystems.particles.AddParticleSystem(new WaterSplashParticleSystem(subsystems.terrain, firePosition, true));
                    subsystems.projectiles.FireProjectile(some_items[random.UniformInt(0, 49)], firePosition, fireDirection, random.Vector3(10, false), miner.ComponentCreature);
                }
            }
            else
            {
                for (var i = 0; i < 10; i++)
                {
                    var randomRight = random.UniformFloat(-20, 20);
                    var randomUp = random.UniformFloat(-2.5f, 20);
                    if (randomRight < 1.2 && randomUp < 1.2) continue;
                    var firePosition = eyePosition + matrix.Right * randomRight + matrix.Up * randomUp + matrix.Forward * random.UniformFloat(-2, 0.8f);
                    var fireDirection = (viewDirection + random.Vector3(0.2f, false) )* 70;
                    subsystems.particles.AddParticleSystem(new WaterSplashParticleSystem(subsystems.terrain, firePosition, true));
                    subsystems.projectiles.FireProjectile(some_items[random.UniformInt(0, 49)], firePosition, fireDirection, random.Vector3(10, false), miner.ComponentCreature);
                }
            }
                
        }
    }
}
