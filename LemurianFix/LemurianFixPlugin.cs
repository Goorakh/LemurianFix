using BepInEx;
using LemurianFix.Content;
using System.Diagnostics;

namespace LemurianFix
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class LemurianFixPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Gorakh";
        public const string PluginName = "LemurianFix";
        public const string PluginVersion = "1.0.0";

        internal static LemurianFixPlugin Instance { get; private set; }
        
        internal ContentPackProvider ContentPackProvider { get; private set; }

        void Awake()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.Init(Logger);

            Instance = SingletonHelper.Assign(Instance, this);

            ContentPackProvider = new ContentPackProvider();
            ContentPackProvider.Register();

            stopwatch.Stop();
            Log.Info_NoCallerPrefix($"Initialized in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
        }

        void OnDestroy()
        {
            Instance = SingletonHelper.Unassign(Instance, this);
        }
    }
}
