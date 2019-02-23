namespace Game
{
    public class CommonSubsystems
    {
        public SubsystemPlayers players = GameManager.Project.FindSubsystem<SubsystemPlayers>(true);

        public ComponentPlayer componentPlayer = GameManager.Project.FindSubsystem<SubsystemPlayers>(true).ComponentPlayers[0];

        public ComponentGui componentGui = GameManager.Project.FindSubsystem<SubsystemPlayers>(true).ComponentPlayers[0].ComponentGui;

        public SubsystemTerrain terrain = GameManager.Project.FindSubsystem<SubsystemTerrain>(true);

        public SubsystemExplosions explosions = GameManager.Project.FindSubsystem<SubsystemExplosions>(true);

        public SubsystemDrawing drawing = GameManager.Project.FindSubsystem<SubsystemDrawing>(true);

        public SubsystemWeather weather = GameManager.Project.FindSubsystem<SubsystemWeather>(true);

        public SubsystemSky sky = GameManager.Project.FindSubsystem<SubsystemSky>(true);

        public SubsystemGameInfo gameInfo = GameManager.Project.FindSubsystem<SubsystemGameInfo>(true);

        public SubsystemTime time = GameManager.Project.FindSubsystem<SubsystemTime>(true);

        public SubsystemPickables pickables = GameManager.Project.FindSubsystem<SubsystemPickables>(true);

        public SubsystemProjectiles projectiles = GameManager.Project.FindSubsystem<SubsystemProjectiles>(true);

        public SubsystemParticles particles = GameManager.Project.FindSubsystem<SubsystemParticles>(true);

        public SubsystemFireworksBlockBehavior fireworks = GameManager.Project.FindSubsystem<SubsystemFireworksBlockBehavior>(true);

        public SubsystemElectricity electricity = GameManager.Project.FindSubsystem<SubsystemElectricity>(true);

        public SubsystemGlow glow = GameManager.Project.FindSubsystem<SubsystemGlow>(true);
    }
}