using BeatSaberMarkupLanguage.GameplaySetup;
using HarmonyLib;
using HitScoreBloomReviver.Settings;
using IPA;
using IPA.Config.Stores;
using IPA.Loader;
using Logger = IPA.Logging.Logger;
using System.Reflection;

namespace HitScoreBloomReviver
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private static Harmony _harmony;
        internal static Logger Logger { get; private set; }
        internal static Plugin Instance { get; private set; }

        [Init]
        public Plugin(IPA.Config.Config conf, Logger log)
        {
            Instance = this;
            Logger = log;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
            _harmony = new Harmony("bs.Exomanz.hsbr");
            Logger.Debug("Config loaded");
            
        }

        [OnEnable]
        public void Enable()
        {
            Logger.Debug("OnEnable");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            GameplaySetup.instance.AddTab("HitScoreBloom", "HitScoreBloomReviver.Settings.settingsView.bsml", SettingsUI.instance);
        }

        [OnDisable]
        public void Disable()
        {
            GameplaySetup.instance.RemoveTab("HitScoreBloom");
            _harmony.UnpatchSelf();
            _harmony = null;
        }
    }
}
