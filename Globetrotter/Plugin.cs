using Dalamud.Game.Command;
using Dalamud.Plugin;
using System;
using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin.Services;

namespace Globetrotter {
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Plugin : IDalamudPlugin {
        private bool _disposedValue;

        [PluginService]
        internal static IPluginLog Log { get; private set; } = null!;

        [PluginService]
        private IDalamudPluginInterface Interface { get; init; } = null!;

        [PluginService]
        private ICommandManager CommandManager { get; init; } = null!;

        [PluginService]
        internal IDataManager DataManager { get; init; } = null!;

        [PluginService]
        internal IGameGui GameGui { get; init; } = null!;

        [PluginService]
        internal ISigScanner SigScanner { get; init; } = null!;

        [PluginService]
        internal IGameInteropProvider GameInteropProvider { get; init; } = null!;

        internal Configuration Config { get; }
        private PluginUi Ui { get; }
        private TreasureMaps Maps { get; }

        public Plugin() {
            this.Config = this.Interface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Config.Initialize(this.Interface);

            this.Ui = new PluginUi(this);
            this.Maps = new TreasureMaps(this);

            this.Interface.UiBuilder.Draw += this.Ui.Draw;
            this.Interface.UiBuilder.OpenConfigUi += this.Ui.OpenSettings;
            this.GameGui.HoveredItemChanged += this.Maps.OnHover;
            this.CommandManager.AddHandler("/pglobetrotter", new CommandInfo(this.OnConfigCommand) {
                HelpMessage = "Show the Globetrotter config",
            });
            this.CommandManager.AddHandler("/tmap", new CommandInfo(this.OnCommand) {
                HelpMessage = "Open the map and place a flag at the location of your current treasure map",
            });
        }

        private void OnConfigCommand(string command, string args) {
            this.Ui.OpenSettings();
        }

        private void OnCommand(string command, string args) {
            this.Maps.OpenMapLocation();
        }

        protected virtual void Dispose(bool disposing) {
            if (this._disposedValue) {
                return;
            }

            if (disposing) {
                this.Interface.UiBuilder.Draw -= this.Ui.Draw;
                this.Interface.UiBuilder.OpenConfigUi -= this.Ui.OpenSettings;
                this.GameGui.HoveredItemChanged -= this.Maps.OnHover;
                this.Maps.Dispose();
                this.CommandManager.RemoveHandler("/pglobetrotter");
                this.CommandManager.RemoveHandler("/tmap");
            }

            this._disposedValue = true;
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
