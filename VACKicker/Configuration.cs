using Rocket.API;

namespace VACKicker
{
    public class Configuration : IRocketPluginConfiguration
    {

        public bool Enabled;

        public void LoadDefaults()
        {
            Enabled = true;
        }
    }
}
