using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;


namespace VACKicker
{
    public class VACKicker : RocketPlugin<Configuration>
    {
        public static VACKicker Instance;
        protected override void Load()
        {
            Instance = this;
            if (Instance.Configuration.Instance.Enabled)
            {
                Logger.LogWarning("VACKicker: Enable");
                U.Events.OnBeforePlayerConnected += OnBeforePlayerConnected;
            }
            else
            {
                Logger.LogWarning("VACKicker: Disable");
            }
            
        }

        protected override void Unload()
        {

        }

        public void OnBeforePlayerConnected(UnturnedPlayer player)
        {
            bool? i = player.SteamProfile.IsVacBanned;

            if (i == true)
            {
                player.Kick("VACKicker has kicked");
                Logger.LogWarning("VACKicker -> " + player.DisplayName + " : kicked");
            }
            else if (i == false)
            {
                Logger.LogWarning("VACKicker -> " + player.DisplayName + ": No VAC record");
            }
            else
            {
                player.Kick("Oops!... VACKicker ERROR!!");
                Logger.LogError("VACKicker -> " + player.DisplayName + ": Null");
            }
        }
    }
}