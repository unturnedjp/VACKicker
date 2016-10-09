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
                U.Events.OnBeforePlayerConnected += OnBeforePlayerConnected;
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
                U.Events.OnBeforePlayerConnected -= OnBeforePlayerConnected;
            }
        }
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"kick_reason", "VACKicker has kicked"},
                    {"vacStatus_true", "VACKicker -> {0} ({1}) : kicked"},
                    {"vacStatus_false", "VACKicker -> {0} ({1}) : No VAC record"},
                    {"vacStatus_null", "VACKicker -> {0} ({1}) : Null"},
                    {"error_reason", "Oops!... VACKicker ERROR!!"}
                };
            }
        }

        public void OnBeforePlayerConnected(UnturnedPlayer player)
        {
            bool? vacStatus = player.SteamProfile.IsVacBanned;

            if (vacStatus == true)
            {
                player.Kick(Translate("kick_reason"));
                Logger.LogWarning(Translate("vacStatus_true", player.DisplayName, player.Id));
            }
            else if (vacStatus == false)
            {
                Logger.LogWarning(Translate("vacStatus_false", player.DisplayName, player.Id));
            }
            else if(Instance.Configuration.Instance.Errorkick && vacStatus == null)
            {
                player.Kick(Translate("error_reason"));
                Logger.LogError(Translate("vacStatus_null", player.DisplayName, player.Id));
            }
            else
            {
                Logger.LogError(Translate("vacStatus_null", player.DisplayName, player.Id));
            }
        }
    }
}