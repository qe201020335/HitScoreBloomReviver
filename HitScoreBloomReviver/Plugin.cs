using HarmonyLib;
using IPA;
using IPA.Loader;
using Logger = IPA.Logging.Logger;

namespace HitScoreBloomReviver
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private readonly Harmony _harmony = new ("com.github.qe201020335.HitScoreBloomReviver");
        internal static Logger Logger { get; private set; } = null!;
        internal static Plugin Instance { get; private set; } = null!;

        [Init]
        public Plugin(Logger logger)
        {
            Instance = this;
            Logger = logger;
            Logger.Info("HitScoreBloomReviver initialized");
        }

        [OnEnable]
        public void Enable()
        {
            if (PluginManager.GetPluginFromId("HitScoreVisualizer") != null)
            {
                Logger.Warn("HitScoreVisualizer detected, not enabling HitScoreBloomReviver.");
                return;
            }
            
            Logger.Debug("OnEnable");
            _harmony.PatchAll();
        }

        [OnDisable]
        public void Disable()
        {
            Logger.Debug("OnDisable");
            _harmony.UnpatchSelf();
        }
    }
}
