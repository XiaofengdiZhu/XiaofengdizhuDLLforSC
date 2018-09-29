using Engine;
using System;

namespace Game
{
    public class TelescopeCamera : BasePerspectiveCamera
    {
        public TelescopeCamera(View view) : base(view)
        {
        }

        public override bool UsesMovementControls
        {
            get
            {
                return true;
            }
        }

        public override bool IsEntityControlEnabled
        {
            get
            {
                return true;
            }
        }

        public override void Activate(Camera previousCamera)
        {
            m_angles = new Vector2(0f, (float)Math.Asin((double)previousCamera.ViewDirection.Y));
            m_angles.X = (float)Math.Acos(previousCamera.ViewDirection.X / Math.Cos(m_angles.Y));
            if (previousCamera.ViewDirection.Z > 0)
            {
                m_angles.X = -m_angles.X;
            }
            base.SetupPerspectiveCamera(previousCamera.ViewPosition, previousCamera.ViewDirection, previousCamera.ViewUp);
        }

        public override void Update(float dt)
        {
            ComponentPlayer componentPlayer = base.View.PlayerData.ComponentPlayer;
            if (componentPlayer == null || base.View.Target == null)
            {
                return;
            }
            ComponentInput componentInput = componentPlayer.ComponentInput;
            Vector3 cameraSneakMove = componentInput.PlayerInput.CameraSneakMove;
            Vector2 cameraLook = componentInput.PlayerInput.CameraLook;
            this.m_angles.X = MathUtils.NormalizeAngle(this.m_angles.X - 4f * cameraLook.X * dt + 0.5f * cameraSneakMove.X * dt);
            this.m_angles.Y = MathUtils.Clamp(MathUtils.NormalizeAngle(this.m_angles.Y + 4f * cameraLook.Y * dt), MathUtils.DegToRad(-45f), MathUtils.DegToRad(80f));
            this.m_distance = MathUtils.Clamp(this.m_distance + 50f * cameraSneakMove.Z * dt, 2f, 100f);
            Vector3 v = Vector3.Transform(new Vector3(this.m_distance, 0f, 0f), Matrix.CreateFromYawPitchRoll(this.m_angles.X, 0f, this.m_angles.Y));
            Vector3 vector = base.View.Target.ComponentBody.BoundingBox.Center();
            Vector3 vector2 = vector + v;
            if (Vector3.Distance(vector2, this.m_position) < 100f)
            {
                Vector3 v2 = vector2 - this.m_position;
                float s = MathUtils.Saturate(10f * dt);
                this.m_position += s * v2;
            }
            else
            {
                this.m_position = vector2;
            }
            Vector3 vector3 = this.m_position - vector;
            float? num = null;
            Vector3 vector4 = Vector3.Normalize(Vector3.Cross(vector3, Vector3.UnitY));
            Vector3 v3 = Vector3.Normalize(Vector3.Cross(vector3, vector4));
            for (int i = 0; i <= 0; i++)
            {
                for (int j = 0; j <= 0; j++)
                {
                    Vector3 v4 = 0.5f * (vector4 * (float)i + v3 * (float)j);
                    Vector3 vector5 = vector + v4;
                    Vector3 end = vector5 + vector3 + Vector3.Normalize(vector3) * 0.5f;
                    TerrainRaycastResult? terrainRaycastResult = base.View.SubsystemViews.SubsystemTerrain.Raycast(vector5, end, false, true, (int value, float distance) => !BlocksManager.Blocks[Terrain.ExtractContents(value)].IsTransparent);
                    if (terrainRaycastResult != null)
                    {
                        num = new float?((num != null) ? MathUtils.Min(num.Value, terrainRaycastResult.Value.Distance) : terrainRaycastResult.Value.Distance);
                    }
                }
            }
            Vector3 vector6;
            if (num != null)
            {
                vector6 = vector + Vector3.Normalize(vector3) * MathUtils.Max(num.Value - 0.5f, 0.2f);
            }
            else
            {
                vector6 = vector + vector3;
            }
            base.SetupPerspectiveCamera(vector6, vector6 - vector, Vector3.UnitY);
        }

        public Vector3 m_position;
        public Vector2 m_angles = new Vector2(0f, MathUtils.DegToRad(30f));
        public float m_distance = 6f;
    }
}