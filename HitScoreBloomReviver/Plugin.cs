using HarmonyLib;
using IPA;
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
            _harmony = new Harmony("bs.Exomanz.hsbr");
            Logger.Debug("Config loaded");
        }

        [OnEnable]
        public void Enable()
        {
            Logger.Debug("OnEnable");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable]
        public void Disable()
        {
            _harmony.UnpatchSelf();
            _harmony = null;
        }
    }
}
