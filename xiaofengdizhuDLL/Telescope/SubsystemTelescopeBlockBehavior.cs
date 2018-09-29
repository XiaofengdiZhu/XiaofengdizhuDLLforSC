using Engine;
using System;
using System.Linq;

namespace Game
{
    public class SubsystemTelescopeBlockBehavior : SubsystemBlockBehavior
    {
        /*protected override void Load(ValuesDictionary valuesDictionary)
        {
            m_cameras.Add(new LoadingCamera(this));
        }*/
        public Camera m_lastCamera;
        public View m_view;
        public bool m_hasInitiated = false;

        public override int[] HandledBlocks
        {
            get
            {
                return new int[]
                {
                    350
                };
            }
        }

        public override bool OnUse(Vector3 start, Vector3 direction, ComponentMiner componentMiner)
        {
            try
            {
                if (!m_hasInitiated)
                {
                    m_view = componentMiner.ComponentPlayer.View;
                    if ((TelescopeCamera)((object)m_view.m_cameras.FirstOrDefault((Camera c) => c is TelescopeCamera)) == null)
                    {
                        m_view.m_cameras.Add(new TelescopeCamera(m_view));
                    }
                }
                if (m_view.ActiveCamera is TelescopeCamera)
                {
                    m_view.ActiveCamera = m_view.FindCamera<FppCamera>(true);
                }
                else
                {
                    m_view.ActiveCamera = m_view.FindCamera<TelescopeCamera>(true);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Warning(e.ToString());
            }
            return false;
        }
    }
}