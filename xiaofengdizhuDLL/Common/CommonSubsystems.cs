using GameEntitySystem;

namespace Game
{
    public class CommonSubsystems
    {
        public Project project
        {
            get
            {
                return GameManager.Project;
            }
        }
        public SubsystemGui gui
        {
            get
            {
                return project.FindSubsystem<SubsystemGui>(true);
            }
        }
        public SubsystemPlayer player
        {
            get
            {
                return project.FindSubsystem<SubsystemPlayer>(true);
            }
        }
        public SubsystemTerrain terrain
        {
            get
            {
                return project.FindSubsystem<SubsystemTerrain>(true);
            }
        }
        public SubsystemExplosions explosions
        {
            get
            {
                return project.FindSubsystem<SubsystemExplosions>(true);
            }
        }
        public SubsystemDrawing drawing
        {
            get
            {
                return project.FindSubsystem<SubsystemDrawing>(true);
            }
        }
        public SubsystemWeather weather
        {
            get
            {
                return project.FindSubsystem<SubsystemWeather>(true);
            }
        }
        public SubsystemSky sky
        {
            get
            {
                return project.FindSubsystem<SubsystemSky>(true);
            }
        }
        public SubsystemGameInfo gameInfo
        {
            get
            {
                return project.FindSubsystem<SubsystemGameInfo>(true);
            }
        }
        public SubsystemTime time
        {
            get
            {
                return project.FindSubsystem<SubsystemTime>(true);
            }
        }
        public SubsystemPickables pickables
        {
            get
            {
                return project.FindSubsystem<SubsystemPickables>(true);
            }
        }
        public SubsystemProjectiles projectiles
        {
            get
            {
                return project.FindSubsystem<SubsystemProjectiles>(true);
            }
        }
        public SubsystemParticles particles
        {
            get
            {
                return project.FindSubsystem<SubsystemParticles>(true);
            }
        }
        public SubsystemFireworksBlockBehavior fireworks
        {
            get
            {
                return project.FindSubsystem<SubsystemFireworksBlockBehavior>(true);
            }
        }
    }
}
