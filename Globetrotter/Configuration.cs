using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace Globetrotter {
    [Serializable]
    internal class Configuration : IPluginConfiguration {
        private IDalamudPluginInterface Plugin { get; set; } = null!;

        public int Version { get; set; } = 1;

        public bool ShowOnHover { get; set; } = true;
        public bool ShowOnDecipher { get; set; } = true;
        public bool ShowOnOpen { get; set; } = true;

        public void Initialize(IDalamudPluginInterface pi) {
            this.Plugin = pi ?? throw new ArgumentNullException(nameof(pi), "DalamudPluginInterface cannot be null");
        }

        public void Save() {
            this.Plugin.SavePluginConfig(this);
        }
    }
}
