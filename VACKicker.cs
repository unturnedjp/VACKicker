using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Permissions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

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
                Logger.LogWarning("================================");
                Logger.LogWarning("|     VACKicker : Enabled      |");
                Logger.LogWarning("================================");
                UnturnedPermissions.OnJoinRequested += Events_OnJoinRequested;
            }
            else
            {
                Logger.LogWarning("================================");
                Logger.LogWarning("|     VACKicker : Disabled     |");
                Logger.LogWarning("================================");
            }
        }

        protected override void Unload()
        {
            if (Instance.Configuration.Instance.Enabled)
            {
                UnturnedPermissions.OnJoinRequested -= Events_OnJoinRequested;
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"vacStatus_true", "{0} ({1}) : kicked"},
                    {"vacStatus_false", "{0} ({1}) : No VAC record"},
                    {"vacStatus_null", "VACKicker >> {0} ({1}) : Null"},
                };
            }
        }
        
        private void Events_OnJoinRequested(CSteamID player, ref ESteamRejection? rejection)
        {
            UnturnedPlayer user = UnturnedPlayer.FromCSteamID(player);
            bool? vacStatus = user.SteamProfile.IsVacBanned;

            if (Instance.Configuration.Instance.Errorkick && (!(vacStatus.HasValue)))
            {
                rejection = ESteamRejection.PLUGIN;
                Logger.LogError(Translate("vacStatus_null", user.DisplayName, player));
            }
            else if (!(vacStatus.HasValue))
            {
                Logger.LogError(Translate("vacStatus_null", user.DisplayName, player));
            }
            else
            {
                if ((bool)vacStatus) { rejection = ESteamRejection.AUTH_VAC_BAN; }
                Logger.Log(Translate((bool)vacStatus ? "vacStatus_true" : "vacStatus_false", user.DisplayName, player));
            }
        }
    }
}