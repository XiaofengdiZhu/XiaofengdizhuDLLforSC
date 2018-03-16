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
        public SubsystemPlayers players
        {
            get
            {
                return project.FindSubsystem<SubsystemPlayers>(true);
            }
        }
        public ComponentPlayer componentPlayer
        {
            get
            {
                return players.ComponentPlayers[0];
            }
        }
        public ComponentGui componentGui
        {
            get
            {
                return componentPlayer.Entity.FindComponent<ComponentGui>(true);
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
        public SubsystemElectricity electricity
        {
            get
            {
                return project.FindSubsystem<SubsystemElectricity>(true);
            }
        }
    }
}
