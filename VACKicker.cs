using Rocket.API.Collections;
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

            if (Instance.Configuration.Instance.Enable)
            {
                Logger.LogWarning("================================");
                Logger.LogWarning("|     VACKicker : Enable       |");
                Logger.LogWarning("================================");
                U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            }
            else
            {
                Logger.LogWarning("================================");
                Logger.LogWarning("|     VACKicker : Disable      |");
                Logger.LogWarning("================================");
            }
            
        }

        protected override void Unload()
        {
            if (Instance.Configuration.Instance.Enable)
            {
                U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"kick_reason", "VACKicker has kicked"},
                    {"vacStatus_true", "{0} ({1}) : kicked"},
                    {"vacStatus_false", "{0} ({1}) : No VAC record"},
                    {"vacStatus_null", "VACKicker >> {0} ({1}) : Null"},
                    {"error_reason", "Oops!... VACKicker ERROR!!"}
                };
            }
        }
        
        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            bool? vacStatus = player.SteamProfile.IsVacBanned;

            if (Instance.Configuration.Instance.Errorkick && (!(vacStatus.HasValue)))
            {
                player.Kick(Translate("error_reason"));
                Logger.LogError(Translate("vacStatus_null", player.DisplayName, player.Id));
            }
            else if (!(vacStatus.HasValue))
            {
                Logger.LogError(Translate("vacStatus_null", player.DisplayName, player.Id));
            }
            else
            {
                if ((bool)vacStatus) { player.Kick(Translate("kick_reason")); }
                Logger.Log(Translate((bool)vacStatus ? "vacStatus_true" : "vacStatus_false", player.DisplayName, player.Id));
            }
        }
    }
}