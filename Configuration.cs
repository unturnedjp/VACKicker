using Rocket.API;

namespace VACKicker
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Enable;
        public bool Errorkick;

        public void LoadDefaults()
        {
            Enable = true;
            Errorkick = false;
        }
    }
}
