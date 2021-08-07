using System.Windows.Controls;
using Torch;
using Torch.API.Plugins;

namespace DataValidateFix
{
    public class DataValidateFixPlugin : TorchPluginBase, IWpfPlugin
    {
        public UserControl GetControl() => new UserControl()
        {
            Content = new TextBlock()
            {
                Inlines = { "Keen fixed all exploits, please remove this plugin" }
            }
        };
    }
}